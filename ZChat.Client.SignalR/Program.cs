using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using ZChat.Shared;

namespace ZChat.Client.SignalR
{
    class Program
    {
        static void Main(string[] args)
        {

            MainAsync().Wait();
            Console.ReadLine();
        }

        static async Task MainAsync()
        {
            try
            {

                var hubConnection = new HubConnection("http://localhost:8080/");
                //hubConnection.TraceLevel = TraceLevels.All;
                //hubConnection.TraceWriter = Console.Out;
                var hubProxy = hubConnection.CreateHubProxy("ChatHub");
                hubProxy.On<Message>("MessageFromServer", message =>
                {
                    Console.WriteLine("Incoming data: {0}", message.Content);
                });
                ServicePointManager.DefaultConnectionLimit = 10;
                var connectionTask = hubConnection.Start();
                connectionTask.Wait();

                Console.WriteLine("write username" + Environment.NewLine);
                var username = Console.ReadLine();
                var b = hubProxy.Invoke<ServiceResult>("Join", username);
                b.Wait();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
