using Lesson_4.View;

internal class EditMiddleware
{
    private readonly RequestDelegate _next;
    public EditMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, IUserRepository repo, ILogger<EditMiddleware> logger)
    {
        if (context.Request.Method == "GET")
        {
            logger.LogInformation("Processing GET request for /edit");
            if (Guid.TryParse(context.Request.Query["id"], out var id))
            {
                var user = repo.Get(id);

                if (user == null)
                {
                    logger.LogWarning("User with ID {Id} not found", id);
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync("User not found");
                    return;
                }

                string form = HtmlMetods.BuildForm<User>("/edit", user);
                string page = HtmlMetods.GenerateHtmlPage(form, "Edit User");

                context.Response.ContentType = "text/html; charset=utf-8";
                logger.LogInformation("Serving edit form for user with ID {Id}", id);
                await context.Response.WriteAsync(page);
            }
            else
            {
                logger.LogWarning("Invalid ID format: {Id}", context.Request.Query["id"]);
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid ID format");
            }
        }
        else if (context.Request.Method == "POST")
        {
            try
            {
                logger.LogInformation("Processing POST request for /edit");
                var form = await context.Request.ReadFormAsync();
                if (Guid.TryParse(form["id"], out var id))
                {
                    logger.LogInformation("Updating user with ID {Id}", id);
                    var user = new User(form["Name"], form["Email"]) { Id = id };
                    repo.Edit(user);
                }
                context.Response.Redirect("/");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing POST request for /edit");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync($"Error: {ex.Message}");
            }
        }
        else
        {
            logger.LogWarning("Unsupported HTTP method: {Method}", context.Request.Method);
            context.Response.StatusCode = 405;
        }
    }
}