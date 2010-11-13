using System;
using System.Threading;
using ThingReferences;

namespace ServerThing
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            Server s = new Server();
            s.onLogMessage += new Server.LogDelegate(s_onLogMessage);
            //new Thread(new ThreadStart(s.mainLoop)).Start();
            s.init();
            s.mainLoop();
        }
        static void s_onLogMessage(string msg)
        {
            log("Server: " + msg);
        }
        static void log(string msg)
        {
            Console.WriteLine(ThingReferences.Config.logPrefix + msg);
        }
    }
    class Server
    {
        ServerNetwork network;
        ServerPluginManager plugins;
        CacheManager cache;
        public void init()
        {
            plugins = new ServerPluginManager();
            plugins.onLogMessage += new ThingReferences.LogDelegate(plugins_onLogMessage);
            plugins.route_toclient += new ServerPluginManager.route_toclient_delegate(plugins_route_toclient);
            plugins.init();
            plugins.addServerPlugin(new SecretServerPlugin());
            new Thread(new ThreadStart(plugins.mainLoop)).Start();

            cache = new CacheManager();
            cache.onLogMessage += new ThingReferences.LogDelegate(cache_onLogMessage);
            cache.route_toclient += new CacheManager.route_toclientDelegate(cache_route_toclient);
            cache.pluginAdded += new CacheManager.pluginAddedDelegate(cache_pluginAdded);
            cache.pluginDeleted += new CacheManager.pluginDeletedDelegate(cache_pluginDeleted);
            cache.init();
            new Thread(new ThreadStart(cache.mainLoop)).Start();

            network = new ServerNetwork();
            network.netKey = Config.networkKey;
            network.onLogMessage += new ServerNetwork.LogDelegate(network_onLogMessage);
            network.route_toserver += new ServerNetwork.onReceiveEventDelegate(network_route_toserver);
            new Thread(new ThreadStart(network.mainLoop)).Start();
            
        }
        void cache_pluginDeleted(string pathRelPluginFile)
        {
            plugins.delPlugin(pathRelPluginFile);
        }
        void cache_pluginAdded(string pathRelPluginFile)
        {
            plugins.addPlugin(pathRelPluginFile);
        }
        void cache_route_toclient(Event ev)
        {
            //network.sendEvent(ev);
            plugins.sourceHub(ev, EventTransfer.SERVERTOCLIENT);
        }
        void cache_onLogMessage(string msg)
        {
            log("cache: " + msg);
        }
        void plugins_route_toclient(Event msg)
        {
            network.sendEvent(msg);
        }
        private void network_route_toserver(Event msg)
        {
            plugins.sourceHub(msg, EventTransfer.CLIENTTOSERVER);
            if (msg._Keyword == KeyWord.CACHE_CLIENTREPORT)
            {
                cache.clientReport(msg);
            }
        }
        private void network_onLogMessage(string msg)
        {
            log("network: " + msg);
        }
        private void plugins_onLogMessage(string msg)
        {
            log("plugins: " + msg);
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
        public void mainLoop()
        {
            Thread.CurrentThread.Name = "Server mainLoop";
            log("mainLoop start!");
            log("press H to shut down");
            while (running)
            {
                Thread.Sleep(100);
                if (Console.ReadKey().Key == ConsoleKey.H)
                    shutdown();
            }
            log("mainLoop stop!");
        }
        ~Server()
        {
            shutdown();
        }
        private bool running = true;
        public void shutdown()
        {
            running = false;
            network.shutdown();
            plugins.shutdown();
            cache.shutdown();
        }
    }
}
