using System;
using System.Collections.Generic;
using System.Text;
using ExtraMegaBlob.References;
using System.Collections;
namespace ExtraMegaBlob
{
    public class plugin : ExtraMegaBlob.References.ServerPlugin
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
        public override void inbox(ExtraMegaBlob.References.Event ev)
        {
            //ThingReferences.Globals.Instance.Data[];

            switch (ev._Keyword)
            {
                case KeyWord.CARTESIAN_SECRETPLAYERLOCATION:


                    References.Vector3 loc = new Vector3(float.Parse(ev._Memories[KeyWord.CARTESIAN_X].Value), float.Parse(ev._Memories[KeyWord.CARTESIAN_Y].Value), float.Parse(ev._Memories[KeyWord.CARTESIAN_Z].Value));
                    if (!globals.Users.Contains(ev._Endpoint))
                    {
                        globals.Users.Add(new User("", loc, ev._Endpoint));
                        log("adding: " + ev._Endpoint);
                    }
                    globals.Users[ev._Endpoint].Location = loc;
                    broadcastPlayerLocationAllExcept(ev._Endpoint, loc);
                    if (!globals.Users[ev._Endpoint].inRange)
                    {
                        float dist = References.Math.distanceBetweenPythagCartesian(loc, tableLocation);
                        log(dist.ToString());
                        if (dist < 30f)
                        {
                            globals.Users[ev._Endpoint].inRange = true;
                            if (openSlots)
                                invite(ev._Endpoint);
                        }
                    }
                    break;
                case KeyWord.TONGITS_PLAYER_INVITE_ACCEPTED:
                    if (numPlayers < 3)
                    {
                        numPlayers++;
                        sendPlayerNumber(ev._Endpoint, numPlayers);
                        if (numPlayers == 3)
                            start();
                    }
                    break;
                default:
                    //  log(ev._Keyword + " from " + ev._Source_FullyQualifiedName);
                    break;
            }
        }
        private void start()
        {
            if (numPlayers != 3) return;
            log("game starting!");
        }
        private void sendPlayerNumber(string Endpoint, int playerNumber)
        {
            Event outevent = new Event();
            outevent._Keyword = KeyWord.TONGITS_PLAYER_NUMBER;
            outevent._IntendedRecipients = EventTransfer.SERVERTOCLIENT;
            outevent._Endpoint = Endpoint;
            outevent._Memories = new Memories();
            outevent._Memories.Add(new Memory("", KeyWord.TONGITS_PLAYER_NUMBER, (playerNumber.ToString())));
            outboxMessage(this, outevent); 
        }
        private void invite(string Endpoint)
        {
            Event outevent = new Event();
            outevent._Keyword = KeyWord.TONGITS_PLAYER_INVITE;
            outevent._IntendedRecipients = EventTransfer.SERVERTOCLIENT;
            outevent._Endpoint = Endpoint;
            outboxMessage(this, outevent);
        }
        bool openSlots = true;
        int numPlayers = 0;
        private void broadcastPlayerLocationAllExcept(string Endpoint, References.Vector3 loc)
        {
            Event outevent = new Event();
            outevent._Keyword = KeyWord.TONGITS_PLAYERLOCATION_UPDATE;
            outevent._IntendedRecipients = EventTransfer.SERVERTOCLIENT;
            outevent._Memories = new Memories();
            outevent._Memories.Add(new Memory("", KeyWord.CARTESIAN_X, (loc.x.ToString())));
            outevent._Memories.Add(new Memory("", KeyWord.CARTESIAN_Y, (loc.y.ToString())));
            outevent._Memories.Add(new Memory("", KeyWord.CARTESIAN_Z, (loc.z.ToString())));
            sendMessageAllUsersExcept(outevent, Endpoint);
        }
        Globals globals = new Globals();
        private References.Vector3 tableLocation = new References.Vector3(0f, 36f, 0f);
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
        private void sendMessageAllUsersExcept(Event msg, string endpoint)
        {

            foreach (User u in globals.Users)
            {
                if (u.Endpoint != endpoint)
                {
                    if (msg._Endpoint.Length > 0)
                        msg._Endpoint += ",";
                    msg._Endpoint += u.Endpoint;
                }
            }
            if (msg._Endpoint != "")
                outboxMessage(this, msg);
        }
        private void sendMessageAllUsers(Event msg)
        {
            foreach (User u in globals.Users)
            {
                if (msg._Endpoint.Length > 0)
                    msg._Endpoint += ",";
                msg._Endpoint += u.Endpoint;
            }
            if (msg._Endpoint != "")
                outboxMessage(this, msg);
        }
        ExtraMegaBlob.References.timer t = new ExtraMegaBlob.References.timer(new TimeSpan(0, 0, 1));
        private bool running = true;

    }
}
