using Microsoft.EntityFrameworkCore;
using ToBeApi.Models;

namespace ToBeApi.Data.Repository
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDBContext applicationDBContext)
            : base(applicationDBContext)
        {

        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges) =>
           FindAll(trackChanges)
           .OrderBy(p => p.CreatedAt)
           .ToList();

        public async Task<Category> GetCategoryAsync(Guid categoryId, bool trackChanges) =>
            await FindByCondition(p => p.Id.Equals(categoryId), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
        await FindByCondition(x => ids.Contains(x.Id), trackChanges)
        .ToListAsync();

    }
}
