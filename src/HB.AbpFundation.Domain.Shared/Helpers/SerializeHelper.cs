using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HB.AbpFundation.Helpers
{
    public static class SerializeHelper
    {
        static JsonSerializerSettings defaultSettings = new JsonSerializerSettings();

        public static JsonSerializerSettings Settings
        {
            get
            {
                defaultSettings.Formatting = Formatting.Indented;
                defaultSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                return defaultSettings;
            }
        }

        public static string ToNewtonJsonString(this object value, JsonSerializerSettings settings = null)
        {
            if (settings == null)
            {
                settings = Settings;
            }
            return JsonConvert.SerializeObject(value, settings);
        }

        public static T ToNewtonJsonObject<T>(this string value, JsonSerializerSettings settings = null)
        {
            if (settings == null)
            {
                settings = Settings;
            }
            return JsonConvert.DeserializeObject<T>(value, settings);
        }
        public static object ToNewtonJsonObject(this string value, JsonSerializerSettings settings = null)
        {
            if (settings == null)
            {
                settings = Settings;
            }
            return JsonConvert.DeserializeObject(value, settings);
        }
    }
}
