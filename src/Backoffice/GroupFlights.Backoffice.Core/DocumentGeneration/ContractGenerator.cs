using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GroupFlights.Sales.Shared.IntegrationEvents;

namespace GroupFlights.Backoffice.Core.DocumentGeneration;

internal class ContractGenerator
{
    public async Task<byte[]> Generate(ContractGenerationRequestedIntegrationEvent request, CancellationToken cancellationToken)
    {
        var assembly = typeof(ContractGenerator).Assembly;

        var template = assembly.GetManifestResourceNames().Single(rsc => rsc.EndsWith("EmptyContract.txt"));

        byte[] contentBytes;
        
        using (Stream stream = assembly.GetManifestResourceStream(template))
        using (StreamReader reader = new StreamReader(stream))
        {
            var content = await reader.ReadToEndAsync();
            content += Environment.NewLine;
            content += JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new JsonStringEnumConverter() }
            });
            
            contentBytes = Encoding.UTF8.GetBytes(content);
        }

        return contentBytes;
    }
    
    private static async Task<byte[]> ReadBytes(Stream input)
    {
        input.Seek(0, SeekOrigin.Begin);
        using MemoryStream ms = new MemoryStream((int)input.Length);
        await input.CopyToAsync(ms);
        return ms.ToArray();
    }
}