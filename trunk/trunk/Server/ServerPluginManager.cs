using System;
using System.Collections;
using System.IO;
using System.Threading;
using ExtraMegaBlob.References;
namespace ExtraMegaBlob.Server
{
    public class ServerPluginManager
    {
        public void mainLoop()
        {
            log("main thread running");
            Thread.CurrentThread.Name = "Plugin Manager mainLoop";
            timer t = new timer(new TimeSpan(0, 0, 1));
            while (running)
            {
                Thread.Sleep(100);
                if (t.elapsed)
                {
                    //detectPluginChanges();
                    t.start();
                }
            }
            log("main thread exiting");
        }
        public void addPlugin(string pathRel)
        {
            string pathAbs = path_cache + pathRel;
            ServerPlugin plugin = compiler.CompileCode(pathAbs);
            if (!object.Equals(null, plugin))
            {
                pluginNameLookup[pathRel] = plugin.Name();
                addServerPlugin(plugin);
            }
        }
        Hashtable pluginNameLookup = new Hashtable(); // relative path = plugin name
        internal void addServerPlugin(ServerPlugin ServerPlugin)
        {
            ServerPlugin.onQuit += new LogDelegate(ServerPlugin_onQuit);
            ServerPlugin.onLog += new LogDelegate(delegate(string s) { log("[" + ServerPlugin.Name() + "]: " + s); });
            ServerPlugin.onOutboxMessage += new ServerPlugin.outboxDelegate(plugin_onOutboxMessage);
            ServerClasses.Add(ServerPlugin);
        }
        public void delPlugin(string pathRel)
        {
            ServerClasses.RemoveAt((string)pluginNameLookup[pathRel]);
        }
        public ServerPluginManager(string path_cache)
        {
            this.path_cache = path_cache;
            compiler.onLog += new LogDelegate(compiler_onLog);
        }
        public ServerPluginManager()
        {
            this.path_cache = ThingPath.path_servercache;
            compiler.onLog += new LogDelegate(compiler_onLog);
        }
        private string path_cache = "";
        public void detectPluginChanges()
        {
            //string pluginPath = ThingReferences.ThingPath.path_base +
            //                    System.IO.Path.DirectorySeparatorChar +
            //                    "plugins";
            foreach (DirectoryInfo di in new DirectoryInfo(path_cache).GetDirectories())
            {
                string pluginFolderPath = di.FullName;
                foreach (FileInfo fi in new DirectoryInfo(pluginFolderPath).GetFiles("*.*"))
                {
                    string ext = Path.GetExtension(fi.Name);
                    if (fi.Name.ToLower().Contains("_server") && (ext == ".cs" || ext == ".vb" || ext == ".js"))
                    {
                        string path_plugin = fi.FullName;
                        try
                        {
                            string md5_plugin = Encryption.md5_file(path_plugin);
                            bool doCompile = false;
                            if (!pluginFileHashes.ContainsKey(path_plugin))
                                doCompile = true;
                            else
                                if (((string)pluginFileHashes[path_plugin]) != md5_plugin)
                                    doCompile = true;
                            if (doCompile)
                            {
                                ServerPlugin plugin = compiler.CompileCode(path_plugin);
                                if (!object.Equals(null, plugin))
                                {
                                    if (pluginFileHashes.ContainsKey(path_plugin))
                                    {
                                        ServerClasses.RemoveAt(plugin.Name());
                                    }
                                    pluginFileHashes[path_plugin] = md5_plugin;
                                    //pluginAddResourceFolderPath(pluginFolderPath);
                                    addServerPlugin(plugin);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log(ex.Message);
                        }
                    }
                }
            }
        }
        private Hashtable pluginFileHashes = new Hashtable();
        internal void init()
        {
            this.log("Plugin Manager initializing");
        }
        ServerPluginCompiler compiler = new ServerPluginCompiler();
        void compiler_onLog(string msg)
        {
            log("[compiler]: " + msg);
        }
        private ServerPlugins ServerClasses = new ServerPlugins();
        #region routing
        void plugin_onOutboxMessage(ServerPlugin Sender, Event ev)
        {
            ev._Source_FullyQualifiedName = Sender.Name();
            sourceHub(ev, EventTransfer.SERVERTOCLIENT);
        }
        public void sourceHub(Event ev, EventTransfer transfer)
        {
            string l = "";
            if (transfer == EventTransfer.SERVERTOCLIENT && ev._Endpoint != "")
                l = " -> " + ev._Endpoint;
            log(ev._Keyword.ToString() + " -> " + transfer.ToString() + l);
            switch (transfer)
            {
                case EventTransfer.TRASH:
                    return;
                case EventTransfer.CLIENTTOCLIENT:
                    return;
                case EventTransfer.CLIENTTOSERVER:
                    route_toAllServers(ev);
                    return;
                case EventTransfer.SERVERTOCLIENT:
                    _route_toclient(ev);
                    return;
                case EventTransfer.SERVERTOSERVER:
                    return;
            }
        }
        private void route_toAllServers(Event ev2)
        {
            for (int i = 0; i < ServerClasses.Count; i++)
            {
                if (ServerClasses[i] != null)
                {
                    bool found = false;
                    foreach (string allowedSubscriber in ((ServerPlugin)ServerClasses[i]).AllowedInputNames())
                    {
                        if (ev2._Source_FullyQualifiedName == allowedSubscriber)
                        {
                            found = true;
                        }
                    }
                    found = true;//bypass security for now
                    if (found)
                    {
                        ((ServerPlugin)ServerClasses[i]).inbox(ev2);
                    }
                }
            }
        }
        private void route_toSingleServer(Event ev2)//to ram
        {
            string sourceName = ev2._Source_FullyQualifiedName;
            for (int i = 0; i < ServerClasses.Count; i++)
            {
                if (ServerClasses[i] != null)
                {
                    string subscribername = ((ServerPlugin)ServerClasses[i]).Name();
                    string[] inputwhitelist = ((ServerPlugin)ServerClasses[i]).AllowedInputNames();
                    bool found = false;
                    foreach (string allowedSubscriber in inputwhitelist)
                    {
                        if (sourceName == allowedSubscriber)
                        {
                            found = true;
                        }
                    }
                    if (found)
                    {
                        _toSingleServer(ev2, subscribername);
                    }
                }
            }
        }
        private void _toSingleServer(Event ev2, string roomName) //to ram
        {
            string[] inputNames = ((ServerPlugin)ServerClasses[roomName]).AllowedInputNames();
            bool found = false;
            foreach (string allowedSubscriber in inputNames)
            {
                if (ev2._Source_FullyQualifiedName == allowedSubscriber)
                {
                    found = true;
                }
            }
            if (found)
            {
                ((ServerPlugin)ServerClasses[roomName]).inbox(ev2);
            }
        }
        #region delegates
        public delegate void route_toclient_delegate(Event msg);
        public event route_toclient_delegate route_toclient;
        private void _route_toclient(Event msg)
        {
            if (!object.Equals(null, this.route_toclient))
            {
                route_toclient(msg);
            }
        }
        #endregion
        #endregion
        private bool running = true;
        internal void shutdown()
        {
            ServerClasses.Shutdown();
            running = false;
        }
        internal void updateHooks()
        {
            ServerClasses.UpdateHooks();
        }
        void ServerPlugin_onLog(string msg)
        {
            log(msg);
        }
        void ServerPlugin_onQuit(string msg)
        {
            log(msg);
        }
        public event LogDelegate onLogMessage;
        internal void log(string msg)
        {
            if (!object.Equals(null, this.onLogMessage))
            {
                onLogMessage(msg);
            }
        }
    }
}