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
                    float dist = References.Math.distanceBetweenPythagCartesian(loc, tableLocation);
                    log(dist.ToString());
                    if (dist < 30f && !globals.Users[ev._Endpoint].inRange)
                    {
                        globals.Users[ev._Endpoint].inRange = true;
                        invite(ev._Endpoint);
                    }
                    if (dist > 30f && globals.Users[ev._Endpoint].inRange)
                    {
                        globals.Users[ev._Endpoint].inRange = false;
                    }
                    break;
                case KeyWord.TONGITS_PLAYER_INVITE_ACCEPTED:
                    if (numPlayers < NUMPLRS)
                    {
                        numPlayers++;
                        sendPlayerNumber(ev._Endpoint, numPlayers);
                        if (numPlayers == NUMPLRS)
                            startNewGame();
                    }
                    break;
                default:
                    //  log(ev._Keyword + " from " + ev._Source_FullyQualifiedName);
                    break;
            }
        }
        private void startNewGame()
        {
            openSlots = false;
            if (numPlayers != NUMPLRS) return;
            log("game starting!");
            initializeCards();
            randomizeCards(10);
            sendGameStartingSignal();
            sendDeckToTable(random);
        }
        private const int NUMPLRS = 1;

        private void sendDeckToTable(List<Card> deck)
        {
            List<Card>.Enumerator cardDealer = random.GetEnumerator();
            while (cardDealer.MoveNext())
            {
                for (int player = 0; player < NUMPLRS; player++)
                {
                    sendCardToTable(cardDealer.Current, player + 1);
                }
                //cardDealer.Current
            }
        }

        private void sendGameStartingSignal()
        {
            Event outevent = new Event();
            outevent._Keyword = KeyWord.TONGITS_GAME_STARTING;
            outevent._IntendedRecipients = EventTransfer.SERVERTOCLIENT;
            sendMessageAllUsers(outevent);
        }
        private void sendCardToTable(Card card, int player)
        {
            Event outevent = new Event();
            outevent._Keyword = KeyWord.TONGITS_CARD_DECK_PLACE;
            outevent._IntendedRecipients = EventTransfer.SERVERTOCLIENT;
            outevent._Endpoint = (string)playerLkupSession[player];

            outevent._Memories = new Memories();
            outevent._Memories.Add(new Memory("", KeyWord.TONGITS_CARD_DATA, card.Name));
            outevent._Memories.Add(new Memory("", KeyWord.TONGITS_PLAYER_NUMBER, player.ToString()));
            //outboxMessage(this, outevent);
            sendMessageAllUsers(outevent);

        }
        private Random ran = new Random((int)DateTime.Now.Ticks);
        private void randomizeCards(int numRounds)
        {
            random = newDeck();

            for (int g = 0; g < numRounds; g++)
            {
                for (int i = 0; i < 52; i++)
                {
                    int k = ran.Next(51);
                    Card temp = random[i];
                    random[i] = random[k];
                    random[k] = temp;
                }
            }
        }
        private void initializeCards()
        {
            deck = newDeck();
        }
        private List<Card> newDeck()
        {
            #region freshDeck
            List<Card> freshDeckNormal = new List<Card>(52);
            freshDeckNormal.Add(new Card("club 2", 0));
            freshDeckNormal.Add(new Card("club 3", 1));
            freshDeckNormal.Add(new Card("club 4", 2));
            freshDeckNormal.Add(new Card("club 5", 3));
            freshDeckNormal.Add(new Card("club 6", 4));
            freshDeckNormal.Add(new Card("club 7", 5));
            freshDeckNormal.Add(new Card("club 8", 6));
            freshDeckNormal.Add(new Card("club 9", 7));
            freshDeckNormal.Add(new Card("club 10", 8));
            freshDeckNormal.Add(new Card("club ace", 9));
            freshDeckNormal.Add(new Card("club jack", 10));
            freshDeckNormal.Add(new Card("club king", 11));
            freshDeckNormal.Add(new Card("club queen", 12));
            freshDeckNormal.Add(new Card("diamond 2", 13));
            freshDeckNormal.Add(new Card("diamond 3", 14));
            freshDeckNormal.Add(new Card("diamond 4", 15));
            freshDeckNormal.Add(new Card("diamond 5", 16));
            freshDeckNormal.Add(new Card("diamond 6", 17));
            freshDeckNormal.Add(new Card("diamond 7", 18));
            freshDeckNormal.Add(new Card("diamond 8", 19));
            freshDeckNormal.Add(new Card("diamond 9", 20));
            freshDeckNormal.Add(new Card("diamond 10", 21));
            freshDeckNormal.Add(new Card("diamond ace", 22));
            freshDeckNormal.Add(new Card("diamond jack", 23));
            freshDeckNormal.Add(new Card("diamond king", 24));
            freshDeckNormal.Add(new Card("diamond queen", 25));
            freshDeckNormal.Add(new Card("heart 2", 26));
            freshDeckNormal.Add(new Card("heart 3", 27));
            freshDeckNormal.Add(new Card("heart 4", 28));
            freshDeckNormal.Add(new Card("heart 5", 29));
            freshDeckNormal.Add(new Card("heart 6", 30));
            freshDeckNormal.Add(new Card("heart 7", 31));
            freshDeckNormal.Add(new Card("heart 8", 32));
            freshDeckNormal.Add(new Card("heart 9", 33));
            freshDeckNormal.Add(new Card("heart 10", 34));
            freshDeckNormal.Add(new Card("heart ace", 35));
            freshDeckNormal.Add(new Card("heart jack", 36));
            freshDeckNormal.Add(new Card("heart king", 37));
            freshDeckNormal.Add(new Card("heart queen", 38));
            freshDeckNormal.Add(new Card("spade 2", 39));
            freshDeckNormal.Add(new Card("spade 3", 40));
            freshDeckNormal.Add(new Card("spade 4", 41));
            freshDeckNormal.Add(new Card("spade 5", 42));
            freshDeckNormal.Add(new Card("spade 6", 43));
            freshDeckNormal.Add(new Card("spade 7", 44));
            freshDeckNormal.Add(new Card("spade 8", 45));
            freshDeckNormal.Add(new Card("spade 9", 46));
            freshDeckNormal.Add(new Card("spade 10", 47));
            freshDeckNormal.Add(new Card("spade ace", 48));
            freshDeckNormal.Add(new Card("spade jack", 49));
            freshDeckNormal.Add(new Card("spade king", 50));
            freshDeckNormal.Add(new Card("spade queen", 51));
            #endregion
            return freshDeckNormal;
        }
        List<Card> random;
        List<Card> deck;
        List<Card> playerHand1;
        List<Card> playerHand2;
        List<Card> playerHand3;
        private class Card
        {
            public Card(string Name, int index)
            {
                this.Name = Name;
                this.index = index;
            }
            public string Name = "";
            public int index;
        }

        Hashtable playerLkupSession = new Hashtable();
        private void sendPlayerNumber(string Endpoint, int playerNumber)
        {
            if (playerNumber == 1)
            {
                playerLkupSession = new Hashtable();
            }
            playerLkupSession.Add(playerNumber, Endpoint);
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
