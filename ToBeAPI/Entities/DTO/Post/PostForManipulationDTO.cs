using System.ComponentModel.DataAnnotations;

namespace ToBeApi.Data.DTO
{
    public class PostForManipulationDTO
    {
        [StringLength(maximumLength: 255, ErrorMessage = "Maximum length for the Description is 255 characters")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is a required field")]
        [StringLength(maximumLength: 255, ErrorMessage = "Maximum length for the Description is 255 characters")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Content is a required field")]
        [StringLength(maximumLength: 255, ErrorMessage = "Maximum length for the Content is 5000 characters")]
        public string Content { get; set; }

        [Required(ErrorMessage = "UserId is a required field")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "CategoryId is a required field")]
        public Guid CategoryId { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }

    public class PostForCreateDTO : PostForManipulationDTO
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class PostForUpdateDTO : PostForManipulationDTO
    {

    }
}
