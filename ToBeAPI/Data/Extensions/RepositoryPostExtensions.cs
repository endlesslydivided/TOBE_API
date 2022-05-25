using ToBeApi.Models;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using ToBeApi.Extensions.Utils;

namespace ToBeApi.Extensions
{
    public static class RepositoryPostExtensions
    {
        public static IQueryable<Post> FilterPosts(this IQueryable<Post> posts, 
        DateTime minCreatedAt, DateTime maxCreatedAt) =>
                posts.Where(p => p.CreatedAt.CompareTo(minCreatedAt) >= 0 && 
                            p.CreatedAt.CompareTo(maxCreatedAt) <= 0);

        public static IQueryable<Post> Search(this IQueryable<Post> posts,
            string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return posts;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return posts.Where(p => p.Title.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Post> Sort(this IQueryable<Post> posts,
            string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return posts.OrderBy(p => p.CreatedAt);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Post>(orderByQueryString);

            if(string.IsNullOrWhiteSpace(orderQuery))
                return posts.OrderBy(posts => posts.CreatedAt);

            return posts.OrderBy(orderQuery);
        }
    }
}
