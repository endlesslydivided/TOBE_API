using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ToBeApi.Data.DTO;
using ToBeApi.Data.Repository;
using ToBeApi.Data.Shaper;
using ToBeApi.Filters;
using ToBeApi.Models;
using ToBeApi.Models.RequestFeatures;
using ToBeApi.Utility;

namespace ToBeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly ILogger<PostController> _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<PostDTO> _dataShaper;
        private readonly PostLinks _postLinks;

        public PostController(IUnitOfWork unit, ILogger<PostController> logger, IMapper mapper, IDataShaper<PostDTO> dataShaper, PostLinks postLinks) 
        {
            _unit = unit;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
            _postLinks = postLinks;
        }


        [HttpGet(Name = "AllPosts")]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))] 
        public async Task<IActionResult> GetPosts([FromQuery] PostParameters postParameters)
        {
            if (!postParameters.ValidCreatedDate)
                return BadRequest("Max creation date can't be less than min creation date.");


            var posts = await _unit.Post.GetAllPostsAsync(postParameters,trackChanges: true);

            Response.Headers.Add("X-Pagination",
            JsonConvert.SerializeObject(posts.MetaData));

            var postsDTO = _mapper.Map<IEnumerable<PostDTO>>(posts);
            var links = _postLinks.TryGenerateLinks(postsDTO, postParameters.Fields,HttpContext);


            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }

        [HttpGet("{id}",Name = "PostById")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            var post = await _unit.Post.GetPostAsync(id, trackChanges: false);
            if(post == null)
            {
                _logger.LogInformation($"Post with id: {id} doesn't exist in the database");
                return NotFound();
            }
            else
            {
                var postDTO = _mapper.Map<PostDTO>(post);
                return Ok(postDTO);
            }
        }

        [HttpPost(Name = "CreatePost")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreatePost([FromBody] PostForCreateDTO post)
        {       
            var postEntity = _mapper.Map<Post>(post);
            _unit.Post.CreatePost(postEntity);
            await _unit.SaveAsync();

            var companyToReturn = _mapper.Map<PostDTO>(postEntity);
            return CreatedAtRoute("CompanyById", new { id = companyToReturn.Id },companyToReturn);
        }

        [HttpDelete("{id}", Name = "DeletePost")]
        [ServiceFilter(typeof(ValidateExistsAttribute))]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            
            var post = HttpContext.Items["post"] as Post;
           
            _unit.Post.DeletePost(post);
            await _unit.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}",Name = "UpdatePost")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateExistsAttribute))]
        public async Task<IActionResult> UpdatePost(Guid postId,[FromBody] PostForUpdateDTO post)
        {
            var postEntity = HttpContext.Items["post"] as Post;

            _mapper.Map(post, postEntity);
            await _unit.SaveAsync();

            return NoContent();

        }

        [HttpPatch("{id}",Name = "PartiallUpdatePost")]
        [ServiceFilter(typeof(ValidateExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdatePostForCompany(Guid postId,[FromBody] JsonPatchDocument<PostForUpdateDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var postEntity = HttpContext.Items["post"] as Post;

            var postToPatch = _mapper.Map<PostForUpdateDTO>(postEntity);
            patchDoc.ApplyTo(postToPatch);

            TryValidateModel(postToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
                
            }
            _mapper.Map(postToPatch, postEntity);

            await _unit.SaveAsync();

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetPostsOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE, PATCH, OPTIONS, ");
            return Ok();
        }
    }
}
