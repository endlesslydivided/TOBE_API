namespace ToBeApi.Data.Repository
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IPostRepository Post { get; }
        ICategoryRepository Category { get; }
        Task SaveAsync();

    }
}
