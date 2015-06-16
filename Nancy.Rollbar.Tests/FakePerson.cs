using System.Collections.Generic;
using Nancy.Security;

namespace Nancy.Rollbar.Tests {
    public class FakePerson : IUserIdentity {
        public FakePerson(int id, string userName, string email, IEnumerable<string> claims) {
            Id = id;
            UserName = userName;
            Email = email;
            Claims = claims;
        }

        public int Id { get; private set; }

        public string UserName { get; private set; }

        public string Email { get; private set; }

        public IEnumerable<string> Claims { get; private set; }
    }
}