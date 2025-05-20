using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ntier.Business.Service;
using Ntier.Shared.Models;

namespace Ntier.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin,User")]
public class FoodNutrientsController : ControllerBase
{
    private readonly IFoodNutrientService _foodNutrientService;
    private readonly ILogger<FoodNutrientsController> _logger;

    public FoodNutrientsController(IFoodNutrientService foodNutrientService, ILogger<FoodNutrientsController> logger)
    {
        _foodNutrientService = foodNutrientService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetFoodNutrients([FromQuery] QueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetFoodNutrients API called with parameters: {QueryParameters}", queryParameters);

        var foodNutrients =
            await _foodNutrientService.GetFilteredFoodNutrientsAsync(queryParameters, cancellationToken);

        return Ok(foodNutrients);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetFoodNutrientById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetFoodNutrientById API called for ID: {Id}", id);

        var foodNutrient = await _foodNutrientService.GetFoodNutrientByIdAsync(id, cancellationToken);

        return Ok(foodNutrient);
    }

    [HttpPost("")]
    public async Task<IActionResult> AddFoodNutrient([FromBody] FoodNutrient foodNutrient,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("AddFoodNutrient API called with foodNutrient: {FoodNutrient}", foodNutrient);

        var createdFoodNutrient = await _foodNutrientService.AddFoodNutrientAsync(foodNutrient, cancellationToken);

        return CreatedAtAction(nameof(GetFoodNutrientById), new { id = createdFoodNutrient.Id }, createdFoodNutrient);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateFoodNutrient(int id, [FromBody] FoodNutrient foodNutrient,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("UpdateFoodNutrient API called for ID: {Id} with foodNutrient: {FoodNutrient}", id,
            foodNutrient);

        var updatedFoodNutrient =
            await _foodNutrientService.UpdateFoodNutrientAsync(id, foodNutrient, cancellationToken);

        return Ok(updatedFoodNutrient);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFoodNutrient(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DeleteFoodNutrient API called for ID: {Id}", id);

        await _foodNutrientService.DeleteFoodNutrientAsync(id, cancellationToken);

        return NoContent();
    }
}