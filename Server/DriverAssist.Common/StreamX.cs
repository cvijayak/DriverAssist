using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace DriverAssist.Common
{
    public static class StreamX
    {
        public static T ToObject<T>(this Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                using (var reader = new JsonTextReader(streamReader))
                {
                    var settings = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        ContractResolver = new JsonContractResolver(),
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    settings.Converters.Add(new StringEnumConverter());

                    var serializer = JsonSerializer.Create(settings);
                    return serializer.Deserialize<T>(reader);
                }
            }
        }
    }
}