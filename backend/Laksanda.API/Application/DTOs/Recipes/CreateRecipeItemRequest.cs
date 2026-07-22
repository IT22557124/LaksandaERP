using System.ComponentModel.DataAnnotations;

namespace Laksanda.API.Application.DTOs.Recipes;

public class CreateRecipeItemRequest
{
    [Required]
    public Guid RawMaterialId { get; set; }

    [Range(typeof(decimal), "0.01", "100")]
    public decimal Percentage { get; set; }

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal Quantity { get; set; }

    [Required]
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;
}
