using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Tools
{
    public class CryptoTool
    {
        #region Base64String
        #region Encode
        public static string EncodeBase64String(string data, Encoding enc = null) => Convert.ToBase64String((enc == null ? Encoding.UTF8 : enc).GetBytes(data));
        public static string EncodeBase64String(byte[] data) => Convert.ToBase64String(data);
        public static string EncodeBase64String(Bitmap data)
        {
            string ret = null;
            if (data != null)
            {
                using (MemoryStream m = new MemoryStream())
                {
                    data.Save(m, System.Drawing.Imaging.ImageFormat.Png);
                    ret = Convert.ToBase64String(m.ToArray());
                }
            }
            return ret;
        }
        #endregion
        #region Decode
        public static T DecodeBase64String<T>(string data, Encoding enc = null)
        {
            T ret = default(T);
            if (typeof(T) == typeof(string)) ret = (T)(object)(enc == null ? Encoding.UTF8 : enc).GetString(Convert.FromBase64String(data));
            else if (typeof(T) == typeof(byte[])) ret = (T)(object)Convert.FromBase64String(data);
            else if (typeof(T) == typeof(Bitmap))
            {
                using (var m = new MemoryStream(Convert.FromBase64String(data)))
                {
                    ret = (T)(object)Bitmap.FromStream(m);
                }
            }
            return ret;
        }
        #endregion
        #endregion

        #region AES256
        #region Encrypt
        public static string EncryptAES256(string Input, string key, Encoding enc = null)
        {
            if (enc == null) enc = Encoding.UTF8;
            byte[] xBuff = null;

            using (RijndaelManaged aes = new RijndaelManaged())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = enc.GetBytes(key);
                aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = enc.GetBytes(Input);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }
            }

            if (xBuff != null) return Convert.ToBase64String(xBuff);
            else return null;
        }
        #endregion
        #region Decrypt
        public static string DecryptAES256(string Input, string key, Encoding enc = null)
        {
            if (enc == null) enc = Encoding.UTF8;
            byte[] xBuff = null;

            using (RijndaelManaged aes = new RijndaelManaged())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = enc.GetBytes(key);
                aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var decrypt = aes.CreateDecryptor();
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Convert.FromBase64String(Input);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }
            }
            if (xBuff != null) return enc.GetString(xBuff);
            else return null;
        }
        #endregion
        #endregion

        #region AES128
        #region Encrypt
        public static string EncryptAES128(string Input, string key, Encoding enc = null)
        {
            if (enc == null) enc = Encoding.UTF8;

            byte[] CipherBytes = null;
            using (RijndaelManaged RijndaelCipher = new RijndaelManaged())
            {
                byte[] PlainText = enc.GetBytes(Input);
                byte[] Salt = enc.GetBytes(key.Length.ToString());

                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(key, Salt);
                ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

                cryptoStream.Write(PlainText, 0, PlainText.Length);
                cryptoStream.FlushFinalBlock();

                CipherBytes = memoryStream.ToArray();

                memoryStream.Close();
                cryptoStream.Close();
            }

            if (CipherBytes != null) return Convert.ToBase64String(CipherBytes);
            else return null;
        }
        #endregion
        #region Decrypt
        public static string DecryptAES128(string Input, string key, Encoding enc = null)
        {
            if (enc == null) enc = Encoding.UTF8;

            byte[] PlainText = null;
            int DecryptedCount = 0;
            using (RijndaelManaged RijndaelCipher = new RijndaelManaged())
            {
                byte[] EncryptedData = Convert.FromBase64String(Input);
                byte[] Salt = enc.GetBytes(key.Length.ToString());

                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(key, Salt);
                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(EncryptedData);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

                PlainText = new byte[EncryptedData.Length];
                DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

                memoryStream.Close();
                cryptoStream.Close();
            }
            if (PlainText != null) return enc.GetString(PlainText, 0, DecryptedCount);
            else return null;
        }
        #endregion
        #endregion
    }
}
