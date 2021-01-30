using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVCJustEatClone.Extensions
{
    public static class UserExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var id = from c in user.Claims
                     where c.Type.Contains("nameidentifier")
                     select c.Value;

            return Int32.Parse(id.First());

        }
    }
}
