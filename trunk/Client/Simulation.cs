using System;
using System.Windows.Forms;
using Mogre;
using MogreFramework;
using MOIS;
using System.Threading;
using System.IO;
using System.Collections;
using ThingReferences;
namespace thing
{
   
    public partial class Simulation
    {
        private static String AssemblyCopyright
        {
            get
            {
                object[] attributes = System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((System.Reflection.AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }
        private static DateTime DateCompiled()
        {
            System.Version v = version;
            DateTime d = new DateTime(
                v.Build * TimeSpan.TicksPerDay +
                v.Revision * TimeSpan.TicksPerSecond * 2
                ).AddYears(1999).AddHours(1);
            return d.Subtract(new TimeSpan(24, 0, 0));
        }
        public static Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        //private InputManager inputManager;
        //private Keyboard inputKeyboard;
        //private Mouse inputMouse;
        //protected OgreWindow mainwindow = null;
        Entity ground_ent = null;
        SceneNode ground_node = null;
        Config conf = null;
        public Simulation()
        {
            //sm.rolldice();
            conf = new Config();
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
            OgreWindow.Instance.mSceneMgr.SetSkyBox(true, "Examples/StormySkyBox", 5000, false);
        }
        bool Root_FrameEnded(FrameEvent evt)
        {
            if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_T))
            {
                //take screenshot
                if (!takeScreenshotInsideFrameEnded())
                {
                    MessageBox.Show("Failed to save screenshot");
                    return true;
                }
            }
            return true;
        }
        void mainwindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            quit();
        }
        private float _trim2 = 0.0f;
        private float trim2
        {
            get
            {
                return _trim2;
            }
            set
            {
                //if (trim2changed.AddTicks(1000).CompareTo(DateTime.Now) > 0)
                // {
                _trim2 = value;
                //     trim2changed = DateTime.Now;
                // }
            }
        }
        private DateTime trim2changed = DateTime.Now;
        float RotateScale_Camera = .001f;//mouse sensitivity
        private DateTime lastFrame = DateTime.Now;
        timer saveTimer = new timer(new TimeSpan(0, 0, 0, 1));
        private bool takeScreenshotInsideFrameEnded()
        {
            bool retVal = true;
            try
            {
                if (saveTimer.elapsed)
                {
                    saveTimer.start();
                    try
                    {
                        if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_T))
                        {
                            OgreWindow.Instance.saveFrame(SaveFrameFile);
                        }
                    }
                    catch { OgreWindow.Instance.log("takeScreenshotInsideFrameEnded: couldnt wire up controls"); }
                }
            }
            catch
            {
                retVal = false;
            }
            return retVal;
        }
        private void quit()
        {
            if (g) return;
            g = true;
            OgreWindow.Instance.ShuttingDown = true;//turn off underlying thread
            ClientPluginManager.shutdown();
            netClient.disconnect();
        }
        private bool g = false;
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
        private string orientationFile { get { return Path.GetDirectoryName(Application.ExecutablePath) + "\\orientation.txt"; } }
        void writeOrientationFile(string s)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(orientationFile, false);
            sw.WriteLine(s);
            sw.WriteLine("");
            sw.Close();
        }
        float[] readOrientationFile()
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(orientationFile);
            string s = sr.ReadLine();
            sr.Close();
            string[] x = s.Split('|');
            float[] retVal = new float[4];
            retVal[0] = float.Parse(x[0]);
            retVal[1] = float.Parse(x[1]);
            retVal[2] = float.Parse(x[2]);
            retVal[3] = float.Parse(x[3]);
            return retVal;
        }
        private DateTime camPanModeLastSet = DateTime.Now;
        private bool _cameraPanMode = false;
        private bool cameraPanMode
        {
            set
            {
                if (camPanModeLastSet.AddSeconds(1).CompareTo(DateTime.Now) > 0)
                {
                    if (!_cameraPanMode)
                    {
                        _cameraPanMode = true;
                    }
                    else
                    {
                        _cameraPanMode = false;
                    }
                    camPanModeLastSet = DateTime.Now;
                }
            }
            get
            {
                return _cameraPanMode;
            }
        }
    }
}