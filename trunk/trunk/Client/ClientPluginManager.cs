using System;
using System.Collections;
using ThingReferences;
namespace thing
{
    public class ClientPluginManager
    {
        public string[] allPlugins
        {
            get
            {
                ArrayList al = new ArrayList();
                foreach (DictionaryEntry de in pluginNameLookup)
                {
                    al.Add(de.Key + " => " + de.Value);
                }
                return (String[])al.ToArray(typeof(string));
            }
        }
        internal void addClientPlugin(ClientPlugin ClientPlugin)
        {
            ClientPlugin.onQuit += new LogDelegate(ClientPlugin_onQuit);
            LogDelegate testDelB = delegate(string s) { log("[" + ClientPlugin.Name() + "] " + s); };
            ClientPlugin.onLog += new LogDelegate(testDelB);
            ClientPlugin.onChat += new LogDelegate(ClientPlugin_onChat);
            ClientPlugin.onOutboxMessage += new ThingReferences.ClientPlugin.outboxDelegate(ClientPlugin_onOutboxMessage);
            try { ClientPlugin.startup(); }
            catch (Exception ex) { log("[" + ClientPlugin.Name() + "] " + ex.Message); }
            ClientClasses.Add(ClientPlugin);
        }
        public void delAllPlugins()
        {
            while (pluginNameLookup.Count > 0)
            {
                foreach (DictionaryEntry de in pluginNameLookup)
                {
                    string plugName = (string)pluginNameLookup[de.Key];
                    pluginNameLookup.Remove(de.Key);
                    ClientClasses.RemoveAt(plugName);
                    listChanged(this.allPlugins);
                    break;
                }
            }
        }
        public void delPlugin(string pathRel)
        {
            ClientClasses.RemoveAt((string)pluginNameLookup[pathRel]);

            pluginNameLookup.Remove(pathRel);
            listChanged(this.allPlugins);
        }
        public void addPlugin(string pathRel)
        {
            string pathAbs = path_cache + pathRel;
            ClientPlugin plugin = ClientPluginCompiler.CompileCode(pathAbs);
            if (!object.Equals(null, plugin))
            {
                pluginNameLookup[pathRel] = plugin.Name();
                listChanged(this.allPlugins);
                addClientPlugin(plugin);
            }
        }

        public delegate void pluginListChangedHandler(string[] plugins);
        public event pluginListChangedHandler onListChanged;
        private void listChanged(string[] plugins)
        {
            if (!object.Equals(null, this.onListChanged))
            {
                onListChanged(plugins);
            }
        }

        private Hashtable pluginNameLookup = new Hashtable(); // relative path = plugin name
        public ClientPluginManager(string path_cache)
        {
            this.path_cache = path_cache;
            ClientPluginCompiler.onLog += new LogDelegate(compiler_onLog);
        }
        public ClientPluginManager()
        {
            this.path_cache = ThingPath.path_cache;
            ClientPluginCompiler.onLog += new LogDelegate(compiler_onLog);
        }
        private string path_cache = "";
        private void compiler_onLog(string msg)
        {
            log("[compiler]: " + msg);
        }
        private thing.Parts.ClientPlugins ClientClasses = new thing.Parts.ClientPlugins();
        private ArrayList ResourceFolderPaths = new ArrayList();
        public delegate void onPluginAddResourceFolderPathDelegate(string ResourceFolderPath);
        private ClientPluginCompiler ClientPluginCompiler = new ClientPluginCompiler();
        internal void FrameStartedHooks(float interpolation)
        {
            ClientClasses.FrameStartedHooks(interpolation);

        }
        internal void shutdown()
        {
            if (g) return;
            g = true;
            ClientClasses.Shutdown();
        }
        private bool g = false;
        internal void updateHooks()
        {
            //Thread t = new Thread(new ThreadStart(_updateHooks));
            //t.Start();
            ClientClasses.UpdateHooks();
        }
        private void _updateHooks()
        {
            ClientClasses.UpdateHooks();
        }
        private Random ran = new Random((int)DateTime.Now.Ticks);
        private void ClientPlugin_onOutboxMessage(ClientPlugin Sender, Event ev)
        {
            sourceHub(ev, EventTransfer.CLIENTTOSERVER);
        }
        private void ClientPlugin_onChat(string msg)
        {
            chat(Config.logPrefix + msg);
        }
        public event LogDelegate onChat;
        public void chat(string msg)
        {
            if (!object.Equals(null, this.onChat))
            {
                onChat(msg);
            }
        }
        #region routing
        public void sourceHub(Event ev, EventTransfer transfer)
        {
            //log(ev._Keyword.ToString() + " -> " + transfer.ToString());
            switch (transfer)
            {
                case EventTransfer.TRASH:
                    return;
                case EventTransfer.CLIENTTOCLIENT:
                    return;
                case EventTransfer.CLIENTTOSERVER:
                    _route_toServer(ev);
                    return;
                case EventTransfer.SERVERTOCLIENT:
                    route_toAllClients(ev);
                    return;
                case EventTransfer.SERVERTOSERVER:
                    return;
            }
        }
        private void route_toAllClients(Event ev2)
        {
            for (int i = 0; i < ClientClasses.Count; i++)
            {
                if (ClientClasses[i] != null)
                {
                    bool found = false;
                    foreach (string allowedSubscriber in ((ClientPlugin)ClientClasses[i]).AllowedInputNames())
                    {
                        if (ev2._Source_FullyQualifiedName == allowedSubscriber)
                        {
                            found = true;
                        }
                    }
                    if (found)
                    {
                        ((ClientPlugin)ClientClasses[i]).inbox(ev2);
                    }
                }
            }
        }
        private void route_tosingleclient(Event ev2)//to ram
        {
            string sourceName = ev2._Source_FullyQualifiedName;
            for (int i = 0; i < ClientClasses.Count; i++)
            {
                if (ClientClasses[i] != null)
                {
                    string subscribername = ((ClientPlugin)ClientClasses[i]).Name();
                    string[] inputwhitelist = ((ClientPlugin)ClientClasses[i]).AllowedInputNames();
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
                        _toSingleClient(ev2, subscribername);
                    }
                }
            }
        }
        private void _toSingleClient(Event ev2, string roomName) //to ram
        {
            string[] inputNames = ((ClientPlugin)ClientClasses[roomName]).AllowedInputNames();
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
                ((ClientPlugin)ClientClasses[roomName]).inbox(ev2);
            }
        }
        public delegate void route_toserver_delegate(Event msg);
        public event route_toserver_delegate route_toserver;
        private void _route_toServer(Event msg)
        {
            if (!object.Equals(null, this.route_toserver))
            {
                route_toserver(msg);
            }
        }
        #endregion
        private void ClientPlugin_onLog(string msg)
        {
            log(msg);
        }
        private void ClientPlugin_onQuit(string msg)
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