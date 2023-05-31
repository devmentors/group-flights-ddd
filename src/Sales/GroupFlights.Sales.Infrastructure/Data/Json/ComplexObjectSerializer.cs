using System.Reflection;
using GroupFlights.Sales.Domain.Shared.DomainEvents.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GroupFlights.Sales.Infrastructure.Data.Json;

internal static class ComplexObjectSerializer
{
    //NOTE: To rozwiazanie tworzy nadmiarowe node'y w JSONie z suffixem "k__BackingField";
    //Sa dwie znane mi opcje rozwiazania tego: atrybut [DataMember] na modelu lub czyszczenie JSONa recznie.
    public static string SerializeToJson<T>(T obj)
    {
        var settings = new JsonSerializerSettings();
        settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
        settings.ContractResolver = new ContractResolverWithPrivates();

        return JsonConvert.SerializeObject(obj, settings);
    }
    
    public static T DeserializeFromJson<T>(string json)
    {
        var settings = new JsonSerializerSettings();
        settings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
        settings.ContractResolver = new ContractResolverWithPrivates();

        return JsonConvert.DeserializeObject<T>(json, settings);
    }

    internal class ContractResolverWithPrivates : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .Select(p => base.CreateProperty(p, memberSerialization))
                .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(p => p.Name != "_domainEvents")
                    .Select(f => base.CreateProperty(f, memberSerialization)))
                .ToList();
            props.ForEach(p => { p.Writable = true; p.Readable = true; });
            return props;
        }
    }
}