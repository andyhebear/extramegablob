using System;
using System.Collections;
using System.Windows.Forms;
using Mogre;
using MogreFramework;
using ExtraMegaBlob.References;
using System.Drawing;
namespace ExtraMegaBlob
{
    public class plugin : ExtraMegaBlob.References.ClientPlugin
    {
        public void sendCap(byte[] capFrameBytes)
        {
            Memories mems = new Memories();
            mems.Add(new Memory("", KeyWord.CAMCAP_JPG, "", capFrameBytes));
            Event ev = new Event();
            ev._Keyword = KeyWord.CAMCAP;
            ev._Memories = mems;
            ev._IntendedRecipients = EventTransfer.CLIENTTOSERVER;
            base.outboxMessage(this, ev);
        }
        private static TimeSpan captureInterval = new TimeSpan(0, 0, 0, 0, 1000);
        private timer captureTimer = new timer(captureInterval);
        private string capTexture = "webcapCapture";
        public override void startup()
        {
            log("starting up!");
            if (!TextureManager.Singleton.ResourceExists(capTexture))
            {
                //OgreWindow.Instance.textures.Add(TextureManager.Singleton.CreateManual(capTexture, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                //    TextureType.TEX_TYPE_2D, 640, 480, 0, PixelFormat.PF_BYTE_BGR,
                //    (int)Mogre.TextureUsage.TU_DYNAMIC, null, false, 0));

                OgreWindow.Instance.textures.Add(TextureManager.Singleton.CreateManual(capTexture, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                    TextureType.TEX_TYPE_2D, 640, 480, 0, PixelFormat.PF_BYTE_BGR));
            }
        }
        public override void shutdown()
        {
            log("shutting down!");
        }
        public override ExtraMegaBlob.References.Vector3 Location()
        {
            return new ExtraMegaBlob.References.Vector3(163, -450, 1500);
        }
        public override float Radius()
        {
            return 30;
        }
        public override string Name()
        {
            return "VideoCapture_Client";
        }
        public override string[] AllowedInputNames()
        {
            return new string[] { };
        }
        public override string[] AllowedOutputNames()
        {
            return new string[] { };
        }
        public override void inbox(ExtraMegaBlob.References.Event ev)
        {
        }
        public override void updateHook()
        {
            if (captureTimer.elapsed)
            {
                Textures textures = OgreWindow.Instance.textures;
                //byte[] capFrameBytes = OgreWindow.Instance.getCapSerialized(OgreWindow.imgFmt.JPG);
                Bitmap bmp = OgreWindow.Instance.getCap();
                if (bmp == null) goto end;
                byte[] rgbData = textures.ConvertImageToRgbValues(bmp);
                textures.Replace2(rgbData, capTexture);
                //OgreWindow.Instance.setCapStatusImage(capFrameBytes);
                //sendCap(capFrameBytes);
                //OgreWindow.Instance.textures.Replace2(capFrameBytes, capTexture);
                end:
                captureTimer.start();
            }
        }
        //private void updateThread()
        //{
        //    while (running)
        //    {
        //        Thread.Sleep(100);
        //        if (captureTimer.elapsed)
        //        {
        //            Textures textures = OgreWindow.Instance.textures;
        //            //byte[] capFrameBytes = OgreWindow.Instance.getCapSerialized(OgreWindow.imgFmt.JPG);
        //            Bitmap bmp = OgreWindow.Instance.getCap2();
        //            if (bmp != null)
        //            {
        //                byte[] rgbData = textures.ConvertImageToRgbValues(bmp);
        //                textures.Replace2(rgbData, capTexture);
        //                //OgreWindow.Instance.setCapStatusImage(capFrameBytes);
        //                //sendCap(capFrameBytes);
        //                //OgreWindow.Instance.textures.Replace2(capFrameBytes, capTexture);
        //            }
        //            captureTimer.start();
        //        }
        //    }
        //}
        //new Thread(new ThreadStart(updateThread)).Start();
        public override void frameHook(float interpolation)
        {
        }
    }
}
