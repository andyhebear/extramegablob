using System;
using System.Collections;
using ExtraMegaBlob.References;
namespace ExtraMegaBlob.Server
{
    public class SecretServerPlugin : ExtraMegaBlob.References.ServerPlugin
    {
        public override string Name()
        {
            return "SecretServerPlugin";
        }
        public override string[] AllowedInputNames()
        {
            return new string[] { "SecretClientPlugin", "TongIts_Server", "ClientMain" };
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] { "SecretClientPlugin", "TongIts_Server" };
        }

        public override void inbox(ExtraMegaBlob.References.Event ev)
        {
            //ThingReferences.Globals.Instance.Data[];

            if (object.Equals(null, Globals.Instance.Users[ev._Endpoint]))
            {
                Globals.Instance.Users[ev._Endpoint] = new User();
                Globals.Instance.Users[ev._Endpoint].Name = "User";
            }
            switch (ev._Keyword)
            {
                case KeyWord.EVENT_CHATMESSAGE:
                    if (ev._Source_FullyQualifiedName == "ClientMain")
                    {
                        string senderName = Globals.Instance.Users[ev._Endpoint].Name;
                        string message = ev._Memories["text"].Value;
                        log(" >>>> " + senderName + " >>>> " + message);
                        if (message == "/name")
                        {
                            Globals.Instance.Users[ev._Endpoint].Name = ev._Endpoint;
                            break;
                        }
                        if (message.Length > 6)
                        {
                            if (message.Substring(0, 6) == "/name ")
                            {
                                string newname = message.Substring(6, message.Length - 6);
                                newname = newname.Trim();
                                if (newname != "")
                                {
                                    Globals.Instance.Users[ev._Endpoint].Name = newname;
                                    break;
                                }
                            }
                        }
                        Event outevent = new Event();
                        outevent._Keyword = KeyWord.EVENT_CHATMESSAGE;
                        outevent._IntendedRecipients = eventScope.CLIENTSPECIFY;
                        outevent._Memories = new Memories();
                        outevent._Memories.Add(new Memory("text", KeyWord.NIL, message, null));
                        outevent._Memories.Add(new Memory("username", KeyWord.NIL, senderName, null));
                        outboxMessage(this, outevent);
                    }
                    break;
                case KeyWord.CARTESIAN_SECRETPLAYERLOCATION:
                    Globals.Instance.Users[ev._Endpoint].Location.x = float.Parse(ev._Memories[KeyWord.CARTESIAN_X].Value);
                    Globals.Instance.Users[ev._Endpoint].Location.y = float.Parse(ev._Memories[KeyWord.CARTESIAN_Y].Value);
                    Globals.Instance.Users[ev._Endpoint].Location.z = float.Parse(ev._Memories[KeyWord.CARTESIAN_Z].Value);
                    break;
                default:
                   // log(ev._Keyword + " from " + ev._Source_FullyQualifiedName);
                    break;
            }
        }

        public override void updateHook()
        {
            if (!running) return;
            if (t.elapsed)
            {
                t.reset();
                t.start();
                //Event ev = new Event();
                //ev._Keyword = KeyWord.TESTLOOP;
                //ev._IntendedRecipients = eventScope.CLIENTSPECIFY;
                ////ev._Memories = new Memories();
                ////ev._Memories.Add(new Memory("", KeyWord.TESTLOOP, ""));
                //outboxMessage(this, ev);
            }
        }
        public override void shutdown()
        {
            running = false;
        }

        ExtraMegaBlob.References.timer t = new ExtraMegaBlob.References.timer(new TimeSpan(0, 0, 1));
        private bool running = true;
    }
}
