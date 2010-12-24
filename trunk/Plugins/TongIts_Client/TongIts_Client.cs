using System;
using System.Collections.Generic;
using System.Text;
using MogreFramework;
using ExtraMegaBlob.References;
using Mogre;
using System.Windows.Forms;
using System.Collections;
using System.IO;
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
                h["wood"] = "\\TongIts\\woodblurred.jpg";
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
        private void makeMaterials()
        {
            Hashtable mats = materials;
            foreach (DictionaryEntry mat in mats)
            {
                ((MaterialPtr)MaterialManager.Singleton.Create((string)mat.Key, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState((string)mat.Value);
            }
        }
        private SceneNode sn_cat;
        private Entity ent_cat;

        private SceneNode sn_table;
        private Entity ent_table;

        private void putLights()
        {
            testLight = OgreWindow.Instance.mSceneMgr.CreateLight("testLight");
            testLight.Type = Light.LightTypes.LT_POINT;
            testLight.Position = new Mogre.Vector3(-117.9847f, 120f, 234.2695f);
            testLight.DiffuseColour = ColourValue.White;
            testLight.SpecularColour = ColourValue.White;
            try
            {
                //OgreWindow.Instance.mSceneMgr.SetSkyBox(true, "cat1", 5000, false);
                //OgreWindow.Instance.mSceneMgr.SetSkyPlane(true, new Plane(Mogre.Vector3.UNIT_Y, 10), "cat1");
               // OgreWindow.Instance.mSceneMgr.SetSkyDome(true, "clouds");

                OgreWindow.Instance.mSceneMgr.SetSkyDome(true, "clouds", 5, 8);

               // OgreWindow.Instance.mSceneMgr._queueSkiesForRendering(OgreWindow.Instance.mCamera);

                chat(OgreWindow.Instance.mSceneMgr.IsSkyDomeEnabled.ToString());
               // OgreWindow.Instance.mCamera.FarClipDistance = 500;

                //Plane plane;
                //plane.d = 1000;
                //plane.normal = Mogre.Vector3.NEGATIVE_UNIT_Y;
                //OgreWindow.Instance.mSceneMgr.SetSkyPlane(true, plane, "clouds", 1500, 75);
            }
            catch (Exception ex) { log(ex.Message); }
        }
        private Entity ent_ground = null;
        private SceneNode sn_ground = null;
        public override void startup()
        {
            log("starting up!");
            makeMaterials();
            putLights();

            MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, new Plane(Mogre.Vector3.UNIT_Y, 0), 1500, 1500, 20, 20, true, 1, 5, 5, Mogre.Vector3.UNIT_Z);
            // Create a ground plane
            ent_ground = OgreWindow.Instance.mSceneMgr.CreateEntity("GroundEntity", "ground");
            ent_ground.CastShadows = false;
            ent_ground.SetMaterialName("dirt");
            sn_ground = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sn_ground.AttachObject(ent_ground);
            sn_ground.Position -= new Mogre.Vector3(0f, 10f, 0f);


            MeshManager.Singleton.CreatePlane("spinnycard", "General", new Plane(Mogre.Vector3.UNIT_Y, 0), 200, 250, 1, 1, true, 1, 1, 1, Mogre.Vector3.UNIT_X);
            ent_spinnycard = OgreWindow.Instance.mSceneMgr.CreateEntity("TestPlaneEntity", "spinnycard");
            ent_spinnycard.CastShadows = true;
            ent_spinnycard.SetMaterialName("heart 6");
            sn_spinnycard = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sn_spinnycard.AttachObject(ent_spinnycard);
            sn_spinnycard.Position += new Mogre.Vector3(0f, 10f, 0f);
            sn_spinnycard.Roll(new Radian(new Degree(90f)));
            sn_spinnycard.Scale(.05f, .05f, .05f);

            ent_table = OgreWindow.Instance.mSceneMgr.CreateEntity("tongitstable", "\\TongIts\\tongitstable.mesh");
            ent_table.SetMaterialName("wood");
            sn_table = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sn_table.AttachObject(ent_table);

            //ent_cat = OgreWindow.Instance.mSceneMgr.CreateEntity("cerealbox1Entity", "\\cat.mesh");
            //ent_cat.SetMaterialName("cat1");
            //sn_cat = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            //sn_cat.AttachObject(ent_cat);
            ready = true;
        }
        public override void shutdown()
        {
            log("shutting down!");
            OgreWindow.Instance.mSceneMgr.DestroyLight(testLight);
            OgreWindow.Instance.mSceneMgr.DestroyEntity(ent_table);
            OgreWindow.Instance.mSceneMgr.DestroyEntity(ent_ground);
            //OgreWindow.Instance.mSceneMgr.DestroyEntity(ent_cat);
            OgreWindow.Instance.mSceneMgr.DestroyEntity(ent_spinnycard);
            //OgreWindow.Instance.mSceneMgr.DestroySceneNode(sn_cat);
            OgreWindow.Instance.mSceneMgr.DestroySceneNode(sn_table);
            OgreWindow.Instance.mSceneMgr.DestroySceneNode(sn_spinnycard);
            OgreWindow.Instance.mSceneMgr.DestroySceneNode(sn_ground);
        }
        public override ExtraMegaBlob.References.Vector3 Location()
        {
            return new ExtraMegaBlob.References.Vector3(0, 10, 15);
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
                sn_spinnycard.Pitch(new Radian(new Degree(1f)));

            }
        }
        timer scaleLimiter = new timer(new TimeSpan(0, 0, 1));

        private bool ready = false;
        public override void frameHook(float interpolation)
        {

        }
        private Random ran = new Random((int)DateTime.Now.Ticks);


        private Light testLight;
        private SceneNode sn_spinnycard;
        private Entity ent_spinnycard;
        ExtraMegaBlob.References.timer t = new ExtraMegaBlob.References.timer(new TimeSpan(0, 0, 0, 0, 1000));
    }
}
