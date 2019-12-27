namespace ServiceClient.Models
{
    /// <summary>
    /// Individual authority, used when deserializing from FSA service
    /// </summary>
    public class Authority
    {
        public int LocalAuthorityId { get; set; }
        public string LocalAuthorityIdCode { get; set; }
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public string RegionName { get; set; }
    }
}
