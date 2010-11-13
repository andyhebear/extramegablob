using MogreFramework;
using Mogre;
namespace ThingReferences
{
    public abstract class ClientPlugin
    {
        public abstract void startup();
        public abstract void shutdown();
        public abstract string[] AllowedOutputNames();
        public abstract string[] AllowedInputNames();
        public abstract string Name();
        public abstract void frameHook(float interpolation);
        public abstract void updateHook();
        public event LogDelegate onQuit;
        public void quit(string msg)
        {
            if (!object.Equals(null, this.onQuit))
            {
                onQuit(this.Name() + msg);
            }
        }
        public delegate void outboxDelegate(ClientPlugin Sender, Event ev);
        public event outboxDelegate onOutboxMessage;
        public void outboxMessage(ClientPlugin Sender, Event ev)
        {
            ev._Source_FullyQualifiedName = Sender.Name();
            if (!object.Equals(null, this.onOutboxMessage))
            {
                onOutboxMessage(Sender, ev);
            }
        }
        public event LogDelegate onChat;
        public void chat(string msg)
        {
            if (!object.Equals(null, this.onChat))
            {
                onChat(msg);
            }
        }
        public event LogDelegate onLog;
        public void log(string msg)
        {
            if (!object.Equals(null, this.onLog))
            {
                onLog(msg);
            }
        }
        public abstract void inbox(Event ev);
        public abstract Vector3 Location();
        public abstract float Radius();
    }
}
