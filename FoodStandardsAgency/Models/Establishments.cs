using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ServiceClient.Models
{
    /// <summary>
    /// List of establishments, used when deserializing from FSA service
    /// </summary>
    [DataContract]
    public class Establishments
    {
        public Establishments()
        {
            this.Items = new List<Establishment>();
        }

        [JsonProperty("establishments")]
        public List<Establishment> Items { get; set; }
    }
}
