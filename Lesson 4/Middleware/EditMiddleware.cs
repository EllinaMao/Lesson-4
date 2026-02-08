internal class EditMiddleware
{
    private readonly RequestDelegate _next;
    public EditMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, IUserRepository repo)
    {
        if (context.Request.Method == "GET")
        {
            if (Guid.TryParse(context.Request.Query["id"], out var id))
            {
                var user = repo.Get(id);
                if (user == null)
                {
                    context.Response.StatusCode = 404;
                    return;
                }
                await context.Response.WriteAsJsonAsync(user);

            }
            else
            {
                context.Response.StatusCode = 400; 
            }
            context.Response.Redirect("/");
           
        }
        else if (context.Request.Method == "POST")
        {
            var form = await context.Request.ReadFormAsync();
            if (Guid.TryParse(form["id"], out var id))
            {
                var user = new User(form["Name"], form["Email"]) { Id = id };
                repo.Edit(user);
            }
            context.Response.Redirect("/");

        }
        else
        {
            context.Response.StatusCode = 405; 
        }
    }
}