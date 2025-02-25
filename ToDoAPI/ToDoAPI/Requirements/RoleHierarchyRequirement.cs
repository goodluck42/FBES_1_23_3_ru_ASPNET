using Microsoft.AspNetCore.Authorization;

namespace ToDoAPI.Requirements;

public class RoleHierarchyRequirement : IAuthorizationRequirement
{
	public required string Role { get; init; }
}