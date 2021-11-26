using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZLMediaKit.Common.Dtos
{
    public interface IResultBase
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }

    public class ResultBase: IResultBase
    {

        [JsonPropertyName("code")]
        public int Code { get; set; }
    }

}
