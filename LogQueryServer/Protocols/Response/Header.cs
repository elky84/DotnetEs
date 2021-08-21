using LogQueryServer.Code;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LogQueryServer.Protocols.Response
{
    public class Header
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public Result Result { get; set; } = Result.Success;

        public string ErrorMessage { get; set; }
    }
}
