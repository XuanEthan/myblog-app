using System.ComponentModel.DataAnnotations;

namespace ExtensionAtributes
{
    public class AllowedImageContentTypes : ValidationAttribute
    {
        private readonly string[] _contentTypes;
        public AllowedImageContentTypes(string[] contentTypes)
        {
            _contentTypes = contentTypes;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file == null) { return ValidationResult.Success; }
            return _contentTypes.Contains(file?.ContentType)
                ? ValidationResult.Success
                : new ValidationResult(ErrorMessage ?? $"The file type is not allowed. Allowed types are: {string.Join(", ", _contentTypes)}");

        }
    }
}
