using System;
using System.Collections.Generic;
using System.Text;
using MogreFramework;
using ThingReferences;
using Mogre;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
namespace thing
{
    public class plugin : ThingReferences.ClientPlugin
    {
        private void makeMaterials()
        {
            try
            {
                ((MaterialPtr)MaterialManager.Singleton.Create("cerealbox1", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\cheerios.jpg");


                ((MaterialPtr)MaterialManager.Singleton.Create("club 10", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_club_10.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("club 2", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_club_2.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("club 3", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_club_3.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("club 4", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_club_4.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("club 5", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_club_5.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("club 6", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_club_6.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("club 7", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_club_7.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("club 8", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_club_8.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("club 9", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_club_9.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("club a", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_club_A.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("club j", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_club_J.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("club k", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_club_K.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("club q", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_club_Q.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("diamond 10", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_diamond_10.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("diamond 2", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_diamond_2.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("diamond 3", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_diamond_3.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("diamond 4", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_diamond_4.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("diamond 5", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_diamond_5.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("diamond 6", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_diamond_6.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("diamond 7", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_diamond_7.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("diamond 8", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_diamond_8.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("diamond 9", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_diamond_9.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("diamond a", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_diamond_A.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("diamond j", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_diamond_J.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("diamond k", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_diamond_K.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("diamond q", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_diamond_Q.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("heart 10", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_heart_10.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("heart 2", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_heart_2.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("heart 3", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_heart_3.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("heart 4", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_heart_4.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("heart 5", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_heart_5.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("heart 6", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_heart_6.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("heart 7", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_heart_7.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("heart 8", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_heart_8.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("heart 9", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_heart_9.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("heart a", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_heart_A.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("heart j", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_heart_J.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("heart k", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_heart_K.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("heart q", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_heart_Q.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("spade 10", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_spade_10.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("spade 2", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_spade_2.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("spade 3", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_spade_3.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("spade 4", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_spade_4.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("spade 5", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_spade_5.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("spade 6", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_spade_6.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("spade 7", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_spade_7.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("spade 8", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_spade_8.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("spade 9", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_spade_9.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("spade a", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_spade_A.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("spade j", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_spade_J.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("spade k", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_spade_K.svg.png");
                ((MaterialPtr)MaterialManager.Singleton.Create("spade q", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\TongIts\\200px-Playing_card_spade_Q.svg.png");
            }
            catch (Exception ex) { log(ex.Message); }
        }

        private SceneNode cerealbox1Node;
        private Entity cerealbox1Entity;
        //private Mesh cerealbox1Mesh;

        public override void startup()
        {
            log("starting up!");
            makeMaterials();
            testSceneNode = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            OgreWindow.Instance.meshes.Add(MeshManager.Singleton.CreatePlane("TestPlane1", "General", new Plane(Mogre.Vector3.UNIT_Y, 0), 200, 250, 1, 1, true, 1, 1, 1, Mogre.Vector3.UNIT_X));
            testPlaneEntity = OgreWindow.Instance.mSceneMgr.CreateEntity("TestPlaneEntity", "TestPlane1");
            testPlaneEntity.SetMaterialName("heart 6");
            testSceneNode.AttachObject(testPlaneEntity);
            testSceneNode.Position += new Mogre.Vector3(0f, 10f, 0f);
            testSceneNode.Roll(new Radian(new Degree(90f)));
            testSceneNode.Scale(.05f, .05f, .05f);

            // Create the first light
            testLight = OgreWindow.Instance.mSceneMgr.CreateLight("testLight");
            testLight.Type = Light.LightTypes.LT_POINT;
            testLight.Position = new Mogre.Vector3(-126.605f, 20f, 36.84557f);
            testLight.DiffuseColour = ColourValue.White;
            testLight.SpecularColour = ColourValue.White;


            //add cereal box
            cerealbox1Node = OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            //  MeshPtr ptrMeshBox = OgreWindow.Instance.meshes["\\cheerios.mesh"];
            cerealbox1Entity = OgreWindow.Instance.mSceneMgr.CreateEntity("cerealbox1Entity", "\\cheerios.mesh"); //load the actual file i guess; cachemanager is supposed to do all background preloading????
            //  Mesh m = (Mesh)ptrMeshBox;
            cerealbox1Entity.SetMaterialName("cerealbox1");
            cerealbox1Node.AttachObject(cerealbox1Entity);
            ready = true;

            
        }
        public override void shutdown()
        {
            log("shutting down!");

            //OgreWindow.Instance.mRoot.


            //OgreWindow.Instance.mSceneMgr.DestroyLight(testLight);
            //OgreWindow.Instance.mSceneMgr.DestroyEntity(testPlaneEntity);
            //OgreWindow.Instance.mSceneMgr.DestroySceneNode(testSceneNode);

            //OgreWindow.Instance.mSceneMgr.DestroyLight(testLight.Name);
            //OgreWindow.Instance.mSceneMgr.DestroyEntity(testPlaneEntity.Name);
            //OgreWindow.Instance.mSceneMgr.DestroySceneNode(testSceneNode.Name);
        }
        public override ThingReferences.Vector3 Location()
        {
            return new ThingReferences.Vector3(0, 0, 15);
        }
        public override float Radius()
        {
            return 30;
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
        public override void inbox(ThingReferences.Event ev)
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
                testSceneNode.Pitch(new Radian(new Degree(1f)));

            }
        }
        timer scaleLimiter = new timer(new TimeSpan(0, 0, 1));

        private bool ready = false;
        public override void frameHook(float interpolation)
        {

        }
        private Random ran = new Random((int)DateTime.Now.Ticks);


        private Light testLight;
        private SceneNode testSceneNode;
        private Entity testPlaneEntity;
        ThingReferences.timer t = new ThingReferences.timer(new TimeSpan(0, 0, 0, 0, 1000));
    }
}
