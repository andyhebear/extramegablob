using System;
using System.IO;
using System.Windows.Forms;
using MogreFramework;
using ThingReferences;
using Mogre;
using System.Threading;
using MOIS;
using System.Collections;
#pragma warning disable 162 //warning CS0162: Unreachable code detected
#pragma warning disable 168 //CS0168: The variable 'ex' is declared but never used
namespace thing
{
    public partial class Simulation
    {
        private CacheManager cache;
        private const bool DISABLE_NETWORK = false;
        public void main()
        {
            try
            {
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
                ClientPluginManager.onListChanged += new thing.ClientPluginManager.pluginListChangedHandler(ClientPluginManager_onListChanged);



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
                OgreWindow.Instance.Text = "thing that was built on: " + DateCompiled().ToString();
                OgreWindow.Instance.onSend += new OgreWindow.sendDelegate(Instance_onSend);
                OgreWindow.Instance.FormClosing += new FormClosingEventHandler(mainwindow_FormClosing);
                if (OgreWindow.Instance.mRoot == null)
                {
                    MessageBox.Show("OgreWindow: not initialized yet");
                }
                OgreWindow.Instance.Show();
                timer t = new timer(new TimeSpan(0, 0, 2));
                t.start();
                OgreWindow.g_m.MouseMoved += new MouseListener.MouseMovedHandler(g_m_MouseMoved);
                try
                {
                    int loops;
                    bool b;

                    Thread th = new Thread(new ThreadStart(netConnect));
                    th.Name = "net connect loop";
                    th.Start();


                    cache.init();
                    foreach (string s in pluginAddQueue)
                    {
                        ClientPluginManager.addPlugin(s);
                    }
                    ClientPluginManager.addClientPlugin(new SecretClientPlugin());//it doesn't show up in the client plugin manager's active plugin list

                    while (true && !OgreWindow.Instance.ShuttingDown) //MAIN LOOP
                    {
                        if (object.Equals(null, OgreWindow.Instance.mRoot)) break;
                        b = true;
                        try
                        {
                            loops = 0;
                            while (DateTime.Now.Ticks > next_game_tick && loops < MAX_FRAMESKIP)
                            {
                                update();
                                next_game_tick += SKIP_TICKS;
                                loops++;
                            }
                            interpolation = (float)(DateTime.Now.Ticks + SKIP_TICKS - next_game_tick) / (float)(SKIP_TICKS);
                            ThingReferences.Math.clamp_hi(1f, ref interpolation);


                            if (!OgreWindow.Instance.pauserendering)
                            {
                                OgreWindow.Instance.renderingframe = true;
                                b = OgreWindow.Instance.mRoot.RenderOneFrame();
                                OgreWindow.Instance.renderingframe = false;
                            }
                        }
                        catch
                        {
                            // MessageBox.Show("main loop1: " + ex.Message);
                        }
                        if (!b) break;
                        //System.Windows.Forms.Application.DoEvents();
                        OgreWindow.Instance.doEvents();
                    }
                    OgreWindow.Instance.saveFrame(SaveFrameFile);
                    OgreWindow.Instance.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // MessageBox.Show("main loop2: " + ex.Message);
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
        private string mainName = "ClientMain";
        void Instance_onSend(string text)
        {
            Event e = new Event();
            e._IntendedRecipients = eventScope.SERVERALL;
            e._Keyword = KeyWord.EVENT_CHATMESSAGE;
            e._Source_FullyQualifiedName = mainName;
            e._Memories = new Memories();
            e._Memories.Add(new Memory("text", KeyWord.NIL, text, null));
            //ClientPluginManager.EventFromCloudAll(e);
            //ClientPluginManager.EventFromCloudSourceSpecify(e);
            ClientPluginManager.sourceHub(e, EventTransfer.CLIENTTOSERVER);
        }
        void ClientPluginManager_onChat(string msg)
        {
            OgreWindow.Instance.addChatMessage(msg);
        }
        bool g_m_MouseMoved(MouseEvent arg)
        {
            MouseState_NativePtr s = arg.state;
            //OgreWindow.Instance.log(s.X.abs.ToString());
            if (arg.state.buttons == 2)
            {
                OgreWindow.Instance.cameraYawNode.Yaw(-s.X.rel * RotateScale_Camera);
                OgreWindow.Instance.cameraRollNode.Pitch(-s.Y.rel * RotateScale_Camera);
            }
            Mogre.Vector3 oldpos = OgreWindow.Instance.cameraNode.Position;
            float mouseZ = (float)arg.state.Z.rel * .1f;
            if (0 != mouseZ)
            {
                MoveScale_Camera_updown -= mouseZ;
            }
            //OgreWindow.Instance.log(arg.state.Z.abs.ToString());
            return true;
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
            TranslateVector_Camera.z += MoveScale_Camera_forwardback * (interpolation + 1);
            TranslateVector_Camera.x += MoveScale_Camera_leftright * (interpolation + 1);
            if (MoveScale_Camera_updown != 0)
            {
                float s = MoveScale_Camera_updown * (interpolation + 1);
                TranslateVector_Camera.y += s;
                MoveScale_Camera_updown -= s;
            }
            //mainwindow.cameraYawNode.Yaw(-inputMouse.MouseState.X.rel * RotateScale_Camera * (interpolation + 1));
            //mainwindow.cameraRollNode.Pitch(-inputMouse.MouseState.Y.rel * RotateScale_Camera * (interpolation + 1));
            //if (OgreWindow.g_m.MouseState.buttons != 3) return true;
            //OgreWindow.Instance.log(OgreWindow.g_m.MouseState.buttons.ToString());
            int buttons = OgreWindow.g_m.MouseState.buttons;

            try
            {
                ClientPluginManager.FrameStartedHooks(interpolation);
                // float pitchAngle = 0.0f;
                //  float pitchAngleSign = 0.0f;
                OgreWindow.Instance.cameraNode.Translate(OgreWindow.Instance.cameraYawNode.Orientation * OgreWindow.Instance.cameraPitchNode.Orientation * TranslateVector_Camera);
                TranslateVector_Camera = new Mogre.Vector3();
                //if (buttons != 2) return true;
                //pitchAngle = (2 *
                //    new Mogre.Degree(Mogre.Math.ACos(
                //        OgreWindow.Instance.cameraPitchNode.Orientation.w))
                //        .ValueDegrees);
                //pitchAngleSign = OgreWindow.Instance.cameraPitchNode.Orientation.x;
                //if (pitchAngle > 90.0f)
                //{
                //    if (pitchAngleSign > 0)
                //        OgreWindow.Instance.cameraPitchNode.Orientation = new Mogre.Quaternion(Mogre.Math.Sqrt(0.5f), Mogre.Math.Sqrt(0.5f), 0, 0);
                //    else if (pitchAngleSign < 0)
                //        OgreWindow.Instance.cameraPitchNode.Orientation = new Mogre.Quaternion(Mogre.Math.Sqrt(0.5f), -Mogre.Math.Sqrt(0.5f), 0, 0);
                //}
            }
            catch
            {
            }
            return true;
        }
        #region camera location control
        private float MoveScale_Camera_forwardback = 0f;
        private float MoveScale_Camera_leftright = 0f;
        private float MoveScale_Camera_updown = 0f;

        private Mogre.Vector3 TranslateVector_Camera = new Mogre.Vector3();
        const float speedcap_forwardback = .15f;
        const float speedcap_leftright = .15f;
        const float incr_forwardback = .0005f;
        const float incr_leftright = .0005f;
        const float brakes_forwardback = incr_forwardback * 2;
        const float brakes_leftright = incr_leftright * 2;
        #endregion
        private void update()
        {
            try
            {
                //inputKeyboard.Capture();
                //inputMouse.Capture();
                ClientPluginManager.updateHooks();
                if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_ESCAPE))
                {
                    quit();
                }
                try
                {
                    if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_W))
                    {
                        if (MoveScale_Camera_forwardback > -speedcap_forwardback)
                            MoveScale_Camera_forwardback -= incr_forwardback;
                    }
                    else if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_S))
                    {
                        if (MoveScale_Camera_forwardback < speedcap_forwardback)
                            MoveScale_Camera_forwardback += incr_forwardback;
                    }
                    else if (MoveScale_Camera_forwardback != 0f)
                    {
                        if (MoveScale_Camera_forwardback > 0f)
                            MoveScale_Camera_forwardback -= incr_forwardback;
                        else
                            MoveScale_Camera_forwardback += incr_forwardback;
                        if (MoveScale_Camera_forwardback < brakes_forwardback && MoveScale_Camera_forwardback > -brakes_forwardback)
                            MoveScale_Camera_forwardback = 0f;
                    }
                    else
                    {
                        MoveScale_Camera_forwardback = 0f;
                    }
                    if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_A))
                    {
                        if (MoveScale_Camera_leftright > -speedcap_leftright)
                            MoveScale_Camera_leftright -= incr_leftright;
                    }
                    else if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_D))
                    {
                        if (MoveScale_Camera_leftright < speedcap_leftright)
                            MoveScale_Camera_leftright += incr_leftright;
                    }
                    else if (MoveScale_Camera_leftright != 0f)
                    {
                        if (MoveScale_Camera_leftright > 0f)
                            MoveScale_Camera_leftright -= incr_leftright;
                        else
                            MoveScale_Camera_leftright += incr_leftright;
                        if (MoveScale_Camera_leftright < brakes_leftright && MoveScale_Camera_leftright > -brakes_leftright)
                            MoveScale_Camera_leftright = 0f;
                    }
                    else
                    {
                        MoveScale_Camera_leftright = 0f;
                    }
                }
                catch { OgreWindow.Instance.log("couldn't wire up camera input"); }
                #region gui updates
                Mogre.Vector3 pos = OgreWindow.Instance.cameraNode.Position;


                OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.label1, "X: " + pos.x.ToString("N"));
                OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.label2, "Y: " + pos.y.ToString("N"));
                OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.label3, "Z: " + pos.z.ToString("N"));
                OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.label4, "S1: " + MoveScale_Camera_forwardback.ToString("N") + " S2: " + MoveScale_Camera_leftright.ToString("N"));

                OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.textBox1, string.Format("x:{0} y:{1} z:{2}", pos.x ,pos.y,pos.z));

                #endregion
            }
            catch { }
        }
        private Random ran = new Random((int)DateTime.Now.Ticks);
    }
}
