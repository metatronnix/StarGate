namespace StargateAPI.Business.Dtos
{
    public class BaseCloudRequest
    {
        public string? KeyID { get; set; }
        public string? SecretKeyId { get; set; }
        public string? CredentialsLabel { get; set; }
    }
}
