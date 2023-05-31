using GroupFlights.Communication.Shared;
using GroupFlights.Communication.Shared.Models;
using GroupFlights.Shared.Plumbing.Events;
using GroupFlights.Shared.Types.Exceptions;
using GroupFlights.Shared.Types.Time;
using GroupFlights.TimeManagement.Core.Data.EF;
using GroupFlights.TimeManagement.Core.Exceptions;
using GroupFlights.TimeManagement.Core.Models;
using GroupFlights.TimeManagement.Shared;
using GroupFlights.TimeManagement.Shared.Events;
using GroupFlights.TimeManagement.Shared.Operations;
using Microsoft.EntityFrameworkCore;

namespace GroupFlights.TimeManagement.Core.Services;

internal class DeadlineService : IDeadlineService
{
    private readonly IClock _clock;
    private readonly ICommunicationApi _communicationApi;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly TimeManagementDbContext _dbContext;

    private static readonly DeadlinesConfiguration Configuration = new(
        new[]
        {
            new NotificationOffset(TimeSpan.FromDays(7)),
            new NotificationOffset(TimeSpan.FromDays(1)),
            new NotificationOffset(TimeSpan.FromHours(6)),
            new NotificationOffset(TimeSpan.FromHours(1))
        });

    public DeadlineService(IClock clock,
        ICommunicationApi communicationApi,
        IEventDispatcher eventDispatcher,
        TimeManagementDbContext dbContext)
    {
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _communicationApi = communicationApi ?? throw new ArgumentNullException(nameof(communicationApi));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    
    public async Task SetUpDeadline(SetUpDeadlineDto request, CancellationToken cancellationToken = default)
    {
        var now = _clock.UtcNow;
        var pendingNotifications = new List<DeadlineNotification>();

        if (request.DeadlineDateUtc < now)
        {
            throw new DeadlineIsAlreadyOverdueException();
        }

        foreach (var notificationConfig in Configuration.NotificationOffsets
                     .OrderByDescending(n => n.OffsetFromDeadlineDate))
        {
            var deadlineOffsetFromNow = request.DeadlineDateUtc - now;
            if (deadlineOffsetFromNow > notificationConfig.OffsetFromDeadlineDate)
            {
                var deadlineNotificationDate = request.DeadlineDateUtc - notificationConfig.OffsetFromDeadlineDate;
                pendingNotifications.Add(new DeadlineNotification(deadlineNotificationDate));
            }
        }

        if (pendingNotifications.Count == 0)
        {
            pendingNotifications.Add(new DeadlineNotification(request.DeadlineDateUtc.AddMinutes(-30)));
        }

        var deadline = new Deadline(
            request.Id, 
            request.CommunicationChannel, 
            request.Participants.Select(p => 
                new DeadlineParticipant(Guid.NewGuid(), p.UserId?.Value, p.Email)).ToArray(), 
            request.Message,
            request.DeadlineDateUtc, 
            pendingNotifications);

        await _dbContext.AddAsync(deadline, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkDeadlineFulfilled(DeadlineId deadlineId, CancellationToken cancellationToken = default)
    {
        var deadline = _dbContext.Deadlines.SingleOrDefault(x => x.Id.Equals(deadlineId));

        if (deadline is null)
        {
            throw new DoesNotExistException();
        }

        if (deadline.Fulfilled is not null)
        {
            throw new DeadlineWasAlreadyFulfilledOrOverdueException();
        }

        deadline.Fulfilled = true;
        _dbContext.Deadlines.Attach(deadline);
        _dbContext.Entry(deadline).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ProcessActiveDeadlines(CancellationToken cancellationToken = default)
    {
        var now = _clock.UtcNow;
        
        var activeDeadlines = _dbContext.Deadlines
            .Include(d => d.Notifications)
            .Include(d => d.Participants)
            .Where(d => d.Fulfilled == null).ToList();

        foreach (var activeDeadline in activeDeadlines)
        {
            if (activeDeadline.DeadlineDateUtc <= now)
            {
                activeDeadline.Fulfilled = false;
                await _eventDispatcher.PublishAsync(new DeadlineOverdueIntegrationEvent(activeDeadline.Id), cancellationToken);
            }
            else
            {
                await NotifyDeadline(activeDeadline, now, cancellationToken);
            }
            
            _dbContext.Deadlines.Update(activeDeadline);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task UpdateDeadline(UpdateDeadlineDueDateDto dueDateUpdate, CancellationToken cancellationToken = default)
    {
        var deadline = _dbContext.Deadlines.SingleOrDefault(x => x.Id.Equals(dueDateUpdate.Id));

        if (deadline is null)
        {
            throw new DoesNotExistException();
        }

        deadline.DeadlineDateUtc = dueDateUpdate.NewDueDate;
        
        _dbContext.Deadlines.Attach(deadline);
        _dbContext.Entry(deadline).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task NotifyDeadline(Deadline activeDeadline, DateTime now, CancellationToken cancellationToken)
    {
        var firstUnsentNotification = activeDeadline.Notifications
            .Where(n => n.State == DeadlineNotification.CompletionState.Pending)
            .MinBy(n => n.DueDate);

        if (firstUnsentNotification is null)
        {
            return;
        }

        if (firstUnsentNotification.DueDate <= now)
        {
            await SendMessageResolved(activeDeadline.Participants, activeDeadline.Message, cancellationToken);
            firstUnsentNotification.State = DeadlineNotification.CompletionState.Sent;
            _dbContext.Notifications.Update(firstUnsentNotification);
        }
    }

    private async Task SendMessageResolved(IReadOnlyCollection<DeadlineParticipant> participants, Message message, CancellationToken cancellationToken)
    {
        foreach (var participant in participants)
        {
            if (participant.UserId is not null)
            {
                await _communicationApi.SendMessageToRegisteredUser(
                    participant.UserId,
                    CommunicationChannel.EmailAndNotification,
                    message,
                    cancellationToken);
            }
            else
            {
                await _communicationApi.SendMessageToAnonymousUser(
                    participant.Email,
                    message,
                    cancellationToken);
            }   
        }
    }
}