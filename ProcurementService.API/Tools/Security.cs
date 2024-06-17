using System.Security.Cryptography;
using System.Text;

namespace ProcurementService.API.Tools
{
    public static class Security
    {
        public static string GetHash(string str)
        {
            var sha512 = SHA512.Create();
            var hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(str));

            return Convert.ToBase64String(hash);
        }
    }
}
