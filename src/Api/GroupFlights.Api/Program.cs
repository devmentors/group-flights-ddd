using System.Text.Json.Serialization;
using GroupFlights.Api;
using GroupFlights.Api.ErrorHandling;
using GroupFlights.Api.UserContext;
using GroupFlights.Backoffice.Api;
using GroupFlights.Communication.Api;
using GroupFlights.Finance.Api;
using GroupFlights.Inquiries.Api;
using GroupFlights.Postsale.Api;
using GroupFlights.Sales.Api;
using GroupFlights.Shared.ModuleDefinition;
using GroupFlights.Shared.Plumbing.Database;
using GroupFlights.TimeManagement.Api;
using GroupFlights.WorkloadManagement.Api;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

Modules.RegisterModule<InquiriesModule>();
Modules.RegisterModule<SalesModule>();
Modules.RegisterModule<BackofficeModule>();
Modules.RegisterModule<FinanceModule>();
Modules.RegisterModule<TimeManagementModule>();
Modules.RegisterModule<CommunicationModule>();
Modules.RegisterModule<WorkloadManagementModule>();
Modules.RegisterModule<PostsaleModule>();

builder.Services.AddControllers();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiDependencies(builder.Configuration);

var app = builder.Build();

app.Services.EnsureDatabaseCreated();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapModulesEndpoints();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<NaiveAccessControlMiddleware>();

app.Run();