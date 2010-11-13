using System;
using MogreFramework;
using ThingReferences;
namespace thing
{
    class ServerRoomTemp : ClientPlugin
    {
        public override string[] AllowedInputNames()
        {
            return new string[] { };
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] { "Zeliard" }; //specify what can get data from the server?
        }
        public override string Name()
        {
            return "server";
        }
        public override void inbox(Event ev)
        {
            switch (ev._Keyword)
            {
                default:
                    break;
            }
        }
        public override void shutdown()
        {
            this._shutdown = true;
            quit("shutdown called");
        }
        private bool _shutdown = false;
        public ServerRoomTemp()
            : base()
        {
        }
        public override void sceneHook(OgreWindow win)
        {
            this.win = win;
        }
        public override void frameHook(float interpolation)//called every video frame before render
        {
            if (!ready()) { frameshavestarted = true; return; }
        }
        public override void updateHook()//called every 10ms
        {
            if (!ready()) return;
        }
        private bool frameshavestarted = false;
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
