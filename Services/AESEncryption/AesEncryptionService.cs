using System.Security.Cryptography;
using System.Text;

namespace CoinDeskAPI.Services.AESEncryption
{
    public class AesEncryptionService : IAesEncryptionService
    {
        private readonly string _key = "QBgrWMfVO93uKON7RZT93YKEqVJT1qX7";

        /// <summary>
        /// 使用 AES 加密演算法加密明文
        /// </summary>
        /// <param name="plainText">要加密的明文</param>
        /// <returns>加密後的字串</returns>
        public string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(_key);
                aesAlg.IV = new byte[16]; // 初始化向量為零

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        /// <summary>
        /// 使用 AES 加密演算法解密密文
        /// </summary>
        /// <param name="cipherText">要解密的密文</param>
        /// <returns>解密後的字串</returns>
        public string Decrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(_key);
                aesAlg.IV = new byte[16]; // 初始化向量為零

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}