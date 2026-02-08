internal class DeleteMiddleware
{
    private readonly RequestDelegate _next;

    public DeleteMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, IUserRepository repo, ILogger<DeleteMiddleware> logger)
    {
        if (context.Request.Method == "GET" || context.Request.Method == "POST")
        {
            if (Guid.TryParse(context.Request.Query["id"], out var id))
            {
                try
                {
                    logger.LogInformation("Attempt to delete user {Id}", id);

                    repo.Delete(id);

                    logger.LogInformation("User {UserId} deleted.", id);
                }
                catch (ArgumentException ex)
                {
                    logger.LogWarning(ex, "Cant delete {UserId}: {Message}", id, ex.Message);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Critical error when deleting {UserId}", id);
                    throw; 
                }
            }
            else
            {
                logger.LogWarning("Unknown id error");
            }

            context.Response.Redirect("/");
            await Task.CompletedTask;
        }
    }
}