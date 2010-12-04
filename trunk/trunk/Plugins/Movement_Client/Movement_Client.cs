namespace ExtraMegaBlob
{
    public class plugin : ExtraMegaBlob.References.ClientPlugin
    {
        public override void startup()
        {
            log("starting up!");
        }
        public override void shutdown()
        {
            log("shutting down!");
        }
        public override ExtraMegaBlob.References.Vector3 Location()
        {
            return new ExtraMegaBlob.References.Vector3(43, 0, 15);
        }
        public override float Radius()
        {
            return 30;
        }
        public override string Name()
        {
            return "Movement_Client";
        }
        public override string[] AllowedInputNames()
        {
            return new string[] { "Movement_Server" };
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] { "Movement_Server" };
        }
        public override void inbox(ExtraMegaBlob.References.Event ev)
        {
        }
        public override void updateHook()
        {
        }
        public override void frameHook(float interpolation)
        {
        }
    }
}
