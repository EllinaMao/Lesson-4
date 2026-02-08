using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Html;

namespace Lesson_4.View
{
    public static class HtmlMetods
    {
        public static string GenerateHtmlPage(string body, string header)
        {
            string html = $"""
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset="utf-8" />
            <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/css/bootstrap.min.css" rel="stylesheet" 
            integrity="sha384-KK94CHFLLe+nY2dmCWGMq91rCGa5gtU4mk92HdvYe+M/SXH301p5ILy+dN9+nJOZ" crossorigin="anonymous">
            <title>{header}</title>
        </head>
        <body>
        <div class="container">
        <h2 class="d-flex justify-content-center">{header}</h2>
        <div class="mt-5"></div>
        {body}
            <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/js/bootstrap.bundle.min.js"
            integrity="sha384-ENjdO4Dr2bkBIFxQpeoTz1HIcje39Wm4jDKdf19U8gI4ddQ3GYNS7NTKfAdVQSZe" crossorigin="anonymous"></script>
        </div>
        </body>
        </html>
        """;
            return html;
        }
        public static string BuildHtmlTable<T>(IEnumerable<T> collection, Func<T, string>? actionsBuilder = null)
        {
            if (!collection.Any()) return "<div class='alert alert-warning'>Список пуст</div>";

            var sb = new StringBuilder();
            sb.Append("<table class='table table-bordered table-striped'>");

            // Получаем свойства
            PropertyInfo[] properties = typeof(T).GetProperties();

            //ЗАГОЛОВОК ТАБЛИЦЫ 
            sb.Append("<thead><tr>");
            foreach (var prop in properties)
            {
                if (prop.Name == "Id") continue;
                sb.Append($"<th>{prop.Name}</th>");
            }
            if (actionsBuilder != null)
            {
                sb.Append("<th>Действия</th>");
            }
            sb.Append("</tr></thead>");

            // ТЕЛО ТАБЛИЦЫ 
            sb.Append("<tbody>");
            foreach (T item in collection)
            {
                sb.Append("<tr>");
                foreach (var prop in properties)
                {
                    if (prop.Name == "Id") continue;
                    var value = prop.GetValue(item);
                    sb.Append($"<td>{value}</td>");
                }

                if (actionsBuilder != null)
                {
                    sb.Append($"<td>{actionsBuilder(item)}</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</tbody></table>");

            return sb.ToString();
        }

        public static string BuildForm<T>(string actionUrl, T? item = default)
        {
            var sb = new StringBuilder();
            sb.Append($"<form method='post' action='{actionUrl}'>");

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var prop in typeof(T).GetProperties())
            {
                var val = item != null ? prop.GetValue(item) : "";

                if (prop.Name == "Id")
                {
                    if (item != null)
                        sb.Append($"<input type='hidden' name='{prop.Name}' value='{val}' />");
                    continue;
                }

                sb.Append($@"<div class='mb-3'>
                    <label>{prop.Name}</label>
                    <input type='text' name='{prop.Name}' value='{val}' class='form-control' required />
                </div>");
            }

            sb.Append("<button type='submit' class='btn btn-success'>Сохранить</button></form>");
            return sb.ToString();
        }
    }
}
