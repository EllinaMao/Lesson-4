using Lesson_4.View;
using Microsoft.AspNetCore.Diagnostics;

public class ErrorMiddleware
{
    private readonly ILogger<ErrorMiddleware> _logger;

    public ErrorMiddleware(RequestDelegate next, ILogger<ErrorMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        var error = exceptionHandlerPathFeature?.Error;

        var path = exceptionHandlerPathFeature?.Path;

        _logger.LogError(error, "Error {Path}", path);

        string errorMessage = "Unknown Error";
        string errorDetail = "";

        if (error is ArgumentException)
        {
            errorMessage = "Data Error";
            errorDetail = error.Message;
        }
        else if (error != null)
        {
            errorMessage = "Server Error";
        }

        string body = $@"
                <div class='alert alert-danger'>
                    <h4 class='alert-heading'>{errorMessage}</h4>
                    <p>{errorDetail}</p>
                    <hr>
                    <p class='mb-0'>Error happened Pls return to main page.</p>
                </div>
                <a href='/' class='btn btn-primary'>На главную</a>
            ";

        string html = HtmlMetods.GenerateHtmlPage(body, "Ошибка!");

        context.Response.ContentType = "text/html; charset=utf-8";
        await context.Response.WriteAsync(html);
    }
}
