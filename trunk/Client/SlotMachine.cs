using System;
using System.Threading;
using MogreFramework;
using System.Collections;
using ThingReferences;
using Mogre;
namespace thing
{
    class SlotMachine : ClientPlugin
    {
        public override string[] AllowedOutputNames()
        {
            return new string[] { "Zeliard" };
        }
        public override string Name()
        {
            return "SlotMachine";
        }
        public override string[] AllowedInputNames()
        {
            return new string[] { "Zeliard_" };
        }
        public override void inbox(Event ev)
        {
            throw new NotImplementedException();
        }
        public override void updateHook()//called every 100ms
        {
            if (!ready()) return;
            if (faraway)
                sn1.ShowBoundingBox = false;
            else
                sn1.ShowBoundingBox = true;

            if (keyboard.IsKeyDown(MOIS.KeyCode.KC_E))
            {
                TranslateVector_sn1.z = 0.001f;
            }
            else
            {
                TranslateVector_sn1.z = 0f;
            }
            if (b2)
            {
                rotatedirection = rotatedirection - (rotatedirection * .05f);
            }
            else
            {
                rotatedirection = rotatedirection + (rotatedirection * .05f);
            }
            if (rotatedirection > 2 || rotatedirection < -2)
            {
                b2 = (!b2);
            }
            if (time_rolldice.AddSeconds(10).CompareTo(DateTime.Now) < 0)
            {
                rotatescale_drum1 = 0f;
                rotatescale_drum2 = 0f;
                rotatescale_drum3 = 0f;
                rotatescale_drum4 = 0f;
            }
            //if (rotatescale_drum1 < -2)
            //{
            //    rotatescale_drum1 -= !rotatedirection;
            //}
            rotatescale_drum1 += umf_step1 * rotatedirection * roll[0];
            rotatescale_drum2 += umf_step1 * rotatedirection * roll[1];
            rotatescale_drum3 += umf_step1 * rotatedirection * roll[2];
            rotatescale_drum4 += umf_step1 * rotatedirection * roll[3];
            if (faraway)
                far_slotmachine1 = true;
            else
                far_slotmachine1 = false;
        }
        public void rolldice()
        {
            Random ra = new Random();
            roll = new float[] { 1f, 1f, 1f, 1f };
            //12 symbols per die
            roll[0] = roll[0] * float.Parse(ra.Next(100).ToString()) * .001f;
            roll[1] = roll[1] * float.Parse(ra.Next(100).ToString()) * .001f;
            roll[2] = roll[2] * float.Parse(ra.Next(100).ToString()) * .001f;
            roll[3] = roll[3] * float.Parse(ra.Next(100).ToString()) * .001f;
            time_rolldice = DateTime.Now;
            rotatescale_drum1 = .001f;
            rotatescale_drum2 = .001f;
            rotatescale_drum3 = .001f;
            rotatescale_drum4 = .001f;
            umf_step1 = .001f;
            rotatedirection = .1f;
            b2 = false;
        }
        public float rotatescale_drum1 = .001f;
        public float rotatescale_drum2 = .001f;
        public float rotatescale_drum3 = .001f;
        public float rotatescale_drum4 = .001f;
        private float[] roll = new float[] { 1f, 1f, 1f, 1f };
        private float umf_step1 = .001f;
        private float rotatedirection = .1f;
        private bool b2 = false;
        public override void shutdown()
        {
            this._shutdown = true;
        }
        DateTime time_rolldice = DateTime.Now;
        private bool _shutdown = false;

        public SlotMachine()
            : base()
        {
        }
        Mogre.Vector3 TranslateVector_sn1 = new Mogre.Vector3();//relative
        Entity entity_myentity = null;
        SceneNode sn_barrel1 = null;
        Entity entity_myentity2 = null;
        SceneNode sn_barrel2 = null;
        Entity entity_myentity3 = null;
        SceneNode sn_barrel3 = null;
        Entity entity_myentity4 = null;
        SceneNode sn_barrel4 = null;
        ArrayList entities = new ArrayList();
        ArrayList scenenodes = new ArrayList();
        bool far_slotmachine1 = false;
        public override void setWindow(OgreWindow win)
        {
            this.win = win;
            //// Create a slot machine barrel
            entity_myentity = win.mSceneMgr.CreateEntity("cylinder123", "cylinder.mesh");
            entity_myentity.CastShadows = true;
            sn_barrel1 = win.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sn_barrel1.AttachObject(entity_myentity);
            sn_barrel1.Position -= new Mogre.Vector3(8f, 6f, 14f);
            sn_barrel1.Rotate(new Mogre.Vector3(0.0f, 0.0f, 1.0f), new Radian(1.55f));
            //// Create a slot machine barrel
            entity_myentity2 = win.mSceneMgr.CreateEntity("cylinder2", "cylinder.mesh");
            entity_myentity2.CastShadows = true;
            sn_barrel2 = win.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sn_barrel2.AttachObject(entity_myentity2);
            sn_barrel2.Position -= new Mogre.Vector3(7.42f, 6f, 14f);
            sn_barrel2.Rotate(new Mogre.Vector3(0.0f, 0.0f, 1.0f), new Radian(1.55f));
            //// Create a slot machine barrel
            entity_myentity3 = win.mSceneMgr.CreateEntity("cylinder3", "cylinder.mesh");
            entity_myentity3.CastShadows = true;
            sn_barrel3 = win.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sn_barrel3.AttachObject(entity_myentity3);
            sn_barrel3.Position -= new Mogre.Vector3(6.84f, 6f, 14f);
            sn_barrel3.Rotate(new Mogre.Vector3(0.0f, 0.0f, 1.0f), new Radian(1.55f));
            //// Create a slot machine barrel
            entity_myentity4 = win.mSceneMgr.CreateEntity("cylinder4", "cylinder.mesh");
            entity_myentity4.CastShadows = true;
            sn_barrel4 = win.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sn_barrel4.AttachObject(entity_myentity4);
            sn_barrel4.Position -= new Mogre.Vector3(6.26f, 6f, 14f);
            sn_barrel4.Rotate(new Mogre.Vector3(0.0f, 0.0f, 1.0f), new Radian(1.55f));
            ent1 = win.mSceneMgr.CreateEntity("border", "border.mesh");
            ent1.CastShadows = true;
            sn1 = win.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sn1.AttachObject(ent1);
            sn1.Position -= new Mogre.Vector3(8.14f, 6f, 14f);
            //ent1 = win.mSceneMgr.CreateEntity("zeliard", "zeliard.mesh");
            //ent1.CastShadows = true;
            //sn1 = win.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            //sn1.AttachObject(ent1);
            //sn1.Position -= new Mogre.Vector3(3f, 6f, 14f);
        }
        Entity ent1;
        SceneNode sn1;
        public override void frameHook(float interpolation)//called every video frame before render
        {
            if (!ready()) { frameshavestarted = true; return; }
            // sn1.Translate(TranslateVector_sn1);
            // ==================
            if (keyboard.IsKeyDown(MOIS.KeyCode.KC_E))
            {
                rolldice();
                win.log("rolling dice");
            }
            if (far_slotmachine1)
            {
                //entity_slotborder.Visible = false;
                sn1.ShowBoundingBox = false;
            }
            else
            {
                //entity_slotborder.Visible = true;
                sn1.ShowBoundingBox = true;
            }
            if (rotatescale_drum1 > 0f)
                sn_barrel1.Yaw(new Radian(rotatescale_drum1));
            if (rotatescale_drum2 > 0f)
                sn_barrel2.Yaw(new Radian(rotatescale_drum2));
            if (rotatescale_drum3 > 0f)
                sn_barrel3.Yaw(new Radian(rotatescale_drum3));
            if (rotatescale_drum4 > 0f)
                sn_barrel4.Yaw(new Radian(rotatescale_drum4));


            if (keyboard.IsKeyDown(MOIS.KeyCode.KC_R))
            {
                win.log("realigning barrels to initial quaternions");
                sn_barrel4.Orientation = sn_barrel4.InitialOrientation;
                sn_barrel3.Orientation = sn_barrel3.InitialOrientation;
                sn_barrel2.Orientation = sn_barrel2.InitialOrientation;
                sn_barrel1.Orientation = sn_barrel1.InitialOrientation;
            }
        }

        private bool frameshavestarted = false;

        bool DistanceChangeDetect = false;
        private bool faraway
        {
            get
            {
                if (ready())
                {
                    bool rdy = (ThingReferences.Math.distanceBetweenPythagCartesian(win.cameraNode.Position, sn1.Position) > 10) ? true : false;
                    if (DistanceChangeDetect != rdy)
                    {
                        DistanceChangeDetect = rdy;
                        string expl = "you are now " + ((rdy) ? "FAR" : "NEAR");
                        win.log(expl);
                    }
                    return rdy;
                }
                else
                    return true;
            }
        }


        public override bool ready()
        {
            if (_shutdown) return false;
            if (frameshavestarted)
                if (!object.Equals(null, win) && !object.Equals(null, sn1) && !object.Equals(null, ent1))
                {
                    if (!object.Equals(null, inputmanager) && !object.Equals(null, keyboard) && !object.Equals(null, mouse))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            else return false;
        }
    }
}
