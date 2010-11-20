using System;
using System.Collections.Generic;
using System.Text;
using MogreFramework;
using ThingReferences;
using Mogre;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.IO;
namespace thing
{
    public class plugin : ThingReferences.ClientPlugin
    {
        private void makeMaterials()
        {
            try
            {
                ((MaterialPtr)MaterialManager.Singleton.Create("cat1", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState("\\cat2.jpg");


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
            cerealbox1Entity = OgreWindow.Instance.mSceneMgr.CreateEntity("cerealbox1Entity", "\\cat.mesh"); //load the actual file i guess; cachemanager is supposed to do all background preloading????
            //  Mesh m = (Mesh)ptrMeshBox;

            // GpuProgramPtr resource = GpuProgramManager.Singleton.CreateProgramFromString("blah", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, program, GpuProgramType.GPT_FRAGMENT_PROGRAM, "");

            string syntax = "";
            string wrld = ResourceGroupManager.Singleton.WorldResourceGroupName;
            #region syntax
            if (GpuProgramManager.Singleton.IsSyntaxSupported("arbvp1"))
            {
                syntax = "arbvp1";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("arbfp1"))
            {
                syntax = "arbfp1";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("vs_3_0"))
            {
                syntax = "vs_3_0";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("vs_2_x"))
            {
                syntax = "vs_2_x";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("vs_2_a"))
            {
                syntax = "vs_2_a";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("vs_2_0"))
            {
                syntax = "vs_2_0";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("vs_1_1"))
            {
                syntax = "vs_1_1";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("ps_3_0"))
            {
                syntax = "ps_3_0";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("ps_2_x"))
            {
                syntax = "ps_2_x";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("ps_2_b"))
            {
                syntax = "ps_2_b";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("ps_2_a"))
            {
                syntax = "ps_2_a";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("ps_2_0"))
            {
                syntax = "ps_2_0";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("ps_2_a"))
            {
                syntax = "ps_2_a";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("ps_1_4"))
            {
                syntax = "ps_1_4";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("ps_1_3"))
            {
                syntax = "ps_1_3";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("ps_1_2"))
            {
                syntax = "ps_1_2";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("ps_1_1"))
            {
                syntax = "ps_1_1";
            }
            else if (GpuProgramManager.Singleton.IsSyntaxSupported("hlsl"))
            {
                syntax = "hlsl";
            }

            else
            {
            }
            #endregion

            //syntax = "ps_1_4";
            string b = "\\programs\\Example_FresnelPS.asm";
            string d = "ocean1.vert";


            GpuProgramPtr prog_frag = GpuProgramManager.Singleton.CreateProgramFromString(d, wrld, Helpers.getFileString(ThingPath.path_cache + b), Mogre.GpuProgramType.GPT_FRAGMENT_PROGRAM, syntax);
            //prog_frag.Load();
            //GpuProgramPtr prog_vert = GpuProgramManager.Singleton.CreateProgramFromString("ocean1.vert", wrld, "", Mogre.GpuProgramType.GPT_VERTEX_PROGRAM, syntax);

            //prog_frag.
            //prog_frag
            try
            {
                prog_frag.Load();
            }
            catch
            {
                if (OgreException.IsThrown)
                    log(OgreException.LastException.FullDescription);
            }

            // c0 : distortionRange
            // c1 : tintColour
            // testure 0 : noiseMap
            // texture 1 : reflectMap
            // texture 2 : refractMap
            // v0.x : fresnel 
            // t0.xyz : noiseCoord
            // t1.xyw : projectionCoord 

            //((GpuProgram)prog_frag).SetVertexTextureFetchRequired(true)

            //prog.Name
            // cerealbox1Entity.SetMaterialName("\\programs\\Ocean2HLSL_Cg.frag", wrld);

            //((MaterialPtr)MaterialManager.Singleton.Create("spade q", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0

            MaterialPtr ptr = MaterialManager.Singleton.Create("ocean1", wrld);
            Pass pass0 = ptr.GetTechnique(0).GetPass(0);
            pass0.SetFragmentProgram(d, true);
            prog_frag.Load();
            GpuProgramParametersSharedPtr paramsPtr = pass0.GetFragmentProgramParameters();
            //paramsPtr.SetNamedConstant("c1", Mogre.ColourValue.Green);
            paramsPtr.SetConstant(0, 13f);
            paramsPtr.SetConstant(1, ColourValue.Green);
            // paramsPtr.SetConstant(2,OgreWindow.Instance.textures[""]);
           // paramsPtr.SetConstant(2, ((double)OgreWindow.Instance.textures["\\cheerios.jpg"].NativePtr));

            pass0.SetFragmentProgramParameters(paramsPtr);
            //Technique t = ptr.CreateTechnique();
            //Pass pass1 = ptr.GetTechnique(0).CreatePass();
            //pass1.CreateTextureUnitState("\\TongIts\\cheerios.jpg");
            //pass0.CreateTextureUnitState("\\TongIts\\cheerios.jpg");
            //pass1.SetFragmentProgram(d, true);
            //pass1.SetVertexProgram(d, true);
            //t.GetPass(0).CreateTextureUnitState("\\TongIts\\cheerios.jpg");

            // p.SetVertexProgram(d, true);
            //p.SetVertexProgram("ocean1.vert", true);




            //ptr.Compile();



            //paramsPtr.SetIgnoreMissingParams(true);

            //MaterialManager.Singleton.GetByName("\\programs\\Ocean2HLSL_Cg.frag", wrld);

            //cerealbox1Entity.SetMaterial(ptr); //crashes
            //cerealbox1Entity.SetMaterialName("ocean1"); //crashes
            cerealbox1Entity.SetMaterialName("cat1");
            cerealbox1Node.AttachObject(cerealbox1Entity);


            OgreWindow.Instance.mSceneMgr.GetEntity("GroundEntity").SetMaterialName("ocean1");





            //ResourceLoaderGpuProgram loader = new ResourceLoaderGpuProgram(ThingPath.path_cache);

            //GpuProgramPtr gpuProgramPtr = GpuProgramManager.Singleton.Load("Ocean2HLSL_Cg.frag", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, true, loader);
            ////GpuProgramManager.Singleton.CreateProgramFromString



            ready = true;


        }
        private unsafe class ResourceLoaderGpuProgram : ManualResourceLoader
        {
            private string path_cache = "";
            public ResourceLoaderGpuProgram(string path_cache)
            {
                this.path_cache = path_cache;
            }
            public override void LoadResource(Resource resource)
            {
                string pathAbs = path_cache + resource.Name;
                string program = Helpers.getFileString(pathAbs);

                resource = GpuProgramManager.Singleton.CreateProgramFromString(resource.Name, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, program, GpuProgramType.GPT_FRAGMENT_PROGRAM, "");
            }
            public override void PrepareResource(Resource resource)
            {
                base.PrepareResource(resource);
            }
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
