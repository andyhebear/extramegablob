using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ExtraMegaBlob.References;
using Mogre;
using MogreFramework;
#pragma warning disable 162 //CS0162: Unreachable code detected
#pragma warning disable 168 //CS0168: The variable 'XYZ' is declared but never used
#pragma warning disable 169 //CS0169: Field 'XYZ' is never used
#pragma warning disable 414 //CS0414: XYZ is assigned but its value is never used
#pragma warning disable 649 //CS0649: Field 'XYZ' is never assigned to, and will always have its default value XX
namespace ExtraMegaBlob.Client
{
    public partial class Simulation
    {
        private CacheManager cache;
        private const bool DISABLE_NETWORK = false;
        private Config conf = null;
        public void main()
        {
            try
            {
                log(Program.header);

                conf = new Config();

                OgreWindow.Instance.textures = new Textures(ThingPath.path_cache);
                OgreWindow.Instance.meshes = new Meshes(ThingPath.path_cache);

                netClient = new ClientNetwork();
                netClient.onLogMessage += new ClientNetwork.LogDelegate(netClient_onLogMessage);
                netClient.onReceiveEvent += new ClientNetwork.onReceiveEventDelegate(netClient_onReceiveEvent);
                netClient.onConnectCompleted += new ClientNetwork.onConnectCompletedDelegate(netClient_onConnectCompleted);
                netClient.onDisconnected += new ClientNetwork.onDisconnectedDelegate(netClient_onDisconnected);


                ClientPluginManager = new ClientPluginManager();
                ClientPluginManager.onLogMessage += new LogDelegate(roomManager_onLogMessage);
                ClientPluginManager.onChat += new LogDelegate(ClientPluginManager_onChat);
                ClientPluginManager.route_toserver += new ClientPluginManager.route_toserver_delegate(clientPluginManager_route_toserver);
                ClientPluginManager.onListChanged += new ClientPluginManager.pluginListChangedHandler(ClientPluginManager_onListChanged);



                cache = new CacheManager();
                cache.onLogMessage += new LogDelegate(cache_onLogMessage);
                cache.route_toserver += new CacheManager.route_toserverDelegate(cache_route_toserver);
                cache.pluginAdded += new CacheManager.pluginAddedDelegate(cache_pluginAdded);
                cache.pluginDeleted += new CacheManager.pluginDeletedDelegate(cache_pluginDeleted);
                cache.textureAdded += new CacheManager.textureAddedDelegate(cache_textureAdded);
                cache.textureDeleted += new CacheManager.textureDeletedDelegate(cache_textureDeleted);
                cache.meshAdded += new CacheManager.meshAddedDelegate(cache_meshAdded);
                cache.meshDeleted += new CacheManager.meshDeletedDelegate(cache_meshDeleted);


                OgreWindow.Instance.SceneCreating += new OgreWindow.SceneEventHandler(SceneCreating);
                OgreWindow.Instance.InitializeOgre();
                OgreWindow.Instance.mRoot.FrameStarted += new FrameListener.FrameStartedHandler(Root_FrameStarted);
                OgreWindow.Instance.Text = Program.header;
                OgreWindow.Instance.onSend += new OgreWindow.sendDelegate(Instance_onSend);
                OgreWindow.Instance.FormClosing += new FormClosingEventHandler(mainwindow_FormClosing);
                if (OgreWindow.Instance.mRoot == null)
                {
                    MessageBox.Show("OgreWindow: not initialized yet");
                }
                OgreWindow.Instance.Show();
                timer t = new timer(new TimeSpan(0, 0, 2));
                t.start();
                try
                {
                    int loops;
                    bool b;
                    new Thread(new ThreadStart(netConnect)).Start();
                    cache.init();
                    foreach (string s in pluginAddQueue)
                    {
                        ClientPluginManager.addPlugin(s);
                    }
                    #region Primary Loop
                    while (!OgreWindow.Instance.ShuttingDown)
                    {
                        if (Mogre.OgreException.IsThrown)
                        {
                            string x = Mogre.OgreException.LastException.FullDescription;
                            log("[ main() ] " + x);
                            Mogre.OgreException.ClearLastException();
                        }
                        if (object.Equals(null, OgreWindow.Instance.mRoot)) break;
                        b = true;
                        loops = 0;
                        while (DateTime.Now.Ticks > next_game_tick && loops < MAX_FRAMESKIP)
                        {
                            update();
                            next_game_tick += SKIP_TICKS;
                            loops++;
                        }
                        interpolation = (float)(DateTime.Now.Ticks + SKIP_TICKS - next_game_tick) / (float)(SKIP_TICKS);
                        ExtraMegaBlob.References.Math.clamp_hi(1f, ref interpolation);
                        if (!OgreWindow.Instance.pauserendering)
                        {
                            OgreWindow.Instance.renderingframe = true;
                            b = OgreWindow.Instance.mRoot.RenderOneFrame();
                            OgreWindow.Instance.renderingframe = false;
                        }
                        if (!b) break;
                        OgreWindow.Instance.doEvents();
                    }
                    #endregion


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Main: " + ex.StackTrace.ToString() + Environment.NewLine + ex.Message);
                if (OgreException.IsThrown)
                    MessageBox.Show(OgreException.LastException.FullDescription,
                                    "An Ogre exception has occurred!");
            }
            quit();
        }
        private void quit()
        {
            if (OgreWindow.Instance.ShuttingDown) return;
            OgreWindow.Instance.ShuttingDown = true;
            ClientPluginManager.shutdown();
            netClient.disconnect();
            OgreWindow.Instance.saveFrame(SaveFrameFile);
            OgreWindow.Instance.Close();
            OgreWindow.Instance.meshes.shutdown();
            OgreWindow.Instance.textures.shutdown();
            OgreWindow.Instance.mRoot.Dispose();
            OgreWindow.Instance.mRoot = null;
            OgreWindow.Instance.mWindow = null;
            OgreWindow.Instance.mCamera = null;
            OgreWindow.Instance.mViewport = null;
            OgreWindow.Instance.mSceneMgr = null;
        }
        Entity ground_ent = null;
        SceneNode ground_node = null;
        void mainwindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            quit();
        }
        private timer saveScreenshotLimiter = new timer(new TimeSpan(0, 0, 0, 1));
        private void saveScreenshot()
        {
            if (saveScreenshotLimiter.elapsed)
            {
                saveScreenshotLimiter.start();
                try
                {
                    OgreWindow.Instance.saveFrame(SaveFrameFile);
                }
                catch { }
            }
        }
        private string SaveFrameFile
        {
            get
            {
                for (int i = 0; i > -1; i++)
                {
                    string part1 = Path.GetDirectoryName(Application.ExecutablePath) + "\\Screenshots\\frame";
                    string part2 = i.ToString("D3");
                    string part3 = ".jpg";
                    string cmp = part1 + part2 + part3;
                    if (!File.Exists(cmp))
                    {
                        return cmp;
                    }
                }
                return null;
            }
        }
        void SceneCreating()
        {

            // Set the ambient light and shadow technique
            //SceneManager mgr = win.SceneManager;
            OgreWindow.Instance.mSceneMgr.SetShadowUseInfiniteFarPlane(true);
            OgreWindow.Instance.mSceneMgr.AmbientLight = ColourValue.Black;
            OgreWindow.Instance.mSceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_ADDITIVE;
            //// Create a ninja
            //entMan1 = win.mSceneMgr.CreateEntity("zigzag", "zigzag.mesh");
            //entMan1.CastShadows = true;
            //entMan1.DisplaySkeleton = true;
            //snMan1 = win.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            //snMan1.AttachObject(entMan1);
            //snMan1.Translate(10.1f, 10.1f, 0.1f);
            ////// Create an earth
            //entity_myentity = win.mSceneMgr.CreateEntity("sphere", "sphere.mesh");
            //entity_myentity.CastShadows = true;
            //// entMan1.DisplaySkeleton = true;
            //scenenode_myscenenode = win.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            //scenenode_myscenenode.AttachObject(entity_myentity);
            //// snMan1.Translate(10.1f, 10.1f, 0.1f);
            //scenenode_myscenenode.Position -= new Mogre.Vector3(8f, 6f, 14f);
            ////snMan1.Rotate(new Quaternion(new Matrix
            //scenenode_myscenenode.Rotate(new Mogre.Vector3(2.0f, 0.0f, 0.0f), new Radian(0.5f));
            //newroom = new thing.newroom();
            //newroom.sceneHook(win);
            //SlotMachine = new thing.SlotMachine();
            //SlotMachine.sceneHook(win);
            //zel = new thing.zeliard();
            //zel.sceneHook(win);

            //ClientPluginManager.setWindow(win);

            //sn1.Rotate(new Mogre.Vector3(0.0f, 0.0f, 1.0f), new Radian(1.55f));
            // Create a pointy
            //entPointy = win.mSceneMgr.CreateEntity("pointy", "pointy.mesh");
            //entPointy.CastShadows = true;
            ////entPointy.
            //snPointy = win.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            //snPointy.AttachObject(entPointy);
            //snPointy.Translate(5.1f, 3.1f, 2.1f);
            //snPointy.Scale(9.5f, 9.5f, 9.5f);
            // Create another ninja
            //bot_ent = win.mSceneMgr.CreateEntity("ninja", "ninja.mesh");
            //bot_ent.CastShadows = true;
            //bot_node = win.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            //bot_node.AttachObject(bot_ent);
            // Define a ground plane
            Plane plane = new Plane(Mogre.Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                plane, 1500, 1500, 20, 20, true, 1, 5, 5, Mogre.Vector3.UNIT_Z);
            // Create a ground plane
            ground_ent = OgreWindow.Instance.mSceneMgr.CreateEntity("GroundEntity", "ground");
            ground_node = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            ground_node.AttachObject(ground_ent);
            ground_ent.SetMaterialName("Examples/Rockwall");
            ground_ent.CastShadows = false;
            ground_node.Position -= new Mogre.Vector3(0f, 10f, 0f);
            // Create the first light
            Light light;
            light = OgreWindow.Instance.mSceneMgr.CreateLight("Light1");
            light.Type = Light.LightTypes.LT_POINT;
            light.Position = new Mogre.Vector3(0, 150, 250);
            light.DiffuseColour = ColourValue.Red;
            light.SpecularColour = ColourValue.Red;
            // Create the second light
            light = OgreWindow.Instance.mSceneMgr.CreateLight("Light2");
            light.Type = Light.LightTypes.LT_DIRECTIONAL;
            light.DiffuseColour = new ColourValue(.25f, .25f, 0);
            light.SpecularColour = new ColourValue(.25f, .25f, 0);
            light.Direction = new Mogre.Vector3(0, -1, -1);
            // Create the third light
            light = OgreWindow.Instance.mSceneMgr.CreateLight("Light3");
            light.Type = Light.LightTypes.LT_SPOTLIGHT;
            light.DiffuseColour = ColourValue.White;
            light.SpecularColour = ColourValue.White;
            light.Direction = new Mogre.Vector3(-1, -1, 0);
            light.Position = new Mogre.Vector3(300, 300, 0);
            light.SetSpotlightRange(new Degree(35), new Degree(50));
            //win.Camera.SetAutoTracking(true, snMan1, new Mogre.Vector3(1, 1, 1));
            // win.SceneManager.SetWorldGeometry("terrain.cfg");
            //win.SceneManager.SetSkyBox(true, "Examples/SpaceSkyBox", 5000, false);

            try
            {
                OgreWindow.Instance.mSceneMgr.SetSkyBox(true, "Examples/StormySkyBox", 5000, false);
            }
            catch (Exception ex) { log(ex.Message); }
            OgreWindow.Instance.SceneReady = true;
        }
        void ClientPluginManager_onListChanged(string[] plugins)
        {
            OgreWindow.Instance.setPluginsActive(plugins);
        }
        void cache_meshDeleted(string pathRelMeshFile)
        {
            OgreWindow.Instance.pause();
            OgreWindow.Instance.meshes.RemoveAt(pathRelMeshFile);
            OgreWindow.Instance.unpause();
        }
        void cache_meshAdded(string pathRelMeshFile)
        {
            OgreWindow.Instance.pause();
            OgreWindow.Instance.meshes.Add(pathRelMeshFile);
            OgreWindow.Instance.unpause();
        }
        void cache_textureDeleted(string pathRelTextureFile)
        {
            OgreWindow.Instance.pause();
            OgreWindow.Instance.textures.RemoveAt(pathRelTextureFile);
            OgreWindow.Instance.unpause();
        }
        void cache_textureAdded(string pathRelTextureFile)
        {
            OgreWindow.Instance.pause();
            OgreWindow.Instance.textures.Add(pathRelTextureFile);
            OgreWindow.Instance.unpause();
        }
        void netClient_onDisconnected()
        {
            // ClientPluginManager.delAllPlugins();
        }
        void cache_pluginDeleted(string pathRelPluginFile)
        {
            ClientPluginManager.delPlugin(pathRelPluginFile);
        }
        void cache_pluginAdded(string pathRelPluginFile)
        {
            //ClientPluginManager.addPlugin(pathRelPluginFile);
            pluginAddQueue.Add(pathRelPluginFile);
        }
        private ArrayList pluginAddQueue = new ArrayList();
        void netClient_onConnectCompleted(string host, string port)
        {
            log("Connected to: " + host + ":" + port);

            cache.sendReport();
        }
        void cache_route_toserver(Event ev)
        {
            ClientPluginManager.sourceHub(ev, EventTransfer.CLIENTTOSERVER);
        }
        private void log(string what)
        {
            OgreWindow.Instance.log(what);
        }
        void cache_onLogMessage(string msg)
        {
            log("cache: " + msg);
        }
        public void netConnect()
        {
            Thread.CurrentThread.Name = "net connect loop";
            if (DISABLE_NETWORK)
            {
                log("Network is DISABLED");
                return;
            }
            while (!OgreWindow.Instance.ShuttingDown)
            {

                if (!netClient.connected && OgreWindow.Instance.SceneReady)
                {
                    try
                    {
                        netClient.connect(conf.serverip_clientcontext, conf.serverport);
                    }
                    catch (Exception ex)
                    {
                        log(ex.Message);
                    }
                }
                Thread.Sleep(1000);
            }
        }
        void Instance_onSend(string text)
        {
            Event e = new Event();
            e._IntendedRecipients = eventScope.SERVERALL;
            e._Keyword = KeyWord.EVENT_CHATMESSAGE;
            e._Source_FullyQualifiedName = "ClientMain";
            e._Memories = new Memories();
            e._Memories.Add(new Memory("text", KeyWord.NIL, text, null));
            ClientPluginManager.sourceHub(e, EventTransfer.CLIENTTOSERVER);
        }
        void ClientPluginManager_onChat(string msg)
        {
            OgreWindow.Instance.addChatMessage(msg);
        }

        private ClientNetwork netClient;
        void netClient_onReceiveEvent(Event msg)
        {
            ClientPluginManager.sourceHub(msg, EventTransfer.SERVERTOCLIENT);
            if (msg._Keyword == KeyWord.CACHE_CLIENTRENAMEFILE)
            {
                cache.renameFile(msg);
            }
            if (msg._Keyword == KeyWord.CACHE_CLIENTDELETEFILE)
            {
                cache.deleteFile(msg);
            }
            if (msg._Keyword == KeyWord.CACHE_CLIENTUPDATEFILE)
            {
                cache.updateFile(msg);
            }
        }
        void clientPluginManager_route_toserver(Event msg)
        {
            if (!netClient.connected)
            {
                return;
            }
            netClient.sendEvent(msg);
        }
        void netClient_onLogMessage(string msg)
        {
            OgreWindow.Instance.log("[netClient]: " + msg);
        }
        void roomManager_onLogMessage(string msg)
        {
            OgreWindow.Instance.log(msg);
        }
        public int TICKS_PER_SECOND { get { return 100; } }
        public int SKIP_TICKS { get { return (10000000 / TICKS_PER_SECOND); } }
        public int MAX_FRAMESKIP { get { return 5; } }
        long next_game_tick = DateTime.Now.Ticks;
        private float interpolation = 0f;
        private ClientPluginManager ClientPluginManager;
        private bool Root_FrameStarted(FrameEvent evt)
        {
            try
            {
                ClientPluginManager.FrameStartedHooks(interpolation);
            }
            catch
            {
            }
            return true;
        }

        private void update()
        {
            try
            {
                ClientPluginManager.updateHooks();
                if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_ESCAPE))
                {
                    quit();
                }
            }
            catch { }
        }
    }
}
