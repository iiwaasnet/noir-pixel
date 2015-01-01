using System;
using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace Api.App.Users.Extensions
{
    public static class IdentityExtensions
    {
        public static bool Self(this IIdentity identity, string userName)
        {
            return identity.IsAuthenticated
                   && identity.GetUserName().Equals(userName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}