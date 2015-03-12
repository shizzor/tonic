using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tonic.web
{
    public class BaseModule : Nancy.NancyModule
    {
        public BaseModule()
        {
            Get["/"] = _ => "Hello";
        }
    }
}