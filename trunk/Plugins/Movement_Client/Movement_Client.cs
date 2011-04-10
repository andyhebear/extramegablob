using MogreFramework;
using MOIS;
using ExtraMegaBlob.References;
using System;
namespace ExtraMegaBlob
{
    public class plugin : ExtraMegaBlob.References.ClientPlugin
    {
        public override void init()
        {
        }
        public override void shutdown()
        {
            log("shutting down!");
        }
        public override ExtraMegaBlob.References.Vector3 Location()
        {
            return new ExtraMegaBlob.References.Vector3(-454.8301f, 9.800894f, 322.5049f);
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
