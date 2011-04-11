using MogreFramework;
using Mogre;
using Mogre.PhysX;
using MOIS;
using System.Collections;
using System.Threading;
using System;
namespace ExtraMegaBlob.References
{
    public abstract class ClientPlugin
    {
        public virtual Hashtable materials_lookup() { return new Hashtable(); }
        public virtual Hashtable meshes_lookup() { return new Hashtable(); }
        public virtual Hashtable skeletons_lookup() { return new Hashtable(); }
        public bool ready = false;
        public void startup()
        {
            new Thread(new ThreadStart(resourceWaitThread)).Start();
        }
        public abstract void shutdown();
        public abstract string[] AllowedOutputNames();
        public abstract string[] AllowedInputNames();
        public abstract string Name();
        public abstract void frameHook(float interpolation);
        public abstract void updateHook();
        public void tryupdateHook()
        {
            try { updateHook(); }
            catch (Exception ex) { log(ex.ToString()); }
        }
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
        #region Helper Functions
        public void preventMousePick(string name)
        {
            Memories mems = new Memories();
            mems.Add(new Memory("Name", KeyWord.NIL, name, null));
            Event ev = new Event();
            ev._Keyword = KeyWord.PREVENTMOUSEPICK;
            ev._Memories = mems;
            ev._IntendedRecipients = EventTransfer.CLIENTTOCLIENT;
            this.outboxMessage(this, ev);
        }
        public void freezePlayer()
        {
            this.chat("you are now seated");
            Event outevent = new Event();
            outevent._Keyword = KeyWord.PLAYER_FREEZE;
            outevent._IntendedRecipients = EventTransfer.CLIENTTOCLIENT;
            outboxMessage(this, outevent);
        }
        public void unfreezePlayer()
        {
            this.chat("you are now standing");
            Event outevent = new Event();
            outevent._Keyword = KeyWord.PLAYER_UNFREEZE;
            outevent._IntendedRecipients = EventTransfer.CLIENTTOCLIENT;
            outboxMessage(this, outevent);
        }
        public void resetPlayer(Mogre.Vector3 loc, Mogre.Quaternion pose)
        {
            Event outevent = new Event();
            outevent._Keyword = KeyWord.PLAYER_RESET;
            outevent._IntendedRecipients = EventTransfer.CLIENTTOCLIENT;
            outevent._Memories = new Memories();
            outevent._Memories.Add(new Memory("", KeyWord.DATA_QUATERNION_W, pose.w.ToString()));
            outevent._Memories.Add(new Memory("", KeyWord.DATA_QUATERNION_X, pose.x.ToString()));
            outevent._Memories.Add(new Memory("", KeyWord.DATA_QUATERNION_Y, pose.y.ToString()));
            outevent._Memories.Add(new Memory("", KeyWord.DATA_QUATERNION_Z, pose.z.ToString()));
            outevent._Memories.Add(new Memory("", KeyWord.DATA_VECTOR3_X, loc.x.ToString()));
            outevent._Memories.Add(new Memory("", KeyWord.DATA_VECTOR3_Y, loc.y.ToString()));
            outevent._Memories.Add(new Memory("", KeyWord.DATA_VECTOR3_Z, loc.z.ToString()));
            this.outboxMessage(this, outevent);
        }
        public void logConsole(string msg)
        {
            OgreWindow.Instance.logConsole(msg);
        }
        public void log(string msg)
        {
            if (!object.Equals(null, this.onLog))
            {
                onLog(msg);
            }
        }
        private void resourceWaitThread()
        {
            while (true)
            {
                Thread.Sleep(1000);
                foreach (DictionaryEntry de in materials_lookup())
                {
                    if (!TextureManager.Singleton.ResourceExists((string)de.Value)) goto waitmore;
                }
                foreach (DictionaryEntry de in skeletons_lookup())
                {
                    if (!SkeletonManager.Singleton.ResourceExists((string)de.Value)) goto waitmore;
                }
                foreach (DictionaryEntry de in meshes_lookup())
                {
                    if (!MeshManager.Singleton.ResourceExists((string)de.Value)) goto waitmore;
                }
                if (!OgreWindow.Instance.SceneReady) goto waitmore;
                break;
            waitmore:
                continue;
            }
            init2();
        }
        public void init2()
        {
            log("starting up!");
            OgreWindow.Instance.pause();
            try
            {
                foreach (DictionaryEntry mat in materials_lookup())
                {
                    materials.Add((MaterialPtr)MaterialManager.Singleton.Create((string)mat.Key, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME));
                    materials[(string)mat.Key].GetTechnique(0).GetPass(0).CreateTextureUnitState((string)mat.Value);
                }
            }
            catch (Exception ex)
            {
                log(ex.ToString());
            }
            init();
            init3();
        }
        public abstract void init();
        private void init3()
        {
            OgreWindow.Instance.unpause();
            log("done starting up!");
            ready = true;
        }
        #endregion
    }
}
