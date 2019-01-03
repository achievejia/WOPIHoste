using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

namespace WOPIHostNetV2
{
    [RunInstaller(true)]
    public partial class SubdocInstaller : System.Configuration.Install.Installer
    {
        public SubdocInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
