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
                    ExtraMegaBlob.References.Vector3 loc = ExtraMegaBlob.References.Vector3.FromString(ev._Memories["loc"].Value);
                    nodes["drone"].Translate(loc.toMogre);
                    break;
                default:
                    break;
            }
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
