using System.Security.Cryptography;
using System.Text;

namespace BlogWebsiteAPI.Services
{
    public class UserRequestFunctions
    {
		public static string PasswordHash(string password, byte[] salt)
		{
			using (var tmp = new Rfc2898DeriveBytes(password, salt))
			{
				return Encoding.Default.GetString(tmp.GetBytes(salt.Length));
			}
		}
    }
}
