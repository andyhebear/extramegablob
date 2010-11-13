using System;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;

namespace ThingReferences
{
    public delegate void LogDelegate(string msg);
    public static class Helpers
    {
        public static bool isClientPlugin(string path)
        {
            string x = Path.GetFileName(path).ToLower();
            if (x.Contains("_client"))
                return true;
            else return false;
        }
        public static bool isServerPlugin(string path)
        {
            string x = Path.GetFileName(path).ToLower();
            if (x.Contains("_server"))
                return true;
            else return false;
        }
        public static bool isPluginFile(string path)
        {
            switch (Path.GetExtension(path).ToLower())
            {
                case ".cs":
                    return true;
                case ".vb":
                    return true;
                case ".js":
                    return true;
                default:
                    return false;
            }
        }
        public static bool isMeshFile(string path)
        {
            switch (Path.GetExtension(path).ToLower())
            {
                case ".mesh":
                    return true;
                default:
                    return false;
            }
        }
        public static bool isTextureFile(string path)
        {
            switch (Path.GetExtension(path).ToLower())
            {
                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;
                case ".png":
                    return true;
                case ".gif":
                    return true;
                case ".tga":
                    return true;
                case ".bmp":
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        /// Compresses byte array using gzip
        /// Compressed data is returned as a byte array
        /// </summary>
        /// <param name="inBytes">The bytes to compress</param>
        public static byte[] compress_gzip(byte[] inBytes)
        {
            MemoryStream ContentsGzippedStream = new MemoryStream();	//create the memory stream to hold the compressed file
            Stream s = new GZipOutputStream(ContentsGzippedStream);		//create the gzip filter
            s.Write(inBytes, 0, inBytes.Length);				        //write the file contents to the filter
            s.Flush();													//make sure everythings ready
            s.Close();													//close and write the compressed data to the memory stream
            return ContentsGzippedStream.ToArray();
        }
        /// <summary>
        /// Decompresses gzip compressed byte array
        /// Decompressed data is returned as a byte array
        /// </summary>
        /// <param name="inBytes">The compressed bytes</param>
        public static byte[] decompress_gzip(byte[] inBytes)
        {
            MemoryStream baseInputStream = new MemoryStream();
            GZipInputStream i = new GZipInputStream(baseInputStream);
            baseInputStream.Write(inBytes, 0, inBytes.Length);
            baseInputStream.Flush();
            baseInputStream.Seek(0, 0);
            MemoryStream outStream = new MemoryStream();
            byte[] buf = new byte[1024];
            int len;
            while ((len = i.Read(buf, 0, 1024)) > 0)
            {
                outStream.Write(buf, 0, len);
            }
            baseInputStream.Close();
            return outStream.ToArray();
        }
        /// <summary>
        /// Reads data from a file.
        /// Data is returned as a byte array
        /// </summary>
        /// <param name="pathFile">The full path to the file</param>
        public static byte[] getFileBytes(string pathFile)
        {
            FileInfo fi = new FileInfo(pathFile);
            FileStream fs = new FileStream(pathFile, FileMode.Open);
            byte[] outbytes = ReadFully(fs, (int)fi.Length);
            fs.Close();
            return outbytes;
        }
        /// <summary>
        /// Reads data from a stream until the end is reached. The
        /// data is returned as a byte array. An IOException is
        /// thrown if any of the underlying IO calls fail.
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <param name="initialLength">The initial buffer length</param>
        public static byte[] ReadFully(Stream stream, int initialLength)
        {
            // If we've been passed an unhelpful initial length, just
            // use 32K.
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If we've reached the end of our buffer, check to see if there's
                // any more information
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // End of stream? If so, we're done
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Nope. Resize the buffer, put in the byte we've just
                    // read, and continue
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }
            // Buffer is now too big. Shrink it.
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }
        public static string toBase64_UTF8(string s)
        {
            return Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(s));
        }
        public static string toBase64(byte[] b)
        {
            return Convert.ToBase64String(b);
        }
        public static string fromBase64_UTF8(string s)
        {
            return System.Text.UTF8Encoding.UTF8.GetString(Convert.FromBase64String(s));
        }
        public static String AllowedChars { get { return "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_ "; } }
        public static string RandomString(Int32 NumSpaces, ref Random ran)
        {
            char[] AllowedChars = Helpers.AllowedChars.ToCharArray();
            System.Text.StringBuilder StringBuilder = new StringBuilder(NumSpaces, NumSpaces);
            for (Int32 i = 0; i < NumSpaces; i++)
            {
                StringBuilder.Append(AllowedChars[ran.Next(0, AllowedChars.Length)]);
            }
            return StringBuilder.ToString();
        }
        public static int RandomInt(int min, int max, ref Random ran)
        {
            return (ran.Next(min, max));
        }
        static public string cleanString(string s)
        {
            return cleanString(s, null);
        }
        static public string cleanString(string s, string replacechar)
        {
            if (s != null && s.Length > 0)
            {
                StringBuilder sb = new StringBuilder(s.Length);
                foreach (char c in s)
                {
                    if (Char.IsControl(c) && c != '\n' && c != '\r' && c != '\t')
                    {
                        if (!object.Equals(null, replacechar))
                        {
                            if (replacechar != "")
                            {
                                sb.Append(replacechar.ToCharArray()[0]);
                            }
                        }
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                s = sb.ToString();
            }
            return s;
        }
    }
}
