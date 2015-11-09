using System;
using System.Globalization;
using Nancy.Rollbar.Api;
using Nancy.Security;
using Valetude.Rollbar;

namespace Nancy.Rollbar.Tests {
    public class FakePersonFactory : IPersonFactory {
        public RollbarPerson GetPerson(IUserIdentity currentUser) {
            var person = currentUser as FakePerson;
            if (person == null) throw new ArgumentException("Current User has to be FakePerson", "currentUser");

            return new RollbarPerson(person.Id.ToString(CultureInfo.InvariantCulture)) {
                UserName = person.UserName,
                Email = person.Email,
            };
        }
    }
}
