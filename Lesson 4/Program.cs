using System.Collections.Concurrent;
using Microsoft.AspNetCore.Server.IISIntegration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IUserRepository, UserRepository>();

var app = builder.Build();


app.UseExceptionHandler("/error");

app.Map("/error", branch =>
{
    branch.UseMiddleware<ErrorMiddleware>();
});
app.Map("/create", appBuild =>
{
    appBuild.UseMiddleware<CreateMiddleware>();
});

app.Map("/edit", appBuild =>
{
    appBuild.UseMiddleware<EditMiddleware>();
});

app.Map("/delete", appBuild =>
{
    appBuild.UseMiddleware<DeleteMiddleware>();
});

app.UseMiddleware<ListMiddleware>();

app.Run();

/*
 
Создать класс «User». Определить интерфейс и репозиторий по управлению пользователями, с доступными действиями: добавить, удалить, получить конкретного пользователя, редактировать, вывести всех пользователей. 
Создать веб-сайт по управлению этими пользователями на несколько страниц (без базы данных, использовать подходящий жизненный цикл сервиса для полноценной работы в одном сеансе).
Весь код можно писать в классе Program.cs или использовать отдельные представления. Обработать возможные ошибочные ситуации, к примеру передачу неверного Id (как в формате так и в плане существования).

 */
