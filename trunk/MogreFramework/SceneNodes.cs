using System;
using System.Collections;
using System.Runtime.Serialization;
using Mogre;
using System.IO;
using System.Runtime.InteropServices;
using MogreFramework;

namespace MogreFramework
{
    public sealed class SceneNodes : IEnumerable
    {
        private ArrayList allSceneNodes = new ArrayList();
        public Boolean Contains(SceneNode a)
        {
            foreach (SceneNode tex in allSceneNodes)
            {
                if (tex.Name == a.Name)
                {
                    return true;
                }
            }
            return false;
        }
        public SceneNode this[int index]
        {
            get
            {
                return (SceneNode)allSceneNodes[index];
            }
            set
            {
                allSceneNodes[index] = value;
            }
        }
        public SceneNode this[String key]
        {
            get
            {
                int i = IndexOf(key);
                if (i < 0)
                    throw new ArgumentOutOfRangeException("key", "\"" + key + "\" is not a valid SceneNode");
                return (SceneNode)allSceneNodes[i];
            }
            set
            {
                int i = IndexOf(key);
                if (i < 0)
                    throw new ArgumentOutOfRangeException("key", "\"" + key + "\" is not a valid SceneNode");
                allSceneNodes[i] = value;
            }
        }
        public SceneNodes()
        {
            allSceneNodes = new ArrayList();
        }
        public int Add(SceneNode sn)
        {
            if (IndexOf(sn.Name) > 0) throw new InvalidOperationException("A SceneNode with the name \"" + sn.Name + "\" already exists.");
            lock (allSceneNodes)
            {
                return allSceneNodes.Add(((SceneNode)sn));
            }
        }
        public int IndexOf(String Name)
        {
            for (int i = 0; i < allSceneNodes.Count; i++)
            {
                if (((SceneNode)allSceneNodes[i]).Name == Name)
                {
                    return i;
                }
            }
            return -1;
        }
        public void RemoveAt(int i)
        {
            lock (allSceneNodes)
            {
                allSceneNodes.RemoveAt(i);
            }
        }
        public void RemoveAt(string sceneNodeName)
        {
            RemoveAt(IndexOf(sceneNodeName));
        }
        public Boolean keyExists(String Name)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (((SceneNode)allSceneNodes[i]).Name == Name)
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
                return allSceneNodes.Count;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SceneNodeEnumerator(allSceneNodes);
        }
        public class SceneNodeEnumerator : IEnumerator
        {
            private int cursor = -1;
            private ArrayList _SceneNodes = null;
            public SceneNodeEnumerator(ArrayList SceneNodes)
            {
                _SceneNodes = SceneNodes;
            }
            public bool MoveNext()
            {
                cursor++;
                return (cursor < _SceneNodes.Count);
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
                        return _SceneNodes[cursor];
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
            for (int i = 0; i < allSceneNodes.Count; i++)
            {
                OgreWindow.Instance.mSceneMgr.DestroySceneNode((SceneNode)allSceneNodes[i]);
            }
            while (this.Count > 0)
            {
                this.RemoveAt(0);
            }
        }
    }
}