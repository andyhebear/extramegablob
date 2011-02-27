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
                h["mushroom"] = "\\TongIts\\Shiitake Mushroom Tree Shii.png";
                h["webcam"] = "webcapCapture";
                h["wood"] = "\\TongIts\\woodblurred.jpg";
                h["Material/TEXFACE/woodblurred.jpg"] = "\\TongIts\\woodblurred.jpg";
                h["Material/TEXFACE/BumpyMetal.jpg"] = "\\TongIts\\BumpyMetal.jpg";
                h["clouds"] = "\\clouds.jpg";
                h["dirt"] = "\\terr_dirt-grass.jpg";
                h["noise"] = "\\normalNoiseColor.png";
                h["cat1"] = "\\cat2.jpg";
                h["club 10"] = "\\TongIts\\200px-Playing_card_club_10.svg.png";
                h["club 2"] = "\\TongIts\\200px-Playing_card_club_2.svg.png";
                h["club 3"] = "\\TongIts\\200px-Playing_card_club_3.svg.png";
                h["club 4"] = "\\TongIts\\200px-Playing_card_club_4.svg.png";
                h["club 5"] = "\\TongIts\\200px-Playing_card_club_5.svg.png";
                h["club 6"] = "\\TongIts\\200px-Playing_card_club_6.svg.png";
                h["club 7"] = "\\TongIts\\200px-Playing_card_club_7.svg.png";
                h["club 8"] = "\\TongIts\\200px-Playing_card_club_8.svg.png";
                h["club 9"] = "\\TongIts\\200px-Playing_card_club_9.svg.png";
                h["club a"] = "\\TongIts\\200px-Playing_card_club_A.svg.png";
                h["club j"] = "\\TongIts\\200px-Playing_card_club_J.svg.png";
                h["club k"] = "\\TongIts\\200px-Playing_card_club_K.svg.png";
                h["club q"] = "\\TongIts\\200px-Playing_card_club_Q.svg.png";
                h["diamond 10"] = "\\TongIts\\200px-Playing_card_diamond_10.svg.png";
                h["diamond 2"] = "\\TongIts\\200px-Playing_card_diamond_2.svg.png";
                h["diamond 3"] = "\\TongIts\\200px-Playing_card_diamond_3.svg.png";
                h["diamond 4"] = "\\TongIts\\200px-Playing_card_diamond_4.svg.png";
                h["diamond 5"] = "\\TongIts\\200px-Playing_card_diamond_5.svg.png";
                h["diamond 6"] = "\\TongIts\\200px-Playing_card_diamond_6.svg.png";
                h["diamond 7"] = "\\TongIts\\200px-Playing_card_diamond_7.svg.png";
                h["diamond 8"] = "\\TongIts\\200px-Playing_card_diamond_8.svg.png";
                h["diamond 9"] = "\\TongIts\\200px-Playing_card_diamond_9.svg.png";
                h["diamond a"] = "\\TongIts\\200px-Playing_card_diamond_A.svg.png";
                h["diamond j"] = "\\TongIts\\200px-Playing_card_diamond_J.svg.png";
                h["diamond k"] = "\\TongIts\\200px-Playing_card_diamond_K.svg.png";
                h["diamond q"] = "\\TongIts\\200px-Playing_card_diamond_Q.svg.png";
                h["heart 10"] = "\\TongIts\\200px-Playing_card_heart_10.svg.png";
                h["heart 2"] = "\\TongIts\\200px-Playing_card_heart_2.svg.png";
                h["heart 3"] = "\\TongIts\\200px-Playing_card_heart_3.svg.png";
                h["heart 4"] = "\\TongIts\\200px-Playing_card_heart_4.svg.png";
                h["heart 5"] = "\\TongIts\\200px-Playing_card_heart_5.svg.png";
                h["heart 6"] = "\\TongIts\\200px-Playing_card_heart_6.svg.png";
                h["heart 7"] = "\\TongIts\\200px-Playing_card_heart_7.svg.png";
                h["heart 8"] = "\\TongIts\\200px-Playing_card_heart_8.svg.png";
                h["heart 9"] = "\\TongIts\\200px-Playing_card_heart_9.svg.png";
                h["heart a"] = "\\TongIts\\200px-Playing_card_heart_A.svg.png";
                h["heart j"] = "\\TongIts\\200px-Playing_card_heart_J.svg.png";
                h["heart k"] = "\\TongIts\\200px-Playing_card_heart_K.svg.png";
                h["heart q"] = "\\TongIts\\200px-Playing_card_heart_Q.svg.png";
                h["spade 10"] = "\\TongIts\\200px-Playing_card_spade_10.svg.png";
                h["spade 2"] = "\\TongIts\\200px-Playing_card_spade_2.svg.png";
                h["spade 3"] = "\\TongIts\\200px-Playing_card_spade_3.svg.png";
                h["spade 4"] = "\\TongIts\\200px-Playing_card_spade_4.svg.png";
                h["spade 5"] = "\\TongIts\\200px-Playing_card_spade_5.svg.png";
                h["spade 6"] = "\\TongIts\\200px-Playing_card_spade_6.svg.png";
                h["spade 7"] = "\\TongIts\\200px-Playing_card_spade_7.svg.png";
                h["spade 8"] = "\\TongIts\\200px-Playing_card_spade_8.svg.png";
                h["spade 9"] = "\\TongIts\\200px-Playing_card_spade_9.svg.png";
                h["spade a"] = "\\TongIts\\200px-Playing_card_spade_A.svg.png";
                h["spade j"] = "\\TongIts\\200px-Playing_card_spade_J.svg.png";
                h["spade k"] = "\\TongIts\\200px-Playing_card_spade_K.svg.png";
                h["spade q"] = "\\TongIts\\200px-Playing_card_spade_Q.svg.png";
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
                h["mushroom"] = "\\TongIts\\mushroom.mesh";
                h["table"] = "\\TongIts\\tongitstable.mesh";
                
                #endregion
                return h;
            }
        }
        private Hashtable skeletons
        {
            get
            {
                Hashtable h = new Hashtable();
                #region meshes
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
                lights.Add(OgreWindow.Instance.mSceneMgr.CreateLight("testLight"));
                lights["testLight"].Type = Light.LightTypes.LT_POINT;
                lights["testLight"].Position = new Mogre.Vector3(-117.9847f, 120f, 234.2695f) + Location().toMogre;
                lights["testLight"].DiffuseColour = ColourValue.White;
                lights["testLight"].SpecularColour = ColourValue.White;

                lights.Add(OgreWindow.Instance.mSceneMgr.CreateLight("testLight2"));
                lights["testLight2"].Type = Light.LightTypes.LT_POINT;
                lights["testLight2"].Position = new Mogre.Vector3(1.408661f, 54.81305f, -3.154539f) + Location().toMogre;
                lights["testLight2"].DiffuseColour = ColourValue.White;
                lights["testLight2"].SpecularColour = ColourValue.White;

                //MeshManager.Singleton.CreatePlane("ground",
                //    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                //    new Plane(Mogre.Vector3.UNIT_Y, 0),
                //    1500, 1500, 20, 20, true, 1, 5, 5, Mogre.Vector3.UNIT_Z);
                //// Create a ground plane
                //entities.Add(OgreWindow.Instance.mSceneMgr.CreateEntity("GroundEntity", "ground"));
                //entities["GroundEntity"].CastShadows = false;
                //entities["GroundEntity"].SetMaterialName("dirt");
                //nodes.Add(OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode("ground"));
                //nodes["ground"].AttachObject(entities["GroundEntity"]);
                //nodes["ground"].Position = new Mogre.Vector3(0f, 0f, 0f) + Location().toMogre;

                //our "ground" is a mushroom :)
                entities.Add(OgreWindow.Instance.mSceneMgr.CreateEntity("mushroom", "\\TongIts\\mushroom.mesh"));
                entities["mushroom"].SetMaterialName("mushroom");
                nodes.Add(OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode("mushroom"));
                nodes["mushroom"].AttachObject(entities["mushroom"]);
                nodes["mushroom"].Position = new Mogre.Vector3(0f, -20f, 0f) + Location().toMogre;
                nodes["mushroom"].Scale(new Mogre.Vector3(100f));
                nodes["mushroom"].Roll(new Radian(new Degree(90f)));
                preventMousePick("mushroom");
                

                MeshManager.Singleton.CreatePlane("spinnycard", "General", new Plane(Mogre.Vector3.UNIT_Y, 0), 200f, 250f, 1, 1, true, 1, 1, 1, Mogre.Vector3.UNIT_X);
                entities.Add(OgreWindow.Instance.mSceneMgr.CreateEntity("TestPlaneEntity", "spinnycard"));
                entities["TestPlaneEntity"].CastShadows = true;
                entities["TestPlaneEntity"].SetMaterialName("club 5");
                nodes.Add(OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode("spinnycard"));
                nodes["spinnycard"].AttachObject(entities["TestPlaneEntity"]);
                nodes["spinnycard"].Position = new Mogre.Vector3(0f, 3f, 0f) + Location().toMogre;
                nodes["spinnycard"].Scale(.001f, .001f, .001f);  

                entities.Add(OgreWindow.Instance.mSceneMgr.CreateEntity("tongitstable", "\\TongIts\\tongitstable.mesh"));
                nodes.Add(OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode("table"));
                nodes["table"].AttachObject(entities["tongitstable"]);
                nodes["table"].Position = new Mogre.Vector3(0f, 36f, 0f) + Location().toMogre;
                nodes["table"].Scale(new Mogre.Vector3(4f));
                preventMousePick("tongitstable");


                
                ready = true;
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }
            OgreWindow.Instance.unpause();
        }
        private void preventMousePick(string name){
            Memories mems = new Memories();
            mems.Add(new Memory("Name", KeyWord.NIL, name, null));
            Event ev = new Event();
            ev._Keyword = KeyWord.PREVENTMOUSEPICK;
            ev._Memories = mems;
            ev._IntendedRecipients = EventTransfer.CLIENTTOCLIENT; 
            base.outboxMessage(this, ev);
        }
        
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
            MeshManager.Singleton.Remove("ground");
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
            return "TongIts_Client";
        }
        public override string[] AllowedInputNames()
        {
            return new string[] { "TongIts_Server" };
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] { "TongIts_Server" };
        }
        public override void inbox(ExtraMegaBlob.References.Event ev)
        {
            //MogreFramework.Globals.Instance.Data[];
            switch (ev._Keyword)
            {
                case KeyWord.CARTESIAN_SECRETPLAYERLOCATION:
                    break;
                default:
                    //log(ev._Keyword + " from " + ev._Source_FullyQualifiedName);
                    //log(ev._WhenRcvd.ToString());
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
                // testSceneNode.Roll(new Radian(new Degree(1f)));
                //testSceneNode.Rotate(new Mogre.Vector3(1f,0f,0f),new Radian(new Degree(1f)));
                // testSceneNode.Scale(2f, 2f, 2f);
                if (scaleLimiter.elapsed)
                {

                     
                    //if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_Z))
                    //{
                    //    testSceneNode.Scale(.3f, .3f, .3f);
                    //    scaleLimiter.start();
                    //}
                }
                nodes["spinnycard"].Yaw(new Radian(new Degree(1f)));
                
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
