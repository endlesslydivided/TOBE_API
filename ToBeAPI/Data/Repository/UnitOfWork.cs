namespace ToBeApi.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDBContext _applicationDBContext;

        private IUserRepository _userRepository;
        private IPostRepository _postRepository;
        private ICategoryRepository _categoryRepository;

        public UnitOfWork(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public IUserRepository User { 
            get 
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_applicationDBContext);
                return _userRepository;
            } 
        }

        public IPostRepository Post
        {
            get
            {
                if (_postRepository == null)
                    _postRepository = new PostRepository(_applicationDBContext);
                return _postRepository;
            }
        }

        public ICategoryRepository Category
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new CategoryRepository(_applicationDBContext);
                return _categoryRepository;
            }
        }

        public Task SaveAsync() => _applicationDBContext.SaveChangesAsync();
    }
}
