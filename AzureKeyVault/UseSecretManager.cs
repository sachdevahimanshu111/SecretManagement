using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AzureKeyVault.SecretManager;

namespace AzureKeyVault.App
{
    public class UseSecretManager
    {
        private static UseSecretManager _instance;
        public static UseSecretManager Instance { get
            {
                if(_instance == null)
                {
                    _instance = new UseSecretManager();
                }

                return _instance;
            }
        }

        private UseSecretManager()
        {

        }

        public async Task<string> GetSecretValue(int secretType, string key)
        {
            ISecretManager secretManager;
            switch (secretType)
            {
                case 1:
                    secretManager = new SecretManager.SecretManager();
                    break;
                case 2:
                    secretManager = new SecretManager.AWSSecretManager();
                    break;
                default:
                    secretManager = new SecretManager.SecretManager();
                    break;
            }

            return await secretManager.GetSecretAsync(key);
        }
    }
}
