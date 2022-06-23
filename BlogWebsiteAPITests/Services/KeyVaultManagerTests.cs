using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlogWebsiteAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsiteAPI.Services.Tests
{
	[TestClass()]
	public class KeyVaultManagerTests
	{
		private readonly IKeyVaultManagement _keyManager;

		public KeyVaultManagerTests()
		{
			_keyManager = new KeyVaultManager();
		}

		[TestMethod()]
		public void GetSecretTest()
		{
			string secretName = "BlogWebsiteConnection";

			var getSecretResult = _keyManager.GetSecret(secretName);

			Assert.IsNotNull(getSecretResult);
		}
	}
}