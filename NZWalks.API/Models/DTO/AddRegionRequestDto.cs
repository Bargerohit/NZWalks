using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage ="Code has to be of minimum 3 character")]
        [MaxLength(3, ErrorMessage = "Code has to be of maximum 3 character")]
        public string Code { get; set; }

        [Required]
        [MaxLength (100)]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
