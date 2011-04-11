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
using MOIS;
using Mogre.PhysX;
namespace ExtraMegaBlob
{
    public class plugin : ExtraMegaBlob.References.ClientPlugin
    {
        public override Hashtable materials_lookup()
        {
            Hashtable h = new Hashtable();
            #region materials
            h["metal"] = "\\Players\\BumpyMetal.jpg";
            h["dirt"] = "\\terr_dirt-grass.jpg";
            #endregion
            return h;
        }
        public override Hashtable meshes_lookup()
        {
            Hashtable h = new Hashtable();
            #region meshes
            h["drone"] = "\\Drone.mesh";
            h["drone"] = "\\baseball.mesh";
            #endregion
            return h;
        }
        public override Hashtable skeletons_lookup()
        {
            Hashtable h = new Hashtable();
            #region skeletons
            h["droneskele"] = "\\Drone.skeleton";
            #endregion
            return h;
        }
        private BoxControllerDesc bcd = new BoxControllerDesc();
        private BoxController control = null;

        public override void init()
        {

            #region ground
            MeshManager.Singleton.CreatePlane("ground",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                new Plane(Mogre.Vector3.UNIT_Y, 0),
                1500, 1500, 20, 20, true, 1, 5, 5, Mogre.Vector3.UNIT_Z);
            // Create a ground plane
            entities.Add(OgreWindow.Instance.mSceneMgr.CreateEntity("GroundEntity", "ground"));
            entities["GroundEntity"].CastShadows = false;
            entities["GroundEntity"].SetMaterialName("dirt");
            nodes.Add(OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode("ground"));
            nodes["ground"].AttachObject(entities["GroundEntity"]);
            nodes["ground"].Position = new Mogre.Vector3(0f, 0f, 0f) + Location().toMogre;

            // the actor properties control the mass, position and orientation
            // if you leave the body set to null it will become a static actor and wont move
            ActorDesc actorDesc2 = new ActorDesc();
            actorDesc2.Density = 4;
            actorDesc2.Body = null;
            actorDesc2.GlobalPosition = nodes["ground"].Position;
            actorDesc2.GlobalOrientation = nodes["ground"].Orientation.ToRotationMatrix();


            PhysXHelpers.StaticMeshData meshdata = new PhysXHelpers.StaticMeshData(entities["GroundEntity"].GetMesh());
            actorDesc2.Shapes.Add(PhysXHelpers.CreateTriangleMesh(meshdata));
            Actor actor2 = null;
            try { actor2 = OgreWindow.Instance.scene.CreateActor(actorDesc2); }
            catch (System.AccessViolationException ex) { log(ex.ToString()); }
            if (actor2 != null)
            {
                // create our special actor node to tie together the scene node and actor that we can update its position later
                ActorNode actorNode2 = new ActorNode(nodes["ground"], actor2);
                actors.Add(actorNode2);
            }
            #endregion



            lights.Add(OgreWindow.Instance.mSceneMgr.CreateLight("playerLight"));
            lights["playerLight"].Type = Light.LightTypes.LT_POINT;
            lights["playerLight"].Position = Location().toMogre;
            lights["playerLight"].DiffuseColour = ColourValue.White;
            lights["playerLight"].SpecularColour = ColourValue.White;

            #region drone

            OgreWindow.Instance.skeletons["\\Drone.skeleton"].Load();
            OgreWindow.Instance.meshes["\\Drone.mesh"].Load();
            OgreWindow.Instance.meshes["\\Drone.mesh"].SkeletonName = "\\Drone.skeleton";

            entities.Add(OgreWindow.Instance.mSceneMgr.CreateEntity("drone", "\\Drone.mesh"));

            entities["drone"].CastShadows = true;
            walkState = entities["drone"].GetAnimationState("walk");
            walkState.Enabled = true;
            walkState.Loop = true;
            entities["drone"].SetMaterialName("metal");
            nodes.Add(OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode("drone"));
            nodes["drone"].AttachObject(entities["drone"]);
            nodes["drone"].Position = new Mogre.Vector3(0f, 40f, 0f) + Location().toMogre;
            nodes["drone"].Scale(new Mogre.Vector3(.3f));

            nodes.Add(nodes["drone"].CreateChildSceneNode("orbit0"));
            nodes.Add(nodes["orbit0"].CreateChildSceneNode("orbit"));
            nodes["orbit"].Position = Location().toMogre;
            nodes["orbit"].AttachObject(OgreWindow.Instance.mCamera);
            nodes["drone"].SetFixedYawAxis(true);



            #endregion


            //#region baseball
            //entities.Add(OgreWindow.Instance.mSceneMgr.CreateEntity("baseball", "\\baseball.mesh"));
            //entities["baseball"].SetMaterialName("baseball");
            ////nodes.Add(OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode("baseball"));
            //nodes.Add(nodes["drone"].CreateChildSceneNode("baseball"));
            //nodes["baseball"].AttachObject(entities["baseball"]);
            //nodes["baseball"].SetScale(.5f, .5f, .5f);
            //nodes["baseball"].SetPosition(-3f, 7f, 3f);
            //// nodes["baseball"].SetScale(5f, 5f, 5f);
            //#endregion

            #region player physics
            bcd = new BoxControllerDesc();
            control = OgreWindow.Instance.physics.ControllerManager.CreateController(OgreWindow.Instance.scene, bcd); //System.NullReferenceException
            #endregion

            nodes.Add(OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode("suspensionY"));

            OgreWindow.g_m.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
            middlemousetimer.reset();
            middlemousetimer.start();

            this.btnLimiter_F.reset();
            this.btnLimiter_F.start();

            ready = true;
            new Thread(new ThreadStart(controlThread)).Start();
            new Thread(new ThreadStart(statusUpdaterThread)).Start();

            localY = nodes["drone"]._getDerivedOrientation() * Mogre.Vector3.UNIT_Y;
            localZ = nodes["drone"]._getDerivedOrientation() * Mogre.Vector3.UNIT_Z;
            localX = nodes["drone"]._getDerivedOrientation() * Mogre.Vector3.UNIT_X;

            OgreWindow.Instance.tbTextToSend.GotFocus += new EventHandler(tbTextToSend_GotFocus);
            OgreWindow.Instance.tbTextToSend.LostFocus += new EventHandler(tbTextToSend_LostFocus);
            OgreWindow.Instance.tbConsole.GotFocus += new EventHandler(tbConsole_GotFocus);
            OgreWindow.Instance.tbConsole.LostFocus += new EventHandler(tbConsole_LostFocus);
        }

        void tbConsole_LostFocus(object sender, EventArgs e)
        {
            consoleBarUsage = false;
        }

        void tbConsole_GotFocus(object sender, EventArgs e)
        {
            consoleBarUsage = true;
        }

        void tbTextToSend_LostFocus(object sender, EventArgs e)
        {
            textBarUsage = false;
        }

        void tbTextToSend_GotFocus(object sender, EventArgs e)
        {
            textBarUsage = true;
        }
        private bool consoleBarUsage = false;
        private bool textBarUsage = false;
        private bool updatingNow = false;
        private bool updateThreadPaused = false;
        private void pauseUpdates()
        {
            updateThreadPaused = true;
            while (updatingNow)
                Thread.Sleep(1);
        }
        private void unPauseUpdates()
        {
            updateThreadPaused = false;
        }
        private void statusUpdaterThread()
        {
            while (ready)
            {
                Thread.Sleep(1);
                if (updateThreadPaused) continue;
                updatingNow = true;
                if (!control.Actor.IsDisposed)
                {
                    if (MoveScale_Camera_forwardback != 0 || MoveScale_Camera_leftright != 0 || MoveScale_Camera_updown != 0)
                    {

                        TranslateVector_Camera.z -= MoveScale_Camera_forwardback;
                        TranslateVector_Camera.x -= MoveScale_Camera_leftright;
                        TranslateVector_Camera.y += MoveScale_Camera_updown;
                        Mogre.Vector3 loc1 = control.Actor.GlobalPosition;
                        Mogre.Vector3 loc = control.Actor.GlobalOrientationQuaternion * TranslateVector_Camera;
                        setPos(loc + loc1);

                        if (LocationBeaconInterval.elapsed)
                        {
                            sendLocationBeacon();
                            LocationBeaconInterval.start();
                        }
                        TranslateVector_Camera = new Mogre.Vector3();
                    }
                    if (middleMouseMode == middleMouseBehavior.orbitCamDistance)
                    {
                        if (middleMouseState == middleMouseStates.scrolldown)
                        {
                            camDistance -= .1f;
                            if (camDistance < 0f)
                                camDistance = 0f;
                            updateCam();
                        }
                        if (middleMouseState == middleMouseStates.scrollup)
                        {
                            camDistance += .1f;
                            if (camDistance > 100f)
                                camDistance = 100f;
                            updateCam();
                        }
                    }
                    if (turning_left)
                        setOrient(control.Actor.GlobalOrientationQuaternion * ModifyAngleAroundAxis(new Degree(2 * .001f), new Mogre.Vector3(0, 1, 0)));
                    if (turning_right)
                        setOrient(control.Actor.GlobalOrientationQuaternion * ModifyAngleAroundAxis(new Degree(-2 * .001f), new Mogre.Vector3(0, 1, 0)));
                }
                updatingNow = false;
                
            }
        }
        private Mogre.Vector3 localY = new Mogre.Vector3();
        private Mogre.Vector3 localZ = new Mogre.Vector3();
        private Mogre.Vector3 localX = new Mogre.Vector3();
        private AnimationState walkState = null;
        public override void shutdown()
        {
            ready = false;
            log("shutting down!");
            actors.shutdown();
            nodes.shutdown();
            entities.shutdown();
            lights.shutdown();
            log("done shutting down!");
        }
        public override ExtraMegaBlob.References.Vector3 Location()
        {
            //return new ExtraMegaBlob.References.Vector3(0f, -168.6846f, -1101.067f);
            return new ExtraMegaBlob.References.Vector3(0f, 0f, 0f);
        }
        public override string Name()
        {
            return "Players_Client";
        }
        public override string[] AllowedInputNames()
        {
            return new string[] { "Movement_Client", "Players_Server" };
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] { "Players_Server" };
        }
        public override void inbox(ExtraMegaBlob.References.Event ev)
        {
            if (!ready) return;
            switch (ev._Keyword)
            {
                case KeyWord.PLAYER_RESET:
                    chat("resetting player");

                    Quaternion q = new Quaternion(
                        float.Parse(ev._Memories[KeyWord.DATA_QUATERNION_W].Value),
                        float.Parse(ev._Memories[KeyWord.DATA_QUATERNION_X].Value),
                        float.Parse(ev._Memories[KeyWord.DATA_QUATERNION_Y].Value),
                        float.Parse(ev._Memories[KeyWord.DATA_QUATERNION_Z].Value));

                    Mogre.Vector3 v = new Mogre.Vector3(
                        float.Parse(ev._Memories[KeyWord.DATA_VECTOR3_X].Value),
                        float.Parse(ev._Memories[KeyWord.DATA_VECTOR3_Y].Value),
                        float.Parse(ev._Memories[KeyWord.DATA_VECTOR3_Z].Value));

                    resetPlayer2(v, q);

                    break;
                case KeyWord.CMD_GO:
                    this.logConsole("parsing coordinates");

                    string arguments = ev._Memories[KeyWord.DATA_STRING].Value.Trim();
                    if (arguments == "") return;
                    //" -63.60852f, 95.18869f, -115.0435f "

                    //resetPlayer2(v, q);
                    string[] x = arguments.Split(',');
                    int a = 0;
                    Mogre.Vector3 u = new Mogre.Vector3();
                    try
                    {
                        for (int s = 0; s < x.Length; s++)
                        {
                            string j = x[s].Trim().Replace("f", "");
                            if (j != "")
                            {
                                switch (a)
                                {
                                    case 0:
                                        u.x = float.Parse(j);
                                        a++;
                                        break;
                                    case 1:
                                        u.y = float.Parse(j);
                                        a++;
                                        break;
                                    case 2:
                                        u.z = float.Parse(j);
                                        a++;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    resetPlayer2(u, nodes["drone"].Orientation);
                    this.logConsole("coordinates: " + u.ToString());

                    break;
                case KeyWord.PLAYER_FREEZE:
                    player_freeze = true;
                    break;
                case KeyWord.PLAYER_UNFREEZE:
                    player_freeze = false;
                    break;
                default:
                    break;
            }
        }
        private bool player_freeze = false;
        private Mogre.Vector3 getOrbitalPosition(Mogre.Vector3 focalPoint, Quaternion direction, float distanceToFocalPoint)
        {
            Mogre.Vector3 eyePos, xAxis, yAxis, zAxis;
            Quaternion m_Orientation = direction;

            Mogre.Matrix3 m_ViewMatrix;

            // Get the rotation matrix from the quaternion
            m_ViewMatrix = m_Orientation.ToRotationMatrix();

            // Get the axis we need them for the camera position calculation (the axes span a new coord frame)
            xAxis = new Mogre.Vector3(m_ViewMatrix[0, 0], m_ViewMatrix[0, 1], m_ViewMatrix[0, 2]);
            yAxis = new Mogre.Vector3(m_ViewMatrix[1, 0], m_ViewMatrix[1, 1], m_ViewMatrix[1, 2]);
            zAxis = new Mogre.Vector3(m_ViewMatrix[2, 0], m_ViewMatrix[2, 1], m_ViewMatrix[2, 2]);


            eyePos = focalPoint + zAxis * distanceToFocalPoint;

            // Transform our vector in the orbital space. Here we rotate and flip it.
            m_ViewMatrix[0, 3] = -xAxis.DotProduct(eyePos);
            m_ViewMatrix[1, 3] = -yAxis.DotProduct(eyePos);
            m_ViewMatrix[2, 3] = -zAxis.DotProduct(eyePos);

            Mogre.Vector3 m_CamPos = new Mogre.Vector3();

            // So extract the cam position for the sake of completeness
            m_CamPos.x = m_ViewMatrix[0, 3];
            m_CamPos.y = m_ViewMatrix[1, 3];
            m_CamPos.z = m_ViewMatrix[2, 3];

            return m_CamPos;
        }
        private Quaternion addQuats(Quaternion q1, Quaternion q2)
        {
            float x = q1.x + q2.x;
            float y = q1.y + q2.y;
            float z = q1.z + q2.z;
            float w = q1.w + q2.w;
            Quaternion q = new Quaternion(w, x, y, z);
            chat(q.Normalise().ToString());
            return q;
        }
        //private Quaternion addQuats2(Quaternion q1, Quaternion q2)
        //{
        //    q1.FromAxes(q2.to
        //    float x = q1.x + q2.x;
        //    float y = q1.y + q2.y;
        //    float z = q1.z + q2.z;
        //    float w = q1.w + q2.w;
        //    return new Quaternion(w, x, y, z).ya;
        //}
        // private float 
        private float normalizeAngleRadian(float angle)
        {
            if (angle < 0 || angle > (float)System.Math.PI * 2) return System.Math.Abs(((float)System.Math.PI * 2) - System.Math.Abs(angle));
            else return angle;
        }
        private Quaternion ModifyAngleAroundAxis(Degree a, Mogre.Vector3 axis)
        {
            //To determine the quaternion for a rotation of α degrees/radians around an axis defined by a vector (x, y, z):
            Quaternion q = new Quaternion();
            q.w = (float)System.Math.Cos((double)(0.5 * a.ValueDegrees));
            q.x = axis.x * (float)System.Math.Sin((double)(0.5 * a.ValueDegrees));
            q.y = axis.y * (float)System.Math.Sin((double)(0.5 * a.ValueDegrees));
            q.z = axis.z * (float)System.Math.Sin((double)(0.5 * a.ValueDegrees));
            return q;
        }
        private bool mouseMoved(MouseEvent arg)
        {
            float RotateScale_CameraPitch = .001f;//mouse sensitivity
            float RotateScale_PlayerTurn = .001f;//mouse sensitivity
            MouseState_NativePtr s = arg.state;
            if (arg.state.buttons == 2)
            {
                //chat("____________________________________________________________");
                nodes["orbit0"].Pitch(s.Y.rel * RotateScale_CameraPitch);
                if (!player_freeze && !textBarUsage && !consoleBarUsage)
                    if (s.X.rel != 0f)
                    {
                        //nodes["drone"].Yaw(-s.X.rel * RotateScale_PlayerTurn);
                        //setOrient(nodes["drone"]._getDerivedOrientation());
                        setOrient(control.Actor.GlobalOrientationQuaternion * ModifyAngleAroundAxis(new Degree(-s.X.rel * RotateScale_PlayerTurn), new Mogre.Vector3(0, 1, 0)));

                        //Mogre.Quaternion orient1 = control.Actor.GlobalOrientationQuaternion;
                        //Mogre.Vector3 rkAxis = new Mogre.Vector3();
                        //Degree rfAngle = new Degree();
                        //orient1.ToRotationMatrix().ToAxisAngle(out rkAxis, out rfAngle);
                        //orient2 = new Quaternion(new Radian(new Degree(rfAngle.ValueDegrees + (-s.X.rel * RotateScale_PlayerTurn))), new Mogre.Vector3(0, 1, 0));
                        ////control.Actor.GlobalOrientationQuaternion = orient2;
                        ////setOrient(orient2);
                        //spin(rfAngle.ValueDegrees + (-s.X.rel * RotateScale_PlayerTurn));
                    }
                updateCam();
            }

            float mouseZ = (float)OgreWindow.g_m.MouseState.Z.rel * .1f;
            //chat(mouseZ.ToString());
            if (mouseZ > 0)
            {
                middleMouseState = middleMouseStates.scrollup;
                middlemousetimer.reset();
                middlemousetimer.start();
            }
            else if (mouseZ < 0)
            {
                middleMouseState = middleMouseStates.scrolldown;
                middlemousetimer.reset();
                middlemousetimer.start();
            }

            return true;
        }
        private void updateCam()
        {
            SceneNode camnode = nodes["orbit"];
            SceneNode node = nodes["drone"];
            Mogre.Vector3 focalPoint = new Mogre.Vector3(0f, 0f, 0f);
            Quaternion direction = node.Orientation;
            direction.Normalise();

            //Mogre.Vector3 m_CamPos = getOrbitalPosition(focalPoint, direction);
            //OgreWindow.Instance.cameraNode.LookAt(focalPoint, Node.TransformSpace.TS_PARENT);
            //OgreWindow.Instance.cameraNode.Position = m_CamPos + focalPoint;


            Mogre.Vector3 m_CamPos = getOrbitalPosition(focalPoint, direction, camDistance);
            camnode.Position = m_CamPos;
            camnode.LookAt(node.Position, Node.TransformSpace.TS_WORLD);

        }
        private timer middlemousetimer = new timer(mmbClutch);
        private enum middleMouseStates
        {
            scrollup,
            scrolldown,
            idle
        }
        private enum middleMouseBehavior
        {
            orbitCamDistance,
            yAxis
        }
        private middleMouseBehavior middleMouseMode = middleMouseBehavior.yAxis;
        private middleMouseStates middleMouseState = middleMouseStates.idle;
        private float camDistance = 50f;
        private float MoveScale_Camera_forwardback = 0f;
        private float MoveScale_Camera_leftright = 0f;
        private float MoveScale_Camera_updown = 0f;
        private Mogre.Vector3 TranslateVector_Camera = new Mogre.Vector3();
        private const float speedcap_forwardback = 555f;
        private const float speedcap_leftright = 555f;
        private const float speedcap_updown = 5.5f;
        private const float incr_forwardback = .05f;
        private const float incr_leftright = .05f;
        private const float incr_updown = 1f;
        private const float brakes_updown = incr_updown * 2;
        private const float brakes_forwardback = incr_forwardback * 2;
        private const float brakes_leftright = incr_leftright * 2;
        private static TimeSpan mmbClutch = new TimeSpan(0, 0, 0, 0, 100);

        private void setPos(Mogre.Vector3 pos)
        {
            if (!player_freeze && !textBarUsage && !control.Actor.IsDisposed)
            {
                control.Actor.MoveGlobalPosition(pos);
                // actors.UpdateAllActors(.0f);
                //nodes["drone"].SetPosition(pos.x, pos.y, pos.z);
            }
        }
        private void setOrient(Quaternion orient)
        {
            if (!player_freeze && !textBarUsage && !control.Actor.IsDisposed)
            {
                control.Actor.GlobalOrientationQuaternion = orient;
                //  actors.UpdateAllActors(.0f);
                //nodes["drone"].SetOrientation(orient.w, orient.x, orient.y, orient.z);
            }
        }
        private void spin(Degree deg)
        {
            Mogre.Quaternion orient1 = control.Actor.GlobalOrientationQuaternion;

            Mogre.Vector3 rkAxis = new Mogre.Vector3();
            Degree rfAngle = new Degree();
            orient1.ToRotationMatrix().ToAxisAngle(out rkAxis, out rfAngle);
            Mogre.Quaternion orient2 = new Quaternion(new Radian(deg), new Mogre.Vector3(0, 1, 0));
            //control.Actor.GlobalOrientationQuaternion = orient2;
            //setOrient(orient2);
            setOrient(orient2);
        }
        private timer LocationBeaconInterval = new timer(new TimeSpan(0, 0, 1));//1 second player world location updates
        private timer btnLimiter_F = new timer(new TimeSpan(0, 0, 1));//1 second player world location updates
        private void sendLocationBeacon()
        {
            sendLocationBeacon(control.Actor.GlobalPosition);
        }
        private void sendLocationBeacon(Mogre.Vector3 pos)
        {
            Memories mems = new Memories();
            mems.Add(new Memory("", KeyWord.CARTESIAN_X, pos.x.ToString(), null));
            mems.Add(new Memory("", KeyWord.CARTESIAN_Y, pos.y.ToString(), null));
            mems.Add(new Memory("", KeyWord.CARTESIAN_Z, pos.z.ToString(), null));
            Event ev = new Event();
            ev._Keyword = KeyWord.CARTESIAN_SECRETPLAYERLOCATION;
            ev._Memories = mems;
            ev._IntendedRecipients = EventTransfer.CLIENTTOSERVER;
            base.outboxMessage(this, ev);
            // log("Location: X=" + imAt.x.ToString() + " Y=" + imAt.y.ToString() + " Z=" + imAt.z.ToString());
        }

        private void resetPlayer2(Mogre.Vector3 loc, Quaternion orient)
        {
           
            //MoveScale_Camera_forwardback = control.Actor.GlobalPosition.z - loc.z;
            //MoveScale_Camera_leftright = control.Actor.GlobalPosition.x - loc.x;
            //MoveScale_Camera_updown = control.Actor.GlobalPosition.y - loc.y;

            //Mogre.Vector3 loc1 = control.Actor.GlobalPosition;
            pauseUpdates();
            Mogre.Vector3 loc2 = control.Actor.GlobalOrientationQuaternion * loc;
            setPos(loc2 + loc);
            TranslateVector_Camera = new Mogre.Vector3(0f, 0f, 0f);
            MoveScale_Camera_forwardback = 0f;
            MoveScale_Camera_leftright = 0f;
            MoveScale_Camera_updown = 0f;
           // setPos(loc);
            setOrient(orient);
            unPauseUpdates();
        }
        //private void resetPlayer3(Mogre.Vector3 loc, Quaternion orient)
        //{
        //    TranslateVector_Camera = new Mogre.Vector3(0f, 0f, 0f);
        //    MoveScale_Camera_forwardback = 0f;
        //    MoveScale_Camera_leftright = 0f;
        //    MoveScale_Camera_updown = 0f;
        //    setPos(loc);
        //    setOrient(orient);
        //}
        private bool turning_left = false;
        private bool turning_right = false;
        private void controlThread()
        {
            //float RotateScale_PlayerTurn = .005f;//mouse sensitivity
            while (ready)
            {
                if (!consoleBarUsage)
                    try
                    {
                        if (middlemousetimer.elapsed)
                        {
                            middleMouseState = middleMouseStates.idle;
                        }
                        if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_W))
                        {



                            if (MoveScale_Camera_forwardback > -speedcap_forwardback)
                                MoveScale_Camera_forwardback -= incr_forwardback;
                        }
                        else if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_S))
                        {
                            if (MoveScale_Camera_forwardback < speedcap_forwardback)
                                MoveScale_Camera_forwardback += incr_forwardback;
                        }
                        else if (MoveScale_Camera_forwardback != 0f)
                        {
                            if (MoveScale_Camera_forwardback > 0f)
                                MoveScale_Camera_forwardback -= incr_forwardback;
                            else
                                MoveScale_Camera_forwardback += incr_forwardback;
                            if (MoveScale_Camera_forwardback < brakes_forwardback && MoveScale_Camera_forwardback > -brakes_forwardback)
                                MoveScale_Camera_forwardback = 0f;
                        }
                        else
                        {
                            MoveScale_Camera_forwardback = 0f;
                        }
                        #region strafe
                        if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_Q))
                        {

                            if (MoveScale_Camera_leftright > -speedcap_leftright)
                                MoveScale_Camera_leftright -= incr_leftright;
                            //nodes["drone"].Yaw(-1f * RotateScale_PlayerTurn);
                            //setOrient(nodes["drone"]._getDerivedOrientation());
                        }
                        else if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_E))
                        {
                            if (MoveScale_Camera_leftright < speedcap_leftright)
                                MoveScale_Camera_leftright += incr_leftright;
                            //nodes["drone"].Yaw(1f * RotateScale_PlayerTurn);
                            //setOrient(nodes["drone"]._getDerivedOrientation());
                        }
                        else if (MoveScale_Camera_leftright != 0f)
                        {
                            if (MoveScale_Camera_leftright > 0f)
                                MoveScale_Camera_leftright -= incr_leftright;
                            else
                                MoveScale_Camera_leftright += incr_leftright;
                            if (MoveScale_Camera_leftright < brakes_leftright && MoveScale_Camera_leftright > -brakes_leftright)
                                MoveScale_Camera_leftright = 0f;
                        }
                        else
                        {
                            MoveScale_Camera_leftright = 0f;
                        }
                        #endregion

                        #region Turn Left / Right
                        if (!OgreWindow.g_kb.IsKeyDown(KeyCode.KC_A) && !OgreWindow.g_kb.IsKeyDown(KeyCode.KC_D))
                        {
                            turning_left = false;
                            turning_right = false;
                        }
                        else if (OgreWindow.g_kb.IsKeyDown(KeyCode.KC_A) && OgreWindow.g_kb.IsKeyDown(KeyCode.KC_D))
                        {
                            turning_left = false;
                            turning_right = false;

                        }
                        else if (OgreWindow.g_kb.IsKeyDown(KeyCode.KC_A))
                        {
                            turning_left = true;
                            turning_right = false;
                        }
                        else if (OgreWindow.g_kb.IsKeyDown(KeyCode.KC_D))
                        {
                            turning_left = false;
                            turning_right = true;
                        }
                        #endregion

                        if (middleMouseState == middleMouseStates.scrolldown)
                        {
                            if (MoveScale_Camera_updown > -speedcap_updown)
                                MoveScale_Camera_updown -= incr_updown;
                        }
                        else if (middleMouseState == middleMouseStates.scrollup)
                        {
                            if (MoveScale_Camera_updown < speedcap_updown)
                                MoveScale_Camera_updown += incr_updown;
                        }
                        else if (MoveScale_Camera_updown != 0f)
                        {
                            if (MoveScale_Camera_updown > 0f)
                                MoveScale_Camera_updown -= incr_updown;
                            else
                                MoveScale_Camera_updown += incr_updown;
                            if (MoveScale_Camera_updown < brakes_updown && MoveScale_Camera_updown > -brakes_updown)
                                MoveScale_Camera_updown = 0f;
                        }
                        else
                        {
                            MoveScale_Camera_updown = 0f;
                        }
                        //TranslateVector_Camera.z -= MoveScale_Camera_forwardback;
                        //TranslateVector_Camera.x -= MoveScale_Camera_leftright;
                        //TranslateVector_Camera.y += MoveScale_Camera_updown;

                        if (MoveScale_Camera_updown != 0f)
                        {
                            //control.Actor.AddForce(new Mogre.Vector3(0f, MoveScale_Camera_updown, 0f));
                            //chat(MoveScale_Camera_updown.ToString());

                            //this.camDistance += MoveScale_Camera_updown;
                            //resetCam();
                        }



                        //control.Actor.AddForce(loc);



                        //actors["drone"].actor.AddLocalTorque(new Mogre.Vector3(0f, 1000f, 0f));
                        //actors["drone"].actor.AddLocalForce(loc);

                        // actors["drone"].actor.LinearVelocity = loc; //kinda works
                        //actors["drone"].actor.GlobalOrientation..Orientation.ToRotationMatrix();
                        // if (nodes["drone"].Orientation.Roll.ValueDegrees > 0)
                        //     actors["drone"].actor.AddLocalTorque(new Mogre.Vector3(0f, 1000f, 0f));

                    }
                    catch (Exception ex) { log(ex.ToString()); }

                Thread.Sleep(100);

            }
        }
        public override void updateHook()
        {
            if (t.elapsed)
            {
                t.reset();
                t.start();
            }
            if (ready)
            {
                if (walkState != null)
                    walkState.AddTime(MoveScale_Camera_forwardback * .1f);

                if (control.Actor == null) return;
                if (!control.Actor.IsSleeping)
                {
                    lights["playerLight"].Position = control.Actor.GlobalPosition;
                    nodes["drone"].Position = control.Actor.GlobalPosition;
                    nodes["drone"].Orientation = control.Actor.GlobalOrientationQuaternion;
                    OgreWindow.Instance.setInfoLabelText(string.Format(" {0}f, {1}f, {2}f ", control.Actor.GlobalPosition.x, control.Actor.GlobalPosition.y, control.Actor.GlobalPosition.z));
                    OgreWindow.Instance.setInfoLabelText2(string.Format(" {0}f, {1}f, {2}f, {3}f ", control.Actor.GlobalOrientationQuaternion.w, control.Actor.GlobalOrientationQuaternion.x, control.Actor.GlobalOrientationQuaternion.y, control.Actor.GlobalOrientationQuaternion.z));
                    OgreWindow.Instance.setInfoLabelText3(string.Format(" {0}f, {1}f, {2}f, {3}f ", nodes["orbit0"]._getDerivedOrientation().w, nodes["orbit0"]._getDerivedOrientation().x, nodes["orbit0"]._getDerivedOrientation().y, nodes["orbit0"]._getDerivedOrientation().z));
                }


            }
        }
        timer scaleLimiter = new timer(new TimeSpan(0, 0, 1));

        public override void frameHook(float interpolation)
        {

            //if (MoveScale_Camera_updown != 0)
            //{
            //    float s = MoveScale_Camera_updown * (interpolation + 1);
            //    TranslateVector_Camera.y += s;
            //    MoveScale_Camera_updown -= s;
            //}
            //try
            //{
            //    ////Mogre.Vector3 translateTo = OgreWindow.Instance.cameraYawNode.Orientation * OgreWindow.Instance.cameraPitchNode.Orientation * TranslateVector_Camera;
            //    //if (TranslateVector_Camera.x != 0f || TranslateVector_Camera.y != 0f || TranslateVector_Camera.z != 0f)
            //    //{
            //    //    updateCam();

            //    //    Mogre.Vector3 loc = nodes["drone"]._getDerivedOrientation() * TranslateVector_Camera;

            //    //    //nodes["drone"].Translate(nodes["drone"]._getDerivedOrientation() * TranslateVector_Camera); //limited to x/z - works
            //    //    //actors["drone"].actor.AddLocalForce(loc);
            //    //    //if (actors["drone"].actor.IsSleeping) actors["drone"].actor.WakeUp(.1f);
            //    //    //actors["drone"].actor.LinearVelocity = loc; //kinda works

            //    //    // actors["drone"].actor.MoveGlobalPosition(actors["drone"].actor.GlobalPosition + loc);

            //    //}

            //    TranslateVector_Camera = new Mogre.Vector3();
            //}
            //catch
            //{
            //}

            //if (turning_left)
            //{
            //    //control.Actor.AddTorque(new Mogre.Vector3(0f, 10f, 0f), ForceModes.SmoothImpulse);
            //    //OgreWindow.Instance.pause();
            //    nodes["drone"].Yaw(1f * 0.005f);
            //    setOrient(nodes["drone"]._getDerivedOrientation());
            //    //OgreWindow.Instance.unpause();
            //}
            //if (turning_right)
            //{
            //    //control.Actor.AddTorque(new Mogre.Vector3(0f, -10f, 0f), ForceModes.SmoothImpulse);
            //    //OgreWindow.Instance.pause();//not helping
            //    nodes["drone"].Yaw(-1f * 0.005f);
            //    setOrient(nodes["drone"]._getDerivedOrientation());
            //    //OgreWindow.Instance.unpause();
            //}
        }
        private Random ran = new Random((int)DateTime.Now.Ticks);


        ExtraMegaBlob.References.timer t = new ExtraMegaBlob.References.timer(new TimeSpan(0, 0, 0, 0, 1000));
    }
}
