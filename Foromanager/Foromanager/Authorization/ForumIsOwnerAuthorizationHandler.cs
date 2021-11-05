using Foromanager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Foromanager.Authorization
{
	public class ForumIsOwnerAuthorizationHandler: AuthorizationHandler<OperationAuthorizationRequirement, Foro>
	{
		UserManager<Usuario> _userManager;

		public ForumIsOwnerAuthorizationHandler(UserManager<Usuario> usermanager)
		{
			_userManager = usermanager;
		}
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,OperationAuthorizationRequirement requirement, Foro resource)
		{
			if(context.User == null || resource == null)
			{
				return Task.CompletedTask;
			}
			if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName   &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.DeleteOperationName )
            {
                return Task.CompletedTask;
            }

            if (resource.OwnerID == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
		}
	}
}