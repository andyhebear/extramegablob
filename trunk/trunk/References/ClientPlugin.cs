using MogreFramework;
using Mogre;
using Mogre.PhysX;
using MOIS;
namespace ExtraMegaBlob.References
{
    public abstract class ClientPlugin
    {
        public abstract void startup();
        public abstract void shutdown();
        public abstract string[] AllowedOutputNames();
        public abstract string[] AllowedInputNames();
        public abstract string Name();
        public abstract void frameHook(float interpolation);
        public abstract void updateHook();
        public event LogDelegate onQuit;
        public void quit(string msg)
        {
            if (!object.Equals(null, this.onQuit))
            {
                onQuit(this.Name() + msg);
            }
        }
        public delegate void outboxDelegate(ClientPlugin Sender, Event ev);
        public event outboxDelegate onOutboxMessage;
        public void outboxMessage(ClientPlugin Sender, Event ev)
        {
            ev._Source_FullyQualifiedName = Sender.Name();
            if (!object.Equals(null, this.onOutboxMessage))
            {
                onOutboxMessage(Sender, ev);
            }
        }
        public event LogDelegate onChat;
        public void chat(string msg)
        {
            if (!object.Equals(null, this.onChat))
            {
                onChat(msg);
            }
        }
        public event LogDelegate onLog;
        public void log(string msg)
        {
            if (!object.Equals(null, this.onLog))
            {
                onLog(msg);
            }
        }
        public abstract void inbox(Event ev);
        public abstract ExtraMegaBlob.References.Vector3 Location();
        #region Globals
        public SceneNodes nodes = OgreWindow.Instance.nodes;
        public ActorNodes actors = OgreWindow.Instance.actors;
        public Entities entities = OgreWindow.Instance.entities;
        public Lights lights = OgreWindow.Instance.lights;
        public Materials materials = OgreWindow.Instance.materials;
        public Meshes meshes = OgreWindow.Instance.meshes;
        public Skeletons skeletons = OgreWindow.Instance.skeletons;
        public Textures textures = OgreWindow.Instance.textures;
        public Physics physics = OgreWindow.Instance.physics;
        public Scene scene = OgreWindow.Instance.scene;
        public InputManager inputmanager = OgreWindow.Instance.g_InputManager;
        public Keyboard keyboard = OgreWindow.g_kb;
        public Mouse mouse = OgreWindow.g_m;
        public JoyStick[] joys = OgreWindow.g_joys;
        #endregion
    }
}
