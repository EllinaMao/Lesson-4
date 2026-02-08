

public interface IUserRepository
{
    void Add(User user);
    void Delete(Guid id);
    User? Get(Guid id);
    void Edit(User user);
    IEnumerable<User> GetAll();
}
