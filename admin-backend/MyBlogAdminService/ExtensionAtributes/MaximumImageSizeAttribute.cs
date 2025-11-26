using System.ComponentModel.DataAnnotations;

namespace ExtensionAtributes
{
    public class MaximumImageSizeAttribute : ValidationAttribute
    {
        int _maxSizeInMB;
        public MaximumImageSizeAttribute(int maxSizeInMB)
        {
            _maxSizeInMB = maxSizeInMB;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file == null) { return ValidationResult.Success; }
            return (file?.Length / (1024 * 1024)) <= _maxSizeInMB // 3
                ? ValidationResult.Success : new ValidationResult(ErrorMessage ?? $"The file size exceeds the maximum allowed size of {_maxSizeInMB} MB.");
        }
    }
}