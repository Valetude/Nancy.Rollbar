using Nancy.Security;
using Valetude.Rollbar;

namespace Nancy.Rollbar.Api {
    public interface IPersonFactory{
        RollbarPerson GetPerson(IUserIdentity currentUser);
    }
}