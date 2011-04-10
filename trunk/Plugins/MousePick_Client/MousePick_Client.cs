using System;
using System.Collections;
using System.Windows.Forms;
using Mogre;
using MogreFramework;
namespace ExtraMegaBlob
{
    public class plugin : ExtraMegaBlob.References.ClientPlugin
    {
        public override void init()
        {
            OgreWindow.Instance.renderBox.MouseClick += new MouseEventHandler(renderBox_MouseClick);
        }
        public override void shutdown()
        {
            log("shutting down!");
        }
        public override ExtraMegaBlob.References.Vector3 Location()
        {
            return new ExtraMegaBlob.References.Vector3(163, 0, 15);
        }
        public override string Name()
        {
            return "MousePick_Client";
        }
        public override string[] AllowedInputNames()
        {
            return new string[] { "TongIts_Client" };
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] { };
        }
        private ArrayList preventSelect = new ArrayList();
        public override void inbox(ExtraMegaBlob.References.Event ev)
        {
            if (ev._Keyword == References.KeyWord.PREVENTMOUSEPICK)
            {
                string name = ev._Memories["Name"].Value;
                if (!preventSelect.Contains(name))
                {
                    preventSelect.Add(name);
                    log("Disabling Mouse Picker for: \"" + name + "\"");
                }
            }
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
            if (e.Button != MouseButtons.Left) return;
            foreach (MovableObject selectedNode in selectedNodes)
            {
                try
                {
                    selectedNode.ParentSceneNode.ShowBoundingBox = false;
                }
                catch (Exception ex)
                {
                    //log(ex.ToString());
                }
            }
            selectedNodes = new ArrayList();
            float scrx = (float)e.X / OgreWindow.Instance.mViewport.ActualWidth;
            float scry = (float)e.Y / OgreWindow.Instance.mViewport.ActualHeight;
            Ray ray = OgreWindow.Instance.mCamera.GetCameraToViewportRay(scrx, scry);
            RaySceneQuery query = OgreWindow.Instance.mSceneMgr.CreateRayQuery(ray);
            RaySceneQueryResult results = query.Execute();
            float nearest = 100000000f; //100 mill
            int nearestIndex = -1;


            for (int i = 0; i < results.Count; i++)
            {
                RaySceneQueryResultEntry entry = results[i];
                if (entry.movable.Name == "MainCamera") continue;
                if (entry.movable.Name.IndexOf("SphereEntity") > -1) continue;
                if (entry.movable.Name.IndexOf("SkyXMeshEnt") > -1) continue;
                if (entry.movable.Name.IndexOf("_Hydrax_GodRays_ManualObject") > -1) continue;
                if (entry.movable.Name.IndexOf("_Hydrax_GodRays_Projector_Camera") > -1) continue;
                if (entry.movable.Name.IndexOf("_Hydrax_Projector_Camera") > -1) continue;
                if (entry.movable.Name.IndexOf("HydraxMeshEnt") > -1) continue;

                bool cancel = false;
                foreach (string s in preventSelect)
                {
                    if (entry.movable.Name.IndexOf(s) > -1)
                    {
                        cancel = true;
                        break;
                    }
                }
                if (cancel) continue;
                if (entry.distance < nearest)
                {
                    nearest = entry.distance;
                    nearestIndex = i;
                }
            }
            if (nearestIndex > -1)
            {
                RaySceneQueryResultEntry entry = results[nearestIndex];
                selectSceneNode(entry.movable);
            }
        }
        private void selectSceneNode(MovableObject moveableObject)
        {
            chat(moveableObject.Name);
            moveableObject.ParentSceneNode.ShowBoundingBox = true;
            selectedNodes.Add(moveableObject);
        }

    }
}
