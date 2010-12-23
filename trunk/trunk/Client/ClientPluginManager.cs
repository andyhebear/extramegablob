using System;
using System.Collections;
using ExtraMegaBlob.References;
using Mogre;
using MogreFramework;
namespace ExtraMegaBlob.Client
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
        internal bool addClientPlugin(ClientPlugin ClientPlugin)
        {
            ClientPlugin.onQuit += new LogDelegate(ClientPlugin_onQuit);
            LogDelegate testDelB = delegate(string s) { log("[" + ClientPlugin.Name() + "] " + s); };
            ClientPlugin.onLog += new LogDelegate(testDelB);
            ClientPlugin.onChat += new LogDelegate(ClientPlugin_onChat);
            ClientPlugin.onOutboxMessage += new ExtraMegaBlob.References.ClientPlugin.outboxDelegate(ClientPlugin_onOutboxMessage);
            try { ClientPlugin.startup(); }
            catch (Exception ex)
            {
                log("[" + ClientPlugin.Name() + "] " + ex.ToString());
                return false;
            }
            ClientClasses.Add(ClientPlugin);
            return true;
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
        public bool delPlugin(string pathRel)
        {
            string pluginName = (string)pluginNameLookup[pathRel];
            try
            {
                ClientClasses.RemoveAt(pluginName);
            }
            catch (Exception ex)
            {
                log("[" + pluginName + "] " + ex.ToString());
                return false;
            }

            pluginNameLookup.Remove(pathRel);
            listChanged(this.allPlugins);
            return true;
        }
        public void addPlugin(string pathRel)
        {
            string pathAbs = path_cache + pathRel;
            ClientPlugin plugin = ClientPluginCompiler.CompileCode(pathAbs);
            if (!object.Equals(null, plugin))
            {
                pluginNameLookup[pathRel] = plugin.Name();
                if (addClientPlugin(plugin))
                {
                    addPluginSphere(plugin.Name());
                }

                listChanged(this.allPlugins);
            }
        }
        string sphereNamePrefix = "";
        private void addPluginSphere(string pluginName)
        {
            string meshName = sphereNamePrefix + "_SphereMesh_" + pluginName;
            string entityName = sphereNamePrefix + "_SphereEntity_" + pluginName;
            string materialName = sphereNamePrefix + "_SphereMaterial_" + pluginName;

            ((MaterialPtr)MaterialManager.Singleton.Create(materialName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\normalNoiseColor.png");

            //  ClientClasses[pluginName].Location

            Textures t = OgreWindow.Instance.textures;

            PrimitiveGenerators.CreateSphere(meshName, ClientClasses[pluginName].Radius(), 8, 8);
            Entity sphereEntity = OgreWindow.Instance.mSceneMgr.CreateEntity(entityName, meshName);
            SceneNode sphereNode = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sphereEntity.SetMaterialName(materialName);
            sphereNode.AttachObject(sphereEntity);
            sphereEntity.CastShadows = false;
            sphereNode.Position = ClientClasses[pluginName].Location().toMogre;
            Helpers.setEntityOpacity(sphereEntity, .9f);
            //sphereNode.SetScale(new Mogre.Vector3(2f));


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
            init(path_cache);
        }
        public ClientPluginManager()
        {
            init(ThingPath.path_cache);
        }
        private void init(string path_cache)
        {
            this.path_cache = path_cache;
            ClientPluginCompiler.onLog += new LogDelegate(compiler_onLog);
            ClientClasses.onLogMessage += new LogDelegate(ClientClasses_onLogMessage);
            sphereNamePrefix = ran.Next(int.MaxValue).ToString();
        }

        void ClientClasses_onLogMessage(string msg)
        {
            log("[ClientClasses] " + msg);
        }
        private string path_cache = "";
        private void compiler_onLog(string msg)
        {
            log("[compiler]: " + msg);
        }
        private ClientPlugins ClientClasses = new ClientPlugins();
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