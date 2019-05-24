using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureKeyVault.SecretManager
{
    public class AWSSecretManager : ISecretManager
    {
        public async Task<string> GetSecretAsync(string vaultKey)
        {
            string secretName = "secretmanagername"; //Name of the secret manager.
            string region = "us-east-2"; //Region where you created secret manager
            string secret = "";

            MemoryStream memoryStream = new MemoryStream();

            var config = new AmazonSecretsManagerConfig { RegionEndpoint = RegionEndpoint.GetBySystemName(region) };
            //config.Timeout = AmazonSecretsManagerConfig.MaxTimeout;
            var client = new AmazonSecretsManagerClient("IAMUserKey", "IAMUserSecret", config); //Key and Secret of I AM User that have permission to access secret manager.
            GetSecretValueRequest request = new GetSecretValueRequest();
            request.SecretId = secretName;

            GetSecretValueResponse response = null;
            // In this sample we only handle the specific exceptions for the 'GetSecretValue' API.
            // See https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
            // We rethrow the exception by default.

            try
            {
                response = Task.Run(async () => await client.GetSecretValueAsync(request)).Result;
            }
            catch (DecryptionFailureException e)
            {
                // Secrets Manager can't decrypt the protected secret text using the provided KMS key.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw e;
            }
            catch (InternalServiceErrorException e)
            {
                // An error occurred on the server side.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw e;
            }
            catch (InvalidParameterException e)
            {
                // You provided an invalid value for a parameter.
                // Deal with the exception here, and/or rethrow at your discretion
                throw e;
            }
            catch (InvalidRequestException e)
            {
                // You provided a parameter value that is not valid for the current state of the resource.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw e;
            }
            catch (ResourceNotFoundException e)
            {
                // We can't find the resource that you asked for.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw e;
            }
            catch (AggregateException e)
            {
                // More than one of the above exceptions were triggered.
                // Deal with the exception here, and/or rethrow at your discretion.
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            // Decrypts secret using the associated KMS CMK.
            // Depending on whether the secret is a string or binary, one of these fields will be populated.
            if (response.SecretString != null)
            {
                var keyvalue = (JObject)JsonConvert.DeserializeObject(response.SecretString);
                secret = Convert.ToString(keyvalue.GetValue(vaultKey));
            }
            else
            {
                memoryStream = response.SecretBinary;
                StreamReader reader = new StreamReader(memoryStream);
                string decodedBinarySecret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }

            if(string.IsNullOrEmpty(secret))
            {
                return "KeyNotFound";
            }
            else
            {
                return secret;
            }
        }
    }
}
