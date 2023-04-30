using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace BlogsApp.WebAPI.Filters
{
	public class ExceptionFilter : Attribute, IExceptionFilter
	{
        public void OnException(ExceptionContext context)
        {
            try
            {
                throw context.Exception;
            }
            // completar con las exceptions que vayamos creando
            catch (Exception)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 500,
                    Content = "Unexpected error -- " + context.Exception.Message
                };
            }
        }
    }
}

