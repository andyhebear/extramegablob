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
            testLight.Position = new Mogre.Vector3(-117.9847f, 120f, 234.2695f) + Location().toMogre;
            testLight.DiffuseColour = ColourValue.White;
            testLight.SpecularColour = ColourValue.White;

            testLight2 = OgreWindow.Instance.mSceneMgr.CreateLight("testLight2");
            testLight2.Type = Light.LightTypes.LT_POINT;
            testLight2.Position = new Mogre.Vector3(88.00402f, 30.67856f, -107.2294f) + Location().toMogre;
            testLight2.DiffuseColour = ColourValue.White;
            testLight2.SpecularColour = ColourValue.White;



            //try
            //{
            //    //OgreWindow.Instance.mSceneMgr.SetSkyBox(true, "cat1", 5000, false);
            //    //OgreWindow.Instance.mSceneMgr.SetSkyPlane(true, new Plane(Mogre.Vector3.UNIT_Y, 10), "cat1");
            //   // OgreWindow.Instance.mSceneMgr.SetSkyDome(true, "clouds");

            //    OgreWindow.Instance.mSceneMgr.SetSkyDome(true, "clouds", 5, 8);

            //   // OgreWindow.Instance.mSceneMgr._queueSkiesForRendering(OgreWindow.Instance.mCamera);

            //    chat(OgreWindow.Instance.mSceneMgr.IsSkyDomeEnabled.ToString());
            //   // OgreWindow.Instance.mCamera.FarClipDistance = 500;

            //    //Plane plane;
            //    //plane.d = 1000;
            //    //plane.normal = Mogre.Vector3.NEGATIVE_UNIT_Y;
            //    //OgreWindow.Instance.mSceneMgr.SetSkyPlane(true, plane, "clouds", 1500, 75);
            //}
            //catch (Exception ex) { log(ex.Message); }
        }
        private Entity ent_ground = null;
        private SceneNode sn_ground = null;
        public override void startup()
        {
            log("starting up!");
            makeMaterials();
            putLights();

            MeshManager.Singleton.CreatePlane("ground",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                new Plane(Mogre.Vector3.UNIT_Y, 0),
                1500, 1500, 20, 20, true, 1, 5, 5, Mogre.Vector3.UNIT_Z);
            // Create a ground plane
            ent_ground = OgreWindow.Instance.mSceneMgr.CreateEntity("GroundEntity", "ground");
            ent_ground.CastShadows = false;
            ent_ground.SetMaterialName("dirt");
            sn_ground = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sn_ground.AttachObject(ent_ground);
            sn_ground.Position -= new Mogre.Vector3(0f, 0f, 0f) + Location().toMogre;


            MeshManager.Singleton.CreatePlane("spinnycard", "General", new Plane(Mogre.Vector3.UNIT_Y, 0), 200f, 250f, 1, 1, true, 1, 1, 1, Mogre.Vector3.UNIT_X);
            ent_spinnycard = OgreWindow.Instance.mSceneMgr.CreateEntity("TestPlaneEntity", "spinnycard");
            ent_spinnycard.CastShadows = true;

            MaterialPtr mat2 = MaterialManager.Singleton.GetByName("heart 6");
            mat2.SetCullingMode(CullingMode.CULL_NONE);
            ent_spinnycard.SetMaterial(mat2);

            //ent_spinnycard.SetMaterialName("heart 6");
            sn_spinnycard = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sn_spinnycard.AttachObject(ent_spinnycard);
            sn_spinnycard.Position += new Mogre.Vector3(0f, 2f, 0f) + Location().toMogre;
            //sn_spinnycard.Roll(new Radian(new Degree(90f)));
            //sn_spinnycard.Pitch(new Radian(new Degree(90f)));
            sn_spinnycard.Scale(.001f, .001f, .001f);

            ent_table = OgreWindow.Instance.mSceneMgr.CreateEntity("tongitstable", "\\TongIts\\tongitstable.mesh");
            //ent_table.SetMaterialName("wood");
            sn_table = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sn_table.AttachObject(ent_table);
            sn_table.Position = new Mogre.Vector3(0f, 1f, 0f) + Location().toMogre;
            sn_table.Scale(new Mogre.Vector3(2f));


            //mo_tetra = PrimitiveGenerators.CreateTetrahedron(tetraName, new Mogre.Vector3(1f, 2f, 1f) + Location().toMogre, 1f, "Material/TEXFACE/woodblurred.jpg");
            //mo_tetra.SetMaterialName(0, "clouds");
            //mo_tetra.SetDebugDisplayEnabled(true);
            //OgreWindow.Instance.mSceneMgr.RootSceneNode.AttachObject(mo_tetra);

            //PrimitiveGenerators.CreateTetrahedron2(tetraName);
            //ent_tetra = OgreWindow.Instance.mSceneMgr.CreateEntity(tetraName+"ent", tetraName);
            //sn_tetra = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            //sn_tetra.AttachObject(ent_tetra);
            //sn_tetra.ShowBoundingBox = true;
            //sn_tetra.Position = new Mogre.Vector3(0f, 6f, 0f) + Location().toMogre;
            //sn_tetra.SetDebugDisplayEnabled(true);
            //ent_tetra.SetMaterialName("Material/TEXFACE/BumpyMetal.jpg");
            //Helpers.setEntityOpacity(ent_tetra, .3f);


            //sn_tetra = PrimitiveGenerators.CreateTestTetrahedron(OgreWindow.Instance.mSceneMgr, "asdfadsf", 10f,
            //    new Mogre.Vector3(0f, 6f, 0f) + Location().toMogre,
            //    "Material/TEXFACE/BumpyMetal.jpg");
            //sn_tetra.ShowBoundingBox = true;

            //MeshPtr ptr = PrimitiveGenerators.CreateTestTetrahedron2(OgreWindow.Instance.mSceneMgr, "asdfasdf", 10f,
            //        new Mogre.Vector3(0f, 6f, 0f) + Location().toMogre,
            //    "Material/TEXFACE/BumpyMetal.jpg");
            //OgreWindow.Instance.meshes.Add(ptr);
            //ent_tetra = OgreWindow.Instance.mSceneMgr.CreateEntity("ent_tetra", "asdfasdf_mesh");
            //sn_tetra = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            //sn_tetra.AttachObject(ent_tetra);
            //sn_tetra.ShowBoundingBox = true;
            //sn_tetra.Position = new Mogre.Vector3(0f, 6f, 0f) + Location().toMogre;
            ////sn_tetra.SetDebugDisplayEnabled(true);
            //ent_tetra.SetMaterialName("Material/TEXFACE/BumpyMetal.jpg");



            MeshPtr mytetra = PrimitiveGenerators.makeTetra("mytetra", "heart 7");
            //MeshPtr mytetra = MeshManager.Singleton.GetByName("mytetra");

            ent_tetra = OgreWindow.Instance.mSceneMgr.CreateEntity("ent_tetra", "mytetra");
            //MaterialPtr mat = MaterialManager.Singleton.GetByName("Material/TEXFACE/BumpyMetal.jpg");
            //mat.SetCullingMode(CullingMode.CULL_NONE);
            ////mat.SetSelfIllumination(.2f, .2f, .2f); 
            //ent_tetra.SetMaterial(mat);



            Helpers.setEntityOpacity(ent_tetra, .5f);
            sn_tetra = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sn_tetra.AttachObject(ent_tetra);
            sn_tetra.ShowBoundingBox = true;
            sn_tetra.Position = new Mogre.Vector3(0f, 6f, 0f) + Location().toMogre;
            //sn_tetra.SetDebugDisplayEnabled(true);
            


            //ent_tetra.getm
            //ent_cat = OgreWindow.Instance.mSceneMgr.CreateEntity("cerealbox1Entity", "\\cat.mesh");
            //ent_cat.SetMaterialName("cat1");
            //sn_cat = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            //sn_cat.AttachObject(ent_cat);
            ready = true;
        }
        SceneNode sn_tetra;
        Entity ent_tetra;
        //ManualObject mo_tetra = null;
        public override void shutdown()
        {
            log("shutting down!");
            // remove tetrahedrons
            //if (OgreWindow.Instance.mSceneMgr.HasManualObject(tetraName))
            //    OgreWindow.Instance.mSceneMgr.GetManualObject(tetraName).Clear();

            MeshManager.Singleton.Remove("mytetra");

            //OgreWindow.Instance.mSceneMgr.DestroyManualObject(mo_tetra); 
            OgreWindow.Instance.mSceneMgr.DestroySceneNode(sn_tetra);
            OgreWindow.Instance.mSceneMgr.DestroySceneNode(sn_table);
            OgreWindow.Instance.mSceneMgr.DestroySceneNode(sn_spinnycard);
            OgreWindow.Instance.mSceneMgr.DestroySceneNode(sn_ground);
            OgreWindow.Instance.mSceneMgr.DestroyLight(testLight2);
            OgreWindow.Instance.mSceneMgr.DestroyLight(testLight);
            OgreWindow.Instance.mSceneMgr.DestroyEntity(ent_tetra);
            OgreWindow.Instance.mSceneMgr.DestroyEntity(ent_table);
            OgreWindow.Instance.mSceneMgr.DestroyEntity(ent_ground);
            //OgreWindow.Instance.mSceneMgr.DestroyEntity(ent_cat);
            OgreWindow.Instance.mSceneMgr.DestroyEntity(ent_spinnycard);
            //OgreWindow.Instance.mSceneMgr.DestroySceneNode(sn_cat);

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
                sn_spinnycard.Pitch(new Radian(new Degree(1f)));
                sn_table.Yaw(new Radian(new Degree(-1f)));
                this.sn_tetra.Yaw(new Radian(new Degree(-1f)));
                this.sn_tetra.Pitch(new Radian(new Degree(-1f)));
            }
        }
        timer scaleLimiter = new timer(new TimeSpan(0, 0, 1));

        private bool ready = false;
        public override void frameHook(float interpolation)
        {

        }
        private Random ran = new Random((int)DateTime.Now.Ticks);


        private Light testLight;
        private Light testLight2;
        private SceneNode sn_spinnycard;
        private Entity ent_spinnycard;
        ExtraMegaBlob.References.timer t = new ExtraMegaBlob.References.timer(new TimeSpan(0, 0, 0, 0, 1000));
    }
}
