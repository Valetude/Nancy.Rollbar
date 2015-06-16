using System.Collections.Generic;
using System.Linq;
using Nancy.Testing;
using Valetude.Rollbar;
using Xunit;

namespace Nancy.Rollbar.Tests {
    public class RollbarPayloadFactoryFixture {
        [Fact]
        public void Test_payload_factory_for_simple_exception() {
            // Given
            var browser = new Browser(with => with
                .Dependency<RollbarDataFactory>()
                .Dependency<RollbarPayloadFactory>()
                .Dependency<FakeErrorHandler>()
                .Dependency<FakeAccessToken>()
                .Dependency<FakeModule>()
                .Dependency<FakePerson>()
                .Dependency<FakePersonFactory>()
                .Module<FakeModule>());

            // When
            var response = browser.Get("/exception");
            var payload = (RollbarPayload)response.Context.Items["TestPayload"];

            // Then
            Assert.Equal(FakeAccessToken.FakeToken, payload.AccessToken);
            Assert.NotNull(payload.RollbarData);
            Assert.Equal("development", payload.RollbarData.Environment);
            Assert.NotNull(payload.RollbarData.Body);
            var body = payload.RollbarData.Body;
            Assert.Null(body.TraceChain);
            Assert.Null(body.Message);
            Assert.Null(body.CrashReport);
            Assert.NotNull(body.Trace);
            var trace = body.Trace;
            Assert.NotNull(trace.Frames);
            Assert.NotEmpty(trace.Frames);
            Assert.All(trace.Frames, frame => {
                Assert.NotNull(frame.Method);
                Assert.NotNull(frame.FileName);
            });
            Assert.NotNull(trace.Exception);
            Assert.Equal("Nancy.Rollbar.Tests.FakeException", trace.Exception.Class);
            Assert.Equal("Fake Exception", trace.Exception.Message);
            Assert.Null(trace.Exception.Description);
            Assert.Equal(ErrorLevel.Error, payload.RollbarData.Level);
            Assert.NotNull(payload.RollbarData.Timestamp);
            Assert.Null(payload.RollbarData.CodeVersion);
            Assert.Equal("windows", payload.RollbarData.Platform);
            Assert.Equal("c#", payload.RollbarData.Language);
            Assert.Equal("Nancy 1.2.0", payload.RollbarData.Framework);
            Assert.Equal("FakeModule", payload.RollbarData.Context);
            Assert.NotNull(payload.RollbarData.Request);
            var request = payload.RollbarData.Request;
            Assert.NotNull(request.Url);
            Assert.EndsWith("/exception", request.Url);
            Assert.Equal("GET", request.Method);
            Assert.Contains("Accept", request.Headers.Keys);
            Assert.Null(request.Params);
            Assert.Null(request.GetParams);
            Assert.Null(request.QueryString);
            Assert.Null(request.PostParams);
            Assert.Null(request.PostBody);
            Assert.Null(request.UserIp);
            Assert.Null(payload.RollbarData.Person);
            Assert.NotNull(payload.RollbarData.Server);
            Assert.NotNull(payload.RollbarData.Server.Host);
            Assert.True(!string.IsNullOrWhiteSpace(payload.RollbarData.Server.Host), "Host not whitespace");
            Assert.NotNull(payload.RollbarData.Server.Root);
            Assert.True(!string.IsNullOrWhiteSpace(payload.RollbarData.Server.Root), "Root not whitespace");
            Assert.Null(payload.RollbarData.Server.Branch);
            Assert.Null(payload.RollbarData.Server.CodeVersion);
            Assert.Equal(2, payload.RollbarData.Server.ToArray().Length);
            Assert.Null(payload.RollbarData.Client);
            Assert.Null(payload.RollbarData.Custom);
            Assert.Null(payload.RollbarData.Fingerprint);
            Assert.Null(payload.RollbarData.Title);
            Assert.Null(payload.RollbarData.Uuid);
        }
    }
}
