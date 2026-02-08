using Lesson_4.View;

internal class CreateMiddleware
{
    private readonly RequestDelegate _next;

    public CreateMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserRepository repo, ILogger<CreateMiddleware> logger)
    {
        if (context.Request.Method == "GET")
        {
            logger.LogInformation("Create page requested");

            string form = HtmlMetods.BuildForm<User>("/create");
            string page = HtmlMetods.GenerateHtmlPage(form, "Create User");
            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.WriteAsync(page);
        }
        else if (context.Request.Method == "POST")
        {
            IFormCollection? form = null;
            try
            {
                logger.LogInformation("Create user attempt");
                form = await context.Request.ReadFormAsync();
                repo.Add(new User(form["Name"], form["Email"]));
                logger.LogInformation("User created successfully");
                context.Response.Redirect("/");
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, "Error creating user");
                var name = form != null ? form["Name"].ToString() : "";
                var email = form != null ? form["Email"].ToString() : "";
                var userAttempt = new User(name, email);

                string errorHtml = $"<div class='alert alert-danger'>{ex.Message}</div>";

                string formHtml = HtmlMetods.BuildForm<User>("/create", userAttempt);
                string page = HtmlMetods.GenerateHtmlPage(errorHtml + formHtml, "Creation error");

                context.Response.StatusCode = 400;
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.WriteAsync(page);
            }
        }
        else
        {
            logger.LogWarning("Unsupported HTTP method: {Method}", context.Request.Method);
            context.Response.StatusCode = 405; 
        }
    }
}