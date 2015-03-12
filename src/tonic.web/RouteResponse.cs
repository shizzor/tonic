using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tonic.web
{
    public abstract class RouteResponse
    {
    }

    public class StaticFileRouteResponse : RouteResponse
    {
        public string Path { get; set; }
    }
}