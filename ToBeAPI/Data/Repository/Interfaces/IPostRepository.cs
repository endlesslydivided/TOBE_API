using ToBeApi.Models;
using ToBeApi.Models.RequestFeatures;

namespace ToBeApi.Data.Repository
{
    public interface IPostRepository
    {
        Task<PagedList<Post>> GetAllPostsAsync(PostParameters postParameters,bool trackChanges);
        Task<Post> GetPostAsync(Guid postId,bool trackChanges);
        Task<IEnumerable<Post>> GetPostsByCategoryAsync(Guid categoryId, bool trackChanges);
        void CreatePost(Post post);
        void DeletePost(Post post);

    }
}
