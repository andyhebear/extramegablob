using System;
using System.Collections;
using System.Runtime.Serialization;
using Mogre;
using System.IO;

namespace MogreFramework
{
    public sealed class Meshes : IEnumerable
    {
        private unsafe class resourceLoaderMeshBasic : ManualResourceLoader
        {
            private string path_cache = "";
            public resourceLoaderMeshBasic(string path_cache)
            {
                this.path_cache = path_cache;
            }
            public override void LoadResource(Resource resource)
            {
                string pathAbs = path_cache + resource.Name;
                FileStream fs = new FileStream(pathAbs, FileMode.Open);
                DataStreamPtr stream = new DataStreamPtr(new ManagedDataStream(fs));
                //Mesh m = new Mesh(resource.Creator, resource.Name, 1234L, "");
                new MeshSerializer().ImportMesh(stream, (Mesh)resource);

                stream.Close();
                fs.Close();
                // resource = m;
            }
            public override void PrepareResource(Resource resource)
            {
                base.PrepareResource(resource);
            }
        }
        private resourceLoaderMeshBasic loader = null;
        private string path_cache = "";
        private ArrayList allMeshes = new ArrayList();
        public Boolean Contains(MeshPtr a)
        {
            foreach (MeshPtr tex in allMeshes)
            {
                if (tex.Name == a.Name)
                {
                    return true;
                }
            }
            return false;
        }
        public MeshPtr this[int index]
        {
            get
            {
                return (MeshPtr)allMeshes[index];
            }
            set
            {
                allMeshes[index] = value;
            }
        }
        public MeshPtr this[String key]
        {
            get
            {
                return (MeshPtr)allMeshes[IndexOf(key)];
            }
            set
            {
                int i = IndexOf(key);
                allMeshes[i] = value;
            }
        }
        public Meshes(string path_cache)
        {
            this.path_cache = path_cache;
            loader = new resourceLoaderMeshBasic(path_cache);
            allMeshes = new ArrayList();
        }
        public int Add(string pathRelFile)
        {
            MeshPtr mesh = MeshManager.Singleton.CreateManual(pathRelFile, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, loader);
            //mesh.Load();
            lock (allMeshes)
            {
                return allMeshes.Add(mesh);
            }
        }
        public int Add(MeshPtr o)
        {
            lock (allMeshes)
            {
                return allMeshes.Add(((MeshPtr)o));
            }
        }
        public int IndexOf(String Name)
        {
            for (int i = 0; i < allMeshes.Count; i++)
            {
                if (((MeshPtr)allMeshes[i]).Name == Name)
                {
                    return i;
                }
            }
            return -1;
        }
        public void RemoveAt(int i)
        {
            lock (allMeshes)
            {
                ((MeshPtr)allMeshes[i]).Unload();
                allMeshes.RemoveAt(i);
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
                if (((MeshPtr)allMeshes[i]).Name == Name)
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
                return allMeshes.Count;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MeshEnumerator(allMeshes);
        }
        public class MeshEnumerator : IEnumerator
        {
            private int cursor = -1;
            private ArrayList _Meshes = null;
            public MeshEnumerator(ArrayList Meshes)
            {
                _Meshes = Meshes;
            }
            public bool MoveNext()
            {
                cursor++;
                return (cursor < _Meshes.Count);
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
                        return _Meshes[cursor];
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
            foreach (MeshPtr ptr in this)
            {
                ptr.Unload();
                MeshManager.Singleton.Remove(ptr.Handle);
                ptr.Dispose();
            }
            MeshManager.Singleton.UnloadAll();
        }
    }
}