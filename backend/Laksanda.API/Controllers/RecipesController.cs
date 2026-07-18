using Laksanda.API.Application.DTOs.Recipes;
using Laksanda.API.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laksanda.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RecipesController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipesController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RecipeDto>>> GetAll(CancellationToken cancellationToken)
    {
        var recipes = await _recipeService.GetAllAsync(cancellationToken);
        return Ok(recipes);
    }

    [HttpGet("{recipeId:guid}")]
    public async Task<ActionResult<RecipeDto>> GetById(Guid recipeId, CancellationToken cancellationToken)
    {
        var recipe = await _recipeService.GetByIdAsync(recipeId, cancellationToken);
        if (recipe is null)
        {
            return NotFound();
        }

        return Ok(recipe);
    }

    [HttpPost]
    public async Task<ActionResult<RecipeDto>> Create([FromBody] CreateRecipeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _recipeService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { recipeId = created.RecipeId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{recipeId:guid}")]
    public async Task<ActionResult<RecipeDto>> Update(Guid recipeId, [FromBody] UpdateRecipeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _recipeService.UpdateAsync(recipeId, request, cancellationToken);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{recipeId:guid}")]
    public async Task<IActionResult> Delete(Guid recipeId, CancellationToken cancellationToken)
    {
        var deleted = await _recipeService.DeleteAsync(recipeId, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
