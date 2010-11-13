using System;
using System.Collections;
using System.Runtime.Serialization;
using Mogre;
using System.IO;

namespace MogreFramework
{
    public sealed class Textures : IEnumerable
    {
        private string path_cache = "";
        private ArrayList allTextures = new ArrayList();
        public Boolean Contains(TexturePtr a)
        {
            foreach (TexturePtr tex in allTextures)
            {
                if (tex.Name == a.Name)
                {
                    return true;
                }
            }
            return false;
        }
        public TexturePtr this[int index]
        {
            get
            {
                return (TexturePtr)allTextures[index];
            }
            set
            {
                allTextures[index] = value;
            }
        }
        public TexturePtr this[String key]
        {
            get
            {
                return (TexturePtr)allTextures[IndexOf(key)];
            }
            set
            {
                int i = IndexOf(key);
                allTextures[i] = value;
            }
        }
        public Textures(string path_cache)
        {
            this.path_cache = path_cache;
            allTextures = new ArrayList();
        }
        public int Add(string pathRelFile)
        {
            string pathAbsImg = path_cache + pathRelFile;
            Mogre.Image image = new Mogre.Image();
            FileStream fs = new FileStream(pathAbsImg, FileMode.Open);
            DataStreamPtr fs2 = new DataStreamPtr(new ManagedDataStream(fs));
            image.Load(fs2);
            TexturePtr texturePtr = TextureManager.Singleton.LoadImage(pathRelFile, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, image);
            fs2.Close();
            fs.Close();
            lock (allTextures)
            {
                return allTextures.Add(texturePtr);
            }
            //ResourcePtr rp2 = MaterialManager.Singleton.Create("MATERIAL_CUSTOM_DYN", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            //MaterialPtr mat3 = (MaterialPtr)rp2;
            //TextureUnitState tState2 = mat3.GetTechnique(0).GetPass(0).CreateTextureUnitState("test2");
            //ent.SetMaterialName("MATERIAL_CUSTOM_DYN");
        }
        public int Add(System.Object o)
        {
            lock (allTextures)
            {
                return allTextures.Add(((TexturePtr)o));
            }
        }
        public int IndexOf(String Name)
        {
            for (int i = 0; i < allTextures.Count; i++)
            {
                if (((TexturePtr)allTextures[i]).Name == Name)
                {
                    return i;
                }
            }
            return -1;
        }
        public void RemoveAt(int i)
        {
            lock (allTextures)
            {
                ((TexturePtr)allTextures[i]).Unload();
                allTextures.RemoveAt(i);
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
                if (((TexturePtr)allTextures[i]).Name == Name)
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
                return allTextures.Count;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new TextureEnumerator(allTextures);
        }
        public class TextureEnumerator : IEnumerator
        {
            private int cursor = -1;
            private ArrayList _Textures = null;
            public TextureEnumerator(ArrayList Textures)
            {
                _Textures = Textures;
            }
            public bool MoveNext()
            {
                cursor++;
                return (cursor < _Textures.Count);
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
                        return _Textures[cursor];
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