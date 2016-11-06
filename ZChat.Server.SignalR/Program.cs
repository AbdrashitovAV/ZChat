using System;
using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using ZChat.Server.SignalR.Hub;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace ZChat.Server.SignalR
{
    class Program
    {
        private static readonly ILog _log = LogManager.GetLogger("GeneralLog");
        private static readonly ILog _messageLog = LogManager.GetLogger("MessageLog");

        private static SignalRServerConnectionManager _connectionManager;

        static void Main(string[] args)
        {
            //TODO: make it loadable from file
            //TODO: make it loadable from arguments
            _log.Info("Programm started");

            var settings = GetSettings(args);

            _connectionManager = new SignalRServerConnectionManager(_log, _messageLog, settings.ServerName);
            GlobalHost.DependencyResolver.Register(typeof(ChatHub), () => new ChatHub(_connectionManager));

            try
            {
                var url = $"http://{settings.Interface}:{settings.Port}";

                using (WebApp.Start(url))
                {
                    _log.Info($"Server running on {url}");
                    Console.ReadLine();
                }
                _log.Info($"Server stopped");
            }
            catch (Exception e)
            {
                _log.Fatal("Cannot start server", e);

                Console.ReadLine();
            }

            _log.Info("Programm closed");
        }

        private static ServerSettings GetSettings(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    return GetSettingsFromInput();
                default:
                    return new ServerSettings { Interface = @"localhost", Port = 8080, ServerName = "ZServer" }; ;
            }


        }

        private static ServerSettings GetSettingsFromInput()
        {
            var serverSettings = new ServerSettings();

            Console.Write("Server:");
            serverSettings.ServerName = Console.ReadLine();

            Console.Write("Network interface:");
            serverSettings.Interface = Console.ReadLine();


            Console.Write("Port:");
            var portString = Console.ReadLine();
            int port;

            while (!int.TryParse(portString, out port))
            {
                Console.WriteLine("Cannot parse port value");
                Console.Write("Port:");
                portString = Console.ReadLine();
            }

            serverSettings.Port = port;

            return serverSettings;
        }
    }
}

