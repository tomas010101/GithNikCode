using System.Threading.Tasks;
using Foromanager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Foromanager.Authorization
{
	public class ForumManagerAuthorizationHandler: AuthorizationHandler<OperationAuthorizationRequirement,Foro>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,OperationAuthorizationRequirement requirement, Foro resource)
		{

			if(context.User == null || resource == null)
			{
				return Task.CompletedTask;
			}

			if(requirement.Name != Constants.ApproveOperationName && requirement.Name != Constants.RejectOperationName)
			{
				context.Succeed(requirement);
			}

			if(context.User.IsInRole(Constants.ForumManagersRole))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}