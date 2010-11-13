using System.Threading;
using Mogre;
using MogreFramework;
using MOIS;
namespace thing
{
    class newroom : Room
    {
        Thread t_updatemovementflags = null;
        private bool _shutdown = false;
        private void thread_updatemovementflags()
        {
            while (!_shutdown)
            {
                threadHook();
                Thread.Sleep(100);
            }
        }
        public newroom()
            : base()
        {
            t_updatemovementflags = new Thread(new ThreadStart(thread_updatemovementflags));
            t_updatemovementflags.Start();
        }
        public override void shutdown()
        {
            this._shutdown = true;
        }
        private Entity ent1;
        private SceneNode sn1;
        Mogre.Vector3 TranslateVector_sn1 = new Mogre.Vector3();
        public override void sceneHook(OgreWindow win)
        {
            this.win = win;
            ent1 = win.mSceneMgr.CreateEntity("zeliard", "zeliard.mesh");
            ent1.CastShadows = true;
            sn1 = win.mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sn1.AttachObject(ent1);
            sn1.Position -= new Mogre.Vector3(3f, 6f, 14f);
        }
        public override void frameHook()//called every video frame before render
        {
            if (!readyCheck) { ready = true; return; }
            sn1.Translate(TranslateVector_sn1);
        }
        private void threadHook()//called every 100ms
        {
            if (!readyCheck) return;
            if (faraway)
                sn1.ShowBoundingBox = false;
            else
                sn1.ShowBoundingBox = true;

            if (keyboard.IsKeyDown(MOIS.KeyCode.KC_E))
            {
                TranslateVector_sn1.z = 0.001f;//relative
            }
           
            else
            {
                TranslateVector_sn1.z = 0f;
            }

        }
        private bool ready = false;
        private bool faraway
        {
            get
            {
                if (readyCheck)
                    return (thing.Game.distanceBetweenPythagCartesian(win.cameraNode.Position, sn1.Position) > 9) ? true : false;
                else
                    return true;
            }
        }
        private bool readyCheck
        {
            get
            {
                if (ready)
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
}
