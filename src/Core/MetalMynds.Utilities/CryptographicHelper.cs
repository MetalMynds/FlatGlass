using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace MetalMynds.Utilities
{
    public class CryptographicHelper
    {
        public const String SignCryptoServiceProvider = "SHA1";

        public static String Sign(String Data)
        {
            return Convert.ToBase64String(GenerateSignature(UTF8Encoding.UTF8.GetBytes(Data)));
        }

        public static String Sign(Byte[] Data)
        {
            return Convert.ToBase64String(GenerateSignature(Data));
        }

        public static Byte[] GenerateSignature(Byte[] Data)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            return sha.ComputeHash(Data);
        }

        public static Boolean TryEncrypt(String PublicXmlKey, String PlainValue, out String Encrypted)
        {
            try
            {
                Encrypted = Encrypt(PublicXmlKey, PlainValue);
                return true;
            }
            catch
            {
                Encrypted = null;
                return false;
            }
        }

        public static Boolean TryDecrypt(String PublicXmlKey, String Encrypted, out String PlainValue)
        {
            try
            {
                PlainValue = Decrypt(PublicXmlKey, Encrypted);
                return true;
            }
            catch
            {
                PlainValue = null;
                return false;
            }
        }

        public static String Encrypt(String PublicXmlKey, String PlainValue)
        {
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            string plainText = String.Empty;
            byte[] plainBytes = null;
            byte[] encryptedBytes = null;
            string encryptedText = string.Empty;

            try
            {
                cspParams = new CspParameters();
                cspParams.ProviderType = 1; // PROV_RSA_FULL
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                rsaProvider.FromXmlString(PublicXmlKey);

                plainBytes = Encoding.ASCII.GetBytes(PlainValue);

                encryptedBytes = rsaProvider.Encrypt(plainBytes, false);

                //MemoryStream stream = new MemoryStream(encryptedBytes);

                //StreamReader reader = new StreamReader(stream);

                //encryptedText = reader.ReadToEnd();

                encryptedText = Convert.ToBase64String(encryptedBytes);

                clean(ref plainBytes);
                //clean(ref plainText);

                //return ASCIIEncoding.ASCII.GetString(encryptedBytes);
                return encryptedText;
            }
            catch (Exception ex)
            {
                throw new EncryptionFailedException(ex);
            }
        }

        internal static Char randomChar()
        {
            byte rnd = (byte)new Random().Next((int)char.MinValue.GetTypeCode(), (int)char.MaxValue.GetTypeCode());
            return Convert.ToChar(rnd);
        }

        internal static void clean(ref String Value)
        {
            char chr = randomChar();
            Value = String.Empty.PadLeft(Value.Length, chr);
        }

        internal static String clean(String Value)
        {
            char chr = randomChar();
            return String.Empty.PadLeft(Value.Length, chr);
        }

        internal static void clean(ref Byte[] Data)
        {
            for (int count = 0; count <= Data.GetUpperBound(0); count++)
            {
                Data[count] = (byte)new Random().Next((int)Byte.MinValue, (int)byte.MaxValue);
            }
        }

        public static void Clean(ref String Value)
        {
            clean(ref Value);
        }

        public static String Clean(String Value)
        {
            return clean(Value);
        }

        public static void Clean(ref Byte[] Data)
        {
            clean(ref Data);
        }

        public static String Decrypt(String PrivateXmlKey, String EncryptedValue)
        {
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            string privateKeyText = String.Empty;
            string plainText = String.Empty;
            byte[] encryptedBytes = null;

            try
            {
                cspParams = new CspParameters();
                cspParams.ProviderType = 1; // PROV_RSA_FULL
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                rsaProvider.FromXmlString(PrivateXmlKey);

                encryptedBytes = new byte[EncryptedValue.Length];

                encryptedBytes = Convert.FromBase64String(EncryptedValue);

                return ASCIIEncoding.ASCII.GetString(rsaProvider.Decrypt(encryptedBytes, false));
            }
            catch (Exception ex)
            {
                throw new DecryptionFailedException(ex);
            }
        }

        public static String CalculateMD5(Byte[] Data)
        {
            MD5CryptoServiceProvider provider = new System.Security.Cryptography.MD5CryptoServiceProvider();

            Data = provider.ComputeHash(Data);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte abyte in Data)
            {
                s.Append(abyte.ToString("x2").ToLower());
            }
            return s.ToString();
        }

        public static String CalculateMD5(String Filename)
        {
            byte[] data;

            data = File.ReadAllBytes(Filename);

            return CalculateMD5(data);
        }

        public static String CalculateMD5(Assembly Binary)
        {
            return CalculateMD5(Binary.Location);
        }
    } // Decrypt

    public class EncryptionFailedException : Exception
    {
        public EncryptionFailedException(Exception InnerException)
            : base("Encryption Failed!", InnerException)
        {
        }
    }

    public class DecryptionFailedException : Exception
    {
        public DecryptionFailedException(Exception InnerException)
            : base("Decryption Failed!", InnerException)
        {
        }
    }
}