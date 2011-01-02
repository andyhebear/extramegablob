using System;
using System.Collections;
using System.Runtime.Serialization;
using Mogre;
using System.IO;

namespace MogreFramework
{
    public sealed class Skeletons : IEnumerable
    {
        private unsafe class resourceLoaderSkeletonBasic : ManualResourceLoader
        {
            private string path_cache = "";
            public resourceLoaderSkeletonBasic(string path_cache)
            {
                this.path_cache = path_cache;
            }
            public override void LoadResource(Resource resource)
            {
                string pathAbs = path_cache + resource.Name;
                FileStream fs = new FileStream(pathAbs, FileMode.Open);
                DataStreamPtr stream = new DataStreamPtr(new ManagedDataStream(fs));
                new SkeletonSerializer().ImportSkeleton(stream, (Skeleton)resource);
                stream.Close();
                fs.Close();
            }
            public override void PrepareResource(Resource resource)
            {
                base.PrepareResource(resource);
            }
        }
        private resourceLoaderSkeletonBasic loader = null;
        private string path_cache = "";
        private ArrayList allSkeletons = new ArrayList();
        public Boolean Contains(SkeletonPtr a)
        {
            foreach (SkeletonPtr tex in allSkeletons)
            {
                if (tex.Name == a.Name)
                {
                    return true;
                }
            }
            return false;
        }
        public SkeletonPtr this[int index]
        {
            get
            {
                return (SkeletonPtr)allSkeletons[index];
            }
            set
            {
                allSkeletons[index] = value;
            }
        }
        public SkeletonPtr this[String key]
        {
            get
            {
                return (SkeletonPtr)allSkeletons[IndexOf(key)];
            }
            set
            {
                int i = IndexOf(key);
                allSkeletons[i] = value;
            }
        }
        public Skeletons(string path_cache)
        {
            this.path_cache = path_cache;
            loader = new resourceLoaderSkeletonBasic(path_cache);
            allSkeletons = new ArrayList();
        }
        public int Add(string pathRelFile)
        {
            SkeletonPtr skel = SkeletonManager.Singleton.Create(pathRelFile, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, true, loader);
            //skel.Load();
            return Add(skel);
        }
        public int Add(SkeletonPtr o)
        {
            lock (allSkeletons)
            {
                return allSkeletons.Add(((SkeletonPtr)o));
            }
        }
        public int IndexOf(String Name)
        {
            for (int i = 0; i < allSkeletons.Count; i++)
            {
                if (((SkeletonPtr)allSkeletons[i]).Name == Name)
                {
                    return i;
                }
            }
            return -1;
        }
        public void RemoveAt(int i)
        {
            lock (allSkeletons)
            {
                ((SkeletonPtr)allSkeletons[i]).Unload();
                allSkeletons.RemoveAt(i);
            }
        }
        public void RemoveAt(string pathRelFile)
        {
            RemoveAt(IndexOf(pathRelFile));
        }
        public Boolean keyExists(String Name)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (((SkeletonPtr)allSkeletons[i]).Name == Name)
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
                return allSkeletons.Count;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SkeletonEnumerator(allSkeletons);
        }
        public class SkeletonEnumerator : IEnumerator
        {
            private int cursor = -1;
            private ArrayList _Skeletons = null;
            public SkeletonEnumerator(ArrayList Skeletons)
            {
                _Skeletons = Skeletons;
            }
            public bool MoveNext()
            {
                cursor++;
                return (cursor < _Skeletons.Count);
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
                        return _Skeletons[cursor];
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
            foreach (SkeletonPtr ptr in this)
            {
                ptr.Unload();
                SkeletonManager.Singleton.Remove(ptr.Handle);
                ptr.Dispose();
            }
            SkeletonManager.Singleton.UnloadAll();
        }
    }
}