using ExtensionAtributes;
using System.ComponentModel.DataAnnotations;

namespace Dtos.Post
{
    public class PostCreateDto
    {
        [AllowedImageContentTypes(
            new string[] { "image/jpg", "image/jpeg" },
            ErrorMessage = "Only JPG or JPEG image types are allowed.")]
        [MaximumImageSize(
            3,
            ErrorMessage = "The image size must not exceed 3 MB.")]
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string? Content { get; set; }
        public ICollection<int>? CategoryIds { get; set; } = new List<int>();

        public ICollection<int>? TagIds { get; set; } = new List<int>();
    }
}
