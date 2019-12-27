using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ServiceClient.Models
{
    /// <summary>
    /// List of authorities, used when deserializing from FSA service
    /// </summary>
    [DataContract]
    public class Authorities
    {
        public Authorities()
        {
            this.Items = new List<Authority>();
        }

        [JsonProperty("authorities")]
        public List<Authority> Items { get; set; }
    }
}
