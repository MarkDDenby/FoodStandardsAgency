namespace ServiceClient.Models
{    
    /// <summary>
    /// Individual establishment, used when deserializing from FSA service
    /// </summary>
    public class Establishment
    {
        public string LocalAuthorityBusinessId { get; set; }
        public string RatingValue { get; set; }
    }
}
