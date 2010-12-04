namespace ExtraMegaBlob.References
{
    public abstract class ServerPlugin
    {
        public abstract void shutdown();
        public abstract string[] AllowedOutputNames();
        public abstract string[] AllowedInputNames();
        public abstract string Name();
        public abstract void updateHook();
        public event LogDelegate onQuit;
        public void quit(string msg)
        {
            if (!object.Equals(null, this.onQuit))
            {
                onQuit(this.Name() + msg);
            }
        }
        public delegate void outboxDelegate(ServerPlugin Sender, Event ev);
        public event outboxDelegate onOutboxMessage;
        public void outboxMessage(ServerPlugin Sender, Event ev)
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
                onLog(msg);
            }
        }
        public abstract void inbox(Event ev);

        //public delegate void outboxToServerPluginDelegate(Event ev, string targetServerPlugin, eventScope scope);
        //public event outboxToServerPluginDelegate onOutboxMessageToServerPlugin;
        //public void outboxMessage(Event ev, string targetServerPlugins,eventScope scope)
        //{
        //    ev._IntendedRecipients = scope;

        //    if (!object.Equals(null, this.onOutboxMessage))
        //    {
        //        onOutboxMessage(Sender, ev);
        //    }
        //}
    }
}
