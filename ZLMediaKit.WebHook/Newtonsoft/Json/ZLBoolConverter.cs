using System;

namespace Newtonsoft.Json
{
    /// <summary>
    /// Json Bool 转换器
    /// </summary>
    public class ZLBoolConverter : JsonConverter
    {
        /// <summary>
        /// Json Bool 转换器
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        public override bool CanWrite => false;
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value.ToString() == "0" || reader.Value.ToString().ToLower()=="false")
            {
                return false;
            }
            else return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
           
        }
    }
}
