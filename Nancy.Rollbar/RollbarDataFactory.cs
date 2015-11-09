using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.Extensions;
using Nancy.Rollbar.Api;
using Valetude.Rollbar;

namespace Nancy.Rollbar {
    public class RollbarDataFactory : IRollbarDataFactory {
        private static readonly string MachineName;
        private readonly IPersonFactory _personFactory;
        private readonly IRootPathProvider _rootPathProvider;

        static RollbarDataFactory() {
            try {
                MachineName = System.Environment.MachineName;
            }
            catch {
                MachineName = null;
            }
        }

        public RollbarDataFactory(IRootPathProvider rootPathProvider) {
            _rootPathProvider = rootPathProvider;
        }

        public RollbarDataFactory(IRootPathProvider rootPathProvider, IPersonFactory personFactory) {
            _rootPathProvider = rootPathProvider;
            _personFactory = personFactory;
        }

        public RollbarData GetData(NancyContext context, Exception exception) {
            if (context == null) throw new ArgumentNullException("context");
            if (exception == null) throw new ArgumentNullException("exception");

            RollbarPerson person = GetPerson(context);
            return new RollbarData(Environment, new RollbarBody(exception)) {
                Level = ErrorLevel.Error,
                Context = GetContext(context),
                Framework = "Nancy " + typeof(StaticConfiguration).Assembly.GetName().Version.ToString(3),
                Request = GetRollbarRequest(context),
                Person = person,
                Server = new RollbarServer {
                    Host = MachineName,
                    Root = _rootPathProvider.GetRootPath(),
                },
            };
        }

        private static string Environment {
            get { return StaticConfiguration.IsRunningDebug ? "development" : "production"; }
        }

        private static string GetContext(NancyContext context) {
            return context.NegotiationContext != null ? context.NegotiationContext.ModuleName + "Module" : "Before Routing";
        }

        private static RollbarRequest GetRollbarRequest(NancyContext context) {
            var rr = new RollbarRequest {
                Url = context.Request.Url.ToString(),
                Method = context.Request.Method,
                Headers = context.Request.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
                Params = ((IDictionary<string, object>)context.Parameters).ToDictionary(x => x.Key, x => x.Value),
                UserIp = context.Request.UserHostAddress,
            };
            if (rr.Params.Count == 0) {
                rr.Params = null;
            }
            if (rr.Method == "GET") {
                rr.GetParams = ((IDictionary<string, object>) context.Request.Query).ToDictionary(x => x.Key, x => x.Value);
                if (rr.GetParams.Count == 0) {
                    rr.GetParams = null;
                }
            }
            if (rr.Method == "POST") {
                rr.PostParams = ((IDictionary<string, object>) context.Request.Form)
                    .Concat((IDictionary<string, object>) context.Parameters)
                    .ToDictionary(x => x.Key, x => x.Value);
                if (rr.PostParams.Count == 0) {
                    rr.PostParams = null;
                }
                rr.PostBody = context.Request.Body.AsString();
            }
            return rr;
        }

        private RollbarPerson GetPerson(NancyContext context) {
            if (context.CurrentUser == null) {
                return null;
            }
            if (_personFactory != null) {
                return _personFactory.GetPerson(context.CurrentUser);
            }
            return new RollbarPerson(context.CurrentUser.UserName) {
                UserName = context.CurrentUser.UserName,
            };
        }
    }
}