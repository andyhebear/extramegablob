using System;
using System.Collections;
using Mogre;
using MogreFramework;

namespace ExtraMegaBlob.References
{
    public sealed class Entities2 : IEnumerable
    {
        private ArrayList allEntities = new ArrayList();
        public Boolean Contains(Entity a)
        {
            foreach (Entity tex in allEntities)
            {
                if (tex.Name == a.Name)
                {
                    return true;
                }
            }
            return false;
        }
        public Entity this[int index]
        {
            get
            {
                return (Entity)allEntities[index];
            }
            set
            {
                allEntities[index] = value;
            }
        }
        public Entity this[String key]
        {
            get
            {
                int i = IndexOf(key);
                if (i < 0)
                    throw new ArgumentOutOfRangeException("key", "\"" + key + "\" is not a valid Entity");
                return (Entity)allEntities[i];
            }
            set
            {
                int i = IndexOf(key);
                if (i < 0)
                    throw new ArgumentOutOfRangeException("key", "\"" + key + "\" is not a valid Entity");
                allEntities[i] = value;
            }
        }
        public Entities2()
        {
            allEntities = new ArrayList();
        }
        public int Add(Entity sn)
        {
            if (IndexOf(sn.Name) > 0) throw new InvalidOperationException("A Entity with the name \"" + sn.Name + "\" already exists.");
            lock (allEntities)
            {
                return allEntities.Add(((Entity)sn));
            }
        }
        public int IndexOf(String Name)
        {
            for (int i = 0; i < allEntities.Count; i++)
            {
                if (((Entity)allEntities[i]).Name == Name)
                {
                    return i;
                }
            }
            return -1;
        }
        public void RemoveAt(int i)
        {
            lock (allEntities)
            {
                allEntities.RemoveAt(i);
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
                if (((Entity)allEntities[i]).Name == entityName)
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
                return allEntities.Count;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new EntityEnumerator(allEntities);
        }
        public class EntityEnumerator : IEnumerator
        {
            private int cursor = -1;
            private ArrayList _Entities = null;
            public EntityEnumerator(ArrayList Entities)
            {
                _Entities = Entities;
            }
            public bool MoveNext()
            {
                cursor++;
                return (cursor < _Entities.Count);
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
                        return _Entities[cursor];
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
            for (int i = 0; i < allEntities.Count; i++)
            {
                OgreWindow.Instance.mSceneMgr.DestroyEntity((Entity)allEntities[i]);
            }
            while (this.Count > 0)
            {
                this.RemoveAt(0);
            }
        }
    }
}