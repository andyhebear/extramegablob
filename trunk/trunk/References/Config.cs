using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ThingReferences
{
    public class Config
    {
        public static byte[] rawKey { get { return Encoding.GetEncoding(1251).GetBytes(Encryption.md5("mykey")); } }
        public static string networkKey { get { return Encryption.md5("mykey"); } } //change to private system
        public string configKey { get { return Encryption.md5("mykey"); } } //change to private system
        public static string delim { get { return Encryption.md5("arbitrary delimiter"); } }
        public static byte[] rawdelim { get { return Encoding.GetEncoding(1251).GetBytes(Encryption.md5("arbitrary delimiter")); } }
        public string uid { get { return Encryption.md5("arbitrary user id"); } } //for future reference

        public static string NowTimeFormatTop { get { return "{0:0000000000}"; } }
        public static string NowTimeFormatBottom { get { return "{0:.0000000}"; } }
        public static string NowTimeFormatSeparate { get { return "{0:0000000000}{1:.0000000}"; } }
        public static string NowTimeFormatJoined { get { return "{0:0000000000.0000000}"; } }
        public static string logTimeFormat { get { return "yyyy/MM/dd HH:mm:ss.fffffff"; } }
        public static string logPrefix
        {
            get
            {
                return "[" + DateTime.Now.ToString(Config.logTimeFormat) + "] ";
            }
        }

        public string serverip { get { return "0.0.0.0"; } }
        public string serverip_clientcontext { get { return "127.0.0.1"; } }
        public string serverport { get { return "420"; } }


        private Hashtable ConfigContainer = null;
        private XmlDocument XmlDocument = null;
        private Encryption Crypt = null;
        public Config()
        {
            XmlDocument = new XmlDocument();
            ConfigContainer = new Hashtable();
            Crypt = new Encryption();
        }
        public Boolean load(String file_path, String Password)
        {
            //System.IO.FileInfo fileInfo = new System.IO.FileInfo(file_path);
            //if (fileInfo.Length > 0)
            //{
            Password = Encryption.md5(Password);//reduces key length by half (not sure), BUT serves to format the input into a 32 character string (temporary solution)
            FileStream fs = new FileStream(file_path, FileMode.OpenOrCreate, FileAccess.Read);
            if (fs.Length < 1)
            {
                fs.Close();
                return false;
            }
            byte[] b = new byte[fs.Length];
            fs.Read(b, 0, (int)fs.Length);
            fs.Close();
            string s = Serialize.UTF8ByteArrayToString(b);
            s = Crypt.Decrypt(s, Password); if (s == "") return false;
            StringReader sr = new StringReader(s);
            XmlDocument.Load(sr);
            foreach (XmlNode n in XmlDocument.ChildNodes)
            {
                if (n.NodeType == XmlNodeType.Element && n.Name == "root")
                {
                    foreach (XmlNode node in n.ChildNodes)
                    {
                        String setting_name = node.Attributes["name"].Value.ToString();
                        String setting_value = node.Attributes["value"].Value.ToString();
                        ConfigContainer[setting_name] = setting_value;
                    }
                }
            }
            //}
            return true;
        }
        public int get_numkeys()
        {
            return ConfigContainer.Count;
        }
        public Hashtable get_config()
        {
            return ConfigContainer;
        }
        public Boolean set_val(String name, String value)
        {
            ConfigContainer[name] = value;
            return true;
        }
        public String get_val(String name)
        {
            if (!object.Equals(null, ConfigContainer[name]))
                return (String)ConfigContainer[name];
            else return string.Empty;
        }
        public MatchCollection get_val(String name, String RegexPattern)
        {
            if (RegexPattern == String.Empty)
                return null;
            else
            {
                Regex regex = new Regex(RegexPattern);
                return regex.Matches((String)ConfigContainer[name]);
            }
        }
        public Hashtable get_vals()
        {
            return ConfigContainer;
        }
        public ArrayList get_vals(String keySearch)
        {
            ArrayList retVal = new ArrayList();
            if (keySearch == String.Empty)
                return retVal;
            else
            {
                foreach (DictionaryEntry de in ConfigContainer)
                {
                    if (((String)de.Key).IndexOf(keySearch) > -1)
                    {
                        retVal.Add(de);
                    }
                }
            }
            return retVal;
        }
        public Boolean save(String file_path, String Password)
        {
            MemoryStream ms = new MemoryStream();
            XmlTextWriter textWriter = new XmlTextWriter(ms, new UTF8Encoding(false));
            textWriter.Namespaces = false;
            textWriter.Indentation = 2;
            textWriter.IndentChar = '\t';
            textWriter.Formatting = Formatting.Indented;
            textWriter.WriteStartDocument();
            textWriter.WriteComment("Configuration File");
            textWriter.WriteComment("Original Filename: " + System.IO.Path.GetFileName(file_path));
            textWriter.WriteStartElement("root");
            foreach (DictionaryEntry de in ConfigContainer)
            {
                textWriter.WriteStartElement("item");
                textWriter.WriteAttributeString("name", (String)de.Key);
                textWriter.WriteAttributeString("value", (String)de.Value);
                textWriter.WriteEndElement();
            }
            textWriter.WriteEndElement();
            textWriter.WriteEndDocument();
            textWriter.Close();

            byte[] b = ms.GetBuffer();
            string ste = Crypt.Encrypt(Serialize.UTF8ByteArrayToString(b), Password);
            FileStream fs = new FileStream(file_path, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] b2 = Serialize.StringToUTF8ByteArray(ste);
            fs.Write(b2, 0, b2.Length);
            fs.Close();
            return true;
        }
        //Deserialization constructor
        public Config(SerializationInfo info, StreamingContext ctxt)
        {
            ConfigContainer = (Hashtable)info.GetValue("ConfigContainer", typeof(Hashtable));
        }
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("ConfigContainer", ConfigContainer);
        }
        public override String ToString()
        {
            Serialize Serialization = new Serialize();
            return Serialization.SerializeObject(this, typeof(Config));
        }
        public static Config FromString(String XmlString)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Config));
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(Serialize.StringToUTF8ByteArray(XmlString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (Config)xs.Deserialize(memoryStream);
        }
    }
}
