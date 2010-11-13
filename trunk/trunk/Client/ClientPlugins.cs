using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ThingReferences;

namespace thing.Parts
{
    [XmlInclude(typeof(Memory))]
    public sealed class ClientPlugins : IEnumerable, ISerializable
    {
        public override String ToString()
        {
            Serialize Serialization = new Serialize();
            return Serialization.SerializeObject(this, typeof(ClientPlugins));
        }
        public static ClientPlugins FromString(String XmlString)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(ClientPlugins));
            MemoryStream memoryStream = new MemoryStream(Serialize.StringToUTF8ByteArray(XmlString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (ClientPlugins)xs.Deserialize(memoryStream);
        }
        private ArrayList allPlugins = new ArrayList();
        //Deserialization constructor
        public ClientPlugins(SerializationInfo info, StreamingContext ctxt)
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
            foreach (ClientPlugin mem in allPlugins)
            {
                if (mem.Name() == ((ClientPlugin)a).Name())
                {
                    return true;
                }
            }
            return false;
        }
        public static ClientPlugins operator +(ClientPlugins m1, ClientPlugins m2)
        {
            ClientPlugins al = m1;
            foreach (ClientPlugin mem in m2)
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
                return (ClientPlugin)allPlugins[index];
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
                return (ClientPlugin)allPlugins[i];
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
        public ClientPlugins()
        {
            allPlugins = new ArrayList();
        }
        public int Add(System.Object o)
        {
            lock (allPlugins)
            {
                return allPlugins.Add(((ClientPlugin)o));
            }
        }
        public int IndexOf(String Name)
        {
            for (int i = 0; i < allPlugins.Count; i++)
            {
                if (((ClientPlugin)allPlugins[i]).Name() == Name)
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
                    ((ClientPlugin)allPlugins[i]).updateHook();
                }
            }
        }
        internal void FrameStartedHooks(float interpolation)
        {
            for (int i = 0; i < allPlugins.Count; i++)
            {
                if (!object.Equals(null, allPlugins[i]))
                {
                    ((ClientPlugin)allPlugins[i]).frameHook(interpolation);
                }
            }
        }
        //internal void setWindow(OgreWindow win)
        //{
        //    for (int i = 0; i < allPlugins.Count; i++)
        //    {
        //        if (!object.Equals(null, allPlugins[i]))
        //        {
        //            ((ClientPlugin)allPlugins[i]).setWindow(win);
        //        }
        //    }
        //}
        //internal void AssignInputDevices(InputManager inputManager, Keyboard inputKeyboard, Mouse inputMouse)
        //{
        //    for (int i = 0; i < allPlugins.Count; i++)
        //    {
        //        if (!object.Equals(null, allPlugins[i]))
        //        {
        //            ((ClientPlugin)allPlugins[i]).inputmanager = inputManager;
        //            ((ClientPlugin)allPlugins[i]).keyboard = inputKeyboard;
        //            ((ClientPlugin)allPlugins[i]).mouse = inputMouse;
        //        }
        //    }
        //}
        internal void Shutdown()
        {
            for (int i = 0; i < allPlugins.Count; i++)
            {
                if (!object.Equals(null, allPlugins[i]))
                {
                    try
                    {
                        ((ClientPlugin)allPlugins[i]).shutdown();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                    }
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
                ((ClientPlugin)allPlugins[i]).shutdown();
                allPlugins.RemoveAt(i);
            }
        }
        public void RemoveAt(int i)
        {
            if (i > -1)
            {
                ((ClientPlugin)allPlugins[i]).shutdown();
                allPlugins.RemoveAt(i);
            }
        }
        public Boolean keyExists(String Name)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (((ClientPlugin)allPlugins[i]).Name() == Name)
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