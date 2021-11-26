using System.Diagnostics.CodeAnalysis;
using ZLMediaKit.Common;

namespace System.Text.Json.Serialization
{
    public class TypeMappingConvert<TType, TImplenetation> : JsonConverter<TType> where TImplenetation :TType, new()
    {
        [return: MaybeNull]
        public override TType? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            TypeMapping.TypeMappings.TryGetValue(typeof(TType), out var newType);
            if(newType != null)
            {
                return (TType)JsonSerializer.Deserialize(ref reader, newType, options);
            }
            return JsonSerializer.Deserialize<TImplenetation>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, TType value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
