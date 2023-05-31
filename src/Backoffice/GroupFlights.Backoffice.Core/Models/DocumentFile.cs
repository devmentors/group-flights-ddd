using GroupFlights.Shared.Types;

namespace GroupFlights.Backoffice.Core.Models;

internal class DocumentFile
{
    public Guid FileId { get; set; }
    public byte[] Content { get; set; }
    public string Name { get; set; }
    public Guid? ContractId { get; set; }
    public UserId Owner { get; set; }
}