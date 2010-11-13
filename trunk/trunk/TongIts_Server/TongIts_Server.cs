using System;
using System.Collections.Generic;
using System.Text;
using ThingReferences;
using System.Collections;
namespace thing
{
    public class plugin : ThingReferences.ServerPlugin
    {
        public override string Name()
        {
            return "TongIts_Server";
        }
        public override string[] AllowedInputNames()
        {
            return new string[] { "TongIts_Client", "SecretClientPlugin" };
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] { "TongIts_Client", "SecretServerPlugin" };
        }
        public override void inbox(ThingReferences.Event ev)
        {
            //ThingReferences.Globals.Instance.Data[];

            switch (ev._Keyword)
            {
                case KeyWord.CARTESIAN_SECRETPLAYERLOCATION:

                    //Event outevent = new Event();
                    //outevent._Keyword = KeyWord.TONGITS_PLAYERLOCATION_UPDATE;
                    //outevent._IntendedRecipients = eventScope.CLIENTSPECIFY;
                    //outevent._Memories = new Memories();
                    //outevent._Memories.Add(new Memory("", KeyWord.CARTESIAN_X, ((player)players[ev._Endpoint]).location.x.ToString()));
                    //outevent._Memories.Add(new Memory("", KeyWord.CARTESIAN_Y, ((player)players[ev._Endpoint]).location.y.ToString()));
                    //outevent._Memories.Add(new Memory("", KeyWord.CARTESIAN_Z, ((player)players[ev._Endpoint]).location.z.ToString()));
                    //sendMessageAllPlayersExcept(outevent, ev._Endpoint);
                    break;
                default:
                  //  log(ev._Keyword + " from " + ev._Source_FullyQualifiedName);
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
                //// log("tick");
                //Event ev = new Event();
                //ev._Keyword = KeyWord.TESTLOOP;
                //ev._IntendedRecipients = eventScope.CLIENTSPECIFY;
                //////ev._Memories = new Memories();
                //////ev._Memories.Add(new Memory("", KeyWord.TESTLOOP, ""));
                //outboxMessage(this, ev);
            }
        }
        public override void shutdown()
        {
            running = false;

            log("shutting down");
        }
        //private void sendMessageAllPlayersExcept(Event msg, string endpoint)
        //{
        //    foreach (DictionaryEntry de in players)
        //    {
        //        if ((string)de.Key != endpoint)
        //        {
        //            if (msg._Endpoint.Length > 0)
        //                msg._Endpoint += ",";
        //            msg._Endpoint += (string)de.Key;
        //        }
        //    }
        //    if (msg._Endpoint != "")
        //        outboxMessage(this, msg);
        //}
        //private void sendMessageAllPlayers(Event msg)
        //{
        //    foreach (DictionaryEntry de in players)
        //    {
        //        if (msg._Endpoint.Length > 0)
        //            msg._Endpoint += ",";
        //        msg._Endpoint += (string)de.Key;
        //    }
        //    if (msg._Endpoint != "")
        //        outboxMessage(this, msg);
        //}
        ThingReferences.timer t = new ThingReferences.timer(new TimeSpan(0, 0, 1));
        private bool running = true;

    }
}
