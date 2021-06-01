using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinalProject.Heplers
{
    public class SecurityManager
    {
        public static async Task SignInAsync(HttpContext httpContext, User user, string schema, string role)
        {
            var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id),
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Sid, user.SecurityStamp),
                            new Claim(ClaimTypes.Role, role)
                        };

            var claimIdentity = new ClaimsIdentity(claims, role);
            var claimPrin = new ClaimsPrincipal(claimIdentity);
            await httpContext.SignInAsync(scheme: schema, principal: claimPrin);
        }
    }
}
