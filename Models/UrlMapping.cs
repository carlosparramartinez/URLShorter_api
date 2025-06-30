using System.ComponentModel.DataAnnotations;
namespace UrlShortenerAPI.Models
{
    public class UrlMappingRequest
    {
        [Required]
        public string Url { get; set; } = string.Empty;
    }


    public class UrlMapping
    {
        public int Id { get; set; }

        [Required]
        public string Url { get; set; } = string.Empty;

        [Required]
        public string ShortCode { get; set; }= string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int AccessCount { get; set; } = 0;
    }
}