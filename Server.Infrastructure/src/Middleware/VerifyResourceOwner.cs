using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Server.Core.src.Entity;

namespace Server.Infrastructure.src.Middleware
{

    public class VerifyResourceOwnerRequirement : IAuthorizationRequirement
    {
        public VerifyResourceOwnerRequirement()
        {
        }
    }

    public class VerifyResourceOwnerHandler : AuthorizationHandler<VerifyResourceOwnerRequirement, Address>
    {
        private readonly ILogger<VerifyResourceOwnerHandler> _logger;

        public VerifyResourceOwnerHandler(ILogger<VerifyResourceOwnerHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, VerifyResourceOwnerRequirement requirement, Address resource)
        {
            _logger.LogInformation("HandleRequirementAsync is called.");

            var claims = context.User.Claims;
            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value; // id of authenticated user
            _logger.LogInformation($"User ID: {userId}");

            if (userId == resource.UserId.ToString())
            {
                _logger.LogInformation("Resource owner is verified.");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Resource owner verification failed.");
            }

            return Task.CompletedTask;
        }
    }
}