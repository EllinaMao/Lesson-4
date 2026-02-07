using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IUserRepository, UserRepository>();
var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
/*
 
Создать класс «User». Определить интерфейс и репозиторий по управлению пользователями, с доступными действиями: добавить, удалить, получить конкретного пользователя, редактировать, вывести всех пользователей. 
Создать веб-сайт по управлению этими пользователями на несколько страниц (без базы данных, использовать подходящий жизненный цикл сервиса для полноценной работы в одном сеансе).
Весь код можно писать в классе Program.cs или использовать отдельные представления. Обработать возможные ошибочные ситуации, к примеру передачу неверного Id (как в формате так и в плане существования).

 */
app.Run();

public record User
{
    public  Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Email { get; set; }

    public User(string name, string email)
    {
        Name = name;
        Email = email;
    }

}
public interface IUserRepository
{
    void Add(User user);
    void Delete(Guid id);
    User? Get(Guid id);
    void Edit(User user);
    IEnumerable<User> GetAll();
}

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new();
    private readonly Lock _lock = new();

    public IEnumerable<User> GetAll()
    {
        lock (this._lock)
        {
            return _users.ToList(); 
        }
    }


    /** 
         * Получить пользователя по ID. Если пользователь не найден, возвращает null.
         * @param id Идентификатор пользователя.
         * @return Пользователь с указанным ID или null, если пользователь не найден.
         */

    public User? Get(Guid id)
    {
        lock (this._lock)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }
    }

    public void Add(User user)
    {
        if (user == null) throw new ArgumentException("Пользователь пуст");

        lock (_lock)
        {
            if (_users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"Email {user.Email} уже занят.");

            _users.Add(user);
        }
    }

    public void Edit(User user)
    {
        lock (_lock)
        {
            var index = _users.FindIndex(u => u.Id == user.Id);
            if (index == -1)
                throw new ArgumentException($"Пользователь с ID {user.Id} не найден.");

            _users[index] = user;
        }
    }

    public void Delete(Guid id)
    {
        lock (_lock)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                throw new ArgumentException($"Пользователь с ID {id} не найден.");
            _users.Remove(user);
        }
        }
}
