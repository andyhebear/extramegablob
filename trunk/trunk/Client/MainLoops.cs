using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ExtraMegaBlob.References;
using Mogre;
using MogreFramework;
using SkyX;
using MHydrax;
using Mogre.PhysX;

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
        private const bool DISABLE_NETWORK = false;
        private const bool DISABLE_MHYDRAX = true;
        public void main()
        {
            try
            {
                log(Program.header);

                conf = new Config();

                OgreWindow.Instance.textures = new Textures(ThingPath.path_cache);
                OgreWindow.Instance.skeletons = new Skeletons(ThingPath.path_cache);
                OgreWindow.Instance.meshes = new Meshes(ThingPath.path_cache);
                OgreWindow.Instance.materials = new Materials();

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
                cache.skeletonAdded += new CacheManager.skeletonAddedDelegate(cache_skeletonAdded);
                cache.skeletonDeleted += new CacheManager.skeletonDeletedDelegate(cache_skeletonDeleted);

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
                    new Thread(new ThreadStart(waterUpdateThread)).Start();

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
        protected SkyManager skyManager;
        private string TerrainMaterialName = "Terrain";
        //Entities entities = new Entities();
        //SceneNodes nodes = new SceneNodes();
        private void SceneCreating()
        {
            SceneManager sm = OgreWindow.Instance.mSceneMgr;
            Root root = OgreWindow.Instance.mRoot;
            Camera camera = OgreWindow.Instance.mCamera;
            Viewport vp = OgreWindow.Instance.mViewport;
            OgreWindow.Instance.mSceneMgr.SetShadowUseInfiniteFarPlane(true);
            sm.AmbientLight = ColourValue.Black;



            #region shadows
            //sm.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_ADDITIVE;
            //sm.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_MODULATIVE;

            sm.ShadowTechnique = ShadowTechnique.SHADOWTYPE_NONE; //skyx breaks when shadows are enabled!
            //sm.ShadowTechnique = ShadowTechnique.SHADOWTYPE_TEXTURE_ADDITIVE;
            //sm.ShadowTechnique = ShadowTechnique.SHADOWTYPE_TEXTURE_ADDITIVE_INTEGRATED;
            //sm.ShadowTechnique = ShadowTechnique.SHADOWTYPE_TEXTURE_MODULATIVE;
            //sm.ShadowTechnique = ShadowTechnique.SHADOWTYPE_TEXTURE_MODULATIVE_INTEGRATED; 
            #endregion





            camera.FarClipDistance = 30000;
            camera.NearClipDistance = .25f;

            //camera.SetPosition(20000, 500, 20000);
            //camera.SetDirection(1, 0, 0);



            skyManager = new SkyManager(sm, OgreWindow.Instance.mCamera);
            skyManager.Create();


            //manager.GPUManager.AddGroundPass(material.GetTechnique(0).CreatePass(), 5000, SceneBlendType.SBT_TRANSPARENT_COLOUR);

            skyManager.CloudsManager.Add(new CloudLayer.LayerOptions());


            #region MHydrax


            if (!DISABLE_MHYDRAX)
            {



                //hydrax = new MHydrax.MHydrax(sm, camera, vp);

                // Hydrax initialization code ---------------------------------------------
                // ------------------------------------------------------------------------

                // Create Hydrax object
                hydrax = new MHydrax.MHydrax(sm, camera, vp);

                // Set hydrax components.
                hydrax.Components = MHydrax.MHydraxComponent.HYDRAX_COMPONENT_CAUSTICS |
                                    MHydrax.MHydraxComponent.HYDRAX_COMPONENT_DEPTH |
                                    MHydrax.MHydraxComponent.HYDRAX_COMPONENT_FOAM |
                                    MHydrax.MHydraxComponent.HYDRAX_COMPONENT_SMOOTH |
                                    MHydrax.MHydraxComponent.HYDRAX_COMPONENT_SUN |
                                    MHydrax.MHydraxComponent.HYDRAX_COMPONENT_UNDERWATER |
                                    MHydrax.MHydraxComponent.HYDRAX_COMPONENT_UNDERWATER_GODRAYS |
                                    MHydrax.MHydraxComponent.HYDRAX_COMPONENT_UNDERWATER_REFLECTIONS;

                //' Create our projected grid module
                //' Parameters:
                //' Hydrax parent pointer
                //' Noise module
                //' Base plane
                //' Normal mode
                //' Projected grid options
                MHydrax.MProjectedGrid m = new MHydrax.MProjectedGrid(hydrax,
                                                    new MHydrax.MPerlin(new MHydrax.MPerlin.MOptions(8, 0.085f, 0.49f, 1.4f, 1.27f, 2f, new Mogre.Vector3(0.5f, 50f, 150000f))),
                                                    new Plane(new Mogre.Vector3(0, 1, 0), new Mogre.Vector3(0, 0, 0)),
                                                    MHydrax.MMaterialManager.MNormalMode.NM_VERTEX,
                                                    new MHydrax.MProjectedGrid.MOptions(256, 35f, 50f, false, false, true, 3.75f));

                //' Set our module
                hydrax.SetModule(m);

                //' Set all parameters instead of loading all parameters from config file:
                //'hydrax.LoadCfg("ProjectedGridDemo.hdx")
                //' #Main options
                hydrax.Position = new Mogre.Vector3(5000, 0, -5000);
                hydrax.PlanesError = 10.5f;
                hydrax.ShaderMode = MHydrax.MMaterialManager.MShaderMode.SM_HLSL;
                hydrax.FullReflectionDistance = 100000000000;
                hydrax.GlobalTransparency = 0;
                hydrax.NormalDistortion = 0.075f;
                hydrax.WaterColor = new Mogre.Vector3(0.139765f, 0.359464f, 0.425373f);
                //' #Sun parameters
                hydrax.SunPosition = new Mogre.Vector3(0, 10000, 0);
                hydrax.SunStrength = 1.75f;
                hydrax.SunArea = 150;
                hydrax.SunColor = new Mogre.Vector3(1f, 0.9f, 0.6f);
                //' #Foam parameters
                hydrax.FoamMaxDistance = 75000000;
                hydrax.FoamScale = 0.0075f;
                hydrax.FoamStart = 0;
                hydrax.FoamTransparency = 1;
                //' #Depth parameters
                hydrax.DepthLimit = 90;
                //' #Smooth transitions parameters
                hydrax.SmoothPower = 5;
                //' #Caustics parameters
                hydrax.CausticsScale = 135;
                hydrax.CausticsPower = 10.5f;
                hydrax.CausticsEnd = 0.8f;
                //' #God rays parameters
                hydrax.GodRaysExposure = new Mogre.Vector3(0.76f, 2.46f, 2.29f);
                hydrax.GodRaysIntensity = 0.015f;
                hydrax.GodRaysManager.SimulationSpeed = 5;
                hydrax.GodRaysManager.NumberOfRays = 100;
                hydrax.GodRaysManager.RaysSize = 0.03f;
                hydrax.GodRaysManager.ObjectsIntersectionsEnabled = false;
                //' #Rtt quality field(0x0 = Auto)
                //' TODO: RTTManager not wrapped yet.
                //'<size>Rtt_Quality_Reflection=0x0
                //'<size>Rtt_Quality_Refraction=0x0
                //'<size>Rtt_Quality_Depth=0x0
                //'<size>Rtt_Quality_URDepth=0x0
                //'<size>Rtt_Quality_GPUNormalMap=0x0

                //' Create water
                hydrax.Create();

                //' Hydrax initialization code end -----------------------------------------
                //' ------------------------------------------------------------------------

                //sm.AmbientLight = new ColourValue(1, 1, 1);
                //camera.FarClipDistance = 99999 * 6;
                //camera.Position = new Mogre.Vector3(312.902f, 206.419f, 1524.02f);
                //camera.Orientation = new Quaternion(0.998f, -0.0121f, -0.0608f, -0.00074f);
            }
            #endregion

            #region physics
            // create the root object
            OgreWindow.Instance.physics = Physics.Create();
            OgreWindow.Instance.physics.Parameters.SkinWidth = 0.0025f;

            // setup default scene params
            SceneDesc sceneDesc = new SceneDesc();
            sceneDesc.SetToDefault();
            sceneDesc.Gravity = new Mogre.Vector3(0, -9.8f, 0);
            sceneDesc.UpAxis = 1; // NX_Y in c++ (I couldn't find the equivilent enum for C#)

            // your class should implement IUserContactReport to use this
            //sceneDesc.UserContactReport = this;

            OgreWindow.Instance.scene = OgreWindow.Instance.physics.CreateScene(sceneDesc);

            // default material
            OgreWindow.Instance.scene.Materials[0].Restitution = 0.5f;
            OgreWindow.Instance.scene.Materials[0].StaticFriction = 0.5f;
            OgreWindow.Instance.scene.Materials[0].DynamicFriction = 0.5f;

            // begin simulation
            OgreWindow.Instance.scene.Simulate(0);
            #endregion

            OgreWindow.Instance.SceneReady = true;
        }

        private MHydrax.MHydrax hydrax = null;
        private void cache_skeletonDeleted(string pathRelSkeletonFile)
        {
            OgreWindow.Instance.pause();
            try { OgreWindow.Instance.skeletons.RemoveAt(pathRelSkeletonFile); }
            catch (Exception ex) { log(ex.ToString()); }
            OgreWindow.Instance.unpause();
        }
        private void cache_skeletonAdded(string pathRelSkeletonFile)
        {
            OgreWindow.Instance.pause();
            try { OgreWindow.Instance.skeletons.Add(pathRelSkeletonFile); }
            catch (Exception ex) { log(ex.ToString()); }
            OgreWindow.Instance.unpause();
        }
        private void DefaultLog_MessageLogged(string message, LogMessageLevel lml, bool maskDebug, string logName)
        {

            log(lml.ToString() + ": " + message);
        }
        private bool Root_FrameStarted(FrameEvent evt)
        {
            //SceneManager sm = OgreWindow.Instance.mSceneMgr;
            //Root root = OgreWindow.Instance.mRoot;
            //Camera cam = OgreWindow.Instance.mCamera;

            //// Check camera height
            //RaySceneQuery raySceneQuery = sm.CreateRayQuery(new Ray(cam.Position + new Mogre.Vector3(0, 1000000, 0), Mogre.Vector3.NEGATIVE_UNIT_Y));
            //RaySceneQueryResult qryResult = raySceneQuery.Execute();

            //RaySceneQueryResult.Iterator it = qryResult.Begin();
            //if (it != qryResult.End() && it.Value.worldFragment != null)
            //{
            //    if (cam.DerivedPosition.y < it.Value.worldFragment.singleIntersection.y + 30)
            //    {
            //        cam.SetPosition(cam.Position.x,
            //                            it.Value.worldFragment.singleIntersection.y + 30,
            //                            cam.Position.z);
            //    }

            //    it.MoveNext();
            //}

            skyManager.TimeMultiplier = 1f;
            skyManager.Update(evt.timeSinceLastFrame);

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
        float envThrottle = 0f;
        protected void HandleInput()
        {
            //base.HandleInput(evt);

            // Show/Hide information

            OgreWindow win = OgreWindow.Instance;

            if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_F1))
            {
                showInformation = !showInformation;

            }

            if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_1) && !(OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_LSHIFT) || OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_RSHIFT)))
            {
                skyManager.TimeMultiplier = 1.0f;
            }
            if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_1) && (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_LSHIFT) || OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_RSHIFT)))
            {
                skyManager.TimeMultiplier = -1.0f;
            }


        }
        private String GetConfigString()
        {
            AtmosphereManager atmo = skyManager.AtmosphereManager;
            int hour = (int)atmo.Time.x;

            int min = (int)((atmo.Time.x - hour) * 60);


            String timeStr = hour.ToString() + ":" + min.ToString();
            String str = "SkyX Plugin demo (Press F1 to show/hide information)";
            if (showInformation)
            {
                str += " - Simuation paused - \n";
            }
            else
            {
                str += "\n-------------------------------------------------------------\nTime: " + timeStr + "\\n";
            }

            if (showInformation)
            {
                str += "-------------------------------------------------------------\n";
                str += "Time: " + timeStr + " [1, Shift+1] (+/-).\n";
                str += "Rayleigh multiplier: " + Mogre.StringConverter.ToString(atmo.RayleighMultiplier) + " [2, Shift+2] (+/-).\n";
                str += "Mie multiplier: " + Mogre.StringConverter.ToString(atmo.MieMultiplier) + " [3, Shift+3] (+/-).\n";
                str += "Exposure: " + Mogre.StringConverter.ToString(atmo.Exposure) + " [4, Shift+4] (+/-).\n";
                str += "Inner radius: " + Mogre.StringConverter.ToString(atmo.InnerRadius) + " [5, Shift+5] (+/-).\n";
                str += "Outer radius: " + Mogre.StringConverter.ToString(atmo.OuterRadius) + " [6, Shift+6] (+/-).\n";
                str += "Number of samples: " + atmo.NumberOfSamples.ToString() + " [7, Shift+7] (+/-).\n";
                str += "Height position: " + Mogre.StringConverter.ToString(atmo.HeightPosition) + " [8, Shift+8] (+/-).\n";
            }

            return str;
        }
        protected bool showInformation;
        private void waterUpdateThread()
        {
            while (!OgreWindow.Instance.ShuttingDown)
            {
                try
                {
                    OgreWindow.Instance.pause();
                    if (!DISABLE_MHYDRAX)
                        hydrax.Update(.1f);
                    envThrottle = 0f;
                    OgreWindow.Instance.unpause();
                }
                catch { }
                Thread.Sleep(100);
            }
        }
        private void update()
        {
            OgreWindow.Instance.scene.FlushStream();
            OgreWindow.Instance.scene.FetchResults(SimulationStatuses.AllFinished, true);
            OgreWindow.Instance.scene.Simulate(.1f);

            if (!DISABLE_MHYDRAX)
                hydrax.SunPosition = skyManager.AtmosphereManager.SunPosition;  //WORKS!
            HandleInput();
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
            OgreWindow.Instance.physics.Dispose();
            ClientPluginManager.shutdown();
            netClient.disconnect();
            saveScreenshot();
            OgreWindow.Instance.Close();
            OgreWindow.Instance.meshes.shutdown();
            OgreWindow.Instance.textures.shutdown();
            OgreWindow.Instance.skeletons.shutdown();
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
            try { OgreWindow.Instance.meshes.RemoveAt(pathRelMeshFile); }
            catch (Exception ex) { log(ex.ToString()); }
            OgreWindow.Instance.unpause();
        }
        private void cache_meshAdded(string pathRelMeshFile)
        {
            OgreWindow.Instance.pause();
            try { OgreWindow.Instance.meshes.Add(pathRelMeshFile); }
            catch (Exception ex) { log(ex.ToString()); }
            OgreWindow.Instance.unpause();
        }
        private void cache_textureDeleted(string pathRelTextureFile)
        {
            OgreWindow.Instance.pause();
            try { OgreWindow.Instance.textures.RemoveAt(pathRelTextureFile); }
            catch (Exception ex) { log(ex.ToString()); }
            OgreWindow.Instance.unpause();
        }
        private void cache_textureAdded(string pathRelTextureFile)
        {
            OgreWindow.Instance.pause();
            try { OgreWindow.Instance.textures.Add(pathRelTextureFile); }
            catch (Exception ex) { log(ex.ToString()); }
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
            e._IntendedRecipients = EventTransfer.CLIENTTOSERVER;
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
            try
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
            catch (Exception ex)
            {
                log(ex.ToString());
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
