using System;
using System.Windows.Forms;
using Mogre;
using MogreFramework;
using MOIS;
using System.Threading;
using System.IO;
using System.Collections;
using ExtraMegaBlob.References;
namespace ExtraMegaBlob.Client
{

    public partial class Simulation
    {
        Entity ground_ent = null;
        SceneNode ground_node = null;
        void mainwindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            quit();
        }
        private float _trim2 = 0.0f;
        private float trim2
        {
            get
            {
                return _trim2;
            }
            set
            {
                //if (trim2changed.AddTicks(1000).CompareTo(DateTime.Now) > 0)
                // {
                _trim2 = value;
                //     trim2changed = DateTime.Now;
                // }
            }
        }
        private DateTime trim2changed = DateTime.Now;
        float RotateScale_Camera = .001f;//mouse sensitivity
        private DateTime lastFrame = DateTime.Now;
        timer saveTimer = new timer(new TimeSpan(0, 0, 0, 1));
        private bool takeScreenshotInsideFrameEnded()
        {
            bool retVal = true;
            try
            {
                if (saveTimer.elapsed)
                {
                    saveTimer.start();
                    try
                    {
                        if (OgreWindow.g_kb.IsKeyDown(MOIS.KeyCode.KC_T))
                        {
                            OgreWindow.Instance.saveFrame(SaveFrameFile);
                        }
                    }
                    catch { OgreWindow.Instance.log("takeScreenshotInsideFrameEnded: couldnt wire up controls"); }
                }
            }
            catch
            {
                retVal = false;
            }
            return retVal;
        }
        private string SaveFrameFile
        {
            get
            {
                for (int i = 0; i > -1; i++)
                {
                    string part1 = Path.GetDirectoryName(Application.ExecutablePath) + "\\Screenshots\\frame";
                    string part2 = i.ToString("D3");
                    string part3 = ".jpg";
                    string cmp = part1 + part2 + part3;
                    if (!File.Exists(cmp))
                    {
                        return cmp;
                    }
                }
                return null;
            }
        }
        private string orientationFile { get { return Path.GetDirectoryName(Application.ExecutablePath) + "\\orientation.txt"; } }
        void writeOrientationFile(string s)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(orientationFile, false);
            sw.WriteLine(s);
            sw.WriteLine("");
            sw.Close();
        }
        float[] readOrientationFile()
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(orientationFile);
            string s = sr.ReadLine();
            sr.Close();
            string[] x = s.Split('|');
            float[] retVal = new float[4];
            retVal[0] = float.Parse(x[0]);
            retVal[1] = float.Parse(x[1]);
            retVal[2] = float.Parse(x[2]);
            retVal[3] = float.Parse(x[3]);
            return retVal;
        }
        private DateTime camPanModeLastSet = DateTime.Now;
        private bool _cameraPanMode = false;
        private bool cameraPanMode
        {
            set
            {
                if (camPanModeLastSet.AddSeconds(1).CompareTo(DateTime.Now) > 0)
                {
                    if (!_cameraPanMode)
                    {
                        _cameraPanMode = true;
                    }
                    else
                    {
                        _cameraPanMode = false;
                    }
                    camPanModeLastSet = DateTime.Now;
                }
            }
            get
            {
                return _cameraPanMode;
            }
        }
    }
}