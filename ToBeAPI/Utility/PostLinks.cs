using Microsoft.Net.Http.Headers;
using ToBeApi.Data.DTO;
using ToBeApi.Data.Shaper;
using ToBeApi.Entities.LinkModels;
using ToBeApi.Entities.Models;
using ToBeApi.Models;
using ToBeApi.Models.LinkModels;

namespace ToBeApi.Utility
{
    public class PostLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<PostDTO> _dataShaper;

        public PostLinks(LinkGenerator linkGenerator, IDataShaper<PostDTO> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<PostDTO> postsDTO, string fields,
            HttpContext httpContext)
        {
            var shapedPosts = ShapeData(postsDTO, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkedPosts(postsDTO, fields, httpContext, shapedPosts);

            return ReturnShapedPosts(shapedPosts);

        }

        private List<Entity> ShapeData(IEnumerable<PostDTO> postsDTO, string fields) =>
            _dataShaper.ShapeData(postsDTO,fields)
                .Select(p => p.Entity)
                .ToList();

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas",
                StringComparison.InvariantCultureIgnoreCase);
        }

        private LinkResponse ReturnShapedPosts(List<Entity> shapedEmployees) => new
        LinkResponse { ShapedEntities = shapedEmployees };

        private LinkResponse ReturnLinkedPosts(IEnumerable<PostDTO> postsDTO, string fields,
            HttpContext httpContext, List<Entity> shapedPosts)
        {
            var postsDTOList = postsDTO.ToList();

            for (var index = 0; index < postsDTOList.Count(); index++) 
            {
                var postsLinks = CreateLinksForPosts(httpContext, postsDTOList[index].Id, fields);
                shapedPosts[index].Add("Links", postsLinks);
            }

            var postsCollection = new LinkCollectionWrapper<Entity>(shapedPosts);
            var linkedPosts = CreateLinksForPosts(httpContext, postsCollection);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedPosts };
        }

        private List<Link> CreateLinksForPosts (HttpContext httpContext, Guid id, string fields ="")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(httpContext, "GetPost", values: new {id,fields}),
                "self",
                "GET"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "DeletePost", values: new {id}),
                "delete_post",
                "DELETE"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "UpdatePost", values: new {id}),
                "update_post",
                "PUT"),
                new Link(_linkGenerator.GetUriByAction(httpContext, "DeletePost", values: new {id}),
                "partially_update_post",
                "PATCH")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForPosts(HttpContext httpContext,LinkCollectionWrapper<Entity> postsWrapper)
        {
            postsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext,
           "GetPost", values: new { }),
            "self",
            "GET"));
            return postsWrapper;
        }


    }
}

