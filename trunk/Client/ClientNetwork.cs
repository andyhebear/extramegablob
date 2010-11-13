using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using thing.Parts;
using System.IO;
using ThingReferences;
using System.Collections;

namespace thing
{
    class NetworkClient
    {
        
        TcpClient tcpclnt = new TcpClient();

        public bool connect()
        {
            if (connected) { log("Already Connected."); return false; }
            tcpclnt = new TcpClient();

            log("Connecting to [ " + serverip + ":" + serverport + " ]");
            int connectTries = 0;
            while (true)
            {
                if (connectTries > 0)
                {
                    log("Tried to connect " + connectTries.ToString() + " times, pausing 60 seconds");
                    try
                    {
                        tcpclnt.Close();
                    }
                    catch (Exception ex) { log(ex.Message); }
                    return false;
                }
                try
                {

                    connectTries++;
                    tcpclnt.Connect(serverip, int.Parse(serverport));
                    if (tcpclnt.Connected)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    log(ex.Message);
                }
                Thread.Sleep(100);
            }
            log("Connected to [ " + serverip + ":" + serverport + " ]");
            _connected = true;
            listenThread = new Thread(new ThreadStart(listenloop));
            listenThread.Start();
            sender_thread1 = new Thread(new ThreadStart(senderLoop1));
            sender_thread1.Start();
            return true;
        }
     
        private void listenloop()
        {
            DateTime stt = DateTime.Now;
            log("ReceiveBufferSize: " + tcpclnt.ReceiveBufferSize);
            while (connected && !this._shutdown)
            {
                Thread.Sleep(100);
                byte[] bb = new byte[1];
                string s = string.Empty;
                while (tcpclnt.Available > 0)
                {
                    Stream stm = tcpclnt.GetStream();
                    long x = 0;
                    bb = new byte[1];
                    int k = stm.Read(bb, 0, 1);
                    
                    for (int i = 0; i < k; i++)
                    {
                        s += Convert.ToChar(bb[i]);
                        if (s.Contains(delim))
                        {
                            s = s.Replace(delim, "");
                            string[] break1 = s.Split('|');
                            if (break1.Length < 3) { log("corrupted packet, x: " + x.ToString() + " k: " + k.ToString()); s = ""; break; }
                            string networkKeyId = break1[0];
                            string salt = break1[1];
                            string cipherText = break1[2];
                            string netKey = networkKey;
                            string decrypted = enc.Decrypt(salt + "|" + cipherText, netKey);
                            if (decrypted == "")
                            {
                                log("Failed to decrypt network message.");
                                break;
                            }
                            decrypted = Helpers.fromBase64_UTF8(decrypted);
                            Event ev = Event.FromString(decrypted);
                            receiveEvent(ev);
                            s = "";
                        }
                    }
                }
            }
        }



        private Thread listenThread;
       

        public bool connected { get { return _connected; } }
        

        private Thread sender_thread1 = null;
        private bool senderDisabled = false;
        public void senderLoop1()
        {
            log("sender thread 1 running");
            try
            {
                while (!this._shutdown)
                {
                    Thread.Sleep(100);
                    if (outQueue.Count > 0)
                    {
                        int pendingCount = outQueue.Count;
                        for (int i = 0; i < pendingCount; i++)
                        {
                            Event ev = (Event)outQueue[i];
                            sendEvent(ev);
                            outQueue.RemoveAt(i);
                            pendingCount = outQueue.Count;
                        }
                    }
                }
            }
            finally
            {
                senderDisabled = true;
                log("sender thread 1 shutting down");
            }
        }
       
    }
}
