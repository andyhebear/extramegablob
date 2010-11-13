using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace ThingReferences
{
    //[XmlInclude(typeof(Memory))]
    public class Memory : ISerializable
    {
        public enum MemoryComponents
        {
            NAME,
            KEY,
            BYTES,
            VALUE
        }
        public byte[] Bytes = new byte[0];
        public String Name = String.Empty;
        public KeyWord Key = KeyWord.NIL;
        public String Value = String.Empty;
        public Memory(String Name_, KeyWord Key_, String Value_, byte[] Bytes_)
        {
            Bytes = Bytes_;
            Name = Name_;
            Key = Key_;
            Value = Value_;
        }
        public Memory(String Name_, KeyWord Key_, String Value_)
        {
            Bytes = null;
            Name = Name_;
            Key = Key_;
            Value = Value_;
        }
        //Normal constructor
        public Memory()
        {
        }
        //Deserialization constructor
        public Memory(SerializationInfo info, StreamingContext ctxt)
        {
            Bytes = (byte[])info.GetValue("Bytes", typeof(byte[]));
            Name = (String)info.GetValue("Name", typeof(String));
            Key = (KeyWord)info.GetValue("Key", typeof(KeyWord));
            Value = (String)info.GetValue("Value", typeof(String));
        }
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Bytes", (Bytes));
            info.AddValue("Bytes", Bytes);
            info.AddValue("Name", Name);
            info.AddValue("Key", Key);
            info.AddValue("Value", Value);
        }
        public override String ToString()
        {
            Serialize Serialization = new Serialize();
            return Serialization.SerializeObject(this, typeof(Memory));
        }
        public static Memory FromString(String XmlString)
        {
            String s = String.Empty;
            return FromString(XmlString, ref s);
        }
        public static Memory FromString(String XmlString, ref string TargetString)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Memory));
            MemoryStream memoryStream = new MemoryStream(Serialize.StringToUTF8ByteArray(XmlString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (Memory)xs.Deserialize(memoryStream);
        }
    }
}