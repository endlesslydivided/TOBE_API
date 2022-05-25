using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToBeApi.Models;

namespace ToBeApi.Data
{
    public class CategoriesConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData
            (
                new Category
                {
                    Id = new Guid("57E1FF2D-F351-4486-AF00-96FF3DA38D4B"),
                    Name = "No category"
                },
                new Category
                {
                    Id = new Guid("5066679A-C10B-403B-B471-2012950BC82C"),
                    Name = "Other"
                }
            );
        }
    }
}
