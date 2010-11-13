using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace ThingReferences
{
    public class Encryption
    {
        private static byte[] encryptStringToBytes_AES(String plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Name");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Name");
            // Declare the streams used
            // to encrypt to an in memory
            // array of bytes.
            MemoryStream msEncrypt = null;
            CryptoStream csEncrypt = null;
            StreamWriter swEncrypt = null;
            // Declare the RijndaelManaged object
            // used to encrypt the data.
            RijndaelManaged aesAlg = null;
            // Declare the bytes used to hold the
            // encrypted data.
            //byte[] encrypted = null;
            try
            {
                // Create a RijndaelManaged object
                // with the specified SessionID and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();
                csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                swEncrypt = new StreamWriter(csEncrypt);
                //Write all data to the stream.
                swEncrypt.Write(plainText);
            }
            finally
            {
                // Clean things up.
                // Close the streams.
                if (swEncrypt != null)
                    swEncrypt.Close();
                if (csEncrypt != null)
                    csEncrypt.Close();
                if (msEncrypt != null)
                    msEncrypt.Close();
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            // Return the encrypted bytes from the memory stream.
            return msEncrypt.ToArray();
        }
        private static String decryptStringFromBytes_AES_tostring(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Name");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Name");
            // TDeclare the streams used
            // to decrypt to an in memory
            // array of bytes.
            MemoryStream msDecrypt = null;
            CryptoStream csDecrypt = null;
            StreamReader srDecrypt = null;
            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;
            // Declare the String used to hold
            // the decrypted text.
            String plaintext = null;
            try
            {
                // Create a RijndaelManaged object
                // with the specified SessionID and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.
                msDecrypt = new MemoryStream(cipherText);
                csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                srDecrypt = new StreamReader(csDecrypt);
                // Read the decrypted bytes from the decrypting stream
                // and place them in a String.
                plaintext = srDecrypt.ReadToEnd();
            }
            finally
            {
                // Clean things up.
                // Close the streams.
                if (srDecrypt != null)
                    srDecrypt.Close();
                if (csDecrypt != null)
                    csDecrypt.Close();
                if (msDecrypt != null)
                    msDecrypt.Close();
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return plaintext;
        }
        public String Encrypt(String StringToEncrypt, String Key32)
        {
            if (Key32.Length != 32) return String.Empty;
            if (StringToEncrypt == String.Empty) return String.Empty;
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.Key = str_to_bytes(Key32);
            myRijndael.GenerateIV();
            String encrypted = Convert.ToBase64String(encryptStringToBytes_AES(StringToEncrypt, myRijndael.Key, myRijndael.IV), Base64FormattingOptions.None);
            String iv = Convert.ToBase64String(myRijndael.IV, Base64FormattingOptions.None);
            return iv + "|" + encrypted;
        }
        public String Decrypt(String StringToDecrypt, String Key32)
        {
            String decrypted = String.Empty;
            try
            {
                if (StringToDecrypt == String.Empty) return String.Empty;
                String[] enc_split = StringToDecrypt.Split("|".ToCharArray());
                decrypted = decryptStringFromBytes_AES_tostring(Convert.FromBase64String(enc_split[1]), str_to_bytes(Key32), Convert.FromBase64String(enc_split[0]));
            }
            catch (Exception)
            { }
            return decrypted;
        }
        private static byte[] str_to_bytes(String str)
        {
            return Encoding.GetEncoding(1251).GetBytes(str);
        }
        public string sha256(string inputString)
        {
            byte[] data = Encoding.GetEncoding(1251).GetBytes(inputString);
            byte[] result;
            SHA256 shaM = new SHA256Managed();
            result = shaM.ComputeHash(data);
            StringBuilder sb = new StringBuilder(result.Length * 2);
            foreach (byte b in result)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
            //return result.tos;
        }
        public static string md5(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();
        }
        public static string md5_file(string path_file)
        {
            StringBuilder sb = new StringBuilder();
            FileStream fs = new FileStream(path_file, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(fs);
            fs.Close();
            foreach (byte hex in hash)
                sb.Append(hex.ToString("x2"));
            return sb.ToString();
        }
    }
    public static class Encryption2
    {
        private static byte[] encrypt_AES(byte[] data, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (Key.Length != 32)
                throw new ArgumentNullException("Key");
            if (data.Length < 1)
                throw new ArgumentNullException("data");
            if (IV.Length != 16)
                throw new ArgumentNullException("IV");
            // Declare the streams used
            // to encrypt to an in memory
            // array of bytes.
            MemoryStream msEncrypt = null;
            CryptoStream csEncrypt = null;
            // Declare the RijndaelManaged object
            // used to encrypt the data.
            RijndaelManaged aesAlg = null;
            // Declare the bytes used to hold the
            // encrypted data.
            //byte[] encrypted = null;
            try
            {
                // Create a RijndaelManaged object
                // with the specified SessionID and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();
                csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                //Write all data to the stream.
                csEncrypt.Write(data, 0, data.Length);
                csEncrypt.FlushFinalBlock();
            }
            finally
            {
                // Clean things up.
                // Close the streams.
                if (csEncrypt != null)
                    csEncrypt.Close();
                if (msEncrypt != null)
                    msEncrypt.Close();
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            // Return the encrypted bytes from the memory stream.
            return msEncrypt.ToArray();
        }
        private static byte[] decrypt_AES(byte[] data, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (Key.Length != 32)
                throw new ArgumentNullException("Key");
            if (data.Length < 1)
                throw new ArgumentNullException("cipherText");
            if (IV.Length != 16)
                throw new ArgumentNullException("IV");
            // TDeclare the streams used
            // to decrypt to an in memory
            // array of bytes.
            MemoryStream msDecrypt = null;
            CryptoStream csDecrypt = null;
            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;
            // the decrypted data.
            byte[] clear = null;
            try
            {
                // Create a RijndaelManaged object
                // with the specified SessionID and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.
                msDecrypt = new MemoryStream(data);
                csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                // Read the decrypted bytes from the decrypting stream
                clear = new byte[data.Length];
                csDecrypt.Read(clear, 0, data.Length);
            }
            finally
            {
                // Clean things up.
                // Close the streams.
                if (csDecrypt != null)
                    csDecrypt.Close();
                if (msDecrypt != null)
                    msDecrypt.Close();
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return clear;
        }
        public static byte[] Encrypt(byte[] data, byte[] Key32)
        {
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.Key = Key32;
            myRijndael.GenerateIV();
            byte[] encrypted = encrypt_AES(data, myRijndael.Key, myRijndael.IV);
            byte[] package = new byte[encrypted.Length + 16];
            Buffer.BlockCopy(myRijndael.IV, 0, package, 0, 16);
            Buffer.BlockCopy(encrypted, 0, package, 16, encrypted.Length);
            return package;
        }
        public static byte[] Decrypt(byte[] encrypted, byte[] Key32)
        {
            try
            {
                byte[] IV = new byte[16];
                byte[] data = new byte[encrypted.Length - 16];
                Buffer.BlockCopy(encrypted, 0, IV, 0, 16);
                Buffer.BlockCopy(encrypted, 16, data, 0, encrypted.Length - 16);
                return decrypt_AES(data, Config.rawKey, IV);
            }
            catch { }
            return null;
        }
    }
}
