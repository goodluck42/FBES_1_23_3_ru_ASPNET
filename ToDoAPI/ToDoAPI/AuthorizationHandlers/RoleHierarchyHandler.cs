using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ToDoAPI.Requirements;
using ToDoAPI.Security;

namespace ToDoAPI.AuthorizationHandlers;

public class RoleHierarchyHandler : AuthorizationHandler<RoleHierarchyRequirement>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
		RoleHierarchyRequirement requirement)
	{
		var roleClaim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

		if (roleClaim is null)
		{
			context.Fail();

			return Task.CompletedTask;
		}

		if (Roles.Priorities[roleClaim.Value] >= Roles.Priorities[requirement.Role])
		{
			context.Succeed(requirement);

			return Task.CompletedTask;
		}

		context.Fail();

		return Task.CompletedTask;
	}
}