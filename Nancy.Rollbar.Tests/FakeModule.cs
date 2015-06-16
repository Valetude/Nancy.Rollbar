using System;
using System.Threading.Tasks;

namespace Nancy.Rollbar.Tests {
    public class FakeModule : NancyModule {
        public FakeModule() {
            Get["/exception"] = _ => {
                ThrowException();
                return null;
            };

            Get["/aggregate"] = _ => {
                ThrowAggregateException();
                return null;
            };
        }

        private static void ThrowAggregateException() {
            Parallel.ForEach(new[] {1, 2}, x => ThrowException());
        }

        private static void ThrowException() {
            throw new FakeException("Fake Exception");
        }
    }
}
