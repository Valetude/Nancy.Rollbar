using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Xunit;

namespace Nancy.Rollbar.Tests {
    public class RollbarDataFactoryFixture {
        const string RootPath = "C:/inetpub/wwwroot/test_project";

        [Fact]
        public void DataFactory_works_normally() {
            // Given
            var df = GetRollbarDataFactory();
            var context = A.Fake<NancyContext>(); // This Throws!
            var exception = GetException();
            df.GetData(context, exception);
        }

        private static RollbarDataFactory GetRollbarDataFactory(IPersonFactory pf = null) {
            var df = new RollbarDataFactory(GetRootPathProvider(), pf);
            return df;
        }

        private static IRootPathProvider GetRootPathProvider() {
            var rpp = A.Fake<IRootPathProvider>();
            rpp.CallsTo(x => x.GetRootPath()).Returns(RootPath);
            return rpp;
        }

        private static Exception GetException() {
            try {
                throw new Exception("Test");
            }
            catch (Exception e) {
                return e;
            }
        }
    }
}
