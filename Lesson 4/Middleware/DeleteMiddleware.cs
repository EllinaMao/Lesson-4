internal class DeleteMiddleware
{
    private readonly RequestDelegate _next;
    public DeleteMiddleware(RequestDelegate next) => _next = next;
    public async Task InvokeAsync(HttpContext context, IUserRepository repo)
    {
        if (context.Request.Method == "POST")
        {
            if (Guid.TryParse(context.Request.Query["id"], out var id))
            {
                try
                {
                    repo.Delete(id);
                }
                catch (ArgumentException)
                {
                }
            }
            context.Response.Redirect("/");
            await Task.CompletedTask;
        }
       
    }
}