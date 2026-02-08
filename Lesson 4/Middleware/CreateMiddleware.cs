using Lesson_4.View;

internal class CreateMiddleware
{
    private readonly RequestDelegate _next;

    public CreateMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserRepository repo)
    {
        if (context.Request.Method == "GET")
        {
            string form = HtmlMetods.BuildForm<User>("/create");
            string page = HtmlMetods.GenerateHtmlPage(form, "Create User");
            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.WriteAsync(page);

        }
        else if (context.Request.Method == "POST")
        {
            try { 
            var form = await context.Request.ReadFormAsync();
            repo.Add(new User(form["Name"], form["Email"]));
            context.Response.Redirect("/");
            }
            catch (ArgumentException) { }
        }
        else
        {
            context.Response.StatusCode = 405; // Method Not Allowed
        }
    }
}