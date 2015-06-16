using Nancy.Security;
using Valetude.Rollbar;

namespace Nancy.Rollbar {
    public interface IPersonFactory{
        RollbarPerson GetPerson(IUserIdentity currentUser);
    }
}