using ToBeApi.Models;

namespace ToBeApi.Data.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {

        public UserRepository(ApplicationDBContext applicationDbContext ) 
            : base(applicationDbContext)
        {

        }
    }
}
