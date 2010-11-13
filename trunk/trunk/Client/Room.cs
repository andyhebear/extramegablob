using MogreFramework;
using MOIS;
using thing.Parts;
namespace thing
{
    abstract class Room
    {
        public abstract void shutdown();
        public Mouse mouse;
        public Keyboard keyboard;
        public InputManager inputmanager;
        public abstract string[] AllowedOutputNames();
        public abstract string[] AllowedInputNames();
        public abstract string Name();
        internal OgreWindow win;
        public Room()
        {
        }
        public abstract void sceneHook(OgreWindow mainwindow);
        public abstract void frameHook(float interpolation);
        public abstract void updateHook();
        public abstract bool ready();//check if ready

        public delegate void LogDelegate(string msg);
        public event LogDelegate onQuit;
        internal void quit(string msg)
        {
            if (!object.Equals(null, this.onQuit))
            {
                onQuit(this.Name() + msg);
            }
        }
        public delegate void outboxDelegate(Room Sender, Event ev);
        public event outboxDelegate onOutboxMessage;
        internal void outboxMessage(Room Sender, Event ev)
        {
            if (!object.Equals(null, this.onOutboxMessage))
            {
                onOutboxMessage(Sender,ev);
            }
        }

        public event LogDelegate onLog;
        internal void log(string msg)
        {
            if (!object.Equals(null, this.onLog))
            {
                onLog("[" + this.Name() + "]: " + msg);
            }
        }
        public abstract void inbox(Event ev);
    }
}
