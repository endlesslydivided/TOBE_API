using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ToBeApi.Data.Repository;

namespace ToBeApi.Filters
{
    public class ValidateExistsAttribute : IAsyncActionFilter
    {

        private readonly IUnitOfWork _unit;
        private readonly ILogger _logger;

        public ValidateExistsAttribute(IUnitOfWork unit, ILogger logger)
        {
            _unit = unit;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT");
            var controller = context.RouteData.Values["contoller"];
            var id = (Guid)context.ActionArguments["id"];

            switch (controller)
            {
                case "post":
                    {
                        var post = await _unit.Post.GetPostAsync(id, trackChanges);
                        if (post == null)
                        {
                            _logger.LogInformation($"Post with id: {id} doesn't exist in the database.");
                            context.Result = new NotFoundResult();
                        }
                        else
                        {
                            context.HttpContext.Items.Add("post", post);
                            await next();
                        }
                        break;
                    }
                case "category":
                    {
                        var category = await _unit.Category.GetCategoryAsync(id, trackChanges);
                        if (category == null)
                        {
                            _logger.LogInformation($"Category with id: {id} doesn't exist in the database.");
                            context.Result = new NotFoundResult();
                        }
                        else
                        {
                            context.HttpContext.Items.Add("category", category);
                            await next();
                        }
                        break;
                    }
            }

          
        }
    }
}
