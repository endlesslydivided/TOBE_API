using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToBeApi.Models;

namespace ToBeApi.Data
{
    public class PostsConfiguration : IEntityTypeConfiguration<Post>
    {
            public void Configure(EntityTypeBuilder<Post> builder)
            {
            builder.HasData
            (
                new Post
                {
                    Id = new Guid("2D4B850F-C79E-4BF6-8C0B-71A36CCC508D"),
                    Description = "First post description",
                    Title = "First post",
                    Content = "Hello world 1",
                    UserId = "1E35FEF4-04DA-466A-AE8C-D6B8D70731C5",
                    CategoryId = new Guid("57E1FF2D-F351-4486-AF00-96FF3DA38D4B")
                },
                new Post
                {
                    Id = new Guid("7D8F9ECB-F5BF-4488-B498-9AB1DAB6DE59"),
                    Description = "Second post description",
                    Title = "Second post",
                    Content = "Hello world 2",
                    UserId = "1991E6CD-2D03-4649-9CEE-C0A6998C7544",
                    CategoryId = new Guid("5066679A-C10B-403B-B471-2012950BC82C")
                }
            ); ;
            }
    }
}
