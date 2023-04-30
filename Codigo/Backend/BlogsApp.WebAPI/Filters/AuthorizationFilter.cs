using System;
using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace BlogsApp.WebAPI.Filters
{
	public class AuthorizationFilter : Attribute, IAuthorizationFilter
	{
        private readonly ISessionLogic sessions;

        public AuthorizationFilter(ISessionLogic sessionsLogic)
        {
            this.sessions = sessionsLogic;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"];
            if (token == null)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Se requiere token"
                };
            }
            else
            {
                if (!sessions.IsValidToken(token))
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 403,
                        Content = "Token inválido"
                    };
                }
            }
        }
    }
}

