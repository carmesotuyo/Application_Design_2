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
            catch (NotFoundDbException ex)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 404,
                    Content = "Error retrieving data -- Data Not Found -- " + ex.Message
                };
            }
            catch (AlreadyExistsDbException ex)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 403,
                    Content = "Error creating data -- Data already exists -- " + ex.Message
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
            catch (NonExistantImplementationException ex)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 404,
                    Content = "Error retrieving data -- No extraction methods found" + ex.Message
                };
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Authorization Error -- " + ex.Message
                };
            }
            catch (InvalidDataException ex)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 400,
                    Content = "Invalid data -- " + ex.Message
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

