using Nancy;
using Nancy.Bootstrapper;
using Nancy.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tonic.web
{
    public class NancyBootstrapper : DefaultNancyBootstrapper
    {
        public NancyBootstrapper()
        {
            //this.ApplicationContainer.Register<IRouteResolver>(new DatabaseRouteResolver());
        }

        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            this.ApplicationContainer.Register<IRouteResolver, DatabaseRouteResolver>();
        }
    }
}
