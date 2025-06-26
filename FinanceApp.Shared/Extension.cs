using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Runtime.InteropServices.ObjectiveC;

namespace FinanceApp.Shared
{
    public static class HttpResponseExtension
    {
        public static async Task<T> ObjectValue<T>(this HttpResponseMessage message)
        {
            var content = await message.Content.ReadAsStringAsync();
            var newObj = JsonConvert.DeserializeObject<T>(content);

            return newObj;
        }
        public static T ParseObject<T>(this dynamic message)
        {
            if (message is string json)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }

            var jsonString = JsonConvert.SerializeObject(message);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

    }
}
