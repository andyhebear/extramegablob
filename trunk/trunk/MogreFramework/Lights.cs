using System;
using System.Collections;
using Mogre;
namespace MogreFramework
{
    public sealed class Lights : IEnumerable
    {
        private ArrayList allLights = new ArrayList();
        public Boolean Contains(Light a)
        {
            foreach (Light tex in allLights)
            {
                if (tex.Name == a.Name)
                {
                    return true;
                }
            }
            return false;
        }
        public Light this[int index]
        {
            get
            {
                return (Light)allLights[index];
            }
            set
            {
                allLights[index] = value;
            }
        }
        public Light this[String key]
        {
            get
            {
                return (Light)allLights[IndexOf(key)];
            }
            set
            {
                int i = IndexOf(key);
                allLights[i] = value;
            }
        }
        public Lights()
        {
            allLights = new ArrayList();
        }
        public int Add(Light sn)
        {
            if (IndexOf(sn.Name) > 0) throw new InvalidOperationException("A Light with the name \"" + sn.Name + "\" already exists.");
            lock (allLights)
            {
                return allLights.Add(((Light)sn));
            }
        }
        public int IndexOf(String Name)
        {
            for (int i = 0; i < allLights.Count; i++)
            {
                if (((Light)allLights[i]).Name == Name)
                {
                    return i;
                }
            }
            return -1;
        }
        public void RemoveAt(int i)
        {
            lock (allLights)
            {
                allLights.RemoveAt(i);
            }
        }
        public void RemoveAt(string lightName)
        {
            RemoveAt(IndexOf(lightName));
        }
        public Boolean keyExists(String lightName)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (((Light)allLights[i]).Name == lightName)
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
                return allLights.Count;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new LightEnumerator(allLights);
        }
        public class LightEnumerator : IEnumerator
        {
            private int cursor = -1;
            private ArrayList _Lights = null;
            public LightEnumerator(ArrayList Lights)
            {
                _Lights = Lights;
            }
            public bool MoveNext()
            {
                cursor++;
                return (cursor < _Lights.Count);
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
                        return _Lights[cursor];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException("Index out of bounds.");
                    }
                }
            }
        }
        public void shutdown()
        {
            for (int i = 0; i < allLights.Count; i++)
            {
                OgreWindow.Instance.mSceneMgr.DestroyLight((Light)allLights[i]);
            }
            while (this.Count > 0)
            {
                this.RemoveAt(0);
            }
        }
    }
}