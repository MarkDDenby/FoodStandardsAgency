using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServiceClient
{
    /// <summary>
    /// Http content extensions helpers
    /// </summary>
    public static class HttpContentExtensions
    {
        /// <summary>
        /// helper to deserialize json into a strongly typed object definition
        /// </summary>
        /// <returns>Task containing deserialized result</returns>
        /// <param name="content">the Http content.</param>
        /// <typeparam name="T">target type to be deserialized into.</typeparam>
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            string json = await content.ReadAsStringAsync();
            T value = JsonConvert.DeserializeObject<T>(json);
            return value;
        }
    }
}
