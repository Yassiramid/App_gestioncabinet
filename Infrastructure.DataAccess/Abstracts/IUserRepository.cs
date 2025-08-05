using Infrastructure.DataAccess.Entities;

namespace Infrastructure.DataAccess.Abstracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        bool login(string user, string pass);
    }
}
