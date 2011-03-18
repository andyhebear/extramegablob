using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
using ExtraMegaBlob.References;
namespace ExtraMegaBlob.Server
{

    class ServerNetwork
    {

        private string path_files { get { return ""; } }

        private static TimeSpan sendTimer { get { return new TimeSpan(0, 0, 0, 0, 100); } }
        public ServerNetwork()
        {
            enc = new Encryption();
        }
        ~ServerNetwork()
        {
        }
        public delegate void LogDelegate(string msg);
        public event LogDelegate onLogMessage;
        private void log(string msg)
        {
            if (!object.Equals(null, this.onLogMessage))
            {
                onLogMessage(msg);
            }
        }
        private Encryption enc = null;
        private bool shutDownListener = false;
        public void shutdown()
        {
            shutDownListener = true;
        }
        private DateTime lastSave = DateTime.Now;
        private bool senderDisabled = false;
        public void mainLoop()
        {
            Thread.CurrentThread.Name = "Network mainLoop";
            log("Listen thread running");
            shutDownListener = false;
            try
            {
                log("Binding to: [" + "0.0.0.0" + ":" + "420" + "]");
                IPAddress ipAd = IPAddress.Parse("0.0.0.0");
                TcpListener myList = new TcpListener(ipAd, int.Parse("420"));
                myList.Start();
                log("Listening for connections on local End point: " + myList.LocalEndpoint);
                while (!shutDownListener)
                {
                    if (myList.Pending()) // Non blocking
                    {
                        try
                        {
                            Socket s = myList.AcceptSocket();
                            if (s.Connected)
                            {
                                Thread t = new Thread(new ParameterizedThreadStart(handleConnection));
                                t.Start(s);
                            }
                        }
                        catch (Exception ex)
                        {
                            log(ex.Message);
                        }
                    }
                    Thread.Sleep(100);
                }
                myList.Stop();
                log("Listener Stopped");
            }
            catch (Exception e)
            {
                log("Error..... " + e.Message);
            }
            log("Listen thread exiting");
        }
        public void sendEvent(Event ev)
        {
            if (senderDisabled)
            {
                log("tried to send without a working sender");
                return;
            }
            if (ev._Endpoint == "")
            {
                foreach (DictionaryEntry de in outQueueRoot)
                {
                    if (null != outQueueRoot[de.Key])
                        ((ArrayList)outQueueRoot[de.Key]).Add(ev);
                }
            }
            else
            {
                string[] endpoints = unique(ev._Endpoint.Split(','));
                foreach (string endp in endpoints)
                {
                    if (null != outQueueRoot[endp])
                        ((ArrayList)outQueueRoot[endp]).Add(ev);
                }
            }
        }
        private string[] unique(string[] notunique)
        {
            ArrayList al = new ArrayList();
            foreach (string endp in notunique)
            {
                if (!al.Contains(endp))
                    al.Add(endp);
            }
            return (string[])al.ToArray(typeof(string));
        }
        public void sendEventFromPlugin(ServerPlugin Sender, Event ev)
        {
            ev._Source_FullyQualifiedName = Sender.Name();
            sendEvent(ev);
        }
        public void queueEvent(Event ev)
        {
            sendEvent(ev);
        }
        public void sendEventAllExcept(Event ev)
        {
            if (senderDisabled)
            {
                //log("tried to send without a working sender");
                return;
            }
            if (ev._Endpoint == "")
            {
                return;
            }
            else
            {
                string[] endpoints = unique(ev._Endpoint.Split(','));
                foreach (string endp in allEndpoints)
                {
                    bool found = false;
                    foreach (string endpoint in endpoints)
                    {
                        if (endpoint == endp)
                            found = true;
                    }
                    if (!found)
                        if (null != outQueueRoot[endp])
                            ((ArrayList)outQueueRoot[endp]).Add(ev);
                }
            }
        }
        private ArrayList allEndpoints = new ArrayList();
        private Hashtable outQueueRoot = new Hashtable();
        private Random ran = new Random((int)DateTime.Now.Ticks);
        private byte[] prepOutgoingEvent(Event ev)
        {
            byte[] data = ev.ToBytes();
            byte[] encrypted = Encryption2.Encrypt(data, Config.rawKey);
            byte[] preamble = BitConverter.GetBytes(encrypted.LongLength);
            byte[] package = new byte[encrypted.Length + 8];
            Buffer.BlockCopy(preamble, 0, package, 0, 8);
            Buffer.BlockCopy(encrypted, 0, package, 8, encrypted.Length);
            return package;
        }
        public string netKey = "";
        public delegate void onReceiveEventDelegate(Event msg);
        public event onReceiveEventDelegate route_toserver;
        private void receiveEvent(Event msg)
        {
            if (!object.Equals(null, this.route_toserver))
            {
                //onReceiveEvent(msg);
                receiveSortQueue.Add(msg);
            }
        }
        private void checkInSortQueue()
        {
            if (!sendThreshold.elapsed) return;
            //selection sort
            for (int i = 0; i < receiveSortQueue.Count; i++)
            {
                for (int a = 0; a < receiveSortQueue.Count; a++)
                {
                    if (((Event)receiveSortQueue[i])._WhenSent < ((Event)receiveSortQueue[a])._WhenSent)
                    {
                        object temp = receiveSortQueue[i];
                        receiveSortQueue[i] = receiveSortQueue[a];
                        receiveSortQueue[a] = temp;
                    }
                }
            }
            //send the events
            for (int i = 0; i < receiveSortQueue.Count; i++)
            {
                if (!object.Equals(null, this.route_toserver))
                {
                    double x = ((Event)receiveSortQueue[i])._WhenSent;
                    //log(Time.NowStringFromDouble(x));
                    route_toserver((Event)receiveSortQueue[i]);
                    receiveSortQueue.RemoveAt(i);
                    i = 0;
                }
            }
            sendThreshold.start();
        }
        private ArrayList receiveSortQueue = new ArrayList();
        private timer sendThreshold = new timer(sendTimer);
        private void handleConnection(object so)
        {
            string logPrefix = "";
            try
            {
                Socket s = (Socket)so;
                string endpoint = s.RemoteEndPoint.ToString();
                allEndpoints.Add(endpoint);
                logPrefix = " < " + endpoint + " > ";
                log(logPrefix + "Socket accepted.");
                string st = string.Empty;
                string _ep = s.RemoteEndPoint.ToString();
                if (object.Equals(null, outQueueRoot[endpoint]))
                {
                    outQueueRoot[endpoint] = new ArrayList();
                }
                s.ReceiveBufferSize = 1000000;
                long packageLength = 0;
                byte[] mainBuffer = new byte[0];
                byte[] tempBuffer = new byte[0];
                while (!shutDownListener)
                {
                    checkInSortQueue();
                    //send an event if there are any
                    if (((ArrayList)outQueueRoot[endpoint]).Count > 0)
                    {
                        Event ev = (Event)((ArrayList)outQueueRoot[endpoint])[0];
                        ((ArrayList)outQueueRoot[endpoint]).RemoveAt(0);
                        ev._WhenSent = Time.Now;
                        byte[] ba = prepOutgoingEvent(ev);
                        if (!s.Connected) { outQueueRoot[endpoint] = null; log("Client Disconnected"); allEndpoints.Remove(endpoint); break; }
                        s.Send(ba, ba.Length, SocketFlags.None);
                    }
                    //read some data if there is any
                    if (s.Available > 0)
                    {
                        byte[] roundBuffer = new byte[32768]; //32k chunks
                        int bytesReadThisRound = s.Receive(roundBuffer);
                        tempBuffer = new byte[mainBuffer.Length];
                        Buffer.BlockCopy(mainBuffer, 0, tempBuffer, 0, mainBuffer.Length);
                        mainBuffer = new byte[bytesReadThisRound + tempBuffer.Length];
                        Buffer.BlockCopy(tempBuffer, 0, mainBuffer, 0, tempBuffer.Length);
                        Buffer.BlockCopy(roundBuffer, 0, mainBuffer, tempBuffer.Length, bytesReadThisRound);
                    }
                    //retrieve preamble if needed
                    if (packageLength == 0 && mainBuffer.Length >= 8)
                    {
                        byte[] header = new byte[8];
                        Buffer.BlockCopy(mainBuffer, 0, header, 0, 8);
                        packageLength = BitConverter.ToInt64(header, 0) + 8;
                    }
                    //derive an event if there are any
                    if (packageLength != 0 && mainBuffer.Length >= packageLength)
                    {
                        byte[] packageCrypt = new byte[packageLength - 8];
                        Buffer.BlockCopy(mainBuffer, 8, packageCrypt, 0, (int)packageLength - 8);
                        tempBuffer = new byte[mainBuffer.Length - packageLength];
                        Buffer.BlockCopy(mainBuffer, (int)packageLength, tempBuffer, 0, mainBuffer.Length - (int)packageLength);
                        mainBuffer = new byte[tempBuffer.Length];
                        Buffer.BlockCopy(tempBuffer, 0, mainBuffer, 0, tempBuffer.Length);
                        packageLength = 0;
                        byte[] packageClear = Encryption2.Decrypt(packageCrypt, Config.rawKey);
                        Event ev = Event.FromBytes(packageClear);
                        ev._WhenRcvd = Time.Now;
                        ev._Endpoint = endpoint;
                        receiveEvent(ev);

                    }
                    //limit CPU usage
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                log(logPrefix + ex.Message);
            }
        }


    }
}
