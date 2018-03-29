using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using WOPIHost.model;
using WOPIHost.util;

namespace WOPIHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = System.Configuration.ConfigurationManager.AppSettings["ListenerHost"];
            var port = System.Configuration.ConfigurationManager.AppSettings["ListenerPort"];

            CobaltServer svr = new CobaltServer(host, Convert.ToInt32(port));
            svr.Start();

            Console.WriteLine("Subdoc中间件，请勿退出！");
            Console.ReadKey();

            svr.Stop();
        }
    }
}
