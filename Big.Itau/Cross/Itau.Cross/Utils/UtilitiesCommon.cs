using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Itau.Common.Utils
{
    public class UtilitiesCommon
    {
        /// <summary>
        /// Obtiene Los applicationSettings del archivo de configuraciones App.config
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetKeyAppSettings(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key) || ConfigurationManager.AppSettings[key] == null)
                {
                    throw new Exception();
                }
                return ConfigurationManager.AppSettings[key];
            }
            catch
            {
                throw new ArgumentNullException("El parametro con la clave (" + key + ") no existe en el archivo de configuración.");
            }
        }

        public class ApplicationAuth
        {
            public static bool loginAplication(string userName, string password)
            {
                AplicationUser user = new AplicationUser();
                if (user.UserName.Equals(userName) && user.Password.Equals(password))
                    return true;
                else
                    return false;
            }

            public class AplicationUser
            {
                public string UserName { get; set; }

                public string Password { get; set; }

                public AplicationUser()
                {
                    UserName = Common.Helpers.ConfigurationHelper.Get("BasicAuthentication.UserName").ToString();
                    Password = Common.Helpers.ConfigurationHelper.Get("BasicAuthentication.Password").ToString();
                }
            }
        }

        public class Cryptrography
        {
            private static readonly string EncriptionKey = "H2506Bravo1996Al";

            public static string Encrypt(string value)
            {
                try
                {
                    var key = Encoding.UTF8.GetBytes(EncriptionKey); //must be 16 chars
                    var rijndael = new RijndaelManaged
                    {
                        BlockSize = 128,
                        KeySize = 128,
                        Key = key,
                        IV = key,
                        Mode = CipherMode.CBC
                    };

                    var transform = rijndael.CreateEncryptor();
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                        {
                            byte[] buffer = Encoding.UTF8.GetBytes(value);
                            cs.Write(buffer, 0, buffer.Length);
                            cs.FlushFinalBlock();
                            cs.Close();
                        }
                        ms.Close();
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
                catch { return string.Empty; }
            }

            public static string Decrypt(string value)
            {
                try
                {
                    var key = Encoding.UTF8.GetBytes(EncriptionKey); //must be 16 chars
                    var rijndael = new RijndaelManaged
                    {
                        BlockSize = 128,
                        KeySize = 128,
                        Key = key,
                        IV = key,
                        Mode = CipherMode.CBC
                    };

                    var buffer = Convert.FromBase64String(value);
                    var transform = rijndael.CreateDecryptor();
                    string decrypted;
                    using (var ms = new MemoryStream())
                    {
                        using (
                            var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                        {
                            cs.Write(buffer, 0, buffer.Length);
                            cs.FlushFinalBlock();
                            decrypted = Encoding.UTF8.GetString(ms.ToArray());
                            cs.Close();
                        }
                        ms.Close();
                    }
                    return decrypted;
                }
                catch { return null; }
            }
        }
    }
}