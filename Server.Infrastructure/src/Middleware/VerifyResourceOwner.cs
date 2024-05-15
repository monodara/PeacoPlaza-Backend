using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Server.Core.src.Entity;
using Server.Service.src.DTO;

namespace Server.Infrastructure.src.Middleware
{

    public class VerifyResourceOwnerRequirement : IAuthorizationRequirement
    {
        public VerifyResourceOwnerRequirement()
        {
        }
    }

    public class VerifyResourceOwnerHandler : AuthorizationHandler<VerifyResourceOwnerRequirement, AddressReadDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, VerifyResourceOwnerRequirement requirement, AddressReadDto resource)
        {
            var claims = context.User.Claims;
            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value; // id of authenticated user
            if(userId == resource.UserId.ToString())
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

}