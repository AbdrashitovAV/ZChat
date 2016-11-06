using System;
using System.ServiceModel;
using log4net;
using ZChat.ServerLogic;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ZChat.Server
{

    class Program
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            log.Info("Server started");




            log.Info("Server stopped");

            //            using (var serverContainer = new UnityContainer())
            //            {
            //                serverContainer.RegisterServerDependencies();
            //
            //                var service = serverContainer.Resolve<WcfHoster>();
            //                service.Start();
            //
            //                Console.WriteLine("Сервер запущен. Для остановки нажмите Enter.");
            //                Console.ReadLine();
            //
            //                service.Stop();
            //            }
        }
    }
}
