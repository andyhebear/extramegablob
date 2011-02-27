using System;
using System.Collections.Generic;
using System.Text;
using MogreFramework;
using ExtraMegaBlob.References;
using Mogre;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;
using MOIS;
namespace ExtraMegaBlob
{
    public class plugin : ExtraMegaBlob.References.ClientPlugin
    {
        private Hashtable materials
        {
            get
            {
                Hashtable h = new Hashtable();
                #region materials
                h["metal"] = "\\Players\\BumpyMetal.jpg";
                #endregion
                return h;
            }
        }
        private Hashtable meshes
        {
            get
            {
                Hashtable h = new Hashtable();
                #region meshes
                h["drone"] = "\\Drone.mesh";
                #endregion
                return h;
            }
        }
        private Hashtable skeletons
        {
            get
            {
                Hashtable h = new Hashtable();
                #region skeletons
                h["droneskele"] = "\\Drone.skeleton";
                #endregion
                return h;
            }
        }
        private SceneNodes nodes = new SceneNodes();
        private Entities entities = new Entities();
        private Lights lights = new Lights();
        private void resourceWaitThread()
        {
            while (true)
            {
                Thread.Sleep(1000);
                foreach (DictionaryEntry de in materials)
                {
                    if (!TextureManager.Singleton.ResourceExists((string)de.Value)) goto waitmore;
                }
                foreach (DictionaryEntry de in skeletons)
                {
                    if (!SkeletonManager.Singleton.ResourceExists((string)de.Value)) goto waitmore;
                }
                foreach (DictionaryEntry de in meshes)
                {
                    if (!MeshManager.Singleton.ResourceExists((string)de.Value)) goto waitmore;
                }
                break;
            waitmore:
                continue;
            }
            init();
        }
        private void init()
        {
            log("starting up! ");
            OgreWindow.Instance.pause();
            try
            {
                Hashtable mats = materials;
                foreach (DictionaryEntry mat in mats)
                {
                    ((MaterialPtr)MaterialManager.Singleton.Create((string)mat.Key, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState((string)mat.Value);
                }

                OgreWindow.Instance.skeletons["\\Drone.skeleton"].Load();
                OgreWindow.Instance.meshes["\\Drone.mesh"].Load();
                OgreWindow.Instance.meshes["\\Drone.mesh"].SkeletonName = "\\Drone.skeleton";

                entities.Add(OgreWindow.Instance.mSceneMgr.CreateEntity("drone", "\\Drone.mesh"));
                entities["drone"].CastShadows = true;
                walkState = entities["drone"].GetAnimationState("walk");
                walkState.Enabled = true;
                walkState.Loop = true;
                entities["drone"].SetMaterialName("metal");
                nodes.Add(OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode("drone"));
                nodes["drone"].AttachObject(entities["drone"]);
                nodes["drone"].Position = new Mogre.Vector3(3f, 1f, 3f) + Location().toMogre;
                nodes["drone"].Scale(new Mogre.Vector3(.3f));

                nodes.Add(nodes["drone"].CreateChildSceneNode("orbit0"));
                nodes.Add(nodes["orbit0"].CreateChildSceneNode("orbit"));
                nodes["orbit"].Position = new Mogre.Vector3(0f, 0f, 40f);
                nodes["orbit"].AttachObject(OgreWindow.Instance.mCamera);
                nodes["drone"].SetFixedYawAxis(true);
                //OgreWindow.Instance.cameraNode.LookAt(

                OgreWindow.g_m.MouseMoved += new MouseListener.MouseMovedHandler(g_m_MouseMoved);

                ready = true;
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }
            OgreWindow.Instance.unpause();
        }
        private void preventMousePick(string name)
        {
            Memories mems = new Memories();
            mems.Add(new Memory("Name", KeyWord.NIL, name, null));
            Event ev = new Event();
            ev._Keyword = KeyWord.PREVENTMOUSEPICK;
            ev._Memories = mems;
            ev._IntendedRecipients = EventTransfer.CLIENTTOCLIENT;
            base.outboxMessage(this, ev);
        }
        private AnimationState walkState = null;
        public override void startup()
        {
            new Thread(new ThreadStart(resourceWaitThread)).Start();
        }
        public override void shutdown()
        {
            ready = false;
            log("shutting down!");
            nodes.shutdown();
            entities.shutdown();
            lights.shutdown();
        }
        public override ExtraMegaBlob.References.Vector3 Location()
        {
            //return new ExtraMegaBlob.References.Vector3(0f, -168.6846f, -1101.067f);
            return new ExtraMegaBlob.References.Vector3(0f, 0f, 0f);
        }
        public override float Radius()
        {
            return 90;
        }
        public override string Name()
        {
            return "Players_Client";
        }
        public override string[] AllowedInputNames()
        {
            return new string[] { "Movement_Client", "Players_Server" };
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] { "Players_Server" };
        }
        public override void inbox(ExtraMegaBlob.References.Event ev)
        {
            if (!ready) return;
            switch (ev._Keyword)
            {
                case KeyWord.MOVEPLAYER:
                    SceneNode node = nodes["drone"];
                    ExtraMegaBlob.References.Vector3 loc = ExtraMegaBlob.References.Vector3.FromString(ev._Memories["loc"].Value);
                    node.Translate(loc.toMogre);
                    updateCam();
                    //OgreWindow.Instance.cameraNode.Position = m_CamPos * node._getDerivedPosition();

                    break;
                case KeyWord.ROTATEPLAYER:
                    //ExtraMegaBlob.References.Vector3 loc = ExtraMegaBlob.References.Vector3.FromString(ev._Memories["loc"].Value);
                    //nodes["drone"].Translate(loc.toMogre);
                    break;
                default:
                    break;
            }
        }
        private Mogre.Vector3 getOrbitalPosition(Mogre.Vector3 focalPoint, Quaternion direction)
        {
            Mogre.Vector3 eyePos, xAxis, yAxis, zAxis;
            Quaternion m_Orientation = direction;

            Mogre.Matrix3 m_ViewMatrix;

            // Get the rotation matrix from the quaternion
            m_ViewMatrix = m_Orientation.ToRotationMatrix();

            // Get the axis we need them for the camera position calculation (the axes span a new coord frame)
            xAxis = new Mogre.Vector3(m_ViewMatrix[0, 0], m_ViewMatrix[0, 1], m_ViewMatrix[0, 2]);
            yAxis = new Mogre.Vector3(m_ViewMatrix[1, 0], m_ViewMatrix[1, 1], m_ViewMatrix[1, 2]);
            zAxis = new Mogre.Vector3(m_ViewMatrix[2, 0], m_ViewMatrix[2, 1], m_ViewMatrix[2, 2]);


            float distanceToObject = 50f;

            eyePos = focalPoint + zAxis * distanceToObject;

            // Transform our vector in the orbital space. Here we rotate and flip it.
            m_ViewMatrix[0, 3] = -xAxis.DotProduct(eyePos);
            m_ViewMatrix[1, 3] = -yAxis.DotProduct(eyePos);
            m_ViewMatrix[2, 3] = -zAxis.DotProduct(eyePos);

            Mogre.Vector3 m_CamPos = new Mogre.Vector3();

            // So extract the cam position for the sake of completeness
            m_CamPos.x = m_ViewMatrix[0, 3];
            m_CamPos.y = m_ViewMatrix[1, 3];
            m_CamPos.z = m_ViewMatrix[2, 3];

            return m_CamPos;
        }
        private float RotateScale_Camera = .001f;//mouse sensitivity
        private bool g_m_MouseMoved(MouseEvent arg)
        {
            SceneNode node = nodes["drone"];
            SceneNode camnode = nodes["orbit0"];
            MouseState_NativePtr s = arg.state;
            if (arg.state.buttons == 2)
            {
                node.Yaw(-s.X.rel * RotateScale_Camera);
                
                updateCam();
                camnode.Pitch(s.Y.rel * RotateScale_Camera);
            }



            return true;
        }
        private void updateCam()
        {
            SceneNode camnode = nodes["orbit"];
            SceneNode node = nodes["drone"];
            Mogre.Vector3 focalPoint = new Mogre.Vector3(0f,0f,0f);
            Quaternion direction = node.Orientation;
            direction.Normalise();
            
            //Mogre.Vector3 m_CamPos = getOrbitalPosition(focalPoint, direction);
            //OgreWindow.Instance.cameraNode.LookAt(focalPoint, Node.TransformSpace.TS_PARENT);
            //OgreWindow.Instance.cameraNode.Position = m_CamPos + focalPoint;


            Mogre.Vector3 m_CamPos = getOrbitalPosition(focalPoint, direction);
            camnode.Position = m_CamPos;
            camnode.LookAt(node.Position, Node.TransformSpace.TS_WORLD);

        }
        public override void updateHook()
        {
            if (t.elapsed)
            {
                t.reset();
                t.start();
            }
            if (ready)
            {
                walkState.AddTime(.01f);
            }
        }
        timer scaleLimiter = new timer(new TimeSpan(0, 0, 1));

        private bool ready = false;
        public override void frameHook(float interpolation)
        {

        }
        private Random ran = new Random((int)DateTime.Now.Ticks);


        ExtraMegaBlob.References.timer t = new ExtraMegaBlob.References.timer(new TimeSpan(0, 0, 0, 0, 1000));
    }
}
