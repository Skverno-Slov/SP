using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;

byte[] key = Encoding.UTF8.GetBytes("12345678123456781234567812345678");
byte[] iv = Encoding.UTF8.GetBytes("1234567890123456");

Encrypt(key, iv, "ЙООО");

static string Encrypt(byte[] key, byte[] iv, string message) 
{
    using (Aes aes = Aes.Create())
    {
        aes.Key = key;
        aes.IV = iv;

        ICryptoTransform encriptor = aes.CreateEncryptor();

        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, encriptor, CryptoStreamMode.Write))
            {
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(message);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}

static string Decrypt(byte[] key, byte[] iv, string message)
{
    using (Aes aes = Aes.Create())
    {
        aes.Key = key;
        aes.IV = iv;

        ICryptoTransform encriptor = aes.CreateEncryptor();

        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, encriptor, CryptoStreamMode.Read))
            {
                using (StreamReader sr = new StreamReader(cs))
                {
                   return sr.ReadToEnd();
                }
            }
        }
    }
}


//Console.WriteLine(
//    Convert.ToBase64String(
//        )
//    );