using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoCSharp.Client
{
    public class AsyncServer
    {

        private CoCClient client;

        public AsyncServer(CoCClient client)
        {
            this.client = client;
            var listener = new HttpListener();

            listener.Prefixes.Add("http://localhost:8080/");
            listener.Prefixes.Add("http://127.0.0.1:8080/");

            listener.Start();

            //while (true)
            //{
            //    try
            //    {
            //        var context = listener.GetContext(); //Block until a connection comes in
            //        context.Response.StatusCode = 200;
            //        context.Response.SendChunked = true;


            //        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this.client.Avatar));
            //        context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            //        context.Response.Close();



            //    }
            //    catch (Exception)
            //    {
            //        // Client disconnected or some other error - ignored for this example
            //    }
            //}
        }
    }
}
