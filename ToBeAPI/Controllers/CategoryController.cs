using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToBeApi.Data;
using ToBeApi.Data.DTO;
using ToBeApi.Data.DTO.Category;
using ToBeApi.Data.Repository;
using ToBeApi.Filters;
using ToBeApi.Models;

namespace ToBeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unit, ILogger<CategoryController> logger,
            IMapper mapper)
        {
            _unit = unit;
            _logger = logger;
            _mapper = mapper;

        }

        [HttpGet("/{categoryId}/posts",Name = "PostsByCategory")]
        [ServiceFilter(typeof(ValidateExistsAttribute))]
        public async  Task<IActionResult> GetPostsByCategory(Guid categoryId)
        {
            var category = HttpContext.Items["category"] as Category;
            var postsByCategory = await _unit.Post.GetPostsByCategoryAsync(categoryId, trackChanges: false);
            var postsDTO = _mapper.Map<IEnumerable<PostDTO>>(postsByCategory);

            return Ok(postsDTO);
        }

        [HttpGet("categories/({ids})", Name = "CategoryCollection")]
        public async Task<IActionResult> GetCategoryCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var categoryEntities = await _unit.Category.GetByIdsAsync(ids, trackChanges: false);
            if (ids.Count() != categoryEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var categoriesToReturn = _mapper.Map<IEnumerable<CategoryDTO>>(categoryEntities);
            return Ok(categoriesToReturn);
        }


    }
}
