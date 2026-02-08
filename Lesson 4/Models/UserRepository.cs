
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
