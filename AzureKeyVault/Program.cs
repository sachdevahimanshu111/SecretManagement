using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureKeyVault.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await RunSecret();
        }

        static async Task RunSecret()
        {
            Console.WriteLine("Enter 1 for Azure and 2 for AWS service");
            int secretType = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the key");
            UseSecretManager useSecretManager = UseSecretManager.Instance;
            string key = Console.ReadLine();
            string secretValue = await useSecretManager.GetSecretValue(secretType, key);
            Console.WriteLine(secretValue != "KeyNotFound" ? "Secret Value for Key is : " + secretValue : "Secret Key Not Avaialable");
            await ContinueExecution();
        }

        static async Task ContinueExecution()
        {
            Console.WriteLine("Enter 1 for Continue Programme else press any key to exit");
            string str = Console.ReadLine();
            if(str == "1")
            {
                await RunSecret();
            }
        }
    }
}
