using System;
using System.Collections;
using Mogre;
using MogreFramework;

namespace MogreFramework
{
    public sealed class Materials : IEnumerable
    {
        private ArrayList allMaterials = new ArrayList();
        public Boolean Contains(MaterialPtr a)
        {
            foreach (MaterialPtr tex in allMaterials)
            {
                if (tex.Name == a.Name)
                {
                    return true;
                }
            }
            return false;
        }
        public Boolean Contains(String key)
        {
            return (IndexOf(key) > -1);
        }
        public MaterialPtr this[int index]
        {
            get
            {
                return (MaterialPtr)allMaterials[index];
            }
            set
            {
                allMaterials[index] = value;
            }
        }
        public MaterialPtr this[String key]
        {
            get
            {
                int i = IndexOf(key);
                if (i < 0)
                    throw new ArgumentOutOfRangeException("key", "\"" + key + "\" is not a valid MaterialPtr");
                return (MaterialPtr)allMaterials[i];
            }
            set
            {
                int i = IndexOf(key);
                if (i < 0)
                    throw new ArgumentOutOfRangeException("key", "\"" + key + "\" is not a valid MaterialPtr");
                allMaterials[i] = value;
            }
        }
        public Materials()
        {
            allMaterials = new ArrayList();
        }
        public int Add(MaterialPtr sn)
        {
            if (IndexOf(sn.Name) > 0) throw new InvalidOperationException("A MaterialPtr with the name \"" + sn.Name + "\" already exists.");
            lock (allMaterials)
            {
                return allMaterials.Add(((MaterialPtr)sn));
            }
        }
        public int IndexOf(String Name)
        {
            for (int i = 0; i < allMaterials.Count; i++)
            {
                if (((MaterialPtr)allMaterials[i]).Name == Name)
                {
                    return i;
                }
            }
            return -1;
        }
        public void RemoveAt(int i)
        {
            lock (allMaterials)
            {
                allMaterials.RemoveAt(i);
            }
        }
        public void RemoveAt(string entityName)
        {
            RemoveAt(IndexOf(entityName));
        }
        public Boolean keyExists(String entityName)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (((MaterialPtr)allMaterials[i]).Name == entityName)
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
                return allMaterials.Count;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MaterialPtrEnumerator(allMaterials);
        }
        public class MaterialPtrEnumerator : IEnumerator
        {
            private int cursor = -1;
            private ArrayList _Materials = null;
            public MaterialPtrEnumerator(ArrayList Materials)
            {
                _Materials = Materials;
            }
            public bool MoveNext()
            {
                cursor++;
                return (cursor < _Materials.Count);
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
                        return _Materials[cursor];
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
            for (int i = 0; i < allMaterials.Count; i++)
            {
                //OgreWindow.Instance.mSceneMgr.DestroyMaterialPtr((MaterialPtr)allMaterials[i]);
            }
            while (this.Count > 0)
            {
                this.RemoveAt(0);
            }
        }
    }
}