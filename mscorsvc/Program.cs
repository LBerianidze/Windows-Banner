using System;
using System.ServiceProcess;

namespace mscorsvc
{
	static class Program
	{
		/// <summary>
		/// Точка входа в приложение.
		/// </summary>
		static void Main(string[] args)
		{
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[] 
			{ 
				new MyService() 
			};
			ServiceBase.Run(ServicesToRun);
		}
	}
}
