using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOPIHostNetV1.util;

namespace WOPIHostNetV1
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = System.Configuration.ConfigurationManager.AppSettings["ListenerHost"];
            var port = System.Configuration.ConfigurationManager.AppSettings["ListenerPort"];

            CobaltServer svr = new CobaltServer(host, Convert.ToInt32(port));
            svr.Start();

            Console.WriteLine("A simple wopi webserver. Press any key to quit.");
            Console.ReadKey();

            svr.Stop();
        }
    }
}
