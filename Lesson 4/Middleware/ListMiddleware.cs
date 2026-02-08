using Lesson_4.View;

internal class ListMiddleware
{
    private readonly RequestDelegate _next;

    public ListMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context, IUserRepository repo)
    {
        var users = repo.GetAll();
        string table = HtmlMetods.BuildHtmlTable(users, u => $@"
            <a href='/edit?id={u.Id}' class='btn btn-sm btn-warning'>Edit</a>
            <a href='/delete?id={u.Id}' class='btn btn-sm btn-danger'>Delete</a>");

        string html = HtmlMetods.GenerateHtmlPage(
            $"<div class='mb-3'><a href='/create' class='btn btn-primary'>+ Создать</a></div>{table}",
            "Список пользователей");

        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync(html);

    }

}