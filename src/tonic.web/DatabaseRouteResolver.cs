using Nancy;
using Nancy.Routing;
using Nancy.Routing.Trie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace tonic.web
{
    public class DatabaseRouteResolver : IRouteResolver
    {
        INancyModuleCatalog catalog;
        INancyModuleBuilder moduleBuilder;
        IRouteCache routeCache;
        IRouteResolverTrie trie;

        //private IDictionary<string, string> routes;
        private IDictionary<string, RouteResponse> routes;

        public DatabaseRouteResolver(INancyModuleCatalog catalog, INancyModuleBuilder moduleBuilder, IRouteCache routeCache, IRouteResolverTrie trie)
        {
            this.catalog = catalog;
            this.moduleBuilder = moduleBuilder;
            this.routeCache = routeCache;
            this.trie = trie;

            this.routes = new Dictionary<string, RouteResponse>
            {
                { "/" , new StaticFileRouteResponse { Path = "test.html" } },
                { "/help", new StaticFileRouteResponse { Path = "help-page.html" } }
            };

            this.BuildTrie();
        }

        public ResolveResult Resolve(NancyContext context)
        {            
            var pathDecoded = HttpUtility.UrlDecode(context.Request.Path);

            if (this.routes.ContainsKey(pathDecoded))
            {
                var value = this.routes[pathDecoded];

                return GetResult(context, value);
            }
            
            
            //var pathDecoded = HttpUtility.UrlDecode(context.Request.Path);
            //var results = this.trie.GetMatches(GetMethod(context), pathDecoded, context);

            //if (!results.Any())
            //{
            //    //var allowedMethods = this.trie.GetOptions(pathDecoded, context).ToArray();

            //    //if (IsOptionsRequest(context))
            //    //{
            //    //    return BuildOptionsResult(allowedMethods, context);
            //    //}

            //    //return IsMethodNotAllowed(allowedMethods) ?
            //    //    BuildMethodNotAllowedResult(context, allowedMethods) :
            //    return GetNotFoundResult(context);
            //}

            //// Sort in descending order
            //Array.Sort(results, (m1, m2) => -m1.CompareTo(m2));

            //for (var index = 0; index < results.Length; index++)
            //{
            //    var matchResult = results[index];
            //    if (matchResult.Condition == null || matchResult.Condition.Invoke(context))
            //    {
            //        return this.BuildResult(context, matchResult);
            //    }
            //}

            return GetNotFoundResult(context);
        }

        private void BuildTrie()
        {
            this.trie.BuildTrie(this.routeCache);
        }

        private static string GetMethod(NancyContext context)
        {
            var requestedMethod = context.Request.Method;

            return requestedMethod;
        }

        private static ResolveResult GetNotFoundResult(NancyContext context)
        {
            return new ResolveResult
            {
                Route = new NotFoundRoute(context.Request.Method, context.Request.Path),
                Parameters = DynamicDictionary.Empty,
                Before = null,
                After = null,
                OnError = null
            };
        }

        private INancyModule GetModuleFromMatchResult(NancyContext context, MatchResult result)
        {
            var module = this.catalog.GetModule(result.ModuleType, context);

            return this.moduleBuilder.BuildModule(module, context);
        }

        private ResolveResult GetResult(NancyContext context, RouteResponse value)
        {
            //var testModule = this.moduleBuilder.BuildModule(new StaticFileModule(), context);
            //Func<dynamic, dynamic> viewFunc = (_) => testModule.View["test.html", true];
            
            //var func = new Func<dynamic, CancellationToken, Task<dynamic>>((a, b) => Task.FromResult<dynamic>(viewFunc(a)));

            //var route = new Route("GET", "/", (a) => true, func);
            //var parameters = DynamicDictionary.Create(context.Parameters);

            var route = GetRouteForResponse(context, value);

            return new ResolveResult
            {
                Route = route,
                Parameters = context.Parameters,
                //Before = associatedModule.Before,
                //After = associatedModule.After,
                //OnError = associatedModule.OnError
            };
        }

        private ResolveResult BuildResult(NancyContext context, MatchResult result)
        {
            
            var associatedModule = this.GetModuleFromMatchResult(context, result);
            //var route = associatedModule.Routes.ElementAt(result.RouteIndex);

            //var func = new Func<dynamic, dynamic>(_ => "Test");
            var func = new Func<dynamic, CancellationToken, Task<dynamic>>((a,b) => Task.FromResult<dynamic>("Test"));

            var route = new Route("GET", "/", (a) => true, func);
            var parameters = DynamicDictionary.Create(result.Parameters);

            return new ResolveResult
            {
                Route = route,
                Parameters = parameters,
                //Before = associatedModule.Before,
                //After = associatedModule.After,
                //OnError = associatedModule.OnError
            };
        }

        private Route GetRouteForResponse(NancyContext context, RouteResponse value)
        {
            //if (value is StaticFileRouteResponse)
            //{
            var resp = value as StaticFileRouteResponse;

            var module = this.moduleBuilder.BuildModule(new StaticFileModule(), context);
            Func<dynamic, dynamic> viewFunc = (_) => module.View[resp.Path];
            var func = new Func<dynamic, CancellationToken, Task<dynamic>>((a, b) => Task.FromResult<dynamic>(viewFunc(a)));

            var route = new Route("GET", "/", (a) => true, func);

            return route;
            //}
        }
    }
}