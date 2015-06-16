# Nancy.Rollbar

Rollbar integration for Nancy built on top of
[Valetude.Rollbar](https://github.com/Valetude/Valetude.Rollbar).

## Why?

So you can report bugs to Rollbar w/out adding dependencies on System.Web like
you get in [RollbarSharp](https://github.com/mroach/RollbarSharp)

## Install

Nuget Package Manager

    install-package Nancy.Rollbar

## What's inside?

There are 5 interfaces with implementations for 3 of them. Filling out the other
two is one part of your job.

First thing to do is to create a class that implements `IHasAccessToken`. The
`RollbarAccessToken` property should return the server side token from Rollbar.

The second thing to do, if you want the `person` field on your rollbar payload
to be filled out right is to implement `IPersonFactory`.  It has a `GetPerson`
method that receives an `IUserIdentity` and returns a `RollbarPerson`. This
allows you to use your knowledge of how you do Auth in your project to set the
`Id` `UserName` and `Email` properties correctly. If you do *not* fill this out,
the `person` field will be filled out automatically using the
`Context.CurrentUser.Username` as both `Id` and `Username`.

The final thing to do is to wire in the error handler. This is done as any
`OnError` hook is done.  If you want to wire it in globally you can put the
appropriate code into a bootstrapper in an override of `ApplicationStartup`. My
preference is to create an implementation of `IApplicationStartup` and to let
Nancy discover, instantiate, and initialize it:

The following illustartes that, but `ScrubPayload` is left unimplemented. You
are 100% responsible for removing passwords and other sensitive information from the
payload that is sent to Rollbar. While they do have some scrubbing that they do, they
cannot guess all the various ways you might name fields that you don't want people to
see. `Nancy.Rollbar` does aboslutely no scrubbing for you.


    using Nancy.Bootstrapper;

    namespace Nancy.Rollbar.Tests {
        public class StartupNancyRollbar : IApplicationStartup {
            private IRollbarPayloadFactory _payloadFactory;
            private IRollbarPayloadSender _payloadSender;

            public StartupNancyRollbar(IRollbarPayloadFactory payloadFactory, IRollbarPayloadSender payloadSender) {
                _payloadFactory = payloadFactory;
                _payloadSender = payloadSender;
            }

            public void Initialize(IPipelines pipelines) {
                pipelines.OnError.AddItemToStartOfPipeline((ctx, err) => {
                    var payload = _payloadFactory.GetPayload(ctx, err);
                    ScrubPayload(payload); // VERY IMPORTANT!
                    var response = _payloadSender.SendPayload(payload);
                    if (response.StatusCode == RollbarResponseCode.Success) {
                        return null; // To Allow the rest of the pipeline to handle the error
                    }
                    // If you want to return null anyway it might not be a bad idea. You can also check if `StaticConfiguration.IsRunningDebug`
                    return "OH NO! ROLLBAR FAILED!";
                });
            }
        }
    }
