using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AzureKeyVault.SecretManager
{
    public interface ISecretManager
    {
        Task<string> GetSecretAsync(string vaultKey);
    }
}
