using MogreFramework;
using MOIS;
using ExtraMegaBlob.References;
using System;
namespace ExtraMegaBlob
{
    public class plugin : ExtraMegaBlob.References.ClientPlugin
    {
        public override void startup()
        {
            log("starting up");
            OgreWindow.g_m.MouseMoved += new MouseListener.MouseMovedHandler(g_m_MouseMoved);
            middlemousetimer.reset();
            middlemousetimer.start();
        }
        public override void shutdown()
        {
            log("shutting down!");
        }
        public override ExtraMegaBlob.References.Vector3 Location()
        {
            return new ExtraMegaBlob.References.Vector3(-454.8301f, 9.800894f, 322.5049f);
        }
        public override float Radius()
        {
            return 30;
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

        private timer middlemousetimer = new timer(mmbClutch);
        public override void updateHook()
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
            }
            catch { OgreWindow.Instance.log("couldn't wire up camera input"); }
            #region gui updates
            Mogre.Vector3 pos = OgreWindow.Instance.mCamera.Position;


            OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.label1, "X: " + pos.x.ToString("N"));
            OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.label2, "Y: " + pos.y.ToString("N"));
            OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.label3, "Z: " + pos.z.ToString("N"));
            OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.label4, "S1: " + MoveScale_Camera_forwardback.ToString("N") + " S2: " + MoveScale_Camera_leftright.ToString("N"));

            OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.textBox1, string.Format("{0}f, {1}f, {2}f", pos.x, pos.y, pos.z));

            #endregion
        }
        private enum middleMouseStates
        {
            scrollup,
            scrolldown,
            idle
        }
        private middleMouseStates middleMouseState = middleMouseStates.idle;
        public override void frameHook(float interpolation)
        {
            TranslateVector_Camera.z += MoveScale_Camera_forwardback * (interpolation + 1);
            TranslateVector_Camera.x += MoveScale_Camera_leftright * (interpolation + 1);
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
                    //OgreWindow.Instance.cameraNode.Translate(translateTo);
                    movePlayer(TranslateVector_Camera);
                    //OgreWindow.Instance.cameraNode.Orientation.
                }
                TranslateVector_Camera = new Mogre.Vector3();
            }
            catch
            {
            }
        }
        private void movePlayer(Mogre.Vector3 loc)
        {
            ExtraMegaBlob.References.Vector3 loc2 = ExtraMegaBlob.References.Vector3.FromMogre(loc);
            string locSerialized = loc2.ToString();
            Memories mems = new Memories();
            mems.Add(new Memory("loc", KeyWord.NIL, locSerialized, null));
            Event ev = new Event();
            ev._Keyword = KeyWord.MOVEPLAYER;
            ev._Memories = mems;
            ev._IntendedRecipients = EventTransfer.CLIENTTOCLIENT;
            base.outboxMessage(this, ev);
        }
        private float MoveScale_Camera_forwardback = 0f;
        private float MoveScale_Camera_leftright = 0f;
        private float MoveScale_Camera_updown = 0f;
        private Mogre.Vector3 TranslateVector_Camera = new Mogre.Vector3();
        private const float speedcap_forwardback = .15f;
        private const float speedcap_leftright = .15f;
        private const float speedcap_updown = .5f;
        private const float incr_forwardback = .0005f;
        private const float incr_leftright = .0005f;
        private const float incr_updown = .0001f;
        private const float brakes_updown = incr_updown * 2;
        private const float brakes_forwardback = incr_forwardback * 2;
        private const float brakes_leftright = incr_leftright * 2;
        private static TimeSpan mmbClutch = new TimeSpan(0, 0, 0, 0, 100);
        private bool g_m_MouseMoved(MouseEvent arg)
        {
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
    }
}
