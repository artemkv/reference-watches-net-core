using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Watches.Exceptions;
using Watches.Models;

namespace Watches
{
    /// <summary>
    /// Converts BadRequestException to BadRequest-compatible response.
    /// This allows you to throw BadRequestException instead of returning BadRequest action result.
    /// </summary>
    public class BadRequestExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var badRequest = context.Exception as BadRequestException;
            if (badRequest != null)
            {
                var error = new SerializableError();
                error.Add(badRequest.Key ?? "Reason", new string[] { context.Exception.Message });
                context.Result = new ObjectResult(error)
                {
                    StatusCode = 400
                };
            }
        }
    }
}
