using Azure.Security.KeyVault.Secrets;
using System.Threading.Tasks;
using System;

namespace BlogWebsiteAPI.Services
{
	public interface IKeyVaultManagement
	{
		public Task<string> GetSecret(string secretName);
	}

	public class KeyVaultManager : IKeyVaultManagement
	{
		private readonly SecretClient _secretClient;

		public KeyVaultManager(SecretClient secret)
		{
			_secretClient = secret;
		}
		public async Task<string> GetSecret(string secretName)
		{
			KeyVaultSecret secretResult = await _secretClient.GetSecretAsync(secretName);
			return secretResult.Value;
		}
	}
}
