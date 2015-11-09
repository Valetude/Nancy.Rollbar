using Valetude.Rollbar;

namespace Nancy.Rollbar.Api {
    public interface IRollbarDataScrubber{
        RollbarData ScrubRollbarData(RollbarData data);
    }
}
