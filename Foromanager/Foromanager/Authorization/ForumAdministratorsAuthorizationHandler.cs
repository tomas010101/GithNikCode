using System.Threading.Tasks;
using Foromanager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Foromanager.Authorization
{
	public class ForumAdministratorsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Foro>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Foro resource)
		{
			if(context.User == null)
			{
				return Task.CompletedTask;
			}

			if(context.User.IsInRole(Constants.ForumAdministratorsRole))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}