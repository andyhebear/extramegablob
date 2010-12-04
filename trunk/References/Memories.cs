using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ExtraMegaBlob.References
{
    [XmlInclude(typeof(Memory))]
    public sealed class Memories : IEnumerable, ISerializable
    {
        public override String ToString()
        {
            Serialize Serialization = new Serialize();
            return Serialization.SerializeObject(this, typeof(Memories));
        }
        public static Memories FromString(String XmlString)
        {
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(Memories));
            MemoryStream memoryStream = new MemoryStream(Serialize.StringToUTF8ByteArray(XmlString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (Memories)xs.Deserialize(memoryStream);
        }
        private ArrayList allMemorys = new ArrayList();
        //Deserialization constructor
        public Memories(SerializationInfo info, StreamingContext ctxt)
        {
            allMemorys = (ArrayList)info.GetValue("allMemorys", typeof(ArrayList));
        }
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("allMemorys", allMemorys);
        }
        public Boolean Contains(Memory a)
        {
            foreach (Memory mem in allMemorys)
            {
                if (mem.Key == a.Key && mem.Value == a.Value)
                {
                    return true;
                }
            }
            return false;
        }
        //public static Memories operator +(Memories m1, Memories m2)
        //{
        //    Memories al = m1;
        //    foreach (Memory mem in m2)
        //    {
        //        if (!al.Contains(mem))
        //        {
        //            al.Add(mem);
        //        }
        //    }
        //    return al;
        //}
        public Memory this[int index]
        {
            get
            {
                return (Memory)allMemorys[index];
            }
            set
            {
                allMemorys[index] = value;
            }
        }
        public Memory this[KeyWord KeyWord]
        {
            get
            {
                int i = IndexOf(KeyWord);
                if (i > -1)
                    return (Memory)allMemorys[i];
                else
                    return null;
            }
            set
            {
                int i = IndexOf(KeyWord);
                if (i > -1)
                    allMemorys[i] = value;
                else
                    throw new InvalidOperationException("Invalid Memory KeyWord: " + KeyWord.ToString());

            }
        }
        public Memory this[String key]
        {
            get
            {
                return (Memory)allMemorys[IndexOf(key)];
            }
            set
            {
                int i = IndexOf(key);
                allMemorys[i] = value;
            }
        }
        //Normal Constructor
        public Memories()
        {
            allMemorys = new ArrayList();
        }
        public int Add(Memory a)
        {
            lock (allMemorys)
            {
                return allMemorys.Add(a);
            }
        }
        public int Add(System.Object o)
        {
            lock (allMemorys)
            {
                return allMemorys.Add(((Memory)o));
            }
        }
        public int IndexOf(String Name)
        {
            for (int i = 0; i < allMemorys.Count; i++)
            {
                if (((Memory)allMemorys[i]).Name == Name)
                {
                    return i;
                }
            }
            return -1;
        }
        public int IndexOf(KeyWord KeyWord)
        {
            for (int i = 0; i < allMemorys.Count; i++)
            {
                if (((Memory)allMemorys[i]).Key == KeyWord)
                {
                    return i;
                }
            }
            return -1;
        }
        public void RemoveAt(int i)
        {
            lock (allMemorys)
            {
                allMemorys.RemoveAt(i);
            }
        }
        public Boolean keyExists(String Name)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (((Memory)allMemorys[i]).Name == Name)
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
                return allMemorys.Count;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MemoryEnumerator(allMemorys);
        }
        public class MemoryEnumerator : IEnumerator
        {
            private int cursor = -1;
            private ArrayList _Memories = null;
            public MemoryEnumerator(ArrayList Memories)
            {
                _Memories = Memories;
            }
            public bool MoveNext()
            {
                cursor++;
                return (cursor < _Memories.Count);
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
                        return _Memories[cursor];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException("Index out of bounds.");
                    }
                }
            }
        }

        public static bool tryGetCommonField(ref Memories Memories, String KeyS, KeyWord KeyK, System.Type OutValueType, Memory.MemoryComponents InPart, Memory.MemoryComponents OutPart, ref object outVar)
        {
            try
            {
                //lock (Memories)
                //{
                String TYPE_STRING = typeof(System.String).ToString();
                String TYPE_INT = typeof(System.Int32).ToString();
                String TYPE_BOOL = typeof(System.Boolean).ToString();
                String TYPE_KEYWORD = typeof(KeyWord).ToString();
                String OUT_VALUE_TYPE = OutValueType.ToString();

                Object o = null;
                // lock (Memories)
                // {
                o = tryGetCommonField(ref Memories, KeyS, KeyK, InPart, OutPart);
                // }

                string s = String.Empty;
                if (object.Equals(null, o))
                {
                    return false;
                }
                if (OUT_VALUE_TYPE == TYPE_STRING)
                {
                    s = (String)o;
                    outVar = s;
                    return true;
                }
                else if (OUT_VALUE_TYPE == TYPE_BOOL)
                {
                    s = (String)o;
                    if (s != String.Empty)
                    {
                        outVar = Boolean.Parse(s);
                        return true;
                    }
                    else
                    {
                        outVar = false;
                        return false;
                    }
                }
                else if (OUT_VALUE_TYPE == TYPE_INT)
                {
                    s = (String)o;
                    if (s != String.Empty)
                    {
                        outVar = System.Int32.Parse(s);
                        return true;
                    }
                    else
                    {
                        outVar = 0;
                        return false;
                    }
                }
                else if (OUT_VALUE_TYPE == TYPE_KEYWORD)
                {
                    outVar = (KeyWord)o;
                    return true;
                }
                else
                {
                    outVar = null;
                    return false;
                }
                //}
            }
            catch //(Exception e)
            {
                // _Log(e.ToString());
                return false;
            }
        }
        private static object tryGetCommonField(ref Memories MemoryHaystack, String KeyS, KeyWord KeyK, Memory.MemoryComponents InPart, Memory.MemoryComponents OutPart)
        {
            Memory MemoryNeedle = new Memory(KeyS, KeyK, String.Empty, null);
            foreach (Memory Memory in MemoryHaystack)
            {
                switch (InPart)
                {
                    case Memory.MemoryComponents.KEY:
                        if (!Object.Equals(null, KeyK))
                        {
                            if (KeyK != KeyWord.NIL)
                            {
                                if (compare(Memory, MemoryNeedle, InPart))
                                {
                                    return get(Memory, OutPart);
                                }
                            }
                        }
                        if (!Object.Equals(null, KeyS))
                        {
                            if (KeyS != String.Empty)
                            {
                                if (compare(Memory, MemoryNeedle, InPart))
                                {
                                    return get(Memory, OutPart);
                                }
                            }
                        }
                        break;
                    case Memory.MemoryComponents.NAME:
                        if (!Object.Equals(null, KeyK))
                        {
                            if (KeyK != KeyWord.NIL)
                            {
                                if (compare(Memory, MemoryNeedle, InPart))
                                {
                                    return get(Memory, OutPart);
                                }
                            }
                        }
                        if (!Object.Equals(null, KeyS))
                        {
                            if (KeyS != String.Empty)
                            {
                                if (compare(Memory, MemoryNeedle, InPart))
                                {
                                    return get(Memory, OutPart);
                                }
                            }
                        }
                        break;
                }
            }
            return null;
        }
        private static bool compare(Memory a, Memory b, Memory.MemoryComponents Part)
        {
            switch (Part)
            {
                case Memory.MemoryComponents.KEY:
                    if (a.Key == b.Key)
                        return true;
                    else
                        return false;
                case Memory.MemoryComponents.NAME:
                    if (a.Name == b.Name)
                        return true;
                    else
                        return false;
                case Memory.MemoryComponents.VALUE:
                    if (a.Value == b.Value)
                        return true;
                    else
                        return false;
                default:
                    return false;
            }
        }
        private static object get(Memory m, Memory.MemoryComponents Part)
        {
            switch (Part)
            {
                case Memory.MemoryComponents.KEY:
                    return m.Key;
                case Memory.MemoryComponents.NAME:
                    return m.Name;
                case Memory.MemoryComponents.VALUE:
                    return m.Value;
                default:
                    return null;
            }
        }

    }
}