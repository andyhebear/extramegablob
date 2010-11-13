using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using ThingReferences;
#pragma warning disable 618 //warning CS0618: 'System.Net.Dns.Resolve(string)' is obsolete: 'Resolve is obsoleted for this type, please use GetHostEntry instead. http://go.microsoft.com/fwlink/?linkid=14202'
namespace thing
{
    public class ClientNetwork
    {
        private static TimeSpan sendTimer { get { return new TimeSpan(0, 0, 0, 0, 100); } }
        public delegate void LogDelegate(string msg);
        public event LogDelegate onLogMessage;
        private void log(string msg)
        {
            if (!object.Equals(null, this.onLogMessage))
            {
                onLogMessage(msg);
            }
        }
        public delegate void onReceiveEventDelegate(Event msg);
        public event onReceiveEventDelegate onReceiveEvent;
        private void receiveEvent(Event msg)
        {
            if (!object.Equals(null, this.onReceiveEvent))
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
                if (!object.Equals(null, this.onReceiveEvent))
                {
                    double x = ((Event)receiveSortQueue[i])._WhenSent;
                    // log(Time.NowStringFromDouble(x));
                    onReceiveEvent((Event)receiveSortQueue[i]);
                    receiveSortQueue.RemoveAt(i);
                    i = 0;
                }
            }
            sendThreshold.start();
        }
        private ArrayList receiveSortQueue = new ArrayList();
        private timer sendThreshold = new timer(sendTimer);
        public bool connected
        {
            get
            {
                if (object.Equals(null, client))
                    return false;
                return client.Connected;
            }
        }
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
        private static Socket client;
        private ArrayList outQueueRoot = new ArrayList();
        public void sendEvent(Event ev)
        {
            if (!connected)
            {
                log("not connected");
                return;
            }
            outQueueRoot.Add(ev);
        }
        public void disconnect()
        {
            if (connected)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            outQueueRoot = new ArrayList();
        }

        public delegate void route_toserverDelegate(Event ev);
        public event route_toserverDelegate route_toserver;
        private void toserver(Event ev)
        {
            if (!object.Equals(null, this.route_toserver))
            {
                route_toserver(ev);
            }
        }

        public delegate void onConnectCompletedDelegate(string host, string port);
        public event onConnectCompletedDelegate onConnectCompleted;
        private void connectCompleted(string host, string port)
        {
            if (!object.Equals(null, this.onConnectCompleted))
            {
                onConnectCompleted(host, port);
            }
        }

        public delegate void onDisconnectedDelegate();
        public event onDisconnectedDelegate onDisconnected;
        private void _onDisconnected()
        {
            if (!object.Equals(null, this.onDisconnected))
            {
                onDisconnected();
            }
        }

        public void connect(string host, string port)
        {
            bool x = false;
            try
            {
                disconnect();
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(host, int.Parse(port));
                client.ReceiveBufferSize = 1000000;
                long packageLength = 0;
                byte[] mainBuffer = new byte[0];
                byte[] tempBuffer = new byte[0];
                byte[] roundBuffer = new byte[32768]; //32k chunks
                Thread.Sleep(500);
                if (connected) { connectCompleted(host, port); x = true; }
                while (connected)
                {
                    checkInSortQueue();
                    //send an event if there are any
                    if (outQueueRoot.Count > 0)
                    {
                        Event ev = (Event)outQueueRoot[0];
                        ev._WhenSent = Time.Now;
                        byte[] ba = prepOutgoingEvent(ev);
                        outQueueRoot.RemoveAt(0);
                        client.Send(ba, ba.Length, SocketFlags.None);
                    }
                    //read some data if there is any
                    if (client.Available > 0)
                    {
                        int bytesReadThisRound = client.Receive(roundBuffer);
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
                        receiveEvent(ev);
                    }
                    //limit CPU usage
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                log(ex.Message);
            }
            finally
            {
                Thread.Sleep(500);
                if (!connected && x) _onDisconnected();
            }
        }
    }
}
