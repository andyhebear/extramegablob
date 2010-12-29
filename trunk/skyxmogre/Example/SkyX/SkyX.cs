using System;
using System.Collections.Generic;
using System.Text;

using SkyX;

namespace Mogre.Demo.SkyX
{
    class SkyX : Mogre.Demo.ExampleApplication.Example
    {
        protected SkyManager manager;
        protected TextAreaOverlayElement textArea;
        protected bool showInformation;

        private string TerrainMaterialName = "Terrain";

        public override void ChooseSceneManager()
        {
            // Get the SceneManager, in this case a generic one
            sceneMgr = root.CreateSceneManager(SceneType.ST_EXTERIOR_CLOSE, "SceneMgr");
        }
        public override void CreateScene()
        {
            // Set some camera params
            camera.FarClipDistance = 30000;
            camera.NearClipDistance = 20;

            camera.SetPosition(20000, 500, 20000);
            camera.SetDirection(1, 0, 0);

            // Create our text area for display SkyX parameters
            CreateTextArea();

            manager = new SkyManager(base.sceneMgr, base.camera);
            manager.Create();

            // Add our ground atmospheric scattering pass to terrain material
            MaterialPtr material = MaterialManager.Singleton.GetByName(TerrainMaterialName);

            manager.GPUManager.AddGroundPass(material.GetTechnique(0).CreatePass(),5000,SceneBlendType.SBT_TRANSPARENT_COLOUR);

            // Create our terrain
            sceneMgr.SetWorldGeometry("Terrain.cfg");

            // Add a basic cloud layer
            manager.CloudsManager.Add(new CloudLayer.LayerOptions());

            //Add frame evnet
            root.FrameStarted += new FrameListener.FrameStartedHandler(FrameStarted);
        }

        protected override void HandleInput(FrameEvent evt)
        {
            base.HandleInput(evt);

            // Show/Hide information

		    if (inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_F1))
            {
                showInformation = !showInformation;

		    }

            if (inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_1) && !(inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_LSHIFT) || inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_RSHIFT)))
            {
                manager.TimeMultiplier = 1.0f;
            }
            if (inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_1) && (inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_LSHIFT) || inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_RSHIFT)))
            {
                manager.TimeMultiplier = -1.0f;
            }


        }
        private bool FrameStarted(FrameEvent evt)
        {
            // Check camera height
            RaySceneQuery raySceneQuery = sceneMgr.CreateRayQuery(new Ray(camera.Position + new Vector3(0, 1000000, 0), Vector3.NEGATIVE_UNIT_Y));
            RaySceneQueryResult qryResult = raySceneQuery.Execute();

            RaySceneQueryResult.Iterator it = qryResult.Begin();
            if (it != qryResult.End() && it.Value.worldFragment != null)
            {
                if (camera.DerivedPosition.y < it.Value.worldFragment.singleIntersection.y + 30)
                {
                    camera.SetPosition(camera.Position.x,
                                        it.Value.worldFragment.singleIntersection.y + 30,
                                        camera.Position.z);
                }

                it.MoveNext();
            }

            //SkyX::AtmosphereManager::Options SkyXOptions = mSkyX->getAtmosphereManager()->getOptions();

            // Time
            if (!showInformation)
            {
                manager.TimeMultiplier = 0.1f;
            }
            else
            {
                manager.TimeMultiplier = 0.0f;
            }

            textArea.Caption = GetConfigString();
            manager.Update(evt.timeSinceLastFrame);

            return true;
        }

        // Create text area for SkyX parameters
        private void CreateTextArea()
	    {
		    // Create a panel
		    OverlayContainer panel = (OverlayContainer)OverlayManager.Singleton.CreateOverlayElement("Panel", "SkyXParametersPanel");
		    panel.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            panel.SetPosition(10,10);
		    panel.SetDimensions(400,400);

		    // Create a text area
		    textArea = (TextAreaOverlayElement)OverlayManager.Singleton.CreateOverlayElement("TextArea", "SkyXParametersTextArea");
		    textArea.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            textArea.SetPosition(0,0);
		    textArea.SetDimensions(100,100);
		    textArea.Caption = "SkyX plugin demo";
            textArea.CharHeight = 16;
            textArea.FontName = "BlueHighway";
		    textArea.ColourBottom = new ColourValue(0.3f, 0.5f, 0.3f);
            textArea.ColourTop = new ColourValue(0.5f, 0.7f, 0.7f);
		    

		    // Create an overlay, and add the panel
		    Overlay overlay = OverlayManager.Singleton.Create("OverlayName");
		    overlay.Add2D(panel);

		    // Add the text area to the panel
		    panel.AddChild(textArea);

		    // Show the overlay
		    overlay.Show();
	    }

        private String GetConfigString()
        {
            AtmosphereManager atmo = manager.AtmosphereManager;
            int hour = (int)atmo.Time.x;

            int min = (int)((atmo.Time.x - hour) * 60);

            
	        String timeStr = Mogre.StringConverter.ToString(hour) + ":" + Mogre.StringConverter.ToString(min);
            String str = "SkyX Plugin demo (Press F1 to show/hide information)";
            if (showInformation)
            {
                str += " - Simuation paused - \n";
            }
            else
            {
                str += "\n-------------------------------------------------------------\nTime: " + timeStr + "\\n";
            }

	        if (showInformation)
	        {
		        str += "-------------------------------------------------------------\n";
		        str += "Time: " + timeStr + " [1, Shift+1] (+/-).\n";
                str += "Rayleigh multiplier: " + Mogre.StringConverter.ToString(atmo.RayleighMultiplier) + " [2, Shift+2] (+/-).\n";
                str += "Mie multiplier: " + Mogre.StringConverter.ToString(atmo.MieMultiplier) + " [3, Shift+3] (+/-).\n";
                str += "Exposure: " + Mogre.StringConverter.ToString(atmo.Exposure) + " [4, Shift+4] (+/-).\n";
                str += "Inner radius: " + Mogre.StringConverter.ToString(atmo.InnerRadius) + " [5, Shift+5] (+/-).\n";
                str += "Outer radius: " + Mogre.StringConverter.ToString(atmo.OuterRadius) + " [6, Shift+6] (+/-).\n";
                str += "Number of samples: " + Mogre.StringConverter.ToString(atmo.NumberOfSamples) + " [7, Shift+7] (+/-).\n";
                str += "Height position: " + Mogre.StringConverter.ToString(atmo.HeightPosition) + " [8, Shift+8] (+/-).\n";
	        }

	        return str;
        }
    } 
}
