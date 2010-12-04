using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ExtraMegaBlob.References
{
    [XmlInclude(typeof(Memories))]
    public class Event : ISerializable
    {
        public Double _WhenSent = Time.Now;
        public eventScope _IntendedRecipients = eventScope.NIL;
        public Double _WhenRcvd = Time.Now;
        public KeyWord _Importance = KeyWord.IMPORTANCE_4_TRASH;
        public Memories _Memories = null;
        public KeyWord _Keyword = KeyWord.NIL;
        public KeyWord _DeliveryState = KeyWord.NIL;
        public String _Source_FullyQualifiedName = String.Empty;
        public String _Endpoint = String.Empty;
        public Event() { }
        internal Event(String Endpoint, String Source_FullyQualifiedName, KeyWord KeyWord, Memories Memories, Double WhenSent, Double WhenRcvd, KeyWord Importance, KeyWord DeliveryState, eventScope IntendedRecipients)
        {
            _Endpoint = Endpoint;
            _Source_FullyQualifiedName = Source_FullyQualifiedName;
            _DeliveryState = DeliveryState;
            _Importance = Importance;
            _Keyword = KeyWord;
            _Memories = Memories;
            _WhenSent = WhenSent;
            _WhenRcvd = WhenRcvd;
            _IntendedRecipients = IntendedRecipients;
        }
        internal Event(String Source_FullyQualifiedName, KeyWord KeyWord, Memories Memories, Double WhenSent, Double WhenRcvd, KeyWord Importance, KeyWord DeliveryState, eventScope IntendedRecipients)
        {
            _Source_FullyQualifiedName = Source_FullyQualifiedName;
            _DeliveryState = DeliveryState;
            _Importance = Importance;
            _Keyword = KeyWord;
            _Memories = Memories;
            _WhenSent = WhenSent;
            _WhenRcvd = WhenRcvd;
            _IntendedRecipients = IntendedRecipients;
        }
        public Event(SerializationInfo info, StreamingContext ctxt)
        {
            _IntendedRecipients = (eventScope)info.GetValue("_IntendedRecipients", typeof(eventScope));
            _WhenSent = (Double)info.GetValue("_WhenSent", typeof(Double));
            _WhenRcvd = (Double)info.GetValue("_WhenRcvd", typeof(Double));
            _Importance = (KeyWord)info.GetValue("_Importance", typeof(KeyWord));
            _Memories = (Memories)info.GetValue("_Memories", typeof(Memories));
            _Keyword = (KeyWord)info.GetValue("_Keyword", typeof(KeyWord));
            _DeliveryState = (KeyWord)info.GetValue("_DeliveryState", typeof(KeyWord));
            _Source_FullyQualifiedName = (String)info.GetValue("_Source_FullyQualifiedName", typeof(String));
            _Endpoint = (String)info.GetValue("_Endpoint", typeof(String));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("_Endpoint", _Endpoint);
            info.AddValue("_WhenSent", _WhenSent);
            info.AddValue("_WhenRcvd", _WhenRcvd);
            info.AddValue("_Importance", _Importance);
            info.AddValue("_Memories", _Memories);
            info.AddValue("_Keyword", _Keyword);
            info.AddValue("_DeliveryState", _DeliveryState);
            info.AddValue("_Source_FullyQualifiedName", _Source_FullyQualifiedName);
        }
        public override String ToString()
        {
            return Serialize.StaticallySerializeObject(this, typeof(Event));
        }
        public byte[] ToBytes()
        {
            return UTF8Encoding.UTF8.GetBytes(this.ToString());
        }
        public static Event FromBytes(byte[] XmlStringBytes)
        {
            return FromString(UTF8Encoding.UTF8.GetString(XmlStringBytes));
        }
        public static Event FromString(String XmlString)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Event));
            MemoryStream memoryStream = new MemoryStream(Serialize.StringToUTF8ByteArray(XmlString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (Event)xs.Deserialize(memoryStream);
        }
    }
}