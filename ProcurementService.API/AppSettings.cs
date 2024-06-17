using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProcurementService.API
{
    public class AppSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DirectoryFiles { get; set; } = string.Empty;
        public AuthOption AuthOptions { get; set; } = new();
        public class AuthOption
        {
            public string Issuer { get; set; } = string.Empty; // издатель токена
            public string Audience { get; set; } = string.Empty;  // потребитель токена
            public string Key { get; set; } = string.Empty;   // ключ для шифрации
            public int Lifetime { get; set; }  // время жизни токена - 1 минута
            public SymmetricSecurityKey SymmetricSecurityKey
            {
                get
                {
                    return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
                }
                private set { }
            }
        }

        public MyDirectory Directories { get; set; } = new();

        public class MyDirectory
        {
            public string Storage { get; set; } = string.Empty;
            public string StorageZip { get; set; } = string.Empty;
            public string Template { get; set; } = string.Empty;
        }
    }
}