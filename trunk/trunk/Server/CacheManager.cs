using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThingReferences;
using System.Collections;
using System.IO;
using System.Threading;

namespace ServerThing
{
    class CacheManager
    {
        private FileSystemWatcher watcher;
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
            log("Root: " + path_cache);
            rebuildHashTable();
            watcher = new FileSystemWatcher(path_cache);
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            watcher.Created += new FileSystemEventHandler(watcher_Created);
            watcher.Renamed += new RenamedEventHandler(watcher_Renamed);
            watcher.Deleted += new FileSystemEventHandler(watcher_Deleted);
            watcher.EnableRaisingEvents = true;
        }
        private void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string pathRel = e.FullPath.Replace(path_cache, "");
            if (Helpers.isPluginFile(pathRel) && Helpers.isServerPlugin(pathRel))
                _pluginDeleted(pathRel);
            md5table.Remove(pathRel);
            fileDeleteFromClient(pathRel, "");
        }
        private void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string pathRelOld = e.OldFullPath.Replace(path_cache, "");
            string pathRelNew = e.FullPath.Replace(path_cache, "");
            string md5 = (string)md5table[pathRelOld];
            if (Helpers.isPluginFile(pathRelOld) && Helpers.isServerPlugin(pathRelOld))
                _pluginDeleted(pathRelOld);
            if (Helpers.isPluginFile(pathRelNew) && Helpers.isServerPlugin(pathRelNew))
                _pluginAdded(pathRelNew);
            md5table.Remove(pathRelOld);
            md5table.Add(pathRelNew, md5);
            fileRenameFromClient("", pathRelOld, pathRelNew);
        }
        private void watcher_Created(object sender, FileSystemEventArgs e)
        {
            string pathRel = e.FullPath.Replace(path_cache, "");
            string md5 = Encryption.md5_file(e.FullPath);
            if (Helpers.isPluginFile(pathRel) && Helpers.isServerPlugin(pathRel))
                _pluginAdded(pathRel);
            md5table[pathRel] = md5;
            fileSendToClient(pathRel, "");
        }
        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(e.FullPath)) return;
            string pathRel = e.FullPath.Replace(path_cache, "");
            if (!watcherChangedQueue.Contains(pathRel))
                watcherChangedQueue.Add(pathRel);
        }
        private void processWatcherChangedQueue()
        {
            string pathRel = (string)watcherChangedQueue[0];
            watcherChangedQueue.RemoveAt(0);
            string pathAbs = path_cache + pathRel;
            try
            {
                if (!File.Exists(pathAbs)) return;
                string md5 = Encryption.md5_file(pathAbs);
                if ((string)md5table[pathRel] != md5)
                {
                    if (Helpers.isPluginFile(pathRel) && Helpers.isServerPlugin(pathRel))
                    {
                        _pluginDeleted(pathRel);
                        _pluginAdded(pathRel);
                    }
                    md5table[pathRel] = md5;
                    fileSendToClient(pathRel, "");
                }
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }
        }
        public void mainLoop()
        {
            log("main thread running");
            Thread.CurrentThread.Name = "Cache Manager mainLoop";
            timer t = new timer(new TimeSpan(0, 0, 1));
            while (running)
            {
                Thread.Sleep(100);
                if (t.elapsed)
                {
                    while (watcherChangedQueue.Count > 0)
                        processWatcherChangedQueue();
                    t.start();
                }
            }
            log("main thread exiting");
        }
        internal void shutdown()
        {
            running = false;
        }
        private bool running = true;
        private ArrayList watcherChangedQueue = new ArrayList();
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
                if (Helpers.isPluginFile(pathRel) && Helpers.isServerPlugin(pathRel))
                    _pluginAdded(pathRel);
                files2.Add(pathRel, Encryption.md5_file(pathAbs));
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
        public void clientReport(Event ev)
        {
            foreach (Memory mem in ev._Memories)
            {
                string fileName_remote = mem.Name;
                string md5_remote = mem.Value;

                string src = path_cache + fileName_remote;
                if (!md5table.ContainsKey(fileName_remote))
                {
                    fileDeleteFromClient(fileName_remote, ev._Endpoint);
                    continue;
                }
                else
                {
                    if ((string)md5table[fileName_remote] != md5_remote)
                    {
                        fileSendToClient(fileName_remote, ev._Endpoint);
                    }
                }
            }
            foreach (DictionaryEntry de in md5table)
            {

                if (ev._Memories.IndexOf((string)de.Key) < 0)
                {
                    fileSendToClient((string)de.Key, ev._Endpoint);
                }
            }

        }
        private void fileSendToClient(string path, string endpoint)
        {
            byte[] bytes = Helpers.compress_gzip(Helpers.getFileBytes(path_cache + "\\" + path));
            Event outevent = new Event();
            outevent._Endpoint = endpoint;
            outevent._Keyword = KeyWord.CACHE_CLIENTUPDATEFILE;
            outevent._Memories = new Memories();
            outevent._Memories.Add(new Memory("file", KeyWord.NIL, path, bytes));
            toclient(outevent);
        }
        private void fileDeleteFromClient(string path, string endpoint)
        {
            Event outevent = new Event();
            outevent._Endpoint = endpoint;
            outevent._Keyword = KeyWord.CACHE_CLIENTDELETEFILE;
            outevent._Memories = new Memories();
            outevent._Memories.Add(new Memory("file", KeyWord.NIL, path));
            toclient(outevent);
        }
        private void fileRenameFromClient(string endpoint, string path, string newpath)
        {
            Event outevent = new Event();
            outevent._Endpoint = endpoint;
            outevent._Keyword = KeyWord.CACHE_CLIENTRENAMEFILE;
            outevent._Memories = new Memories();
            outevent._Memories.Add(new Memory("file", KeyWord.NIL, path));
            outevent._Memories.Add(new Memory("newfile", KeyWord.NIL, newpath));
            toclient(outevent);
        }
        public delegate void route_toclientDelegate(Event ev);
        public event route_toclientDelegate route_toclient;
        private void toclient(Event ev)
        {
            if (!object.Equals(null, this.route_toclient))
            {
                route_toclient(ev);
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
