using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureKeyVault.SecretManager
{
    public class SecretManager : ISecretManager
    {
        public async Task<string> GetSecretAsync(string vaultKey)
        {
            try
            {
                string vaultUrl = "https://yourvaulturl.vault.azure.net/"; //Change it to URL of your vault.
                var client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessTokenAsync), new HttpClient());
                var secret = await client.GetSecretAsync(vaultUrl, vaultKey);
                return secret.Value;
            }
            catch(Microsoft.Azure.KeyVault.Models.KeyVaultErrorException)
            {
                return "KeyNotFound";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        private async Task<string> GetAccessTokenAsync(string authority, string resource, string scope)
        {
            //DEMO ONLY
            //Storing ApplicationId and Key in code is bad idea :)
            var appCredentials = new ClientCredential("app registration key", "app registration secret"); //App Registration Key and Secret that have access permission of your key vault.
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, appCredentials);
            return result.AccessToken;
        }
    }
}
