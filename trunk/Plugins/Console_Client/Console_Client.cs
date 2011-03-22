using System;
using System.Collections;
using System.Windows.Forms;
using Mogre;
using MogreFramework;
namespace ExtraMegaBlob
{
    public class plugin : ExtraMegaBlob.References.ClientPlugin
    {
        public override void startup()
        {
            log("starting up");
            OgreWindow.Instance.onConsoleInput += new OgreWindow.consoleInputDelegate(Instance_onConsoleInput);
            log("done starting up");
        }

        private void Instance_onConsoleInput(string text)
        {
            logConsole("> " + text);
            string[] x = text.Split(' ');

            if (text.Length > 1)
            {
                if (text.Substring(0, 2) == "go")
                {
                    //do something
                }
            }
        }
        public override void shutdown()
        {
            log("shutting down");
        }
        public override ExtraMegaBlob.References.Vector3 Location()
        {
            return new ExtraMegaBlob.References.Vector3(0, 0, 0);
        }
        public override string Name()
        {
            return "Console_Client";
        }
        public override string[] AllowedInputNames()
        {
            return new string[] { };
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] { };
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
