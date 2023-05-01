using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BlogsApp.Domain.Exceptions;
using BlogsApp.DataAccess.Interfaces.Exceptions;

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
            catch (NotFoundDbException)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 404,
                    Content = "Error retrieving data -- Data Not Found"
                };
            }
            catch (AlreadyExistsDbException)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 403,
                    Content = "Error creating data -- Data already exists"
                };
            }
            catch (BadInputException ex)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 400,
                    Content = "Error with input -- " + ex.Message
                };
            }
            catch (InterruptedActionException ex)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 400,
                    Content = "Error with input -- " + ex.Message
                };
            }
            catch (NonExistantImplementationException)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 404,
                    Content = "Error retrieving data -- No extraction methods found"
                };
            }
            catch (UnauthorizedAccessException)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Authorization Error -- " + context.Exception.Message
                };
            }
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

