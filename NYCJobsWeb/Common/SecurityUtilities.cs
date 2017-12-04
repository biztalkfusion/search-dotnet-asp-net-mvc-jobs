using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace NYCJobsWeb.Common
{
    public class SecurityUtilities
    {
        public static string Encrypt(string clearText, string encryptionkey)
        {
            var encryptionKey = encryptionkey;
            var clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                if (encryptor != null)
                {
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText, string encryptionkey)
        {
            var encryptionKey = encryptionkey;
            cipherText = cipherText.Replace(" ", "+");
            var cipherBytes = Convert.FromBase64String(cipherText);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                if (encryptor != null)
                {
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            return cipherText;
        }

        public static string EncryptUrl(string clearText, string encryptionkey)
        {
            return Encrypt(clearText, encryptionkey).Replace("+", "-").Replace("/", "_");
        }

        public static string DecryptUrl(string cipherText, string encryptionkey)
        {
            return Decrypt(cipherText.Replace("-", "+").Replace("_", "/"), encryptionkey);
        }

        public static string GetUrlEncryptedString(List<string> encrptionData, string encryptionKey)
        {
            var clearText = string.Join("||", encrptionData);

            return HttpUtility.UrlEncode(Encrypt(clearText, encryptionKey));
        }

        public static IList<string> GetDecryptedUrlData(string cipherText, string encryptionkey)
        {
            var resultText = Decrypt(HttpUtility.UrlDecode(cipherText), encryptionkey);

            return Regex.Split(resultText, Regex.Escape("||")).ToList();
        }
    }
}