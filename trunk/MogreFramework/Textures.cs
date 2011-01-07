using System;
using System.Collections;
using System.Runtime.Serialization;
using Mogre;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;

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
        }
        public void Replace(byte[] bytes, string textureName)
        {
            if (this.IndexOf(textureName) < 0) throw new ArgumentException("The texture \"" + textureName + "\"doesn't exist");
            Mogre.Image image = new Mogre.Image();
            MemoryStream ms = new MemoryStream(bytes);
            DataStreamPtr fs2 = new DataStreamPtr(new ManagedDataStream(ms));
            image.Load(fs2);
            PixelBox imagBox = image.GetPixelBox();
            TexturePtr pTexture = this[textureName];
            TextureManager lTextureManager = TextureManager.Singleton;
            HardwarePixelBuffer buffer = pTexture.GetBuffer();
            unsafe
            {
                buffer.BlitFromMemory(imagBox);
            }
            image.Dispose();
            fs2.Close();
            ms.Close();
        }
        //public unsafe void Replace3(Mogre.Image image, string textureName)
        //{
        //    if (this.IndexOf(textureName) < 0) throw new ArgumentException("The texture \"" + textureName + "\"doesn't exist");
        //    TexturePtr pTexture = this[textureName];
        //    HardwarePixelBufferSharedPtr texBuffer = pTexture.GetBuffer();
        //    texBuffer.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);
        //    PixelBox pb = texBuffer.CurrentLock;

        //    PixelBox imagBox = image.GetPixelBox();
        //    imagBox.data
        //    Marshal.Copy(bytes, 0, pb.data, bytes.Length);
        //    texBuffer.Unlock();
        //    texBuffer.Dispose();
        //}

        public byte[] ConvertImageToRgbValues(byte[] inBytes)
        {
            Mogre.Image image = new Mogre.Image();
            MemoryStream ms = new MemoryStream(inBytes);
            DataStreamPtr fs2 = new DataStreamPtr(new ManagedDataStream(ms));
            image.Load(fs2);
            PixelBox imagBox = image.GetPixelBox();
            uint bytes = imagBox.GetConsecutiveSize();
            byte[] rgbValues = new byte[bytes];
            Marshal.Copy( imagBox.data, rgbValues, 0, (int)bytes);
            image.Dispose();
            fs2.Close();
            ms.Close();
            return rgbValues;
        }
        public byte[] ConvertImageToRgbValues(Bitmap image)
        {
            System.Drawing.Imaging.BitmapData bmpData = image.LockBits(
                new System.Drawing.Rectangle(0, 0, image.Width, image.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            IntPtr ptr = bmpData.Scan0;
            int bytes = System.Math.Abs(bmpData.Stride) * image.Height;
            byte[] rgbValues = new byte[bytes];
            Marshal.Copy(ptr, rgbValues, 0, bytes);
            image.UnlockBits(bmpData);
            return rgbValues;
        }
        public unsafe void Replace2(byte[] bytes, string textureName)
        {
            if (this.IndexOf(textureName) < 0) throw new ArgumentException("The texture \"" + textureName + "\"doesn't exist");
            TexturePtr pTexture = this[textureName];
            HardwarePixelBufferSharedPtr texBuffer = pTexture.GetBuffer();
            texBuffer.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);
            PixelBox pb = texBuffer.CurrentLock;
            Marshal.Copy(bytes, 0, pb.data, bytes.Length);
            texBuffer.Unlock();
            texBuffer.Dispose();
        }
        public unsafe void Replace3(byte[] bytes, string textureName)
        {
            using (ResourcePtr rpt = TextureManager.Singleton.GetByName(textureName))
            {
                using (TexturePtr texture = rpt)
                {
                    HardwarePixelBufferSharedPtr texBuffer = texture.GetBuffer();
                    texBuffer.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);
                    PixelBox pb = texBuffer.CurrentLock;

                    Marshal.Copy(bytes, 0, pb.data, bytes.Length);

                    texBuffer.Unlock();
                    texBuffer.Dispose();
                }
            }
        }

        //unsafe
        //{
        //    void* pointer = buffer.Lock(0, image.Width * image.Height * 4, HardwareBuffer.LockOptions.HBL_NORMAL);
        //    PixelBox pBox = buffer.CurrentLock;
        //    pBox.format = PixelFormat.PF_X8R8G8B8;

        //    //PixelBox imagBox = image.GetPixelBox();

        //    Marshal.Copy(imgBytes, 0, pBox.data, imgBytes.Length);

        //    //Marshal.Copy(imagBox.data,pBox. 0, pBox.data, (int)imagBox.GetConsecutiveSize());
        //}

        //TexturePtr lTextureWithRtt = lTextureManager.CreateManual(pathRelFile + "_temp_rtt", 
        //    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
        //    TextureType.TEX_TYPE_2D, 512, 512, 0, PixelFormat.PF_R8G8B8,
        //   (int)Mogre.TextureUsage.TU_DYNAMIC_WRITE_ONLY, null, false, 0);

        //TexturePtr lIntermediateTexture = lTextureManager.CreateManual(pathRelFile + "_temp_inter",
        //    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
        //    TextureType.TEX_TYPE_2D, 512, 512, 0, PixelFormat.PF_R8G8B8,
        //   (int)Mogre.TextureUsage.TU_DYNAMIC_WRITE_ONLY, null, false, 0);




        //buffer.
        //TexturePtr texturePtr = TextureManager.Singleton.LoadImage(pathRelFile, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, image);
        //fs2.Close();
        //fs.Close();
        //lock (allTextures)
        //{
        //    return allTextures.Add(texturePtr);
        //}
        //ResourcePtr rp2 = MaterialManager.Singleton.Create("MATERIAL_CUSTOM_DYN", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
        //MaterialPtr mat3 = (MaterialPtr)rp2;
        //TextureUnitState tState2 = mat3.GetTechnique(0).GetPass(0).CreateTextureUnitState("test2");
        //ent.SetMaterialName("MATERIAL_CUSTOM_DYN");
        //public static void ReplaceTexture(HardwarePixelBuffer buffer, byte[] frame, int ancho, int alto)
        //{
        //    unsafe
        //    {

        //        buffer.Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
        //        PixelBox pBox = buffer.CurrentLock;
        //        //pBox.format = PixelFormat.PF_BYTE_BGRA;
        //        pBox.format = PixelFormat.PF_X8R8G8B8;



        //        //Marshal.Copy(frame, 0, pBox.data, (alto * ancho * 4));
        //        Marshal.Copy(frame, 0, pBox.data, frame.Length);

        //        buffer.Unlock();
        //    }
        //}
        //public void ShowImageOnTexture(System.Drawing.Image img, Entity ent)
        //{
        //    ent.SetMaterialName("MATERIAL_CUSTOM_DYN");
        //    MemoryStream ms = new MemoryStream();
        //    img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
        //    ReplaceTexture("DynTexture", ms.ToArray(), img.Width, img.Height);
        //}
        public int Add(TexturePtr o)
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

        public void shutdown()
        {
            foreach (TexturePtr ptr in this)
            {
                ptr.Unload();
                TextureManager.Singleton.Remove(ptr.Handle);
                ptr.Dispose();
            }
            TextureManager.Singleton.UnloadAll();
        }
    }
}