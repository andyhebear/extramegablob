using System;
using System.IO;
using System.Text;
using System.Xml;

namespace ExtraMegaBlob.References
{
    public class Serialize
    {
        public String SerializeObject(Object pObject, Type Type_)
        {
            String XmlizedString = null;
            MemoryStream memoryStream = new MemoryStream();
            System.Xml.Serialization.XmlSerializer xs = null;
            xs = new System.Xml.Serialization.XmlSerializer(pObject.GetType());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("\t");
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.CheckCharacters = true;
            settings.Encoding = new UTF8Encoding(false, false);
            settings.NewLineChars = Environment.NewLine;
            settings.NewLineHandling = NewLineHandling.Replace;
            settings.OmitXmlDeclaration = true;
            XmlWriter Writer = XmlWriter.Create(memoryStream, settings);
            xs.Serialize(Writer, pObject);
            Writer.Flush();
            XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
            return XmlizedString;
        }
        public static String StaticallySerializeObject(Object pObject, Type Type_)
        {
            String XmlizedString = null;
            MemoryStream memoryStream = new MemoryStream();
            System.Xml.Serialization.XmlSerializer xs = null;
            //XmlSerializer[] xs2 = null;
            //xs2 = XmlSerializer.FromTypes(new Type[] { pObject.GetType() });
            //xs = xs2[0];
            xs = new System.Xml.Serialization.XmlSerializer(pObject.GetType());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("\t");
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.CheckCharacters = true;
            settings.Encoding = new UTF8Encoding(false, false);
            settings.NewLineChars = Environment.NewLine;
            settings.NewLineHandling = NewLineHandling.Replace;
            settings.OmitXmlDeclaration = true;
            XmlWriter Writer = XmlWriter.Create(memoryStream, settings);
            xs.Serialize(Writer, pObject);
            Writer.Flush();
            XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
            return XmlizedString;
        }
        public static String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }
        public static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
    }
}
