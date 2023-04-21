using System;
using BlogsApp.Factory;

namespace BlogsApp.WebAPI
{
	public class Startup
	{
		public Startup()
		{
		}

        //...STARTUP CODE
        public void ConfigureServices(IServiceCollection services)
        {
            //...CONFIGURE_SERVICES CODE

            ServiceFactory factory = new ServiceFactory(services);
        }
    }
}
}

