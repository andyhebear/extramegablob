using System;
using System.Collections;
using System.Windows.Forms;
using Mogre;
using MogreFramework;
namespace ExtraMegaBlob
{
    public class plugin : ExtraMegaBlob.References.ClientPlugin
    {
        public override void startup()
        {
            log("starting up!");
            OgreWindow.Instance.renderBox.MouseClick += new MouseEventHandler(renderBox_MouseClick);
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
            return "MousePick_Client";
        }
        public override string[] AllowedInputNames()
        {
            return new string[] {};
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] {};
        }
        public override void inbox(ExtraMegaBlob.References.Event ev)
        {
        }
        public override void updateHook()
        {
        }
        public override void frameHook(float interpolation)
        {
        }
        private ArrayList selectedNodes = new ArrayList();
        private void renderBox_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (MovableObject selectedNode in selectedNodes)
            {
                try
                {
                    selectedNode.ParentSceneNode.ShowBoundingBox = false;
                }
                catch (Exception ex)
                {
                    log(ex.ToString());
                }
            }
            selectedNodes = new ArrayList();
            float scrx = (float)e.X / OgreWindow.Instance.mViewport.ActualWidth;
            float scry = (float)e.Y / OgreWindow.Instance.mViewport.ActualHeight;
            Ray ray = OgreWindow.Instance.mCamera.GetCameraToViewportRay(scrx, scry);
            RaySceneQuery query = OgreWindow.Instance.mSceneMgr.CreateRayQuery(ray);
            RaySceneQueryResult results = query.Execute();

            //chat(results.Count.ToString());
            foreach (RaySceneQueryResultEntry entry in results)
            {
                if (entry.movable.Name == "MainCamera") continue;
                //chat(entry.movable.Name);
                entry.movable.ParentSceneNode.ShowBoundingBox = true;
                selectedNodes.Add(entry.movable);
            }
        }
    }
}
