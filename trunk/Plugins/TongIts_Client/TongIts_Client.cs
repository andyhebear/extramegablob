using System;
using System.Collections.Generic;
using System.Text;
using MogreFramework;
using ExtraMegaBlob.References;
using Mogre;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Threading;
using Mogre.PhysX;
using MOIS;
namespace ExtraMegaBlob
{
    public class plugin : ExtraMegaBlob.References.ClientPlugin
    {

        private string[] suits = { "club", "heart", "spade", "diamond" };
        private string[] faces = { "jack", "king", "queen", "ace" };
        private Hashtable meshes_lookup
        {
            get
            {
                Hashtable h = new Hashtable();
                #region meshes
                h["mushroom"] = "\\TongIts\\mushroom.mesh";
                h["table"] = "\\TongIts\\tongitstable.mesh";
                h["card"] = "\\TongIts\\Card.mesh";
                #endregion
                return h;
            }
        }
        private Hashtable skeletons_lookup
        {
            get
            {
                Hashtable h = new Hashtable();
                #region meshes
                #endregion
                return h;
            }
        }
        private Hashtable materials_lookup
        {
            get
            {
                Hashtable h = new Hashtable();
                #region materials
                h["baseball"] = "\\baseball-40.gif";
                h["mushroom"] = "\\TongIts\\Shiitake Mushroom Tree Shii.png";
                h["webcam"] = "webcapCapture";
                h["wood"] = "\\TongIts\\woodblurred.jpg";
                h["Material/TEXFACE/woodblurred.jpg"] = "\\TongIts\\woodblurred.jpg";
                h["Material/TEXFACE/BumpyMetal.jpg"] = "\\TongIts\\BumpyMetal.jpg";
                h["clouds"] = "\\clouds.jpg";
                // h["dirt"] = "\\terr_dirt-grass.jpg";
                h["noise"] = "\\normalNoiseColor.png";
                h["blankcardface"] = "\\TongIts\\cardface-blank.png";
                h["cat1"] = "\\cat2.jpg";
                h["club 10"] = "\\TongIts\\200px-Playing_card_club_10.svg.png";
                h["club 2"] = "\\TongIts\\200px-Playing_card_club_2.svg.png";
                h["club 3"] = "\\TongIts\\200px-Playing_card_club_3.svg.png";
                h["club 4"] = "\\TongIts\\200px-Playing_card_club_4.svg.png";
                h["club 5"] = "\\TongIts\\200px-Playing_card_club_5.svg.png";
                h["club 6"] = "\\TongIts\\200px-Playing_card_club_6.svg.png";
                h["club 7"] = "\\TongIts\\200px-Playing_card_club_7.svg.png";
                h["club 8"] = "\\TongIts\\200px-Playing_card_club_8.svg.png";
                h["club 9"] = "\\TongIts\\200px-Playing_card_club_9.svg.png";
                h["club ace"] = "\\TongIts\\200px-Playing_card_club_A.svg.png";
                h["club jack"] = "\\TongIts\\200px-Playing_card_club_J.svg.png";
                h["club king"] = "\\TongIts\\200px-Playing_card_club_K.svg.png";
                h["club queen"] = "\\TongIts\\200px-Playing_card_club_Q.svg.png";
                h["diamond 10"] = "\\TongIts\\200px-Playing_card_diamond_10.svg.png";
                h["diamond 2"] = "\\TongIts\\200px-Playing_card_diamond_2.svg.png";
                h["diamond 3"] = "\\TongIts\\200px-Playing_card_diamond_3.svg.png";
                h["diamond 4"] = "\\TongIts\\200px-Playing_card_diamond_4.svg.png";
                h["diamond 5"] = "\\TongIts\\200px-Playing_card_diamond_5.svg.png";
                h["diamond 6"] = "\\TongIts\\200px-Playing_card_diamond_6.svg.png";
                h["diamond 7"] = "\\TongIts\\200px-Playing_card_diamond_7.svg.png";
                h["diamond 8"] = "\\TongIts\\200px-Playing_card_diamond_8.svg.png";
                h["diamond 9"] = "\\TongIts\\200px-Playing_card_diamond_9.svg.png";
                h["diamond ace"] = "\\TongIts\\200px-Playing_card_diamond_A.svg.png";
                h["diamond jack"] = "\\TongIts\\200px-Playing_card_diamond_J.svg.png";
                h["diamond king"] = "\\TongIts\\200px-Playing_card_diamond_K.svg.png";
                h["diamond queen"] = "\\TongIts\\200px-Playing_card_diamond_Q.svg.png";
                h["heart 10"] = "\\TongIts\\200px-Playing_card_heart_10.svg.png";
                h["heart 2"] = "\\TongIts\\200px-Playing_card_heart_2.svg.png";
                h["heart 3"] = "\\TongIts\\200px-Playing_card_heart_3.svg.png";
                h["heart 4"] = "\\TongIts\\200px-Playing_card_heart_4.svg.png";
                h["heart 5"] = "\\TongIts\\200px-Playing_card_heart_5.svg.png";
                h["heart 6"] = "\\TongIts\\200px-Playing_card_heart_6.svg.png";
                h["heart 7"] = "\\TongIts\\200px-Playing_card_heart_7.svg.png";
                h["heart 8"] = "\\TongIts\\200px-Playing_card_heart_8.svg.png";
                h["heart 9"] = "\\TongIts\\200px-Playing_card_heart_9.svg.png";
                h["heart ace"] = "\\TongIts\\200px-Playing_card_heart_A.svg.png";
                h["heart jack"] = "\\TongIts\\200px-Playing_card_heart_J.svg.png";
                h["heart king"] = "\\TongIts\\200px-Playing_card_heart_K.svg.png";
                h["heart queen"] = "\\TongIts\\200px-Playing_card_heart_Q.svg.png";
                h["spade 10"] = "\\TongIts\\200px-Playing_card_spade_10.svg.png";
                h["spade 2"] = "\\TongIts\\200px-Playing_card_spade_2.svg.png";
                h["spade 3"] = "\\TongIts\\200px-Playing_card_spade_3.svg.png";
                h["spade 4"] = "\\TongIts\\200px-Playing_card_spade_4.svg.png";
                h["spade 5"] = "\\TongIts\\200px-Playing_card_spade_5.svg.png";
                h["spade 6"] = "\\TongIts\\200px-Playing_card_spade_6.svg.png";
                h["spade 7"] = "\\TongIts\\200px-Playing_card_spade_7.svg.png";
                h["spade 8"] = "\\TongIts\\200px-Playing_card_spade_8.svg.png";
                h["spade 9"] = "\\TongIts\\200px-Playing_card_spade_9.svg.png";
                h["spade ace"] = "\\TongIts\\200px-Playing_card_spade_A.svg.png";
                h["spade jack"] = "\\TongIts\\200px-Playing_card_spade_J.svg.png";
                h["spade king"] = "\\TongIts\\200px-Playing_card_spade_K.svg.png";
                h["spade queen"] = "\\TongIts\\200px-Playing_card_spade_Q.svg.png";
                #endregion
                return h;
            }
        }
        private List<Quaternion> seatPositions
        {
            get
            {
                List<Quaternion> playerSeatOrientation = new List<Quaternion>();
                playerSeatOrientation.Add(new Quaternion(0.2246418f, 0f, 0.9744414f, 0f));
                playerSeatOrientation.Add(new Quaternion(0.9731094f, 0f, 0.2303433f, 0f));
                playerSeatOrientation.Add(new Quaternion(0.6975411f, 0f, 0.7165448f, 0f));
                return playerSeatOrientation;
            }
        }
        private List<Mogre.Vector3> cardPrimaryLocations
        {
            get
            {
                List<Mogre.Vector3> playerSeatLocation = new List<Mogre.Vector3>();
                playerSeatLocation.Add(Location().toMogre + new Mogre.Vector3(0f, 0f, 0f));//dealer's cards
                playerSeatLocation.Add(Location().toMogre + new Mogre.Vector3(0f, -10f, 0f));//player 1's cards
                return playerSeatLocation;
            }
        }
        private List<Mogre.Vector3> seatLocations
        {
            get
            {
                List<Mogre.Vector3> playerSeatLocation = new List<Mogre.Vector3>();
                playerSeatLocation.Add(Location().toMogre + new Mogre.Vector3(0f, 0f, 0f));
                playerSeatLocation.Add(Location().toMogre + new Mogre.Vector3(-1.291305f, 35.18927f, 2.11423f));
                playerSeatLocation.Add(Location().toMogre + new Mogre.Vector3(-0.03845761f, 35.18927f, -3.819804f));
                playerSeatLocation.Add(Location().toMogre + new Mogre.Vector3(4.63741f, 35.18927f, -0.9184573f));
                return playerSeatLocation;
            }
        }
        private void resourceWaitThread()
        {
            while (true)
            {
                Thread.Sleep(1000);
                foreach (DictionaryEntry de in materials_lookup)
                {
                    if (!TextureManager.Singleton.ResourceExists((string)de.Value)) goto waitmore;
                }
                foreach (DictionaryEntry de in skeletons_lookup)
                {
                    if (!SkeletonManager.Singleton.ResourceExists((string)de.Value)) goto waitmore;
                }
                foreach (DictionaryEntry de in meshes_lookup)
                {
                    if (!MeshManager.Singleton.ResourceExists((string)de.Value)) goto waitmore;
                }
                break;
            waitmore:
                continue;
            }
            init();
        }
        private void init()
        {
            log("starting up! ");
            OgreWindow.Instance.pause();
            try
            {
                Hashtable mats = materials_lookup;
                foreach (DictionaryEntry mat in mats)
                {
                    materials.Add((MaterialPtr)MaterialManager.Singleton.Create((string)mat.Key, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME));
                    materials[(string)mat.Key].GetTechnique(0).GetPass(0).CreateTextureUnitState((string)mat.Value);
                }
                lights.Add(OgreWindow.Instance.mSceneMgr.CreateLight("testLight"));
                lights["testLight"].Type = Light.LightTypes.LT_POINT;
                lights["testLight"].Position = new Mogre.Vector3(-117.9847f, 120f, 234.2695f) + Location().toMogre;
                lights["testLight"].DiffuseColour = ColourValue.White;
                lights["testLight"].SpecularColour = ColourValue.White;

                lights.Add(OgreWindow.Instance.mSceneMgr.CreateLight("testLight2"));
                lights["testLight2"].Type = Light.LightTypes.LT_POINT;
                lights["testLight2"].Position = new Mogre.Vector3(1.408661f, 54.81305f, -3.154539f) + Location().toMogre;
                lights["testLight2"].DiffuseColour = ColourValue.White;
                lights["testLight2"].SpecularColour = ColourValue.White;

                //MeshManager.Singleton.CreatePlane("ground",
                //    ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                //    new Plane(Mogre.Vector3.UNIT_Y, 0),
                //    1500, 1500, 20, 20, true, 1, 5, 5, Mogre.Vector3.UNIT_Z);
                //// Create a ground plane
                //entities.Add(OgreWindow.Instance.mSceneMgr.CreateEntity("GroundEntity", "ground"));
                //entities["GroundEntity"].CastShadows = false;
                //entities["GroundEntity"].SetMaterialName("dirt");
                //nodes.Add(OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode("ground"));
                //nodes["ground"].AttachObject(entities["GroundEntity"]);
                //nodes["ground"].Position = new Mogre.Vector3(0f, 0f, 0f) + Location().toMogre;

                //our "ground" is a mushroom :)
                entities.Add(OgreWindow.Instance.mSceneMgr.CreateEntity("mushroom", "\\TongIts\\mushroom.mesh"));
                entities["mushroom"].SetMaterialName("mushroom");
                nodes.Add(OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode("mushroom"));
                nodes["mushroom"].AttachObject(entities["mushroom"]);
                nodes["mushroom"].Position = new Mogre.Vector3(0f, -20f, 0f) + Location().toMogre;
                nodes["mushroom"].Scale(new Mogre.Vector3(100f));
                nodes["mushroom"].Roll(new Radian(new Degree(90f)));
                preventMousePick("mushroom");


                #region physics

                // physics
                // attaching a body to the actor makes it dynamic, you can set things like initial velocity
                BodyDesc bodyDesc = new BodyDesc();
                bodyDesc.LinearVelocity = new Mogre.Vector3(0, 2, 5);

                // the actor properties control the mass, position and orientation
                // if you leave the body set to null it will become a static actor and wont move
                ActorDesc actorDesc = new ActorDesc();
                actorDesc.Density = 4;
                actorDesc.Body = null;
                actorDesc.GlobalPosition = nodes["mushroom"].Position;
                actorDesc.GlobalOrientation = nodes["mushroom"].Orientation.ToRotationMatrix();

                // a quick trick the get the size of the physics shape right is to use the bounding box of the entity
                //actorDesc.Shapes.Add(new SphereShapeDesc(1f));//entities["drone"].BoundingBox.HalfSize * scale, entities["drone"].BoundingBox.Center * scale



                PhysXHelpers.StaticMeshData meshdata = new PhysXHelpers.StaticMeshData(entities["mushroom"].GetMesh());
                actorDesc.Shapes.Add(PhysXHelpers.CreateTriangleMesh(meshdata));

                Actor actor = null;
                // finally, create the actor in the physics scene
                try { actor = OgreWindow.Instance.scene.CreateActor(actorDesc); }
                catch { }
                // if (actor == null)
                //    OgreWindow.Instance.CloseForm();
                if (actor != null)
                {
                    // create our special actor node to tie together the scene node and actor that we can update its position later
                    ActorNode actorNode = new ActorNode(nodes["mushroom"], actor);
                    actors.Add(actorNode);
                }
                #endregion



                #region table
                entities.Add(OgreWindow.Instance.mSceneMgr.CreateEntity("tongitstable", "\\TongIts\\tongitstable.mesh"));
                nodes.Add(OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode("table"));
                nodes["table"].AttachObject(entities["tongitstable"]);
                nodes["table"].Position = new Mogre.Vector3(0f, 36f, 0f) + Location().toMogre;
                //nodes["table"].Scale(new Mogre.Vector3(4f));
                //preventMousePick("tongitstable");
                // physics
                // attaching a body to the actor makes it dynamic, you can set things like initial velocity
                bodyDesc.LinearVelocity = new Mogre.Vector3(0, 0, 0);

                // the actor properties control the mass, position and orientation
                // if you leave the body set to null it will become a static actor and wont move
                ActorDesc tableActorDesc = new ActorDesc();
                tableActorDesc.Density = 4;
                tableActorDesc.Body = null;
                tableActorDesc.GlobalPosition = nodes["table"].Position;
                tableActorDesc.GlobalOrientation = nodes["table"].Orientation.ToRotationMatrix();
                PhysXHelpers.StaticMeshData tableMeshData = new PhysXHelpers.StaticMeshData(entities["tongitstable"].GetMesh());
                tableActorDesc.Shapes.Add(PhysXHelpers.CreateTriangleMesh(tableMeshData));

                Actor tableActor = null;
                // finally, create the actor in the physics scene
                try { tableActor = OgreWindow.Instance.scene.CreateActor(tableActorDesc); }
                catch { }
                if (tableActor != null)
                {
                    actors.Add(new ActorNode(nodes["table"], tableActor));
                }
                #endregion

                #region baseball
                entities.Add(OgreWindow.Instance.mSceneMgr.CreateEntity("baseball", "\\baseball.mesh"));
                entities["baseball"].SetMaterialName("baseball");
                //nodes.Add(OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode("baseball"));
                nodes.Add(nodes["table"].CreateChildSceneNode("baseball"));
                nodes["baseball"].AttachObject(entities["baseball"]);
                //nodes["baseball"].SetScale(.5f, .5f, .5f);
                nodes["baseball"].SetPosition(Location().x, Location().y, Location().z);
                // nodes["baseball"].SetScale(5f, 5f, 5f);
                #endregion

                #region cards

                Thread.Sleep(5000);
                
                //only 1 entity needed
                entities.Add(OgreWindow.Instance.mSceneMgr.CreateEntity("card", "\\TongIts\\Card.mesh"));

                foreach (string suit in suits)
                {
                    for (int i = 2; i < 11; i++)
                    {
                        string cardname = string.Format("{0} {1}", suit, i);
                        createCard(cardname);
                        placeCard(0, cardname);
                    }
                    foreach (string face in faces)
                    {
                        string cardname = string.Format("{0} {1}", suit, face);
                        createCard(cardname);
                        placeCard(0, cardname);
                    }
                }

                //MeshManager.Singleton.CreatePlane("spinnycard", "General", new Plane(Mogre.Vector3.UNIT_Y, 0), 200f, 250f, 1, 1, true, 1, 1, 1, Mogre.Vector3.UNIT_X);

                //// physics
                //// attaching a body to the actor makes it dynamic, you can set things like initial velocity
                //BodyDesc cardBodyDesc = null;
                //bodyDesc.LinearVelocity = new Mogre.Vector3(0, 6, 0);

                //// the actor properties control the mass, position and orientation
                //// if you leave the body set to null it will become a static actor and wont move
                //ActorDesc cardActorDesc = new ActorDesc();
                //cardActorDesc.Density = 4;
                //cardActorDesc.Body = cardBodyDesc;
                //cardActorDesc.GlobalPosition = nodes["club 5"].Position;
                //cardActorDesc.GlobalOrientation = nodes["club 5"].Orientation.ToRotationMatrix();
                //PhysXHelpers.StaticMeshData cardMeshData = new PhysXHelpers.StaticMeshData(entities["club 5"].GetMesh());
                //cardActorDesc.Shapes.Add(PhysXHelpers.CreateConvexHull(cardMeshData));

                //Actor cardActor = null;
                //// finally, create the actor in the physics scene
                //try { cardActor = OgreWindow.Instance.scene.CreateActor(cardActorDesc); }
                //catch { }
                //// if (actor == null)
                ////    OgreWindow.Instance.CloseForm();
                //if (cardActor != null)
                //{
                //    actors.Add(new ActorNode(nodes["club 5"], cardActor));
                //}

                #endregion
                resetPlayer(1);
                this.btnLimiter_F.reset();
                this.btnLimiter_F.start();
                ready = true;
            }

            catch (Exception ex)
            {
                log(ex.ToString());
            }
            OgreWindow.Instance.unpause();
        }
        private void createCard(string cardName)
        {
            //entities.Add(OgreWindow.Instance.mSceneMgr.CreateEntity(cardName, "\\TongIts\\Card.mesh"));
            //entities[cardName].SetMaterialName(cardName);
            nodes.Add(nodes["table"].CreateChildSceneNode(cardName));
            nodes[cardName].AttachObject(entities["card"]);
            nodes[cardName].Position = Location().toMogre + (new Mogre.Vector3(0f, 15f, 0f)); //this line is wrong
        }


        public override void startup()
        {
            new Thread(new ThreadStart(resourceWaitThread)).Start();
        }
        public override void shutdown()
        {
            ready = false;
            log("shutting down!");
            nodes.shutdown();
            entities.shutdown();
            lights.shutdown();
            MeshManager.Singleton.Remove("ground");
        }
        public override ExtraMegaBlob.References.Vector3 Location()
        {
            return new ExtraMegaBlob.References.Vector3(0f, -168.6846f, -1101.067f);
            //return new ExtraMegaBlob.References.Vector3(0f, 0f, 0f);
        }
        public override string Name()
        {
            return "TongIts_Client";
        }
        public override string[] AllowedInputNames()
        {
            return new string[] { "TongIts_Server" };
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] { "TongIts_Server" };
        }
        public override void inbox(ExtraMegaBlob.References.Event ev)
        {
            //MogreFramework.Globals.Instance.Data[];
            switch (ev._Keyword)
            {
                case KeyWord.TONGITS_GAME_STARTING:
                    resetPlayer(this.playerNumber);
                    //freezePlayer();
                    chat("game is starting");
                    break;
                case KeyWord.TONGITS_PLAYER_INVITE:
                    freezePlayer();
                    if (OgreWindow.Instance.AskQuestionBool("Want to join a game of TongIts?"))
                    {
                        acceptNewGame();
                    }
                    unfreezePlayer();
                    break;
                case KeyWord.TONGITS_CARD_DECK_PLACE:
                    int playerNumber = int.Parse(ev._Memories[KeyWord.TONGITS_PLAYER_NUMBER].Value);
                    string cardName = ev._Memories[KeyWord.TONGITS_CARD_DATA].Value;
                    //chat(string.Format("Placing a Deck Card Player:{0} Card:{1}", ev._Memories[KeyWord.TONGITS_PLAYER_NUMBER].Value, ev._Memories[KeyWord.TONGITS_CARD_DATA].Value));
                    placeCard(playerNumber, cardName);
                    break;

                case KeyWord.TONGITS_PLAYER_NUMBER:
                    playerNumber = int.Parse(ev._Memories[KeyWord.TONGITS_PLAYER_NUMBER].Value);
                    break;

                default:
                    //log(ev._Keyword + " from " + ev._Source_FullyQualifiedName);
                    //log(ev._WhenRcvd.ToString());
                    break;
            }
        }
        private void placeCard(int playerNumber, string cardName)
        {
            nodes[cardName].Translate(cardPrimaryLocations[playerNumber].x,
                       cardPrimaryLocations[playerNumber].y,
                       cardPrimaryLocations[playerNumber].z);


        }

        public void acceptNewGame()
        {
            Event outevent = new Event();
            outevent._Keyword = KeyWord.TONGITS_PLAYER_INVITE_ACCEPTED;
            outevent._IntendedRecipients = EventTransfer.CLIENTTOSERVER;
            outboxMessage(this, outevent);
        }
        private int playerNumber = 0;
        public override void updateHook()
        {
            if (ready)
            {
                nodes["baseball"].Yaw(new Radian(new Degree(1f)));
                actors.UpdateAllActors(.1f);

                if (btnLimiter_F.elapsed)
                {
                    if (keyboard.IsKeyDown(KeyCode.KC_F))
                    {
                        resetPlayer(1);
                    }
                    btnLimiter_F.start();
                } 
                
            }
        }
        private timer btnLimiter_F = new timer(new TimeSpan(0, 0, 1));


        private void resetPlayer(int playerNumber)
        {
            resetPlayer(seatLocations[playerNumber], seatPositions[playerNumber]);
        }

        timer scaleLimiter = new timer(new TimeSpan(0, 0, 1));

        private bool ready = false;
        public override void frameHook(float interpolation)
        {

        }
        private Random ran = new Random((int)DateTime.Now.Ticks);
    }
}
