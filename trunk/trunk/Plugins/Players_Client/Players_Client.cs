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
        private Hashtable materials
        {
            get
            {
                Hashtable h = new Hashtable();
                #region materials
                h["metal"] = "\\Players\\BumpyMetal.jpg";
                h["dirt"] = "\\terr_dirt-grass.jpg";
                #endregion
                return h;
            }
        }
        private Hashtable meshes
        {
            get
            {
                Hashtable h = new Hashtable();
                #region meshes
                h["drone"] = "\\Drone.mesh";
                #endregion
                return h;
            }
        }
        private Hashtable skeletons
        {
            get
            {
                Hashtable h = new Hashtable();
                #region skeletons
                h["droneskele"] = "\\Drone.skeleton";
                #endregion
                return h;
            }
        }
        private SceneNodes nodes = new SceneNodes();
        private ActorNodes actors = new ActorNodes();
        private Entities entities = new Entities();
        private Lights lights = new Lights();
        private CapsuleControllerDesc ccd = new CapsuleControllerDesc();
        private BoxControllerDesc bcd = new BoxControllerDesc();
        private BoxController control = null;
        private void resourceWaitThread()
        {
            while (true)
            {
                Thread.Sleep(1000);

                foreach (DictionaryEntry de in materials)
                {
                    if (!TextureManager.Singleton.ResourceExists((string)de.Value)) goto waitmore;
                }
                foreach (DictionaryEntry de in skeletons)
                {
                    if (!SkeletonManager.Singleton.ResourceExists((string)de.Value)) goto waitmore;
                }
                foreach (DictionaryEntry de in meshes)
                {
                    if (!MeshManager.Singleton.ResourceExists((string)de.Value)) goto waitmore;
                }
                if (!OgreWindow.Instance.mSceneMgr.HasSceneNode("mushroom")) goto waitmore;
                if (!OgreWindow.Instance.SceneReady) goto waitmore;


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
                Hashtable mats = materials;
                foreach (DictionaryEntry mat in mats)
                {
                    ((MaterialPtr)MaterialManager.Singleton.Create((string)mat.Key, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME)).GetTechnique(0).GetPass(0).CreateTextureUnitState((string)mat.Value);
                }

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
                #region ground physics
                // the actor properties control the mass, position and orientation
                // if you leave the body set to null it will become a static actor and wont move
                ActorDesc actorDesc2 = new ActorDesc();
                actorDesc2.Density = 4;
                actorDesc2.Body = null;
                actorDesc2.GlobalPosition = nodes["ground"].Position;
                actorDesc2.GlobalOrientation = nodes["ground"].Orientation.ToRotationMatrix();


                PhysXHelpers.StaticMeshData meshdata = new PhysXHelpers.StaticMeshData(entities["GroundEntity"].GetMesh());
                actorDesc2.Shapes.Add(PhysXHelpers.CreateTriangleMesh(meshdata));

                // finally, create the actor in the physics scene
                Actor actor2 = OgreWindow.Instance.scene.CreateActor(actorDesc2);

                // create our special actor node to tie together the scene node and actor that we can update its position later
                ActorNode actorNode2 = new ActorNode(nodes["ground"], actor2);
                actors.Add(actorNode2);
                #endregion


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
                nodes["orbit"].Position = new Mogre.Vector3(0f, 0f, 0f);
                nodes["orbit"].AttachObject(OgreWindow.Instance.mCamera);
                nodes["drone"].SetFixedYawAxis(true);

                #region player physics

                // CapsuleShapeDesc csd = new CapsuleShapeDesc(10, 20, this.Location().toMogre);



                //Controller c = new BoxControllerDesc();


                control = OgreWindow.Instance.physics.ControllerManager.CreateController(OgreWindow.Instance.scene, bcd);
                //Physics..ControllerManager
                //BodyDesc boxController = new BoxControllerDesc();

                //   Camera, new eyecm.PhysX.CapsuleShapeDesc(25, 10)

                //Controller cnt = new Controller();


                //BodyDesc bodyDesc = new BodyDesc();


                // physics
                // attaching a body to the actor makes it dynamic, you can set things like initial velocity

                // the actor properties control the mass, position and orientation
                // if you leave the body set to null it will become a static actor and wont move
                ActorDesc actorDesc = new ActorDesc();
                actorDesc.Density = 4;
                actorDesc.Body = new BodyDesc();
                actorDesc.GlobalPosition = nodes["drone"].Position;
                actorDesc.GlobalOrientation = nodes["drone"].Orientation.ToRotationMatrix();

                // a quick trick the get the size of the physics shape right is to use the bounding box of the entity
                //actorDesc.Shapes.Add(new SphereShapeDesc(1f));//entities["drone"].BoundingBox.HalfSize * scale, entities["drone"].BoundingBox.Center * scale

                PhysXHelpers.StaticMeshData meshdata2 = new PhysXHelpers.StaticMeshData(entities["drone"].GetMesh());
                actorDesc.Shapes.Add(PhysXHelpers.CreateConvexHull(meshdata2));

                // finally, create the actor in the physics scene
                Actor actor = OgreWindow.Instance.scene.CreateActor(actorDesc);

                // create our special actor node to tie together the scene node and actor that we can update its position later
                ActorNode actorNode = new ActorNode(nodes["drone"], actor);
                actors.Add(actorNode);
                //control.Actor = actorNode;




                //actors["drone"].actor.BodyFlags.Kinematic = true;
                //actors["drone"].actor.BodyFlags.FrozenRotY = true;
                //actors["drone"].actor.BodyFlags.FrozenRotZ = true;
                //actors["drone"].actor.BodyFlags.FrozenRotX = true;
                //actors["drone"].actor.BodyFlags.FrozenRot = true;
                #endregion

                nodes.Add(OgreWindow.Instance.mSceneMgr.RootSceneNode.CreateChildSceneNode("suspensionY"));

                OgreWindow.g_m.MouseMoved += new MouseListener.MouseMovedHandler(mouseMoved);
                middlemousetimer.reset();
                middlemousetimer.start();
                ready = true;
                new Thread(new ThreadStart(controlThread)).Start();


                localY = nodes["drone"]._getDerivedOrientation() * Mogre.Vector3.UNIT_Y;
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }
            OgreWindow.Instance.unpause();
            log("done starting up! ");
        }

        private Mogre.Vector3 localY = new Mogre.Vector3();
        private void preventMousePick(string name)
        {
            Memories mems = new Memories();
            mems.Add(new Memory("Name", KeyWord.NIL, name, null));
            Event ev = new Event();
            ev._Keyword = KeyWord.PREVENTMOUSEPICK;
            ev._Memories = mems;
            ev._IntendedRecipients = EventTransfer.CLIENTTOCLIENT;
            base.outboxMessage(this, ev);
        }
        private AnimationState walkState = null;
        public override void startup()
        {
            new Thread(new ThreadStart(resourceWaitThread)).Start();
        }
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
        public override float Radius()
        {
            return 90;
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
                case KeyWord.MOVEPLAYER:

                    //OgreWindow.Instance.cameraNode.Position = m_CamPos * node._getDerivedPosition();

                    break;
                case KeyWord.ROTATEPLAYER:
                    //updateCam();
                    //ExtraMegaBlob.References.Vector3 loc = ExtraMegaBlob.References.Vector3.FromString(ev._Memories["loc"].Value);
                    //nodes["drone"].Translate(loc.toMogre);
                    break;
                default:
                    break;
            }
        }
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
        private float RotateScale_Camera = .001f;//mouse sensitivity
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
        private float normalizeAngle(float angle)
        {
            if (angle < 0 || angle > (float)System.Math.PI * 2) return System.Math.Abs(((float)System.Math.PI * 2) - System.Math.Abs(angle));
            else return angle;
        }
        private bool mouseMoved(MouseEvent arg)
        {
            float RotateScale_CameraX = .1f;//mouse sensitivity
            float RotateScale_CameraY = .001f;//mouse sensitivity
            MouseState_NativePtr s = arg.state;
            if (arg.state.buttons == 2)
            {
                float mouseRelX = (float)s.X.rel * (float)RotateScale_CameraX;
                chat("____________________________________________________________");
                chat(mouseRelX.ToString());
                //nodes["drone"].Yaw(-s.X.rel * RotateScale_Camera);
                //actors["drone"].actor.AddForce(new Mogre.Vector3(-s.X.rel * RotateScale_Camera, 0f, 0f), ForceModes.Force, true);
               // Mogre.Vector3 orientationdeltaX = new Mogre.Vector3(s.X.rel * RotateScale_CameraX, 0f, 0f);

                //chat("x.rel:" + s.X.rel.ToString());
                //control.
                //Quaternion orientationDelta2 = orientationdeltaX.GetRotationTo(new Mogre.Vector3(0, 0, 0));

                //actors["drone"].actor.AngularVelocity = orientationdelta;
                nodes["orbit0"].Pitch(s.Y.rel * RotateScale_CameraY);
                //nodes["drone"].Yaw(-s.X.rel * RotateScale_Camera);

                //  Mogre.Matrix3 matrix1 = control.Actor.GlobalOrientation.;
                // Mogre.Matrix3 matrix2 = orientationdeltaX

                Mogre.Quaternion orient1 = control.Actor.GlobalOrientationQuaternion;
                //Mogre.Quaternion orient2 = new Quaternion(new Radian(new Degree(45f)), orientationdeltaX);


                Radian rfYAngle = new Radian();
                Radian rfPAngle = new Radian();
                Radian rfRAngle = new Radian();
                orient1.ToRotationMatrix().ToEulerAnglesXYZ(out rfYAngle, out rfPAngle, out rfRAngle);
                
                rfPAngle = new Radian(new Degree(mouseRelX));

                //rfPAngle.ValueDegrees = normalizeAngle(rfPAngle.ValueDegrees);

                //if (rfPAngle. > 360f)
                //    rfPAngle = 0f;
                //if (rfPAngle < 0f)
                //    rfPAngle = 360f;

                //rfYAngle.Valu
                chat(rfPAngle.ValueDegrees.ToString());
                Matrix3 mat2 = new Matrix3();
                mat2.FromEulerAnglesYXZ(rfYAngle, rfPAngle, rfRAngle);
                Quaternion mOrientDest = new Quaternion();
                mOrientDest.FromRotationMatrix(mat2);
                //Quaternion g = localY.GetRotationTo(Mogre.Vector3.UNIT_Y);
                //chat(g.ToString());
                //mat.ToEulerAnglesYXZ(yRad, pRad, rRad);

                // Quaternion orient3 = new Quaternion((Quaternion)(orient1 * orient2));




                //pRad +=Ogre::Degree(0.8 * timeSinceLastFrame);


                //control.Actor.AddForce(loc);
                //control.Actor.MoveGlobalOrientation(mOrientDest);
                control.Actor.GlobalOrientationQuaternion = mOrientDest;


                //myBodyX05->addTorque(Vector3(0, 30000, 0)); //*(100*100*10*5000)/1000);      
                //myBodyX06->addTorque(Vector3(0, 120000, 0)); //*(100*100*10*5000)/1000);   
                //actors["drone"].actor.GlobalOrientationQuaternion += orientationDelta2;
                //actors["drone"].actor.MoveGlobalOrientation(actors["drone"].actor.GlobalOrientationQuaternion + orientationDelta2);

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


            Mogre.Vector3 m_CamPos = getOrbitalPosition(focalPoint, direction, 50f);
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
        private middleMouseStates middleMouseState = middleMouseStates.idle;
        private float MoveScale_Camera_forwardback = 0f;
        private float MoveScale_Camera_leftright = 0f;
        private float MoveScale_Camera_updown = 0f;
        private Mogre.Vector3 TranslateVector_Camera = new Mogre.Vector3();
        private const float speedcap_forwardback = .55f;
        private const float speedcap_leftright = .55f;
        private const float speedcap_updown = .5f;
        private const float incr_forwardback = .005f;
        private const float incr_leftright = .005f;
        private const float incr_updown = .0001f;
        private const float brakes_updown = incr_updown * 2;
        private const float brakes_forwardback = incr_forwardback * 2;
        private const float brakes_leftright = incr_leftright * 2;
        private static TimeSpan mmbClutch = new TimeSpan(0, 0, 0, 0, 100);

        private void controlThread()
        {
            while (ready)
            {
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
                    if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_A))
                    {
                        if (MoveScale_Camera_leftright > -speedcap_leftright)
                            MoveScale_Camera_leftright -= incr_leftright;
                    }
                    else if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_D))
                    {
                        if (MoveScale_Camera_leftright < speedcap_leftright)
                            MoveScale_Camera_leftright += incr_leftright;
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
                    TranslateVector_Camera.z -= MoveScale_Camera_forwardback;
                    TranslateVector_Camera.x -= MoveScale_Camera_leftright;
                    TranslateVector_Camera.y += MoveScale_Camera_updown;


                    Mogre.Vector3 loc1 = control.Actor.GlobalPosition;
                    Mogre.Vector3 loc = nodes["drone"]._getDerivedOrientation() * TranslateVector_Camera;

                    //control.Actor.AddForce(loc);
                    control.Actor.MoveGlobalPosition(loc + loc1);


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
                walkState.AddTime(.01f);
                //actors.UpdateAllActors(.1f);
                actors.UpdateActor(.1f, "drone", control.Actor);
            }
        }
        timer scaleLimiter = new timer(new TimeSpan(0, 0, 1));

        private bool ready = false;
        public override void frameHook(float interpolation)
        {
            TranslateVector_Camera.z -= MoveScale_Camera_forwardback * (interpolation + 1);
            TranslateVector_Camera.x -= MoveScale_Camera_leftright * (interpolation + 1);
            TranslateVector_Camera.y += MoveScale_Camera_updown * (interpolation + 1);
            //if (MoveScale_Camera_updown != 0)
            //{
            //    float s = MoveScale_Camera_updown * (interpolation + 1);
            //    TranslateVector_Camera.y += s;
            //    MoveScale_Camera_updown -= s;
            //}
            try
            {
                //Mogre.Vector3 translateTo = OgreWindow.Instance.cameraYawNode.Orientation * OgreWindow.Instance.cameraPitchNode.Orientation * TranslateVector_Camera;
                if (TranslateVector_Camera.x != 0f || TranslateVector_Camera.y != 0f || TranslateVector_Camera.z != 0f)
                {
                    updateCam();

                    Mogre.Vector3 loc = nodes["drone"]._getDerivedOrientation() * TranslateVector_Camera;

                    //nodes["drone"].Translate(nodes["drone"]._getDerivedOrientation() * TranslateVector_Camera); //limited to x/z - works
                    //actors["drone"].actor.AddLocalForce(loc);
                    //if (actors["drone"].actor.IsSleeping) actors["drone"].actor.WakeUp(.1f);
                    //actors["drone"].actor.LinearVelocity = loc; //kinda works

                    // actors["drone"].actor.MoveGlobalPosition(actors["drone"].actor.GlobalPosition + loc);

                }
                TranslateVector_Camera = new Mogre.Vector3();
            }
            catch
            {
            }
        }
        private Random ran = new Random((int)DateTime.Now.Ticks);


        ExtraMegaBlob.References.timer t = new ExtraMegaBlob.References.timer(new TimeSpan(0, 0, 0, 0, 1000));
    }
}
