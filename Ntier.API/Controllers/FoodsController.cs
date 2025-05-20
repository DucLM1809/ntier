using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ntier.Business.Service;
using Ntier.Shared.Models;

namespace Ntier.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin,User")]
public class FoodsController : ControllerBase
{
    private readonly IFoodService _foodService;
    private readonly ILogger<FoodsController> _logger;

    public FoodsController(IFoodService foodService, ILogger<FoodsController> logger)
    {
        _foodService = foodService;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetFoods([FromQuery] QueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetFoods API called with parameters: {QueryParameters}", queryParameters);

        var foods = await _foodService.GetFilteredFoodsAsync(queryParameters, cancellationToken);

        return Ok(new ApiResponse<List<Food>>(foods, true, "Foods fetched successfully", StatusCodes.Status200OK));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetFoodById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetFoodById API called for ID: {Id}", id);

        var food = await _foodService.GetFoodByIdAsync(id, cancellationToken);

        return Ok(new ApiResponse<Food>(food, true, "Food fetched successfully", StatusCodes.Status200OK));
    }

    [HttpPost("")]
    public async Task<IActionResult> AddFood([FromBody] Food food, CancellationToken cancellationToken)
    {
        _logger.LogInformation("AddFood API called with food: {Food}", food);

        var createdFood = await _foodService.AddFoodAsync(food, cancellationToken);

        return CreatedAtAction(nameof(GetFoodById), new { id = createdFood.Id },
            new ApiResponse<Food>(createdFood, true, "Food created successfully", StatusCodes.Status201Created));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateFood(int id, [FromBody] Food food, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UpdateFood API called for ID: {Id} with food: {Food}", id, food);

        var updatedFood = await _foodService.UpdateFoodAsync(id, food, cancellationToken);

        return Ok(new ApiResponse<Food>(updatedFood, true, "Food updated successfully", StatusCodes.Status200OK));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteFood(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DeleteFood API called for ID: {Id}", id);

        await _foodService.DeleteFoodAsync(id, cancellationToken);

        return NoContent();
    }
}