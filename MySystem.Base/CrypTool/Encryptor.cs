using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CrypTool
{
    public sealed class Encryptor<TBlockCipher, TDigest>
        where TBlockCipher : IBlockCipher, new()
        where TDigest : IDigest, new()
    {
        private readonly Encoding encoding;

        private IBlockCipher blockCipher;

        private BufferedBlockCipher cipher;

        private HMac mac;

        private readonly byte[] key;

        public Encryptor(Encoding encoding, byte[] key, byte[] macKey)
        {
            this.encoding = encoding;
            this.key = key;
            Init(key, macKey);
        }

        private void Init(byte[] key, byte[] macKey)
        {
            blockCipher = new CbcBlockCipher(new TBlockCipher());
            cipher = new BufferedBlockCipher(blockCipher);
            mac = new HMac(new TDigest());
            mac.Init(new KeyParameter(macKey));
        }

        public string Encrypt(string plain)
        {
            return Convert.ToBase64String(EncryptBytes(plain));
        }

        public byte[] EncryptBytes(string plain)
        {
            byte[] input = encoding.GetBytes(plain);

            if ((input.Length % blockCipher.GetBlockSize()) > 0)
            {
                byte[] newResult = new byte[(input.Length + (blockCipher.GetBlockSize() - (input.Length % blockCipher.GetBlockSize())))];
                input.CopyTo(newResult, 0);
                input = newResult;
            }

            byte[] iv = GenerateIV();

            byte[] cipher = BouncyCastleCrypto(true, input, new ParametersWithIV(new KeyParameter(key), iv));
            byte[] message = CombineArrays(iv, cipher);

            mac.Reset();
            mac.BlockUpdate(message, 0, message.Length);
            byte[] digest = new byte[mac.GetUnderlyingDigest().GetDigestSize()];
            mac.DoFinal(digest, 0);

            byte[] result = CombineArrays(digest, message);
            return result;
        }

        public byte[] DecryptBytes(byte[] bytes)
        {
            // split the digest into component parts
            byte[] digest = new byte[mac.GetUnderlyingDigest().GetDigestSize()];
            byte[] message = new byte[bytes.Length - digest.Length];
            byte[] iv = new byte[blockCipher.GetBlockSize()];
            byte[] cipher = new byte[message.Length - iv.Length];

            Buffer.BlockCopy(bytes, 0, digest, 0, digest.Length);
            Buffer.BlockCopy(bytes, digest.Length, message, 0, message.Length);
            if (!IsValidHMac(digest, message))
            {
                throw new CryptoException();
            }

            Buffer.BlockCopy(message, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(message, iv.Length, cipher, 0, cipher.Length);

            byte[] result = BouncyCastleCrypto(false, cipher, new ParametersWithIV(new KeyParameter(key), iv));
            return result;
        }

        public string Decrypt(byte[] bytes)
        {
            return encoding.GetString(DecryptBytes(bytes));
        }

        public string Decrypt(string cipher)
        {
            return Decrypt(Convert.FromBase64String(cipher));
        }

        private bool IsValidHMac(byte[] digest, byte[] message)
        {
            mac.Reset();
            mac.BlockUpdate(message, 0, message.Length);
            byte[] computed = new byte[mac.GetUnderlyingDigest().GetDigestSize()];
            mac.DoFinal(computed, 0);
            return computed.SequenceEqual(digest);
        }

        private byte[] BouncyCastleCrypto(bool forEncrypt, byte[] input, ICipherParameters parameters)
        {
            try
            {
                cipher.Init(forEncrypt, parameters);

                return cipher.DoFinal(input);
            }
            catch (CryptoException)
            {
                throw;
            }
        }

        private byte[] GenerateIV()
        {
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                // 1st block
                byte[] result = new byte[blockCipher.GetBlockSize()];
                provider.GetBytes(result);

                return result;
            }
        }

        private static byte[] CombineArrays(byte[] source1, byte[] source2)
        {
            byte[] result = new byte[source1.Length + source2.Length];
            Buffer.BlockCopy(source1, 0, result, 0, source1.Length);
            Buffer.BlockCopy(source2, 0, result, source1.Length, source2.Length);

            return result;
        }
    }
}
