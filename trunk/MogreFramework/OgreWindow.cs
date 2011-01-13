using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DirectShowLib;
using Mogre;
using MOIS;
using ExtraMegaBlob;
#pragma warning disable 168 //CS0168: The variable 'ex' is declared but never used
namespace MogreFramework
{
    public partial class OgreWindow : GUI_helper
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
        private static String title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        private static Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        public static string header
        {
            get
            {
                return title + " v" + version + " Compiled " + DateCompiled().ToString();
            }
        }


        public bool pauserendering = false;
        public bool renderingframe = false;

        public void pause()
        {
            pauserendering = true;
            while (renderingframe)
                Thread.Sleep(1);
        }
        public void unpause()
        {
            pauserendering = false;
        }
        public Textures textures = null;
        public Meshes meshes = null;
        public Skeletons skeletons = null;

        #region mouse and kb input
        static string[] g_DeviceType = {"OISUnknown", "OISKeyboard", "OISMouse", "OISJoyStick",
							 "OISTablet", "OISOther"};
        public static InputManager g_InputManager;
        public static Keyboard g_kb;
        public static Mouse g_m;
        public static JoyStick[] g_joys;
        private void DoInputsStartup()
        {
            ParamList pl = new ParamList();
            pl.Insert("WINDOW", this.Handle.ToString());
            //Default mode is foreground exclusive..but, we want to show mouse - so nonexclusive
            pl.Insert("w32_mouse", "DISCL_FOREGROUND");
            pl.Insert("w32_mouse", "DISCL_NONEXCLUSIVE");
            //This never returns null.. it will raise an exception on errors
            g_InputManager = InputManager.CreateInputSystem(pl);
            uint v = InputManager.VersionNumber;
            log("OIS Version: " + (v >> 16) + "." + ((v >> 8) & 0x000000FF) + "." + (v & 0x000000FF)
                + "\n\tRelease Name: " //+ InputManager.VersionName
                + "\n\tPlatform: " + g_InputManager.InputSystemName()
                + "\n\tNumber of Mice: " + g_InputManager.GetNumberOfDevices(MOIS.Type.OISMouse)
                + "\n\tNumber of Keyboards: " + g_InputManager.GetNumberOfDevices(MOIS.Type.OISKeyboard)
                + "\n\tNumber of Joys/Pads = " + g_InputManager.GetNumberOfDevices(MOIS.Type.OISJoyStick));
            //List all devices
            DeviceList list = g_InputManager.ListFreeDevices();
            foreach (KeyValuePair<MOIS.Type, string> pair in list)
                log("\n\tDevice: " + g_DeviceType[(int)pair.Key] + " Vendor: " + pair.Value);
            g_kb = (Keyboard)g_InputManager.CreateInputObject(MOIS.Type.OISKeyboard, true);
            //g_kb.KeyPressed += new KeyListener.KeyPressedHandler(KeyPressed);
            //g_kb.KeyReleased += new KeyListener.KeyReleasedHandler(KeyReleased);
            g_m = (Mouse)g_InputManager.CreateInputObject(MOIS.Type.OISMouse, true);
            //g_m.MouseMoved += new MouseListener.MouseMovedHandler(MouseMoved);
            //g_m.MousePressed += new MouseListener.MousePressedHandler(MousePressed);
            //g_m.MouseReleased += new MouseListener.MouseReleasedHandler(MouseReleased);
            //MouseState_NativePtr ms = g_m.MouseState;
            //ms.width = renderBox.Width;
            //ms.height = renderBox.Height;
            //This demo only uses at max 4 joys
            int numSticks = g_InputManager.GetNumberOfDevices(MOIS.Type.OISJoyStick);
            if (numSticks > 4) numSticks = 4;
            g_joys = new JoyStick[numSticks];
            for (int i = 0; i < numSticks; ++i)
            {
                g_joys[i] = (JoyStick)g_InputManager.CreateInputObject(MOIS.Type.OISJoyStick, true);
                //g_joys[i].AxisMoved += new JoyStickListener.AxisMovedHandler(AxisMoved);
                //g_joys[i].ButtonPressed += new JoyStickListener.ButtonPressedHandler(JoyButtonPressed);
                //g_joys[i].ButtonReleased += new JoyStickListener.ButtonReleasedHandler(JoyButtonReleased);
                //g_joys[i].PovMoved += new JoyStickListener.PovMovedHandler(PovMoved);
                //g_joys[i].Vector3Moved += new JoyStickListener.Vector3MovedHandler(Vector3Moved);
            }
        }
        //bool Vector3Moved(JoyStickEvent arg, int index)
        //{
        //    MOIS.Vector3 vec = arg.state.GetVector(index);
        //    log("\n" + arg.device.Vendor() + ". Orientation # " + index
        //        + " X Value: " + vec.x
        //        + " Y Value: " + vec.y
        //        + " Z Value: " + vec.z);
        //    return true;
        //}
        //bool PovMoved(JoyStickEvent arg, int pov)
        //{
        //    log("\n" + arg.device.Vendor() + ". POV" + pov + " ");
        //    if ((arg.state.get_mPOV(pov).direction & Pov_NativePtr.North) != 0) //Going up
        //        Console.Write("North");
        //    else if ((arg.state.get_mPOV(pov).direction & Pov_NativePtr.South) != 0) //Going down
        //        Console.Write("South");
        //    if ((arg.state.get_mPOV(pov).direction & Pov_NativePtr.East) != 0) //Going right
        //        Console.Write("East");
        //    else if ((arg.state.get_mPOV(pov).direction & Pov_NativePtr.West) != 0) //Going left
        //        Console.Write("West");
        //    if (arg.state.get_mPOV(pov).direction == Pov_NativePtr.Centered) //stopped/centered out
        //        Console.Write("Centered");
        //    return true;
        //}
        //bool JoyButtonReleased(JoyStickEvent arg, int button)
        //{
        //    log("\n" + arg.device.Vendor() + ". Button Released # " + button);
        //    return true;
        //}
        //bool JoyButtonPressed(JoyStickEvent arg, int button)
        //{
        //    log("\n" + arg.device.Vendor() + ". Button Pressed # " + button);
        //    return true;
        //}
        //bool AxisMoved(JoyStickEvent arg, int axis)
        //{
        //    //Provide a little dead zone
        //    Axis_NativePtr axiscls = arg.state.GetAxis(axis);
        //    if (axiscls.abs > 2500 || axiscls.abs < -2500)
        //        log("\n" + arg.device.Vendor() + ". Axis # " + axis + " Value: " + axiscls.abs);
        //    return true;
        //}
        //bool MouseReleased(MouseEvent arg, MouseButtonID id)
        //{
        //    MouseState_NativePtr s = arg.state;
        //    log("\nMouse button #" + id + " released. Abs("
        //              + s.X.abs + ", " + s.Y.abs + ", " + s.Z.abs + ") Rel("
        //              + s.X.rel + ", " + s.Y.rel + ", " + s.Z.rel + ")");
        //    return true;
        //}
        //bool MousePressed(MouseEvent arg, MouseButtonID id)
        //{
        //    MouseState_NativePtr s = arg.state;
        //    log("\nMouse button #" + id + " pressed. Abs("
        //              + s.X.abs + ", " + s.Y.abs + ", " + s.Z.abs + ") Rel("
        //              + s.X.rel + ", " + s.Y.rel + ", " + s.Z.rel + ")");
        //    return true;
        //}
        //bool MouseMoved(MouseEvent arg)
        //{
        //    MouseState_NativePtr s = arg.state;
        //    //log("\nMouseMoved: Abs("
        //    //          + s.X.abs + ", " + s.Y.abs + ", " + s.Z.abs + ") Rel("
        //    //          + s.X.rel + ", " + s.Y.rel + ", " + s.Z.rel + ")");
        //    multi_onMouseMove(arg);
        //    return true;
        //}
        //public delegate void inp_mousemoved_dele(MouseEvent arg);
        //public event inp_mousemoved_dele onMouseMove;
        //private void multi_onMouseMove(MouseEvent arg){
        //    if (!object.Equals(null, onMouseMove))
        //    {
        //        onMouseMove(arg);
        //    }
        //}
        //bool KeyReleased(KeyEvent arg)
        //{
        //    if (arg.key == KeyCode.KC_ESCAPE || arg.key == KeyCode.KC_Q)
        //        this.ShuttingDown = true;
        //    return true;
        //}
        //bool KeyPressed(KeyEvent arg)
        //{
        //    log("\nKeyPressed {" + arg.key
        //        + ", " + ((Keyboard)(arg.device)).GetAsString(arg.key)
        //        + "} || Character (" + (char)arg.text + ")");
        //    return true;
        //}
        #endregion
        public class timer
        {
            private DateTime startedat = DateTime.Now;
            private bool finished = false;
            public TimeSpan interval;
            public void start()
            {
                startedat = DateTime.Now;
                finished = false;
            }
            public timer(TimeSpan interval)
            {
                this.interval = interval;
            }
            public bool elapsed
            {
                get
                {
                    if (finished) return true;
                    else
                    {
                        bool ret = (startedat.Add(interval).CompareTo(DateTime.Now) < 0) ? true : false;
                        if (ret) finished = ret;
                        return ret;
                    }
                }
            }
        }
        private timer tim = new timer(new TimeSpan(0, 0, 0, 0, 150));
        Bitmap bmpScreenshot;
        Graphics gfxScreenshot;
        public void saveFrame(string path)
        {
            this.Show();
            bmpScreenshot = new Bitmap(getWindowDimensions()[0], getWindowDimensions()[1], System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            SetForegroundWindow(this.Handle);
            Thread.Sleep(100);//not sure if needed
            gfxScreenshot.CopyFromScreen(getWindowLocation()[0], getWindowLocation()[1], 0, 0, new Size(getWindowDimensions()[0], getWindowDimensions()[1]), CopyPixelOperation.SourceCopy);
            bmpScreenshot.Save(path, ImageFormat.Jpeg);
            log("saved: " + path);
        }
        [DllImportAttribute("User32.dll", SetLastError = true)]
        internal static extern IntPtr FindWindow(String ClassName, String WindowName);
        [DllImportAttribute("User32.dll", SetLastError = true)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);
        #region GUI
        public static string logTimeFormat { get { return "yyyy/MM/dd HH:mm:ss.fffffff"; } }
        public static string logPrefix
        {
            get
            {
                return "[" + DateTime.Now.ToString(logTimeFormat) + "] ";
            }
        }
        public void log(string what)
        {
            if (this._ShuttingDown) return;
            string x = logPrefix + what + Environment.NewLine;
            if (cbAutoScrollLog.Checked)
                this.ListBoxItemAddAndScrollDown(listBox1, x);
            else
                this.ListBoxItemAdd(listBox1, x);
        }
        public enum UI_ELEMENT
        {
            label1,
            label2,
            label3,
            label4,
            label5,
            textBox1
        }
        public void updateCoords(UI_ELEMENT element, string status)
        {



            switch (element)
            {
                case UI_ELEMENT.label1:
                    LabelSetText(label1, status);
                    break;
                case UI_ELEMENT.label2:
                    LabelSetText(label2, status);
                    break;
                case UI_ELEMENT.label3:
                    LabelSetText(label3, status);
                    break;
                case UI_ELEMENT.label4:
                    LabelSetText(label4, status);
                    break;
                case UI_ELEMENT.textBox1:
                    TextBoxClear(textBox1);
                    TextBoxAppendText(textBox1, status);
                    break;
                //case UI_ELEMENT.label5:
                //    LabelSetText(label5, status);
                //    break;
            }
        }
        private uint SETTINGS_WIDTH
        {
            get
            {
                return (uint)renderBox.Width;
            }
        }
        private uint SETTINGS_HEIGHT
        {
            get
            {
                return (uint)renderBox.Height;
            }
        }
        private int[] getWindowLocation()
        {
            IntPtr acWindow = this.Handle;
            ExtraMegaBlob.EnumWindowsItem enumw = new EnumWindowsItem(acWindow);
            return new int[] { enumw.Location.X, enumw.Location.Y };
        }
        private int[] getWindowDimensions()
        {
            IntPtr acWindow = this.Handle;
            ExtraMegaBlob.EnumWindowsItem enumw = new EnumWindowsItem(acWindow);
            return new int[] { enumw.Size.Width, enumw.Size.Height };
        }
        #endregion
        public Root mRoot = null;
        public RenderWindow mWindow = null;
        public Camera mCamera = null;
        public Viewport mViewport = null;
        public SceneManager mSceneMgr = null;
        public bool ShuttingDown { set { if (value) { _ShuttingDown = true; } } get { return _ShuttingDown; } }
        private bool _ShuttingDown = false;

        public SceneNode cameraNode = null;
        public SceneNode cameraYawNode = null;
        public SceneNode cameraPitchNode = null;
        public SceneNode cameraRollNode = null;
        public delegate void SceneEventHandler();
        public event SceneEventHandler SceneCreating;
        public bool pauseEvents = false;
        public void doEvents()
        {
            if (!pauseEvents)
            {
                g_kb.Capture();
                g_m.Capture();
                System.Windows.Forms.Application.DoEvents();
            }
        }
        #region Constructor
        public OgreWindow()
        {
            InitializeComponent();
            log(header);
            this.Icon = global::MogreFramework.Properties.Resources.OgreHead;
            DoInputsStartup();
            pbCapture.Resize += new EventHandler(pbCapture_Resize);
            CaptureVideo(pbCapture.Handle);
            this.Text = header;
        }
        public static OgreWindow Instance
        {
            get
            {
                return Nested.instance;
            }
        }
        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
            internal static readonly OgreWindow instance = new OgreWindow();
        }
        #endregion
        #region Public Methods
        public int fps = 0;
        public int frameCounter = 0;
        public TimeSpan frameDelay = new TimeSpan(1);
        /// <summary>
        /// Initializes ogre and shows the splash screen as it loads.
        /// </summary>
        public void InitializeOgre()
        {
            if (mRoot != null)
                throw new Exception("Ogre is already initialized!");
            Splash splash = new Splash();
            splash.Show();
            try
            {
                splash.Increment("Creating the root object...");
                mRoot = new Root();
                //  mRoot.ShowConfigDialog();
                splash.Increment("Loading resources...");
                InitResources();
                splash.Increment("Setting up DirectX...");
                SetupDirectX();
                splash.Increment("Creating the window...");
                //CreateRenderWindow(this.Handle);
                CreateRenderWindow(this.renderBox.Handle);
                splash.Increment("Initializing resources...");
                InitializeResources();
                splash.Increment("Creating Ogre objects...");
                CreateSceneManager();
                CreateCamera();
                CreateViewport();
                //splash.Increment("Creating input handler...");
                //CreateInputHandler();
                //mRoot.FrameEnded += new FrameListener.FrameEndedHandler(mRoot_FrameEnded);
                splash.Increment("Creating scene...");
                OnSceneCreating();
            }
            finally
            {
                splash.Close();
                splash.Dispose();
            }
        }

        private bool _sceneready = false;
        public bool SceneReady { set { if (value) { _SceneReady = true; } } get { return _SceneReady; } }
        private bool _SceneReady = false;
        #endregion
        #region Protected Virtual Methods
        /// <summary>
        /// Creates the scene manager which will be used.
        /// If you override this function, you must set the SceneManager property of this
        /// class to be the result.  Example:
        ///   this.SceneManager = this.Root.CreateSceneManager(...);
        /// </summary>
        protected virtual void CreateSceneManager()
        {
            //mSceneMgr = mRoot.CreateSceneManager("TerrainSceneManager");
            mSceneMgr = mRoot.CreateSceneManager(SceneType.ST_GENERIC, "Main SceneManager");
            //this->pSceneManager = pRoot->createSceneManager("TerrainSceneManager");
        }
        /// <summary>
        /// Creates the camera for this class.
        /// If you override this function, you must set the Camera property of this class
        /// to be the result.  Example:
        ///   this.Camera = this.SceneManager.CreateCamera(...);
        /// </summary>
        protected virtual void CreateCamera()
        {
            //mCamera = mSceneMgr.CreateCamera("MainCamera");
            //mCamera.NearClipDistance = 1;
            //mCamera.Position = new Mogre.Vector3(0, 0, 300);
            //mCamera.LookAt(Mogre.Vector3.ZERO);
            mCamera = mSceneMgr.CreateCamera("MainCamera");
            mCamera.NearClipDistance = 1;
            mCamera.AutoAspectRatio = true;
            // mCamera.Position = new Mogre.Vector3(0, 0, 300);
            // mCamera.LookAt(Mogre.Vector3.ZERO);
            //// Create the camera's thop node (which will only handle position).
            //this->cameraNode = this->sceneManager->getRootSceneNode()->createChildSceneNode();
            //this->cameraNode->setPosition(0, 0, 500);
            //// Create the camera's yaw node as a child of camera's top node.
            //this->cameraYawNode = this->cameraNode->createChildSceneNode();
            //// Create the camera's pitch node as a child of camera's yaw node.
            //this->cameraPitchNode = this->cameraYawNode->createChildSceneNode();
            //// Create the camera's roll node as a child of camera's pitch node
            //// and attach the camera to it.
            //this->cameraRollNode = this->cameraPitchNode->createChildSceneNode();
            //this->cameraRollNode->attachObject(this->camera);
            // Create the camera's top node (which will only handle position).
            cameraNode = mSceneMgr.RootSceneNode.CreateChildSceneNode();
            // Create the camera's yaw node as a child of camera's top node.
            cameraYawNode = cameraNode.CreateChildSceneNode();
            // Create the camera's pitch node as a child of camera's yaw node.
            cameraPitchNode = cameraYawNode.CreateChildSceneNode();
            // Create the camera's roll node as a child of camera's pitch node
            // and attach the camera to it.
            cameraRollNode = cameraPitchNode.CreateChildSceneNode();
            cameraRollNode.AttachObject(mCamera);
        }
        /// <summary>
        /// Creates the viewport for this class.
        /// If you override this function, you must set the viewport property of this class
        /// to be the result:
        ///   this.Viewport = this.RenderWindow.AddViewport(...);
        /// </summary>
        protected virtual void CreateViewport()
        {
            mViewport = mWindow.AddViewport(mCamera);
            mViewport.BackgroundColour = new ColourValue(0.0f, 0.0f, 0.0f, 1.0f);
            //Viewport = this.RenderWindow.AddViewport(Camera);
            //        Viewport.BackgroundColour = ColourValue.Black;
            //        Camera.AspectRatio = Viewport.ActualWidth / Viewport.ActualHeight;
        }
        /// <summary>
        /// Creates the input handler for this class.  If you wish to create your own input handler,
        /// override this function and set it up.
        /// </summary>
        //protected virtual void CreateInputHandler()
        //{
        //    new DefaultInputHandler(this);
        //}
        /// <summary>
        /// Creates the render window for this class.
        /// If you override this function, you must set the RenderWindow property.  Example:
        ///   this.RenderWindow = this.Root.CreateRenderWindow(...);
        /// </summary>
        /// <param name="handle">The window handle to render ogre in.</param>
        protected virtual void CreateRenderWindow(IntPtr handle)
        {
            mRoot.Initialise(false, "Main Ogre Window");
            if (handle != IntPtr.Zero)
            {
                NameValuePairList misc = new NameValuePairList();
                misc["externalWindowHandle"] = handle.ToString();
                // misc["FSAA"] = "4";        // anti aliasing factor (0, 2, 4 ...)
                // misc["vsync"] = "true";    // by Ogre default: false
                //Width = (int)SETTINGS_WIDTH;
                //Height = (int)SETTINGS_HEIGHT;
                mWindow = mRoot.CreateRenderWindow("Autumn main RenderWindow", SETTINGS_WIDTH, SETTINGS_HEIGHT, false, misc);
                this.FormBorderStyle = FormBorderStyle.Sizable;
            }
            else
            {
                mWindow = mRoot.CreateRenderWindow("Autumn main RenderWindow", SETTINGS_WIDTH, SETTINGS_HEIGHT, false);
            }
        }
        /// <summary>
        /// Initializes the resources which the program uses.
        /// </summary>
        protected virtual void InitResources()
        {
            ConfigFile cf = new ConfigFile();
            cf.Load("resources.cfg", "\t:=", true);
            ConfigFile.SectionIterator seci = cf.GetSectionIterator();
            String secName, typeName, archName;
            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                ConfigFile.SettingsMultiMap settings = seci.Current;
                foreach (KeyValuePair<string, string> pair in settings)
                {
                    typeName = pair.Key;
                    archName = pair.Value;
                    ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName, secName);
                }
            }
        }
        #region Event Triggers
        protected virtual void OnSceneCreating()
        {
            if (SceneCreating != null)
                SceneCreating();
        }
        #endregion
        #endregion
        #region Private Methods
        void SetupDirectX()
        {
            RenderSystem rs = mRoot.GetRenderSystemByName("Direct3D9 Rendering Subsystem"); //OpenGL Rendering Subsystem
            mRoot.RenderSystem = rs;
            rs.SetConfigOption("Full Screen", "No");
            rs.SetConfigOption("Video Mode", SETTINGS_WIDTH.ToString() + " x " + SETTINGS_HEIGHT.ToString() + " @ 32-bit colour");
        }

        static void InitializeResources()
        {
            TextureManager.Singleton.DefaultNumMipmaps = 5;
            ResourceGroupManager rm = ResourceGroupManager.Singleton;
            //rm.DeclareResource("", "", "", new Const_NameValuePairList());
            rm.InitialiseAllResourceGroups();
        }
        #endregion
        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                //mRoot.RenderSystem.SetConfigOption("Video Mode", SETTINGS_WIDTH.ToString() + " x " + SETTINGS_HEIGHT.ToString() + " @ 32-bit colour");
                //MouseState_NativePtr ms = g_m.MouseState;
                //ms.width = (int)SETTINGS_WIDTH;
                //ms.height = (int)SETTINGS_HEIGHT;
                //                        log("resetting mouseState dimensions: X= " + SETTINGS_WIDTH.ToString() + " Y= " + SETTINGS_HEIGHT.ToString());
                // NameValuePairList misc = new NameValuePairList();
                //misc["externalWindowHandle"] = renderBox.Handle.ToString();
                // // misc["FSAA"] = "4";        // anti aliasing factor (0, 2, 4 ...)
                // // misc["vsync"] = "true";    // by Ogre default: false
                ////Width = (int)SETTINGS_WIDTH;
                ////Height = (int)SETTINGS_HEIGHT;
                //mWindow = mRoot.CreateRenderWindow("Autumn main RenderWindow", SETTINGS_WIDTH, SETTINGS_HEIGHT, false, misc);
                mWindow.Resize(SETTINGS_WIDTH, SETTINGS_HEIGHT);
                // mWindow.RemoveAllViewports();
                //mWindow.AddViewport(
                mCamera.SetWindow(0f, 0f, (float)SETTINGS_WIDTH, (float)SETTINGS_HEIGHT);
                // OgreWindow.g_m.MouseState.width
                //mViewport.SetDimensions(0f, 0f, (float)SETTINGS_WIDTH, (float)SETTINGS_HEIGHT);
                //mViewport.Height = (float)SETTINGS_HEIGHT;
                //Viewport = this.RenderWindow.AddViewport(Camera);
                //RenderWindow newWindow = mRoot.CreateRenderWindow("Autumn main RenderWindow", SETTINGS_WIDTH, SETTINGS_HEIGHT, false, misc);
                // mRoot._getCurrentSceneManager().DestinationRenderSystem._setViewport(mWindow);
                //CreateRenderWindow
                mCamera.AspectRatio = (float)SETTINGS_WIDTH / (float)SETTINGS_HEIGHT;
            }
            catch (Exception ex)
            {
                log(ex.Message);
            }
            //this.Camera.Viewport
            //this.Camera.Viewport.ActualHeight = SETTINGS_HEIGHT;
            //this.Camera.Viewport.ActualWidth = SETTINGS_WIDTH;
            // CreateRenderWindow(this.pictureBox1.Handle);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            pauseEvents = true;
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (string line in listBox1.Items)
                {
                    sb.Append(line);
                }
                Clipboard.SetText(sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            pauseEvents = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            pauseEvents = true;
            try
            {
                StringBuilder sb = new StringBuilder();
                ListBox.SelectedIndexCollection selected = listBox1.SelectedIndices;
                for (int i = 0; i < selected.Count; i++)
                {
                    sb.Append(listBox1.Items[selected[i]]);
                }
                Clipboard.SetText(sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            pauseEvents = false;
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            preSend();
        }
        private void preSend()
        {
            string s = tbTextToSend.Text.Trim();
            if (s == "") return;
            send(s);
            tbTextToSend.Text = string.Empty;
        }
        public delegate void sendDelegate(string text);
        public event sendDelegate onSend;
        private void send(string text)
        {
            if (!object.Equals(null, this.onSend))
            {
                onSend(text);
            }
        }
        public void addChatMessage(string msg)
        {
            ListBoxItemAddAndScrollDown(lbChatProximity, msg);
        }
        public void setPluginsActive(string[] activePlugins)
        {
            string[] ap2 = new string[activePlugins.Length];
            activePlugins.CopyTo(ap2, 0);
            for (int i = 0; i < activePlugins.Length; i++)
            {
                for (int a = 0; a < activePlugins.Length; a++)
                {
                    if (string.Compare(ap2[i], ap2[a]) > 0)
                    {
                        string tmp = ap2[a];
                        ap2[a] = ap2[i];
                        ap2[i] = tmp;
                    }
                }
            }
            ListBoxClear(lbPluginsActive);
            foreach (string plugName in activePlugins)
            {
                ListBoxItemAdd(lbPluginsActive, plugName);
            }
        }
        private void btnSend_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }
        private void tbTextToSend_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter
                && e.Control)
            {
                tbTextToSend.Text += Environment.NewLine;
            }
            else if (e.KeyCode == Keys.Enter
               && !e.Control)
            {
                preSend();
            }
        }
        #region Capture stuff
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_GRAPHNOTIFY:
                    {
                        HandleGraphEvent();
                        break;
                    }
            }
            // Pass this message to the video window for notification of system changes
            if (this.videoWindow != null)
                this.videoWindow.NotifyOwnerMessage(m.HWnd, m.Msg, m.WParam, m.LParam);
            base.WndProc(ref m);
        }
        // a small enum to record the graph state
        enum PlayState
        {
            Stopped,
            Paused,
            Running,
            Init
        };
        public const int WM_GRAPHNOTIFY = 0x8000 + 1; // Application-defined message to notify app of filtergraph events
        private IVideoWindow videoWindow = null;
        private IMediaControl mediaControl = null;
        private IMediaEventEx mediaEventEx = null;
        private IGraphBuilder graphBuilder = null;
        private ICaptureGraphBuilder2 captureGraphBuilder = null;
        private PlayState currentState = PlayState.Stopped;
        private DsROTEntry rot = null;
        private ISampleGrabber samplegrabber = null;
        private void CaptureVideo(IntPtr ctlHandle)
        {
            int hr = 0;
            IBaseFilter sourceFilter = null;
            try
            {
                // Get DirectShow interfaces
                GetInterfaces(ctlHandle);
                // Attach the filter graph to the capture graph
                hr = this.captureGraphBuilder.SetFiltergraph(this.graphBuilder);
                //captureGraphBuilder.RenderStream(PinCategory.Preview,MediaType.Video,
                DsError.ThrowExceptionForHR(hr);
                // Use the system device enumerator and class enumerator to find
                // a video capture/preview device, such as a desktop USB video camera.
                sourceFilter = FindCaptureDevice();
                if (sourceFilter == null)
                {
                    log("Couldn't find a video input device.");
                    return;
                }
                // Add Capture filter to our graph.
                hr = this.graphBuilder.AddFilter(sourceFilter, "Video Capture");
                DsError.ThrowExceptionForHR(hr);

                this.samplegrabber = (ISampleGrabber)new SampleGrabber();
                AMMediaType mt = new AMMediaType();
                mt.majorType = MediaType.Video;
                mt.subType = MediaSubType.RGB24;
                mt.formatType = FormatType.VideoInfo;
                samplegrabber.SetMediaType(mt);
                //samplegrabber.

                hr = this.graphBuilder.AddFilter((IBaseFilter)samplegrabber, "samplegrabber");
                DsError.ThrowExceptionForHR(hr);


                IBaseFilter nullRenderer = (IBaseFilter)new NullRenderer();
                hr = graphBuilder.AddFilter(nullRenderer, "Null Renderer");



                // Render the preview pin on the video capture filter
                // Use this instead of this.graphBuilder.RenderFile
                hr = this.captureGraphBuilder.RenderStream(PinCategory.Preview, MediaType.Video, sourceFilter, (IBaseFilter)samplegrabber, nullRenderer);
                //DsError.ThrowExceptionForHR(hr);
                if (hr != 0) log(DsError.GetErrorText(hr));



                // Now that the filter has been added to the graph and we have
                // rendered its stream, we can release this reference to the filter.
                Marshal.ReleaseComObject(sourceFilter);
                // Set video window style and position

                //SetupVideoWindow(ctlHandle);

                // Add our graph to the running object table, which will allow
                // the GraphEdit application to "spy" on our graph
                rot = new DsROTEntry(this.graphBuilder);
                // Start previewing video data
                hr = this.mediaControl.Run();
                DsError.ThrowExceptionForHR(hr);
                // Remember current state
                this.currentState = PlayState.Running;


                samplegrabber.SetBufferSamples(true);
                samplegrabber.SetOneShot(false);

            }
            catch
            {
                MessageBox.Show("CaptureVideo(ctlHandle) suffered a fatal error.");
            }
        }
        // This version of FindCaptureDevice is provide for education only.
        // A second version using the DsDevice helper class is define later.
        private IBaseFilter FindCaptureDevice()
        {
            int hr = 0;
            IEnumMoniker classEnum = null;
            IMoniker[] moniker = new IMoniker[1];
            object source = null;
            ICreateDevEnum devEnum = (ICreateDevEnum)new CreateDevEnum();
            hr = devEnum.CreateClassEnumerator(FilterCategory.VideoInputDevice, out classEnum, 0);
            DsError.ThrowExceptionForHR(hr);
            Marshal.ReleaseComObject(devEnum);
            if (classEnum == null)
            {
                try { Marshal.ReleaseComObject(moniker[0]); }
                catch { }
                return null;
            }
            while (classEnum.Next(moniker.Length, moniker, IntPtr.Zero) == 0)
            {
                Guid iid = typeof(IBaseFilter).GUID;
                try
                {
                    moniker[0].BindToObject(null, null, ref iid, out source);
                }
                catch (Exception ex) { }
                if (source != null) // Use the first video capture device on the device list.
                    break;
            }
            try { Marshal.ReleaseComObject(moniker[0]); }
            catch { }
            try { Marshal.ReleaseComObject(classEnum); }
            catch { }
            return (IBaseFilter)source;
        }
        /*
            // Uncomment this version of FindCaptureDevice to use the DsDevice helper class
            // (and comment the first version of course)
            public IBaseFilter FindCaptureDevice()
            {
              System.Collections.ArrayList devices;
              object source;
              // Get all video input devices
              devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
              // Take the first device
              DsDevice device = (DsDevice)devices[0];
              // Bind Moniker to a filter object
              Guid iid = typeof(IBaseFilter).GUID;
              device.Mon.BindToObject(null, null, ref iid, out source);
              // An exception is thrown if cast fail
              return (IBaseFilter) source;
            }
        */
        private void GetInterfaces(IntPtr ControlHandle)
        {
            int hr = 0;
            // An exception is thrown if cast fail
            this.graphBuilder = (IGraphBuilder)new FilterGraph();
            this.captureGraphBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            this.mediaControl = (IMediaControl)this.graphBuilder;
            this.videoWindow = (IVideoWindow)this.graphBuilder;

            this.mediaEventEx = (IMediaEventEx)this.graphBuilder;
            hr = this.mediaEventEx.SetNotifyWindow(ControlHandle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);
        }
        private void CloseInterfaces()
        {
            // Stop previewing data
            if (this.mediaControl != null)
                this.mediaControl.StopWhenReady();
            this.currentState = PlayState.Stopped;
            // Stop receiving events
            if (this.mediaEventEx != null)
                this.mediaEventEx.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero);
            // Relinquish ownership (IMPORTANT!) of the video window.
            // Failing to call put_Owner can lead to assert failures within
            // the video renderer, as it still assumes that it has a valid
            // parent window.
            if (this.videoWindow != null)
            {
                this.videoWindow.put_Visible(OABool.False);
                this.videoWindow.put_Owner(IntPtr.Zero);

            }
            // Remove filter graph from the running object table
            if (rot != null)
            {
                rot.Dispose();
                rot = null;
            }
            // Release DirectShow interfaces
            Marshal.ReleaseComObject(this.mediaControl); this.mediaControl = null;
            Marshal.ReleaseComObject(this.mediaEventEx); this.mediaEventEx = null;
            Marshal.ReleaseComObject(this.videoWindow); this.videoWindow = null;
            Marshal.ReleaseComObject(this.graphBuilder); this.graphBuilder = null;
            Marshal.ReleaseComObject(this.captureGraphBuilder); this.captureGraphBuilder = null;
        }
        private void SetupVideoWindow(IntPtr ctlHandle)
        {
            int hr = 0;
            // Set the video window to be a child of the main window
            hr = this.videoWindow.put_Owner(ctlHandle);
            DsError.ThrowExceptionForHR(hr);
            hr = this.videoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren);
            DsError.ThrowExceptionForHR(hr);
            // Use helper function to position video window in client rect 
            // of main application window
            ResizeVideoWindow();

            //videoWindow.put_Caption("HELLO!!!111");

            // Make the video window visible, now that it is properly positioned
            hr = this.videoWindow.put_Visible(OABool.True);
            DsError.ThrowExceptionForHR(hr);
        }
        private void ResizeVideoWindow()
        {
            if (this.videoWindow != null)
            {
                int[] dimensions = getNominalCapDimensions();
                this.videoWindow.SetWindowPosition(0, 0, dimensions[0], dimensions[1]);
            }
        }
        private int[] getNominalCapDimensions()
        {
            int maxWidth = pbCapture.Width;
            int maxHeight = pbCapture.Height;
            int w = (maxHeight >= maxWidth) ? maxWidth : maxHeight;
            int h = (maxHeight >= maxWidth) ? maxWidth : maxHeight;
            return new int[] { w, h };
        }
        private void ChangePreviewState(bool showVideo)
        {
            int hr = 0;
            // If the media control interface isn't ready, don't call it
            if (this.mediaControl == null)
                return;
            if (showVideo)
            {
                if (this.currentState != PlayState.Running)
                {
                    // Start previewing video data
                    hr = this.mediaControl.Run();
                    this.currentState = PlayState.Running;
                }
            }
            else
            {
                // Stop previewing video data
                hr = this.mediaControl.StopWhenReady();
                this.currentState = PlayState.Stopped;
            }
        }
        private void HandleGraphEvent()
        {
            int hr = 0;
            EventCode evCode;
            IntPtr evParam1, evParam2;
            if (this.mediaEventEx == null)
                return;
            while (this.mediaEventEx.GetEvent(out evCode, out evParam1, out evParam2, 0) == 0)
            {
                // Free event parameters to prevent memory leaks associated with
                // event parameter data.  While this application is not interested
                // in the received events, applications should always process them.
                hr = this.mediaEventEx.FreeEventParams(evCode, evParam1, evParam2);
                DsError.ThrowExceptionForHR(hr);
                // Insert event processing code here, if desired
            }
        }
        private void pbCapture_Resize(object sender, EventArgs e)
        {
            // Stop graph when Form is iconic
            if (this.WindowState == FormWindowState.Minimized)
                ChangePreviewState(false);
            // Restart Graph when window come back to normal state
            if (this.WindowState == FormWindowState.Normal)
                ChangePreviewState(true);
            ResizeVideoWindow();
        }
        public void setCapStatusImage(Bitmap b)
        {
            int[] newsiz = getNominalCapDimensions();
            Bitmap r = ResizeBitmap(b, newsiz[0], newsiz[1]);
            pbCapture.Image = r;
        }
        public void setCapStatusImage(byte[] bmpBytes)
        {
            setCapStatusImage(unserializeBitmap(bmpBytes));
        }
        private System.Drawing.Bitmap unserializeBitmap(byte[] bmpBytes)
        {
            MemoryStream ms = new MemoryStream(bmpBytes);
            System.Drawing.Bitmap img = (Bitmap)System.Drawing.Bitmap.FromStream(ms);
            // Do NOT close the stream!
            return img;
        }
        public byte[] getCapSerialized(imgFmt fmt)
        {
            return (serializeBitmap(getCap(), fmt));
        }
        public byte[] serializeBitmap(Bitmap b, imgFmt fmt)
        {
            MemoryStream ms = new MemoryStream();
            b.Save(ms, convertImgType(fmt));
            byte[] bmpBytes = ms.GetBuffer();
            b.Dispose();
            ms.Close();
            return bmpBytes;
        }
        public Bitmap getCap()
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                VideoInfoHeader videoheader = new VideoInfoHeader();
                AMMediaType grab = new AMMediaType();

                samplegrabber.GetConnectedMediaType(grab);
                videoheader = (VideoInfoHeader)Marshal.PtrToStructure(grab.formatPtr, typeof(VideoInfoHeader));
                int width = videoheader.BmiHeader.Width;
                int height = videoheader.BmiHeader.Height;
                Bitmap b = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                uint bytesPerPixel = (uint)(24 >> 3);
                uint extraBytes = ((uint)width * bytesPerPixel) % 4;
                uint adjustedLineSize = bytesPerPixel * ((uint)width + extraBytes);
                uint sizeOfImageData = (uint)(height) * adjustedLineSize;
                BitmapData bd1 = b.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                int bufsize = (int)sizeOfImageData;
                int n = samplegrabber.GetCurrentBuffer(ref bufsize, bd1.Scan0);
                b.UnlockBits(bd1);
                b.RotateFlip(RotateFlipType.RotateNoneFlipY);
                return b;
            }
            catch (Exception ex)
            {
                log(ex.ToString());
                return null;
            }
        }

        public delegate Bitmap getCapDelegate();
        public Bitmap getCap2()
        {
            if (this.InvokeRequired)
            {
                getCapDelegate dele = new getCapDelegate(getCap3);
                object o = this.Invoke(dele);
                if (o != null)
                    return (Bitmap)o;
                else return null;
            }
            else
            {
                return getCap3();
            }
        }
        private Bitmap getCap3()
        {
            return getCap();
        }


        private Bitmap ResizeBitmap(Bitmap b, int nWidth, int nHeight)
        {
            Bitmap result = new Bitmap(nWidth, nHeight);
            using (Graphics g = Graphics.FromImage((System.Drawing.Image)result))
                g.DrawImage(b, 0, 0, nWidth, nHeight);
            return result;
        }
        public enum imgFmt
        {
            BMP,
            JPG,
            GIF,
            PNG,
            TIFF,
            WMF,
            MBMP,
            ICO,
            EXIF,
            EMF
        }
        private ImageFormat convertImgType(imgFmt fmtIn)
        {
            switch (fmtIn)
            {
                case imgFmt.BMP:
                    return ImageFormat.Bmp;
                case imgFmt.JPG:
                    return ImageFormat.Jpeg;
                case imgFmt.GIF:
                    return ImageFormat.Gif;
                case imgFmt.PNG:
                    return ImageFormat.Png;
                case imgFmt.TIFF:
                    return ImageFormat.Tiff;
                case imgFmt.WMF:
                    return ImageFormat.Wmf;
                case imgFmt.MBMP:
                    return ImageFormat.MemoryBmp;
                case imgFmt.ICO:
                    return ImageFormat.Icon;
                case imgFmt.EXIF:
                    return ImageFormat.Exif;
                case imgFmt.EMF:
                    return ImageFormat.Emf;
            }
            return ImageFormat.Bmp;
        }
        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            try { Clipboard.SetText(textBox1.Text); }
            catch (Exception ex) { log(ex.Message); }
        }
    }
}
