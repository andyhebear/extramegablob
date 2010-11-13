using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ThingReferences;

namespace ServerThing
{
    [XmlInclude(typeof(Memory))]
    public sealed class ServerPlugins : IEnumerable, ISerializable
    {
        public override String ToString()
        {
            Serialize Serialization = new Serialize();
            return Serialization.SerializeObject(this, typeof(ServerPlugins));
        }
        public static ServerPlugins FromString(String XmlString)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(ServerPlugins));
            MemoryStream memoryStream = new MemoryStream(Serialize.StringToUTF8ByteArray(XmlString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (ServerPlugins)xs.Deserialize(memoryStream);
        }
        private ArrayList allPlugins = new ArrayList();
        //Deserialization constructor
        public ServerPlugins(SerializationInfo info, StreamingContext ctxt)
        {
            allPlugins = (ArrayList)info.GetValue("allRooms", typeof(ArrayList));
        }
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("allRooms", allPlugins);
        }
        public Boolean Contains(object a)
        {
            foreach (ServerPlugin mem in allPlugins)
            {
                if (mem.Name() == ((ServerPlugin)a).Name())
                {
                    return true;
                }
            }
            return false;
        }
        public static ServerPlugins operator +(ServerPlugins m1, ServerPlugins m2)
        {
            ServerPlugins al = m1;
            foreach (ServerPlugin mem in m2)
            {
                if (!al.Contains(mem))
                {
                    al.Add(mem);
                }
            }
            return al;
        }
        public object this[int index]
        {
            get
            {
                return (ServerPlugin)allPlugins[index];
            }
            set
            {
                allPlugins[index] = value;
            }
        }
        public object this[String key]
        {
            get
            {
                int i = IndexOf(key);
                if (i < 0) return null;
                return (ServerPlugin)allPlugins[i];
            }
            set
            {
                int i = IndexOf(key);
                if (i > -1)
                    allPlugins[i] = value;
                else
                {
                    allPlugins.Add(value);
                }
            }
        }
        //Normal Constructor
        public ServerPlugins()
        {
            allPlugins = new ArrayList();
        }
        public int Add(System.Object o)
        {
            lock (allPlugins)
            {
                return allPlugins.Add(((ServerPlugin)o));
            }
        }
        public int IndexOf(String Name)
        {
            for (int i = 0; i < allPlugins.Count; i++)
            {
                if (((ServerPlugin)allPlugins[i]).Name() == Name)
                {
                    return i;
                }
            }
            return -1;
        }
        internal void UpdateHooks()
        {
            for (int i = 0; i < allPlugins.Count; i++)
            {
                if (!object.Equals(null, allPlugins[i]))
                {
                    ((ServerPlugin)allPlugins[i]).updateHook();
                }
            }
        }
        internal void Shutdown()
        {
            for (int i = 0; i < allPlugins.Count; i++)
            {
                if (!object.Equals(null, allPlugins[i]))
                {
                    ((ServerPlugin)allPlugins[i]).shutdown();
                }
            }
        }
        //public int IndexOf(KeyWord KeyWord)
        //{
        //    for (int i = 0; i < allRooms.Count; i++)
        //    {
        //        if (((Memory)allRooms[i]).Key == KeyWord)
        //        {
        //            return i;
        //        }
        //    }
        //    return -1;
        //}
        public void RemoveAt(String Name)
        {
            int i = IndexOf(Name);
            if (i > -1)
            {
                ((ServerPlugin)allPlugins[i]).shutdown();
                allPlugins.RemoveAt(i);
            }
        }
        public void RemoveAt(int i)
        {
            if (i > -1)
            {
                ((ServerPlugin)allPlugins[i]).shutdown();
                allPlugins.RemoveAt(i);
            }
        }
        public Boolean keyExists(String Name)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (((ServerPlugin)allPlugins[i]).Name() == Name)
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
                return allPlugins.Count;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new PluginEnumerator(allPlugins);
        }
        public class PluginEnumerator : IEnumerator
        {
            private int cursor = -1;
            private ArrayList _Rooms = null;
            public PluginEnumerator(ArrayList Rooms)
            {
                _Rooms = Rooms;
            }
            public bool MoveNext()
            {
                cursor++;
                return (cursor < _Rooms.Count);
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
                        return _Rooms[cursor];
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