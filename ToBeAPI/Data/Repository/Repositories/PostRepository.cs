using Microsoft.EntityFrameworkCore;
using ToBeApi.Extensions;
using ToBeApi.Models;
using ToBeApi.Models.RequestFeatures;

namespace ToBeApi.Data.Repository
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(ApplicationDBContext applicationDBContext)
            : base(applicationDBContext)
        {

        }

        public async Task<PagedList<Post>> GetAllPostsAsync(PostParameters postParameters, bool trackChanges) 
        {
            var posts = await FindAll(trackChanges)
               .FilterPosts(postParameters.MinCreatedAt, postParameters.MaxCreatedAt)
               .Search(postParameters.SearchTerm)
               .OrderBy(p => p.CreatedAt)
               .Skip((postParameters.PageNumber - 1) * postParameters.PageSize)
               .Take(postParameters.PageSize)
               .ToListAsync();

            return new PagedList<Post>(posts, Count(), postParameters.PageNumber, postParameters.PageSize);
        }
            

        public async Task<Post> GetPostAsync(Guid postId, bool trackChanges) =>
            await FindByCondition(p => p.Id.Equals(postId), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Post>> GetPostsByCategoryAsync(Guid categoryId, bool trackChanges) =>
            await FindByCondition(p => p.CategoryId.Equals(categoryId), trackChanges)
            .OrderBy(p => p.CreatedAt).ToListAsync();


        public void DeletePost(Post post) => Delete(post);

        public void CreatePost(Post post) => Create(post);

    }
}
