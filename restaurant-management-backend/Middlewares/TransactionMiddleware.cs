using restaurant_management_backend.Data;

namespace restaurant_management_backend.Middlewares
{
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;

        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
        {
            if (context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                await _next(context);

                var statusCode = context.Response.StatusCode;
                if (statusCode >= 200 && statusCode <= 299)
                {
                    await transaction.CommitAsync(); // If the response is a success code (2xx), commit the transaction
                }
                else
                {
                    await transaction.RollbackAsync(); // If the response is an error code (4xx or 5xx), roll back the transaction
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
