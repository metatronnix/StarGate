namespace StargateAPI.Business.Dtos
{
    public class SecretResponse
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Engine { get; set; } = string.Empty;

        public string Host { get; set; } = string.Empty;

        public int Port { get; set; }

        public string DBInstanceIdentifier { get; set; } = string.Empty;
    }
}
