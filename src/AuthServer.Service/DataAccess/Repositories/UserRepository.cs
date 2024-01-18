namespace AuthServer.Service.DataAccess.Repositories;

public class UserRepository : Repository<AuthDbContext, User, Guid>, IUserRepository
{
    public UserRepository(AuthDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}