using Nancy.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tonic.web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new NancyOptions { Bootstrapper = new NancyBootstrapper() };

            app.UseNancy(options);
        }
    }
}