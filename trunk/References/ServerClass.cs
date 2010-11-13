namespace ThingReferences
{
    public abstract class ServerClass
    {
        public abstract void shutdown();
        public abstract string[] AllowedOutputNames();
        public abstract string[] AllowedInputNames();
        public abstract string Name();
        public abstract void updateHook();
        public abstract bool ready();//check if ready
        public delegate void LogDelegate(string msg);
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
        public event LogDelegate onLog;
        public void log(string msg)
        {
            if (!object.Equals(null, this.onLog))
            {
                onLog("[" + this.Name() + "]: " + msg);
            }
        }
        public abstract void inbox(Event ev);
    }
}
