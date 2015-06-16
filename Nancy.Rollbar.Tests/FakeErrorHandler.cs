using Nancy.Bootstrapper;

namespace Nancy.Rollbar.Tests {
    public class FakeErrorHandler : IApplicationStartup {
        private readonly RollbarPayloadFactory _payloadFactory;

        public FakeErrorHandler(RollbarPayloadFactory payloadFactory) {
            _payloadFactory = payloadFactory;
        }

        public void Initialize(IPipelines pipelines) {
            pipelines.OnError.AddItemToStartOfPipeline((ctx, err) => {
                ctx.Items["TestPayload"] = _payloadFactory.GetPayload(ctx, err);
                return "Exception Caught";
            });
        }
    }
}
