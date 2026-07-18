using System.ComponentModel.DataAnnotations;

namespace Laksanda.API.Application.DTOs.Recipes;

public class CreateRecipeItemRequest
{
    [Required]
    public Guid RawMaterialId { get; set; }

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal Quantity { get; set; }
}
