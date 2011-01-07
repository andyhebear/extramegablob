using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraMegaBlob.References;
using System.Collections;
using System.IO;

namespace ExtraMegaBlob.Client
{
    class CacheManager
    {
        private Hashtable md5table = new Hashtable();
        private string path_cache = "";
        public CacheManager(string path_cache)
        {
            this.path_cache = path_cache;
        }
        public CacheManager()
        {
            this.path_cache = ThingPath.path_cache;
        }
        public event LogDelegate onLogMessage;
        private void log(string msg)
        {
            if (!object.Equals(null, this.onLogMessage))
            {
                onLogMessage(msg);
            }
        }
        public void init()
        {
            log("Cache Manager initializing");
            rebuildHashTable();
        }
        public void sendReport()
        {
            Event outevent = new Event();
            outevent._Keyword = KeyWord.CACHE_CLIENTREPORT;
            outevent._Memories = new Memories();
            foreach (DictionaryEntry de in md5table)
            {
                outevent._Memories.Add(new Memory((string)de.Key, KeyWord.NIL, (string)de.Value));
            }
            toserver(outevent);
        }
        private void rebuildHashTable()
        {
            log("Rebuilding hash table");
            Hashtable files = new Hashtable();
            getTreeLinear("\\", ref files);
            log("hashing " + files.Count.ToString() + " files");
            Hashtable files2 = new Hashtable();
            foreach (DictionaryEntry de in files)
            {
                string pathRel = (string)de.Key;
                string pathAbs = path_cache + pathRel;
                if (Helpers.isPluginFile(pathRel) && Helpers.isClientPlugin(pathRel))
                    _pluginAdded(pathRel);
                if (Helpers.isTextureFile(pathRel))
                    _textureAdded(pathRel);
                if (Helpers.isMeshFile(pathRel))
                    _meshAdded(pathRel);
                if (Helpers.isSkeletonFile(pathRel))
                    _skeletonAdded(pathRel);

                bool s = false;
                try { string md5 = Encryption.md5_file(pathAbs); s = true; }
                catch (UnauthorizedAccessException)
                {
                }
                catch (Exception ex)
                {
                    log(ex.Message);
                }

                if (s) files2.Add(pathRel, Encryption.md5_file(pathAbs));
            }
            this.md5table = files2;
        }
        private void getTreeLinear(string path, ref Hashtable files)
        {
            string src = Path.GetDirectoryName(path_cache + "\\" + path);
            string src2 = src.Replace(path_cache, "") + "\\";
            DirectoryInfo di2 = new DirectoryInfo(src);
            foreach (FileInfo fi in di2.GetFiles())
            {
                string path3 = src2 + fi.Name;
                files.Add(path3, null);
            }
            foreach (DirectoryInfo di in di2.GetDirectories())
            {
                string src3 = src2 + di.Name + "\\";
                getTreeLinear(src3, ref files);
            }
        }
        public void updateFile(Event ev)
        {
            byte[] fileBytes = Helpers.decompress_gzip(ev._Memories["file"].Bytes);
            string pathRel = ev._Memories["file"].Value;
            string pathAbs = path_cache + pathRel;
            string pathDirAbs = Path.GetDirectoryName(pathAbs);
            if (!Directory.Exists(pathDirAbs))
                Directory.CreateDirectory(pathDirAbs);
            FileStream fs = File.Open(pathAbs, FileMode.OpenOrCreate);
            fs.SetLength(0);
            fs.Write(fileBytes, 0, fileBytes.Length);
            fs.Close();
            string md5 = Encryption.md5_file(pathAbs);
            if (md5table.ContainsKey(pathRel))
                if ((string)md5table[pathRel] != md5)
                {
                    if (Helpers.isPluginFile(pathRel) && Helpers.isClientPlugin(pathRel))
                        _pluginDeleted(pathRel);
                    if (Helpers.isTextureFile(pathRel))
                        _textureDeleted(pathRel);
                    if (Helpers.isMeshFile(pathRel))
                        _meshDeleted(pathRel);
                    if (Helpers.isSkeletonFile(pathRel))
                        _skeletonDeleted(pathRel);
                }
            if (Helpers.isPluginFile(pathRel) && Helpers.isClientPlugin(pathRel))
                _pluginAdded(pathRel);
            if (Helpers.isTextureFile(pathRel))
                _textureAdded(pathRel);
            if (Helpers.isMeshFile(pathRel))
                _meshAdded(pathRel);
            if (Helpers.isSkeletonFile(pathRel))
                _skeletonAdded(pathRel);
            md5table[pathRel] = md5;
            log("saved: " + pathRel);
        }
        public void renameFile(Event ev)
        {
            string pathRelOld = ev._Memories["file"].Value;
            string pathRelNew = ev._Memories["newfile"].Value;
            string pathAbsOld = path_cache + pathRelOld;
            string pathAbsNew = path_cache + pathRelNew;
            string md5 = (string)md5table[pathRelOld];

            if (Helpers.isPluginFile(pathRelOld) && Helpers.isClientPlugin(pathRelOld))
                _pluginDeleted(pathRelOld);
            if (Helpers.isPluginFile(pathRelNew) && Helpers.isClientPlugin(pathRelNew))
                _pluginAdded(pathRelNew);

            if (Helpers.isTextureFile(pathRelOld))
                _textureDeleted(pathRelOld);
            if (Helpers.isTextureFile(pathRelNew))
                _textureAdded(pathRelNew);

            if (Helpers.isMeshFile(pathRelOld))
                _meshDeleted(pathRelOld);
            if (Helpers.isMeshFile(pathRelNew))
                _meshAdded(pathRelNew);

            if (Helpers.isSkeletonFile(pathRelOld))
                _skeletonDeleted(pathRelOld);
            if (Helpers.isSkeletonFile(pathRelNew))
                _skeletonAdded(pathRelNew);

            md5table.Remove(pathRelOld);
            md5table.Add(pathRelNew, md5);

            File.Move(pathAbsOld, pathAbsNew);
            log(string.Format("renamed: {0} to {1}", pathRelOld, pathRelNew));
        }
        public void deleteFile(Event ev)
        {
            string pathRel = ev._Memories["file"].Value;
            string pathAbs = path_cache + pathRel;
            if (File.Exists(pathAbs))
            {
                if (Helpers.isPluginFile(pathRel) && Helpers.isClientPlugin(pathRel))
                    _pluginDeleted(pathRel);
                if (Helpers.isTextureFile(pathRel))
                    _textureDeleted(pathRel);
                if (Helpers.isMeshFile(pathRel))
                    _meshDeleted(pathRel);
                if (Helpers.isSkeletonFile(pathRel))
                    _skeletonDeleted(pathRel);
                File.Delete(pathAbs);
            }
            md5table.Remove(pathRel);
            log("deleted: " + pathRel);
        }
        public delegate void route_toserverDelegate(Event ev);
        public event route_toserverDelegate route_toserver;
        private void toserver(Event ev)
        {
            if (!object.Equals(null, this.route_toserver))
            {
                route_toserver(ev);
            }
        }
        public delegate void skeletonDeletedDelegate(string pathRelSkeletonFile);
        public event skeletonDeletedDelegate skeletonDeleted;
        private void _skeletonDeleted(string pathRelSkeletonFile)
        {
            if (!object.Equals(null, this.skeletonDeleted))
            {
                skeletonDeleted(pathRelSkeletonFile);
            }
        }
        public delegate void skeletonAddedDelegate(string pathRelSkeletonFile);
        public event skeletonAddedDelegate skeletonAdded;
        private void _skeletonAdded(string pathRelSkeletonFile)
        {
            if (!object.Equals(null, this.skeletonAdded))
            {
                skeletonAdded(pathRelSkeletonFile);
            }
        }
        public delegate void meshDeletedDelegate(string pathRelMeshFile);
        public event meshDeletedDelegate meshDeleted;
        private void _meshDeleted(string pathRelMeshFile)
        {
            if (!object.Equals(null, this.meshDeleted))
            {
                meshDeleted(pathRelMeshFile);
            }
        }
        public delegate void meshAddedDelegate(string pathRelMeshFile);
        public event meshAddedDelegate meshAdded;
        private void _meshAdded(string pathRelMeshFile)
        {
            if (!object.Equals(null, this.meshAdded))
            {
                meshAdded(pathRelMeshFile);
            }
        }
        public delegate void textureDeletedDelegate(string pathRelTextureFile);
        public event textureDeletedDelegate textureDeleted;
        private void _textureDeleted(string pathRelTextureFile)
        {
            if (!object.Equals(null, this.textureDeleted))
            {
                textureDeleted(pathRelTextureFile);
            }
        }
        public delegate void textureAddedDelegate(string pathRelTextureFile);
        public event textureAddedDelegate textureAdded;
        private void _textureAdded(string pathRelTextureFile)
        {
            if (!object.Equals(null, this.textureAdded))
            {
                textureAdded(pathRelTextureFile);
            }
        }
        public delegate void pluginAddedDelegate(string pathRelPluginFile);
        public event pluginAddedDelegate pluginAdded;
        private void _pluginAdded(string pathRelPluginFile)
        {
            if (!object.Equals(null, this.pluginAdded))
            {
                pluginAdded(pathRelPluginFile);
            }
        }
        public delegate void pluginDeletedDelegate(string pathRelPluginFile);
        public event pluginDeletedDelegate pluginDeleted;
        private void _pluginDeleted(string pathRelPluginFile)
        {
            if (!object.Equals(null, this.pluginDeleted))
            {
                pluginDeleted(pathRelPluginFile);
            }
        }
    }
}
