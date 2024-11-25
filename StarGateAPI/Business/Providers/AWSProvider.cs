using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using StargateAPI.Business.Dtos;
using System.Text.Json;
using StargateAPI.Business.Interfaces;

namespace StargateAPI.Business.Providers
{
    public class AWSProvider : ICloudProvider
    {
        public async static Task<SecretResponse> GetCredentials(BaseCloudRequest cloudRequest)
        {
            // Could be better exception  handling here. 
            if ((cloudRequest == null) || ((cloudRequest.KeyID == null) || (cloudRequest.SecretKeyId ==  null) || (cloudRequest.CredentialsLabel == null)))
                throw new Exception("Unable to access.");

            var request = new GetSecretValueRequest { SecretId = cloudRequest.CredentialsLabel };
            var secretsManagerClient = new AmazonSecretsManagerClient(cloudRequest.KeyID, cloudRequest.SecretKeyId);
            var response = await secretsManagerClient.GetSecretValueAsync(request);
            var secretString = response.SecretString;
            return JsonSerializer.Deserialize<SecretResponse>(secretString);
        }

    }
}
