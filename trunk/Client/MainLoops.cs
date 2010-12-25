using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ExtraMegaBlob.References;
using Mogre;
using MogreFramework;
#region disable annoying warnings
#pragma warning disable 162 //CS0162: Unreachable code detected
#pragma warning disable 168 //CS0168: The variable 'XYZ' is declared but never used
#pragma warning disable 169 //CS0169: Field 'XYZ' is never used
#pragma warning disable 414 //CS0414: 'XYZ' is assigned but its value is never used
#pragma warning disable 649 //CS0649: Field 'XYZ' is never assigned to, and will always have its default value XX
#endregion
namespace ExtraMegaBlob.Client
{
    public partial class Simulation
    {
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

                //LogManager lm = new LogManager();

                
                OgreWindow.Instance.SceneCreating += new OgreWindow.SceneEventHandler(SceneCreating);
                OgreWindow.Instance.InitializeOgre();
                LogManager.Singleton.DefaultLog.MessageLogged += new LogListener.MessageLoggedHandler(DefaultLog_MessageLogged);
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
                    new Thread(new ThreadStart(checkPluginAddQueueLoop)).Start();
                    new Thread(new ThreadStart(checkNetUpdateFileQueueLoop)).Start();
                    cache.init();
                    #region Primary Loop
                    while (!OgreWindow.Instance.ShuttingDown)
                    {
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
        void DefaultLog_MessageLogged(string message, LogMessageLevel lml, bool maskDebug, string logName)
        {
            
            log(lml.ToString()+": "+message);
        }
        private bool Root_FrameStarted(FrameEvent evt)
        {
            try
            {
                ClientPluginManager.FrameStartedHooks(interpolation);
            }
            catch
            {
                log("[ main() ] FrameStarted exception while doing plugins' hooks");
            }
            return true;
        }
        private void update()
        {
            checkOgreException();
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
        private void quit()
        {
            if (OgreWindow.Instance.ShuttingDown) return;
            OgreWindow.Instance.ShuttingDown = true;
            ClientPluginManager.shutdown();
            netClient.disconnect();
            saveScreenshot();
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
        private CacheManager cache;
        private ClientPluginManager ClientPluginManager;
        private ClientNetwork netClient;
        private const bool DISABLE_NETWORK = false;
        private Config conf = null;
        private Entity ground_ent = null;
        private SceneNode ground_node = null;
        private void mainwindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            quit();
        }
        private timer saveScreenshotLimiter = new timer(new TimeSpan(0, 0, 0, 1));
        private void checkPluginAddQueueLoop()
        {
            Thread.CurrentThread.Name = "plugin add queue loop";
            while (!OgreWindow.Instance.ShuttingDown)
            {
                Thread.Sleep(100);
                for (int i = 0; i < pluginAddQueue.Count; i++)
                {
                    string pathRel = (string)pluginAddQueue[i];
                    ClientPluginManager.addPlugin(pathRel);
                    pluginAddQueue.RemoveAt(i);
                    break;
                }
            }
        }
        private void checkNetUpdateFileQueueLoop()
        {
            Thread.CurrentThread.Name = "file update queue loop";
            while (!OgreWindow.Instance.ShuttingDown)
            {
                Thread.Sleep(100);
                for (int i = 0; i < netUpdateFileQueue.Count; i++)
                {
                    Event msg = (Event)netUpdateFileQueue[i];
                    try
                    {
                        cache.updateFile(msg);
                    }
                    catch (Exception ex)
                    {
                        log(ex.ToString());
                    }
                    netUpdateFileQueue.RemoveAt(i);
                    break;
                }
            }
        }
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
        private void SceneCreating()
        {
            //OgreWindow.Instance.mSceneMgr.SetShadowUseInfiniteFarPlane(true);
            OgreWindow.Instance.mSceneMgr.AmbientLight = ColourValue.Black;
            OgreWindow.Instance.mSceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_ADDITIVE;
            OgreWindow.Instance.SceneReady = true;
        }
        private void checkOgreException()
        {
            if (Mogre.OgreException.IsThrown)
            {
                string a = "";
                OgreException x = Mogre.OgreException.LastException;
                a += "Source: " + x.Source + "; Number: " + x.Number.ToString() + "; Description: " + x.Description;
                log("[ main() ] " + a);
                Mogre.OgreException.ClearLastException();
            }
        }
        private void ClientPluginManager_onListChanged(string[] plugins)
        {
            OgreWindow.Instance.setPluginsActive(plugins);
        }
        private void cache_meshDeleted(string pathRelMeshFile)
        {
            OgreWindow.Instance.pause();
            OgreWindow.Instance.meshes.RemoveAt(pathRelMeshFile);
            OgreWindow.Instance.unpause();
        }
        private void cache_meshAdded(string pathRelMeshFile)
        {
            OgreWindow.Instance.pause();
            OgreWindow.Instance.meshes.Add(pathRelMeshFile);
            OgreWindow.Instance.unpause();
        }
        private void cache_textureDeleted(string pathRelTextureFile)
        {
            OgreWindow.Instance.pause();
            OgreWindow.Instance.textures.RemoveAt(pathRelTextureFile);
            OgreWindow.Instance.unpause();
        }
        private void cache_textureAdded(string pathRelTextureFile)
        {
            OgreWindow.Instance.pause();
            OgreWindow.Instance.textures.Add(pathRelTextureFile);
            OgreWindow.Instance.unpause();
        }
        private void netClient_onDisconnected()
        {
            // ClientPluginManager.delAllPlugins();
        }
        private void cache_pluginDeleted(string pathRelPluginFile)
        {
            ClientPluginManager.delPlugin(pathRelPluginFile);
        }
        private void cache_pluginAdded(string pathRelPluginFile)
        {
            //ClientPluginManager.addPlugin(pathRelPluginFile);
            pluginAddQueue.Add(pathRelPluginFile);
        }
        private ArrayList pluginAddQueue = new ArrayList();
        private ArrayList netUpdateFileQueue = new ArrayList();
        private void netClient_onConnectCompleted(string host, string port)
        {
            log("Connected to: " + host + ":" + port);

            cache.sendReport();
        }
        private void cache_route_toserver(Event ev)
        {
            ClientPluginManager.sourceHub(ev, EventTransfer.CLIENTTOSERVER);
        }
        private void log(string what)
        {
            OgreWindow.Instance.log(what);
        }
        private void cache_onLogMessage(string msg)
        {
            log("cache: " + msg);
        }
        private void netConnect()
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
        private void Instance_onSend(string text)
        {
            Event e = new Event();
            e._IntendedRecipients = eventScope.SERVERALL;
            e._Keyword = KeyWord.EVENT_CHATMESSAGE;
            e._Source_FullyQualifiedName = "ClientMain";
            e._Memories = new Memories();
            e._Memories.Add(new Memory("text", KeyWord.NIL, text, null));
            ClientPluginManager.sourceHub(e, EventTransfer.CLIENTTOSERVER);
        }
        private void ClientPluginManager_onChat(string msg)
        {
            OgreWindow.Instance.addChatMessage(msg);
        }
        private void netClient_onReceiveEvent(Event msg)
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
                //cache.updateFile(msg);
                netUpdateFileQueue.Add(msg);
            }
        }
        private void clientPluginManager_route_toserver(Event msg)
        {
            if (!netClient.connected)
            {
                return;
            }
            netClient.sendEvent(msg);
        }
        private void netClient_onLogMessage(string msg)
        {
            OgreWindow.Instance.log("[netClient]: " + msg);
        }
        private void roomManager_onLogMessage(string msg)
        {
            OgreWindow.Instance.log(msg);
        }
        private int TICKS_PER_SECOND { get { return 100; } }
        private int SKIP_TICKS { get { return (10000000 / TICKS_PER_SECOND); } }
        private int MAX_FRAMESKIP { get { return 5; } }
        private long next_game_tick = DateTime.Now.Ticks;
        private float interpolation = 0f;

    }
}
