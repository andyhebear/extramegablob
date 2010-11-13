using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace ServerThing
{
    class Encryption
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
        private String decryptStringFromBytes_AES(byte[] cipherText, byte[] Key, byte[] IV)
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
                decrypted = decryptStringFromBytes_AES(Convert.FromBase64String(enc_split[1]), str_to_bytes(Key32), Convert.FromBase64String(enc_split[0]));
            }
            catch (Exception)
            { }

            return decrypted;
        }
        private static byte[] str_to_bytes(String str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }
        public string sha256(string inputString)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] data = encoding.GetBytes(inputString);
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
        public string md5(string input)
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
    }
}
