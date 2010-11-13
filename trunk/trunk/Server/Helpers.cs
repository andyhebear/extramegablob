using System;
using System.Collections.Generic;
using System.Text;

namespace ServerThing
{
    static class Helpers
    {
        public static string toBase64_UTF8(string s)
        {
            return Convert.ToBase64String(System.Text.UTF8Encoding.UTF8.GetBytes(s));
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
