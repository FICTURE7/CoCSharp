using CoCSharp.Server.Core;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.Server
{
    public class WebApi
    {
        public WebApi(Server server)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            _server = server;
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:8081/");
            _listener.Prefixes.Add("http://127.0.0.1:8081/");

#if !DEBUG
            _listener.Prefixes.Add($"http://{GetPublicIP()}:8081/");
#endif

            var curProcess = Process.GetCurrentProcess();
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");            
        }

        private readonly Server _server;
        private readonly HttpListener _listener;
        private readonly PerformanceCounter _cpuCounter;

        public void Start()
        {
            _listener.Start();

            AcceptNewRequest();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        // Look up the external IP address using ipify.org.
        private string GetPublicIP()
        {
            using (var wc = new WebClient())
                return wc.DownloadString("https://api.ipify.org/");
        }

        private void AcceptNewRequest()
        {
            var k = HandleRequestAsync();
        }

        private async Task HandleRequestAsync()
        {
            var context = await _listener.GetContextAsync();

            // Begins handling new requests.
            AcceptNewRequest();

            //_server.Log.Info("Handling new web API request.");

            var builder = new StringBuilder();
            if (context.Request.Url.LocalPath == "/net")
            {
                var numConnections = _server.Clients.Count;
                var totalConnections = _server.TotalConnections;
                var totalBytesReceived = _server.Settings.Statistics.TotalByteReceived;
                var totalBytesSent = _server.Settings.Statistics.TotalByteSent;
                var totalMessagesReceived = _server.Settings.Statistics.TotalMessagesReceived;
                var totalMessagesSent = _server.Settings.Statistics.TotalMessagesSent;

                builder.Append("numConnections=").Append(numConnections).Append(";");
                builder.Append("totalConnections=").Append(totalConnections).Append(";");
                builder.Append("totalBytesReceived=").Append(totalBytesReceived).Append(";");
                builder.Append("totalByteSent=").Append(totalBytesSent).Append(";");
                builder.Append("totalMessaggesReceived=").Append(totalMessagesReceived).Append(";");
                builder.Append("totalMessaggesSent=").Append(totalMessagesSent).Append(";");
            }
            else if (context.Request.Url.LocalPath == "/db")
            {
                var totalEntries = ((LiteDbManager)_server.Db).TotalEntries;

                builder.Append("totalEntries=").Append(totalEntries).Append(";");
            }
            else if (context.Request.Url.LocalPath == "/sys")
            {
                var cpuUsage = _cpuCounter.NextValue();
                //var memUsage = _memCounter.NextValue();
                var memUsage = Process.GetCurrentProcess().WorkingSet64;

                builder.Append("cpuUsage=").Append(cpuUsage).Append(";");
                builder.Append("memUsage=").Append(memUsage).Append(";");
            }
            else
            {
                builder.Append("bad request, check path");
                context.Response.StatusCode = 400;
            }

            var str = builder.ToString();
            var buffer = Encoding.UTF8.GetBytes(str);

            context.Response.ContentType = "application/json";
            await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);

            context.Response.Close();
        }
    }
}
