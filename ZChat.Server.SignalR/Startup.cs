using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace ZChat.Server.SignalR
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                var hubConfiguration = new HubConfiguration()
                {
                    EnableDetailedErrors = true
                };

                map.RunSignalR(hubConfiguration);
            });
        }
    }
}