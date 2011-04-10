using System;
using System.Collections;
using System.Windows.Forms;
using Mogre;
using MogreFramework;
using ExtraMegaBlob.References;
namespace ExtraMegaBlob
{
    public class plugin : ExtraMegaBlob.References.ClientPlugin
    {
        public override void init()
        {
            OgreWindow.Instance.onConsoleInput += new OgreWindow.consoleInputDelegate(Instance_onConsoleInput);
        }

        private void Instance_onConsoleInput(string text)
        {
            logConsole("> " + text);
            string[] x = text.Split(' ');

            if (text.Length > 1)
            {
                if (text.Substring(0, 2) == "go")
                {
                    cmd_go(text.Substring(2, text.Length - 2));
                }
            }
        }
        private void cmd_go(string s)
        {
            Event ev = new Event();
            ev._Memories = new Memories();
            ev._Memories.Add(new Memory("", KeyWord.DATA_STRING, s));
            ev._Keyword = KeyWord.CMD_GO;
            ev._IntendedRecipients = EventTransfer.CLIENTTOCLIENT;
            this.outboxMessage(this, ev);
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
