using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.PhysX;
using System.Collections;
using MogreFramework;

namespace MogreFramework
{
    public class ActorNode
    {
        public SceneNode sceneNode;
        public Actor actor;

        public ActorNode(SceneNode sceneNode, Actor actor)
        {
            this.sceneNode = sceneNode;
            this.actor = actor;
        }
        internal void Update(float deltaTime)
        {
            if (actor == null) return;
            if (!actor.IsSleeping)
            {
                this.sceneNode.Position = actor.GlobalPosition;
                this.sceneNode.Orientation = actor.GlobalOrientationQuaternion;
            }
        }
        internal void Update2(float deltaTime, Actor a)
        {
            if (a == null) return;
            if (!a.IsSleeping)
            {
                this.sceneNode.Position = a.GlobalPosition;
                this.sceneNode.Orientation = a.GlobalOrientationQuaternion;
            }
        }
    }
    public sealed class ActorNodes : IEnumerable
    {
        private ArrayList allActorNodes = new ArrayList();
        public void UpdateAllActors(float DeltaTime)
        {
            foreach (ActorNode actorNode in allActorNodes)
                actorNode.Update(DeltaTime);
        }
        public void UpdateActor(float DeltaTime, string ActorNodeName, Actor src)
        {
            int i = this.IndexOf(ActorNodeName);
            if (i < 0) return;
            if (allActorNodes[i] == null) return;
            ActorNode actorNode = ((ActorNode)allActorNodes[i]);
            if (actorNode == null) return;
            actorNode.Update2(DeltaTime, src);
        }
        public Boolean Contains(ActorNode a)
        {
            foreach (ActorNode tex in allActorNodes)
            {
                if (tex.sceneNode.Name == a.sceneNode.Name)
                {
                    return true;
                }
            }
            return false;
        }
        public ActorNode this[int index]
        {
            get
            {
                return (ActorNode)allActorNodes[index];
            }
            set
            {
                allActorNodes[index] = value;
            }
        }
        public ActorNode this[String key]
        {
            get
            {
                int i = IndexOf(key);
                if (i < 0)
                    throw new ArgumentOutOfRangeException("key", "\"" + key + "\" is not a valid ActorNode");
                return (ActorNode)allActorNodes[i];
            }
            set
            {
                int i = IndexOf(key);
                if (i < 0)
                    throw new ArgumentOutOfRangeException("key", "\"" + key + "\" is not a valid ActorNode");
                allActorNodes[i] = value;
            }
        }
        public ActorNodes()
        {
            allActorNodes = new ArrayList();
        }
        public int Add(ActorNode sn)
        {
            if (IndexOf(sn.sceneNode.Name) > 0) throw new InvalidOperationException("A ActorNode with the name \"" + sn.sceneNode.Name + "\" already exists.");
            lock (allActorNodes)
            {
                return allActorNodes.Add(((ActorNode)sn));
            }
        }
        public int IndexOf(String Name)
        {
            for (int i = 0; i < allActorNodes.Count; i++)
            {
                if (((ActorNode)allActorNodes[i]).sceneNode.Name == Name)
                {
                    return i;
                }
            }
            return -1;
        }
        public void RemoveAt(int i)
        {
            lock (allActorNodes)
            {
                allActorNodes.RemoveAt(i);
            }
        }
        public void RemoveAt(string sceneNodeName)
        {
            RemoveAt(IndexOf(sceneNodeName));
        }
        public Boolean keyExists(String Name)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (((ActorNode)allActorNodes[i]).sceneNode.Name == Name)
                {
                    return true;
                }
            }
            return false;
        }
        public int Count
        {
            get
            {
                return allActorNodes.Count;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ActorNodeEnumerator(allActorNodes);
        }
        public class ActorNodeEnumerator : IEnumerator
        {
            private int cursor = -1;
            private ArrayList _ActorNodes = null;
            public ActorNodeEnumerator(ArrayList ActorNodes)
            {
                _ActorNodes = ActorNodes;
            }
            public bool MoveNext()
            {
                cursor++;
                return (cursor < _ActorNodes.Count);
            }
            public void Reset()
            {
                cursor = -1;
            }
            public object Current
            {
                get
                {
                    try
                    {
                        return _ActorNodes[cursor];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException("Index out of bounds.");
                    }
                }
            }
        }
        public void shutdown()
        {
            for (int i = 0; i < allActorNodes.Count; i++)
            {
                ((ActorNode)allActorNodes[i]).actor.Dispose();
            }
            while (this.Count > 0)
            {
                this.RemoveAt(0);
            }
        }
    }
}
