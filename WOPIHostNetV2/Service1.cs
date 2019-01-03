using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using WOPIHostNetV2.util;

namespace WOPIHostNetV2
{
    public partial class Service1 : ServiceBase
    {

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly CobaltServer cobaltServer = new CobaltServer(System.Configuration.ConfigurationManager.AppSettings["listenerURL"]);


        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            cobaltServer.Start();
        }

        protected override void OnStop()
        {
            cobaltServer.Stop();
        }
    }
}
