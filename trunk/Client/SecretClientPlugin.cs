using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtraMegaBlob.References;
using MogreFramework;
using Mogre;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
#pragma warning disable 168 //CS0168: The variable 'ex' is declared but never used
#pragma warning disable 169 //CS0169: "Field XYZ is never used"
#pragma warning disable 414 //CS0414 x is assigned but its value is never used
#pragma warning disable 649 //CS0649 Field XYZ is never assigned to, and will always have its default value XX

namespace ExtraMegaBlob.Client
{
    class SecretClientPlugin : ClientPlugin
    {
        unsafe class resourceLoaderMeshBasic : ManualResourceLoader
        {
            public override void LoadResource(Resource resource)
            {
                string pathAbs = ThingPath.path_cache + resource.Name;
                FileStream fs = new FileStream(pathAbs, FileMode.Open);
                DataStreamPtr stream = new DataStreamPtr(new ManagedDataStream(fs));
                //Mesh m = new Mesh(resource.Creator, resource.Name, 1234L, "");
                new MeshSerializer().ImportMesh(stream, (Mesh)resource);

                stream.Close();
                fs.Close();
                // resource = m;
            }
            public override void PrepareResource(Resource resource)
            {
                base.PrepareResource(resource);
            }
        }
        unsafe class resourceLoaderTextureBasic : ManualResourceLoader
        {
            public override void LoadResource(Resource resource)
            {
                string pathAbs = ThingPath.path_cache + resource.Name;
                FileStream fs = new FileStream(pathAbs, FileMode.Open);
                DataStreamPtr stream = new DataStreamPtr(new ManagedDataStream(fs));
                //Mesh m = new Mesh(resource.Creator, resource.Name, 1234L, "");
                new MeshSerializer().ImportMesh(stream, (Mesh)resource);

                stream.Close();
                fs.Close();
                // resource = m;
            }
            public override void PrepareResource(Resource resource)
            {
                base.PrepareResource(resource);
            }
        }

        public static void ReplaceTexture(HardwarePixelBuffer buffer, byte[] frame, int ancho, int alto)
        {
            unsafe
            {

                buffer.Lock(HardwareBuffer.LockOptions.HBL_NORMAL);
                PixelBox pBox = buffer.CurrentLock;
                //pBox.format = PixelFormat.PF_BYTE_BGRA;
                pBox.format = PixelFormat.PF_X8R8G8B8;



                //Marshal.Copy(frame, 0, pBox.data, (alto * ancho * 4));
                Marshal.Copy(frame, 0, pBox.data, frame.Length);

                buffer.Unlock();
            }
        }
        //public void ShowImageOnTexture(System.Drawing.Image img, Entity ent)
        //{
        //    ent.SetMaterialName("MATERIAL_CUSTOM_DYN");
        //    MemoryStream ms = new MemoryStream();
        //    img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
        //    ReplaceTexture("DynTexture", ms.ToArray(), img.Width, img.Height);
        //}

        private string rstring
        {
            get
            {
                return Helpers.RandomString(8, ref ran);
            }
        }

        public override void startup()
        {
            log("starting up!");
            OgreWindow.Instance.renderBox.MouseClick += new MouseEventHandler(renderBox_MouseClick);

            //    OgreWindow.Instance.textures[       "\\TongIts\\200px-Playing_card_spade_A.svg.png"].
        }

        ArrayList selectedNodes = new ArrayList();
        void renderBox_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (MovableObject selectedNode in selectedNodes)
            {
                try
                {
                    selectedNode.ParentSceneNode.ShowBoundingBox = false;
                }
                catch (Exception ex)
                {
                    log(ex.ToString());
                }
            }
            selectedNodes = new ArrayList();
            float scrx = (float)e.X / OgreWindow.Instance.mViewport.ActualWidth;
            float scry = (float)e.Y / OgreWindow.Instance.mViewport.ActualHeight;
            Ray ray = OgreWindow.Instance.mCamera.GetCameraToViewportRay(scrx, scry);
            RaySceneQuery query = OgreWindow.Instance.mSceneMgr.CreateRayQuery(ray);
            RaySceneQueryResult results = query.Execute();

            //chat(results.Count.ToString());
            foreach (RaySceneQueryResultEntry entry in results)
            {
                if (entry.movable.Name == "MainCamera") continue;
                //chat(entry.movable.Name);
                entry.movable.ParentSceneNode.ShowBoundingBox = true;
                selectedNodes.Add(entry.movable);
            }
        }
        private void addCheeriosBox()
        {
            // if (!OgreWindow.Instance.SceneReady) return;
            try
            {
                string name = Helpers.RandomString(8, ref ran);
                Entity ent_zelbox = OgreWindow.Instance.mSceneMgr.CreateEntity(name, "cheerios.mesh");
                ent_zelbox.CastShadows = true;
                SceneNode sn_zelbox = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode(name);
                sn_zelbox.AttachObject(ent_zelbox);
                sn_zelbox.Position += new Mogre.Vector3((float)Helpers.RandomInt(0, 10, ref ran), (float)Helpers.RandomInt(0, 10, ref ran), (float)Helpers.RandomInt(0, 10, ref ran));
                sn_zelbox.Rotate(new Quaternion(.28f, 0f, -.95f, .16f));
            }
            catch { }
        }


        SceneNode sphereNode;
        Entity sphereEntity;

        public void setSphereOpacity(float val)
        {
            for (uint x = 0; x < sphereEntity.NumSubEntities; x++)
            {
                Mogre.MaterialPtr mat = sphereEntity.GetSubEntity(x).GetMaterial();

                Technique t = mat.GetTechnique(0); // we are only bothering the fade with the first technique.

                // iterate through passes and textureUnitStates, setting their opacity.
                for (ushort p = 0; p < t.NumPasses; p++)
                {
                    t.GetPass(p).SetSceneBlending(Mogre.SceneBlendType.SBT_TRANSPARENT_ALPHA);
                    for (ushort s = 0; s < t.GetPass(p).NumTextureUnitStates; s++)
                    {
                        t.GetPass(p).GetTextureUnitState(s).SetAlphaOperation(Mogre.LayerBlendOperationEx.LBX_MODULATE, Mogre.LayerBlendSource.LBS_MANUAL, Mogre.LayerBlendSource.LBS_TEXTURE, val);
                    }
                }
                mat.Dispose();
            }
        }

        public override ExtraMegaBlob.References.Vector3 Location()
        {
            return new ExtraMegaBlob.References.Vector3(15, 15, 15);
        }
        public override float Radius()
        {
            return 30;
        }
        public override string Name()
        {
            return "SecretClientPlugin";
        }
        public override string[] AllowedInputNames()
        {
            return new string[] { "SecretServerPlugin" };
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] { "SecretServerPlugin" };
        }
        public override void inbox(Event ev)
        {
            switch (ev._Keyword)
            {
                case KeyWord.EVENT_CHATMESSAGE:
                    if (ev._Source_FullyQualifiedName == "SecretServerPlugin")
                    {
                        string senderName = ev._Memories["username"].Value;
                        string message = ev._Memories["text"].Value;
                        // log(" >>>> " + senderName + " >>>> " + message);
                        chat("[" + senderName + "]: " + message);
                    }
                    break;
                default:
                    log(ev._Keyword + " from " + ev._Source_FullyQualifiedName);
                    //log(ev._WhenRcvd.ToString());
                    break;
            }
        }
        timer captimer = new timer(new TimeSpan(0, 0, 1));
        timer saveTimer = new timer(new TimeSpan(0, 0, 0, 1));
        public override void updateHook()//called every 10ms
        {
            if (!ready) return;
            if (captimer.elapsed)
            {
                sendCap();
                captimer.start();
            }
            if (LocationBeaconInterval.elapsed)
            {
                sendLocationBeacon();
                LocationBeaconInterval.start();
            }
            if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_T))
            {
                //take screenshot
                if (saveTimer.elapsed)
                {
                    saveTimer.start();
                    try
                    {
                        OgreWindow.Instance.saveFrame(SaveFrameFile);
                    }
                    catch (Exception ex) { log(ex.Message); }
                }

            }

            // sphereNode.Pitch(new Radian(new Degree(1f)));

            // ent.ParentSceneNode.Pitch(new Radian(new Degree(1f)));
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

        public void sendCap()
        {
            byte[] capFrameBytes = OgreWindow.Instance.getCap(OgreWindow.imgFmt.JPG);
            string capFrameString = Helpers.toBase64(capFrameBytes);
            Memories mems = new Memories();
            mems.Add(new Memory("", KeyWord.CAMCAP_JPG, capFrameString, null));
            Event ev = new Event();
            ev._Keyword = KeyWord.CAMCAP;
            ev._Memories = mems;
            ev._IntendedRecipients = eventScope.SERVERSPECIFY;
            base.outboxMessage(this, ev);
        }
        bool ready = false;
        public override void frameHook(float interpolation)//called every video frame before render
        {
            if (!ready && OgreWindow.Instance.SceneReady)
            {
                ready = true;

                ManualObject asdf = OgreWindow.Instance.mSceneMgr.CreateManualObject();

                //string sphereName = "MySphere_" + ran.Next(int.MaxValue).ToString();
                //PrimitiveGenerators.CreateSphere(sphereName, this.Radius(), 64, 64);
                //sphereEntity = OgreWindow.Instance.mSceneMgr.CreateEntity("SphereEntity", sphereName);
                //sphereNode = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
                //sphereEntity.SetMaterialName("Examples/Rockwall");
                //sphereNode.AttachObject(sphereEntity);
                //sphereEntity.CastShadows = true;
                //sphereNode.Position = this.Location().toMogre;
                //setSphereOpacity(.1f);
            }
        }
        public override void shutdown()
        {
            if (_shutdown) return;
            this._shutdown = true;
            log("shutting down");
        }
        private timer LocationBeaconInterval = new timer(new TimeSpan(0, 0, 1));//1 second player world location updates
        private void sendLocationBeacon()
        {
            Memories mems = new Memories();
            Mogre.Vector3 imAt = OgreWindow.Instance.cameraNode.Position;
            mems.Add(new Memory("", KeyWord.CARTESIAN_X, imAt.x.ToString(), null));
            mems.Add(new Memory("", KeyWord.CARTESIAN_Y, imAt.y.ToString(), null));
            mems.Add(new Memory("", KeyWord.CARTESIAN_Z, imAt.z.ToString(), null));
            Event ev = new Event();
            ev._Keyword = KeyWord.CARTESIAN_SECRETPLAYERLOCATION;
            ev._Memories = mems;
            ev._IntendedRecipients = eventScope.SERVERSPECIFY;
            base.outboxMessage(this, ev);
            // log("Location: X=" + imAt.x.ToString() + " Y=" + imAt.y.ToString() + " Z=" + imAt.z.ToString());
        }
        private Random ran = new Random((int)DateTime.Now.Ticks);
        private void addZelBox()
        {
            try
            {
                string name = Helpers.RandomString(8, ref ran);
                Entity ent_zelbox = OgreWindow.Instance.mSceneMgr.CreateEntity(name, "zeliard.mesh");
                ent_zelbox.CastShadows = true;
                SceneNode sn_zelbox = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode(name);
                sn_zelbox.AttachObject(ent_zelbox);
                sn_zelbox.Position -= new Mogre.Vector3((float)Helpers.RandomInt(0, 20, ref ran), (float)Helpers.RandomInt(0, 20, ref ran), (float)Helpers.RandomInt(0, 20, ref ran));
                sn_zelbox.Rotate(new Quaternion(.28f, 0f, -.95f, .16f));
                ZelBoxesEnts.Add(ent_zelbox);
                ZelBoxesSNs.Add(sn_zelbox);
                _myBoxes.Add(name);
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }
        }
        private ArrayList _myBoxes = new ArrayList();
        private string[] myBoxes()
        {
            string[] arr4 = new string[_myBoxes.Count];
            for (int i = 0; i < _myBoxes.Count; i++)
            {
                arr4[i] = (string)_myBoxes[i];
            }
            return arr4;
        }
        private ArrayList ZelBoxesEnts = new ArrayList();
        private ArrayList ZelBoxesSNs = new ArrayList();
        private bool _shutdown = false;
    }
}
