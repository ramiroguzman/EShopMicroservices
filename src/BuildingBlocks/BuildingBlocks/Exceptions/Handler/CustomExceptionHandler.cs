
using System.Reflection.Metadata.Ecma335;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler
(ILogger<CustomExceptionHandler> logger)
: IExceptionHandler
{
   public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
   {

      logger.LogError("Error Message: {exceptionMessage}, Time {time}", exception.Message, DateTime.UtcNow);

      (string Detail, string Title, int Status) details = exception switch
      {
         BadRequestException => (
             exception.Message,
             exception.GetType().Name,
             context.Response.StatusCode = StatusCodes.Status400BadRequest
          ),
         ValidationException => (
            exception.Message,
            exception.GetType().Name,
            context.Response.StatusCode = StatusCodes.Status400BadRequest
         ),
         NotFoundException => (
           exception.Message,
             exception.GetType().Name,
             context.Response.StatusCode = StatusCodes.Status404NotFound
          ),
         InternalServerException => (
             exception.Message,
             exception.GetType().Name,
             context.Response.StatusCode = StatusCodes.Status500InternalServerError
          ),
         _ => (
            exception.Message,
            exception.Message,
            context.Response.StatusCode = StatusCodes.Status500InternalServerError
         )
      };

      var problemDetails = new ProblemDetails
      {
         Title = details.Title,
         Detail = details.Detail,
         Status = details.Status,
         Instance = context.Request.Path
      };
      problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

      if (exception is ValidationException validationException)
      {
         problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
      }
      await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
      return true;
   }

}

