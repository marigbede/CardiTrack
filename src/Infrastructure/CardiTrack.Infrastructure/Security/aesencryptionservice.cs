using System.Security.Cryptography;
using System.Text;

namespace CardiTrack.Infrastructure.Security;

/// <summary>
/// AES-256 encryption service for sensitive data (OAuth tokens, medical notes)
/// HIPAA Compliant - Uses AES-256-GCM for authenticated encryption
/// </summary>
public class AesEncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    private const int NonceSize = 12; // 96 bits for GCM
    private const int TagSize = 16;   // 128 bits authentication tag

    public AesEncryptionService(string base64Key)
    {
        if (string.IsNullOrWhiteSpace(base64Key))
            throw new ArgumentException("Encryption key cannot be null or empty", nameof(base64Key));

        _key = Convert.FromBase64String(base64Key);

        if (_key.Length != 32) // 256 bits
            throw new ArgumentException("Key must be 256 bits (32 bytes)", nameof(base64Key));
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
            return plainText;

        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var cipherBytes = EncryptBytes(plainBytes);
        return Convert.ToBase64String(cipherBytes);
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
            return cipherText ?? string.Empty;

        var cipherBytes = Convert.FromBase64String(cipherText);
        var plainBytes = DecryptBytes(cipherBytes);
        return Encoding.UTF8.GetString(plainBytes);
    }

    public byte[] EncryptBytes(byte[] plainBytes)
    {
        if (plainBytes == null || plainBytes.Length == 0)
            return plainBytes ?? Array.Empty<byte>();

        // Generate random nonce
        var nonce = new byte[NonceSize];
        RandomNumberGenerator.Fill(nonce);

        // Encrypt with AES-GCM
        var tag = new byte[TagSize];
        var cipherBytes = new byte[plainBytes.Length];

        using var aesGcm = new AesGcm(_key, TagSize);
        aesGcm.Encrypt(nonce, plainBytes, cipherBytes, tag);

        // Combine: nonce + tag + ciphertext
        var result = new byte[NonceSize + TagSize + cipherBytes.Length];
        Buffer.BlockCopy(nonce, 0, result, 0, NonceSize);
        Buffer.BlockCopy(tag, 0, result, NonceSize, TagSize);
        Buffer.BlockCopy(cipherBytes, 0, result, NonceSize + TagSize, cipherBytes.Length);

        return result;
    }

    public byte[] DecryptBytes(byte[] cipherBytes)
    {
        if (cipherBytes == null || cipherBytes.Length == 0)
            return cipherBytes ?? Array.Empty<byte>();

        if (cipherBytes.Length < NonceSize + TagSize)
            throw new ArgumentException("Invalid cipher text", nameof(cipherBytes));

        // Extract nonce, tag, and ciphertext
        var nonce = new byte[NonceSize];
        var tag = new byte[TagSize];
        var encrypted = new byte[cipherBytes.Length - NonceSize - TagSize];

        Buffer.BlockCopy(cipherBytes, 0, nonce, 0, NonceSize);
        Buffer.BlockCopy(cipherBytes, NonceSize, tag, 0, TagSize);
        Buffer.BlockCopy(cipherBytes, NonceSize + TagSize, encrypted, 0, encrypted.Length);

        // Decrypt with AES-GCM
        var plainBytes = new byte[encrypted.Length];

        using var aesGcm = new AesGcm(_key, TagSize);
        aesGcm.Decrypt(nonce, encrypted, tag, plainBytes);

        return plainBytes;
    }

    /// <summary>
    /// Generates a new 256-bit encryption key
    /// </summary>
    public static string GenerateKey()
    {
        var key = new byte[32]; // 256 bits
        RandomNumberGenerator.Fill(key);
        return Convert.ToBase64String(key);
    }
}
