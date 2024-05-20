using Microsoft.EntityFrameworkCore;
using Server.Core.src.Common;
using Server.Service.src.Shared;


namespace Server.Infrastructure.src.Middleware
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ArgumentException ex)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid Data");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized access");
                }
            }

            catch (DbUpdateException ex)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync(ex.InnerException.Message);
                }
            }
            catch (CustomException ex)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = (int)ex.StatusCode;
                    await context.Response.WriteAsync(ex.Message);
                }
            }
            catch (Exception ex)
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(ex.Message);
                }
            }

        }

    }
}
