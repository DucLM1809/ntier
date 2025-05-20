using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ntier.Business.Service;
using Ntier.Shared.Models;

namespace Ntier.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize(Roles = "Admin,User")]
public class NutrientsController : ControllerBase
{
    private readonly ILogger<NutrientsController> _logger;
    private readonly INutrientService _nutrientService;

    public NutrientsController(INutrientService nutrientService, ILogger<NutrientsController> logger)
    {
        _nutrientService = nutrientService;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetNutrients([FromQuery] QueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetNutrients API called with parameters: {QueryParameters}", queryParameters);

        var nutrients = await _nutrientService.GetAllNutrientsAsync(queryParameters, cancellationToken);

        return Ok(new ApiResponse<List<Nutrient>>(nutrients, true, "Nutrients fetched successfully",
            StatusCodes.Status200OK));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetNutrientById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetNutrientById API called for ID: {Id}", id);

        var nutrient = await _nutrientService.GetNutrientByIdAsync(id, cancellationToken);

        return Ok(new ApiResponse<Nutrient>(nutrient, true, "Nutrient fetched successfully", StatusCodes.Status200OK));
    }

    [HttpPost("")]
    public async Task<IActionResult> AddNutrient([FromBody] Nutrient nutrient, CancellationToken cancellationToken)
    {
        _logger.LogInformation("AddNutrient API called with nutrient: {Nutrient}", nutrient);

        var createdNutrient = await _nutrientService.AddNutrientAsync(nutrient, cancellationToken);

        return CreatedAtAction(nameof(GetNutrientById), new { id = createdNutrient.Id },
            new ApiResponse<Nutrient>(createdNutrient, true, "Nutrient created successfully",
                StatusCodes.Status201Created));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateNutrient(int id, [FromBody] Nutrient nutrient,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("UpdateNutrient API called for ID: {Id} with nutrient: {Nutrient}", id, nutrient);

        var updatedNutrient = await _nutrientService.UpdateNutrientAsync(nutrient, cancellationToken);

        return Ok(new ApiResponse<Nutrient>(updatedNutrient, true, "Nutrient updated successfully",
            StatusCodes.Status200OK));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteNutrient(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DeleteNutrient API called for ID: {Id}", id);

        await _nutrientService.DeleteNutrientAsync(id, cancellationToken);

        return NoContent();
    }
}