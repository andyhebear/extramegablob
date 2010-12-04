using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ExtraMegaBlob.References
{
    [XmlInclude(typeof(User))]
    public sealed class Users : IEnumerable, ISerializable
    {
        public override String ToString()
        {
            Serialize Serialization = new Serialize();
            return Serialization.SerializeObject(this, typeof(Users));
        }
        public static Users FromString(String XmlString)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Users));
            MemoryStream memoryStream = new MemoryStream(Serialize.StringToUTF8ByteArray(XmlString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (Users)xs.Deserialize(memoryStream);
        }
        private ArrayList allUsers = new ArrayList();
        public Users(SerializationInfo info, StreamingContext ctxt)
        {
            allUsers = (ArrayList)info.GetValue("allUsers", typeof(ArrayList));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("allUsers", allUsers);
        }
        public Boolean Contains(string Endpoint)
        {
            foreach (User usr in allUsers)
            {
                if (usr.Endpoint == Endpoint)
                {
                    return true;
                }
            }
            return false;
        }
        public User this[int index]
        {
            get
            {
                return (User)allUsers[index];
            }
            set
            {
                allUsers[index] = value;
            }
        }
        public User this[string Endpoint]
        {
            get
            {
                int i = IndexOf(Endpoint);
                if (i > -1)
                    return (User)allUsers[i];
                else
                    return null;
            }
            set
            {
                value.Endpoint = Endpoint;
                int i = IndexOf(Endpoint);
                if (i > -1)
                {
                    allUsers[i] = value;
                }
                else
                {
                    this.Add(value);
                }

            }
        }
        public Users()
        {
            allUsers = new ArrayList();
        }
        public int Add(User a)
        {
            lock (allUsers)
            {
                return allUsers.Add(a);
            }
        }
        public int Add(System.Object o)
        {
            lock (allUsers)
            {
                return allUsers.Add(((User)o));
            }
        }
        public int IndexOf(String Endpoint)
        {
            for (int i = 0; i < allUsers.Count; i++)
            {
                if (((User)allUsers[i]).Endpoint == Endpoint)
                {
                    return i;
                }
            }
            return -1;
        }
        public void RemoveAt(int i)
        {
            lock (allUsers)
            {
                allUsers.RemoveAt(i);
            }
        }
        public Boolean nameExists(String Name)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (((User)allUsers[i]).Name == Name)
                {
                    return true;
                }
            }
            return false;
        }
        public int Count
        {
            get
            {
                return allUsers.Count;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new UserEnumerator(allUsers);
        }
        public class UserEnumerator : IEnumerator
        {
            private int cursor = -1;
            private ArrayList _Users = null;
            public UserEnumerator(ArrayList Users)
            {
                _Users = Users;
            }
            public bool MoveNext()
            {
                cursor++;
                return (cursor < _Users.Count);
            }
            public void Reset()
            {
                cursor = -1;
            }
            public object Current
            {
                get
                {
                    try
                    {
                        return _Users[cursor];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException("Index out of bounds.");
                    }
                }
            }
        }
    }
}