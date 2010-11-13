using System;
using System.Collections;
using Mogre;
using MogreFramework;
using ThingReferences;
namespace thing
{
    class zeliard : ClientClass
    {
        private Random ran = new Random((int)DateTime.Now.Ticks);
        public override string[] AllowedInputNames()
        {
            return new string[] { "server" };
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] { };
        }
        public override string Name()
        {
            return "Zeliard";
        }
        public override void inbox(Event ev)
        {
            switch (ev._Keyword)
            {
                case KeyWord.STATE_BROADCASTING:
                    if (ev._Memories[KeyWord.TESTLOOP] != null)
                    {
                        int g = Int32.Parse(ev._Memories[KeyWord.TESTLOOP].Value) + 1;
                        log("testloop: " + g.ToString());

                        Memory mem = new Memory("", KeyWord.TESTLOOP, g.ToString());
                        Memories mems = new Memories();
                        mems.Add(mem);
                        Event ev2 = new Event();
                        ev2._Keyword = KeyWord.STATE_BROADCASTING;
                        ev2._Memories = mems;
                        ev2._IntendedRecipients = eventScope.SERVERALL;
                        base.outboxMessage(this, ev2);
                        addCheeriosBox();
                        //string attacker = ev._Memories[KeyWord.STATE_READY].Value;
                        //addZelBox();
                        //for (int i = 0; i < ZelBoxesSNs.Count; i++)
                        //{
                        //    SceneNode sninquestion = ((SceneNode)ZelBoxesSNs[i]);
                        //    if (sninquestion.Name == attacker)
                        //    {
                        //        //float scale = ((float)ran.NextDouble() * 2);
                        //        sninquestion.ResetToInitialState();
                        //    }
                        //}
                    }
                    break;
            }
        }
        public void initTestLoop()
        {
            Memory mem = new Memory("", KeyWord.TESTLOOP, "1");
            Memories mems = new Memories();
            mems.Add(mem);
            Event ev = new Event();
            ev._Keyword = KeyWord.STATE_BROADCASTING;
            ev._Memories = mems;
            ev._IntendedRecipients = eventScope.SERVERALL;
            base.outboxMessage(this, ev);
        }
        public override void shutdown()
        {
            this._shutdown = true;
            quit("shutdown called");
        }
        DateTime time_rolldice = DateTime.Now;
        private bool _shutdown = false;
        public zeliard(string guid)
            : base()
        {
            collisionPrevention = guid;
        }
        private string collisionPrevention = "";

        public override void sceneHook(OgreWindow win)
        {
            this.win = win;
            //// Create a slot machine barrel
            //ent_zelbox = win.mSceneMgr.CreateEntity("zelbox_" + collisionPrevention, "zeliard.mesh");
            //ent_zelbox.CastShadows = true;
            //sn_zelbox = win.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            //sn_zelbox.AttachObject(ent_zelbox);
            //sn_zelbox.Position -= new Mogre.Vector3(4f, 4f, 12f);
            //sn_zelbox.Rotate(new Quaternion(.28f, 0f, -.95f, .16f));


        }

        private ArrayList ZelBoxesEnts = new ArrayList();
        private ArrayList ZelBoxesSNs = new ArrayList();

        private void addZelBox()
        {
            string name = Helpers.RandomString(8, ref ran);
            Entity ent_zelbox = win.mSceneMgr.CreateEntity(name, "zeliard.mesh");
            ent_zelbox.CastShadows = true;
            SceneNode sn_zelbox = win.mSceneMgr.RootSceneNode.CreateChildSceneNode(name);
            sn_zelbox.AttachObject(ent_zelbox);
            sn_zelbox.Position -= new Mogre.Vector3((float)Helpers.RandomInt(0, 20, ref ran), (float)Helpers.RandomInt(0, 20, ref ran), (float)Helpers.RandomInt(0, 20, ref ran));
            sn_zelbox.Rotate(new Quaternion(.28f, 0f, -.95f, .16f));
            ZelBoxesEnts.Add(ent_zelbox);
            ZelBoxesSNs.Add(sn_zelbox);
            _myBoxes.Add(name);
        }
        private void addCheeriosBox()
        {
            if (win.ShuttingDown) return;
            string name = Helpers.RandomString(8, ref ran);
            Entity ent_zelbox = win.mSceneMgr.CreateEntity(name, "cheerios.mesh");
            ent_zelbox.CastShadows = true;
            SceneNode sn_zelbox = win.mSceneMgr.RootSceneNode.CreateChildSceneNode(name);
            sn_zelbox.AttachObject(ent_zelbox);
            sn_zelbox.Position -= new Mogre.Vector3((float)Helpers.RandomInt(0, 20, ref ran), (float)Helpers.RandomInt(0, 20, ref ran), (float)Helpers.RandomInt(0, 20, ref ran));
            sn_zelbox.Rotate(new Quaternion(.28f, 0f, -.95f, .16f));
            ZelBoxesEnts.Add(ent_zelbox);
            ZelBoxesSNs.Add(sn_zelbox);
            _myBoxes.Add(name);
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

        public override void frameHook(float interpolation)//called every video frame before render
        {
            if (!ready()) { frameshavestarted = true; return; }

        }
        private void randomBorder(bool onoff)
        {
            if (ZelBoxesSNs.Count < 1)
            {
                return;
            }
            ((SceneNode)ZelBoxesSNs[Helpers.RandomInt(0, ZelBoxesSNs.Count, ref ran)]).ShowBoundingBox = onoff;
        }
        public override void updateHook()//called every 10ms
        {
            if (!ready()) return;
            //if (faraway)
            //    randomBorder(false);
            //else
            //{
            //    randomBorder(true);
            //}
            if (keyboard.IsKeyDown(MOIS.KeyCode.KC_Q))
            {

                initTestLoop();
            }
            if (keyboard.IsKeyDown(MOIS.KeyCode.KC_H))
            {
                if (ZelBoxesSNs.Count > 0)
                {
                    for (int i = 0; i < ZelBoxesSNs.Count; i++)
                    {
                        win.mSceneMgr.DestroySceneNode(((SceneNode)ZelBoxesSNs[i]).Name);


                    }
                    ZelBoxesSNs = new ArrayList();
                    ZelBoxesEnts = new ArrayList();
                }
            }
            //if (keyboard.IsKeyDown(MOIS.KeyCode.KC_J))
            //{
            //    if (ZelBoxesSNs.Count > 0)
            //    {
            //        sendAttack();
            //    }
            //}


            if (keyboard.IsKeyDown(MOIS.KeyCode.KC_Z))
            {
                foreach (SceneNode sn in ZelBoxesSNs)
                {
                    sn.Yaw(new Radian(new Degree(1f)));
                }
            }
            if (keyboard.IsKeyDown(MOIS.KeyCode.KC_X))
            {
                foreach (SceneNode sn in ZelBoxesSNs)
                {
                    sn.Pitch(new Radian(new Degree(1f)));
                }
            }
            if (keyboard.IsKeyDown(MOIS.KeyCode.KC_C))
            {
                foreach (SceneNode sn in ZelBoxesSNs)
                {
                    sn.Roll(new Radian(new Degree(1f)));
                }
            }
        }
        private void logZelOrientation()
        {
            //string s = "";
            return;
            //sn_zelbox.Orientation.x.ToString("N");
            //s += "X: " + sn_zelbox.Orientation.x.ToString("N") + "  ";
            //s += "Y: " + sn_zelbox.Orientation.y.ToString("N") + "  ";
            //s += "Z: " + sn_zelbox.Orientation.z.ToString("N") + "  ";
            //s += "W: " + sn_zelbox.Orientation.w.ToString("N") + "  ";
            //log(s);
        }
        private bool frameshavestarted = false;
        bool DistanceChangeDetect = false;
        private bool faraway
        {
            get
            {
                if (ready())
                {
                    bool rdy = (ThingReferences.Math.distanceBetweenPythagCartesian(base.win.cameraNode.Position, new Mogre.Vector3(4f, 4f, 12f)) > 10) ? true : false;
                    if (DistanceChangeDetect != rdy)
                    {
                        DistanceChangeDetect = rdy;
                        string expl = "you are now " + ((rdy) ? "FAR" : "NEAR") + " to the zeliard box";
                        log(expl);
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
                if (!object.Equals(null, win))
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
