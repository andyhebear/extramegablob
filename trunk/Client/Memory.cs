using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace thing.Parts
{
    //[XmlInclude(typeof(Memory))]
    public class Memory : ISerializable
    {
        public enum MemoryComponents
        {
            NAME,
            KEY,
            VALUE
        }
        public String Name = String.Empty;
        public KeyWord Key = KeyWord.NIL;
        public String Value = String.Empty;
        public Memory(String Name_, KeyWord Key_, String Value_)
        {
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
            Name = (String)info.GetValue("Name", typeof(String));
            Key = (KeyWord)info.GetValue("Key", typeof(KeyWord));
            Value = (String)info.GetValue("Value", typeof(String));
        }
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Name", Name);
            info.AddValue("Key", Key);
            info.AddValue("Value", Value);
        }
        public override String ToString()
        {
            thing.Parts.Serialize Serialization = new thing.Parts.Serialize();
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
            MemoryStream memoryStream = new MemoryStream(thing.Parts.Serialize.StringToUTF8ByteArray(XmlString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (Memory)xs.Deserialize(memoryStream);
        }
    }
}