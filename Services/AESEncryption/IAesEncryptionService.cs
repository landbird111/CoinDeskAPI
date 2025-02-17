namespace CoinDeskAPI.Services.AESEncryption;

// 定義介面 IAesEncryptionService
public interface IAesEncryptionService
{
    /// <summary>
    /// 使用 AES 加密演算法加密明文
    /// </summary>
    /// <param name="plainText">要加密的明文</param>
    /// <returns>加密後的字串</returns>
    string Encrypt(string plainText);

    /// <summary>
    /// 使用 AES 加密演算法解密密文
    /// </summary>
    /// <param name="cipherText">要解密的密文</param>
    /// <returns>解密後的字串</returns>
    string Decrypt(string cipherText);
}
