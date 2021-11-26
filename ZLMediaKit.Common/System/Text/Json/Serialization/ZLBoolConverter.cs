namespace System.Text.Json.Serialization
{
    public class ZLBoolConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.None:
                    return false;
                case JsonTokenType.StartObject:
                    return false;
                case JsonTokenType.EndObject:
                    return false;
                case JsonTokenType.StartArray:
                    return false;
                case JsonTokenType.EndArray:
                    return false;
                case JsonTokenType.PropertyName:
                    return false;
                case JsonTokenType.Comment:
                    return false;
                case JsonTokenType.String:
                    return reader.GetString() == "1";
                case JsonTokenType.Number:
                    return reader.GetInt32() == 1;
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.Null:
                    return false;
                default:
                    break;
            }
            return reader.GetInt32() == 1;
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value ? "1" : "0");
        }
    }
}
