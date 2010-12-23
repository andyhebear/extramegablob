using MogreFramework;
using MOIS;
namespace ExtraMegaBlob
{
    public class plugin : ExtraMegaBlob.References.ClientPlugin
    {
        public override void startup()
        {
            log("starting up");
            OgreWindow.g_m.MouseMoved += new MouseListener.MouseMovedHandler(g_m_MouseMoved);
        }
        public override void shutdown()
        {
            log("shutting down!");
        }
        public override ExtraMegaBlob.References.Vector3 Location()
        {
            return new ExtraMegaBlob.References.Vector3(43, 0, 15);
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
        public override void updateHook()
        {
            try
            {
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
            }
            catch { OgreWindow.Instance.log("couldn't wire up camera input"); }
            #region gui updates
            Mogre.Vector3 pos = OgreWindow.Instance.cameraNode.Position;


            OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.label1, "X: " + pos.x.ToString("N"));
            OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.label2, "Y: " + pos.y.ToString("N"));
            OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.label3, "Z: " + pos.z.ToString("N"));
            OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.label4, "S1: " + MoveScale_Camera_forwardback.ToString("N") + " S2: " + MoveScale_Camera_leftright.ToString("N"));

            OgreWindow.Instance.updateCoords(OgreWindow.UI_ELEMENT.textBox1, string.Format("x:{0} y:{1} z:{2}", pos.x, pos.y, pos.z));

            #endregion
        }
        public override void frameHook(float interpolation)
        {
            TranslateVector_Camera.z += MoveScale_Camera_forwardback * (interpolation + 1);
            TranslateVector_Camera.x += MoveScale_Camera_leftright * (interpolation + 1);
            if (MoveScale_Camera_updown != 0)
            {
                float s = MoveScale_Camera_updown * (interpolation + 1);
                TranslateVector_Camera.y += s;
                MoveScale_Camera_updown -= s;
            }
            try
            {
                OgreWindow.Instance.cameraNode.Translate(OgreWindow.Instance.cameraYawNode.Orientation * OgreWindow.Instance.cameraPitchNode.Orientation * TranslateVector_Camera);
                TranslateVector_Camera = new Mogre.Vector3();
            }
            catch
            {
            }
        }
        private float RotateScale_Camera = .001f;//mouse sensitivity
        private float MoveScale_Camera_forwardback = 0f;
        private float MoveScale_Camera_leftright = 0f;
        private float MoveScale_Camera_updown = 0f;
        private Mogre.Vector3 TranslateVector_Camera = new Mogre.Vector3();
        private const float speedcap_forwardback = .15f;
        private const float speedcap_leftright = .15f;
        private const float incr_forwardback = .0005f;
        private const float incr_leftright = .0005f;
        private const float brakes_forwardback = incr_forwardback * 2;
        private const float brakes_leftright = incr_leftright * 2;
        private bool g_m_MouseMoved(MouseEvent arg)
        {
            MouseState_NativePtr s = arg.state;
            if (arg.state.buttons == 2)
            {
                OgreWindow.Instance.cameraYawNode.Yaw(-s.X.rel * RotateScale_Camera);
                OgreWindow.Instance.cameraRollNode.Pitch(-s.Y.rel * RotateScale_Camera);
            }
            Mogre.Vector3 oldpos = OgreWindow.Instance.cameraNode.Position;
            float mouseZ = (float)arg.state.Z.rel * .1f;
            if (0 != mouseZ)
            {
                MoveScale_Camera_updown -= mouseZ;
            }
            return true;
        }
    }
}
