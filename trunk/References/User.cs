using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace ExtraMegaBlob.References
{
    public class User : ISerializable
    {
        public enum UserComponents
        {
            NAME,
            LOCATION,
            ENDPOINT
        }
        public String Name = String.Empty;
        public ExtraMegaBlob.References.Vector3 Location = new ExtraMegaBlob.References.Vector3();
        public string Endpoint = "";

        public User(String Name, ExtraMegaBlob.References.Vector3 Location, String Endpoint)
        {
            this.Name = Name;
            this.Location = Location;
            this.Endpoint = Endpoint;
        }
        //Normal constructor
        public User()
        {
        }
        //Deserialization constructor
        public User(SerializationInfo info, StreamingContext ctxt)
        {
            Name = (String)info.GetValue("Name", typeof(String));
            Location = (ExtraMegaBlob.References.Vector3)info.GetValue("Location", typeof(ExtraMegaBlob.References.Vector3));
            Endpoint = (String)info.GetValue("Endpoint", typeof(String));
        }
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Name", Name);
            info.AddValue("Location", Location);
            info.AddValue("Endpoint", Endpoint);
        }
        public override String ToString()
        {
            Serialize Serialization = new Serialize();
            return Serialization.SerializeObject(this, typeof(User));
        }
        public static User FromString(String XmlString)
        {
            String s = String.Empty;
            return FromString(XmlString, ref s);
        }
        public static User FromString(String XmlString, ref string TargetString)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(User));
            MemoryStream memoryStream = new MemoryStream(Serialize.StringToUTF8ByteArray(XmlString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (User)xs.Deserialize(memoryStream);
        }
    }
}