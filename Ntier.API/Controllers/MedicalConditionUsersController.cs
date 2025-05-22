using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ntier.Business.Service;
using Ntier.Shared.Dtos;
using Ntier.Shared.Models;

namespace Ntier.API.Controllers;

[ApiController]
[Route("api/v1/medical-condition-users")]
[Authorize(Roles = "Admin,User")]
public class MedicalConditionUsersController : ControllerBase
{
    private readonly ILogger<MedicalConditionUsersController> _logger;
    private readonly IMedicalConditionUserService _medicalConditionUserService;

    public MedicalConditionUsersController(IMedicalConditionUserService medicalConditionUserService,
        ILogger<MedicalConditionUsersController> logger)
    {
        _medicalConditionUserService = medicalConditionUserService;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetMedicalConditionUsers([FromQuery] QueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetMedicalConditionUsers API called with parameters: {QueryParameters}",
            queryParameters);

        var medicalConditionUsers = await _medicalConditionUserService.GetFilteredMedicalConditionUsersAsync(
            queryParameters, cancellationToken);

        return Ok(new ApiResponse<List<MedicalConditionUser>>(medicalConditionUsers, true,
            "Medical condition users fetched successfully", StatusCodes.Status200OK));
    }

    [HttpPost("")]
    public async Task<IActionResult> AddMedicalConditionUser([FromBody] MedicalConditionUserDto medicalConditionUserDto,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("AddMedicalConditionUser API called with data: {MedicalConditionUserDto}",
            medicalConditionUserDto);

        var createdMedicalConditionUsers = await _medicalConditionUserService.AddMedicalConditionUserAsync(
            medicalConditionUserDto, cancellationToken);

        return CreatedAtAction(nameof(GetMedicalConditionUsers), new { id = createdMedicalConditionUsers },
            new ApiResponse<List<MedicalConditionUser>>(createdMedicalConditionUsers, true,
                "Medical condition users created successfully", StatusCodes.Status201Created));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMedicalConditionUser(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DeleteMedicalConditionUser API called for ID: {Id}", id);

        await _medicalConditionUserService.DeleteMedicalConditionUserAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetMedicalConditionUserById(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetMedicalConditionUserById API called for ID: {Id}", id);

        var medicalConditionUser = await _medicalConditionUserService.GetMedicalConditionUserByIdAsync(id,
            cancellationToken);

        return Ok(new ApiResponse<MedicalConditionUser>(medicalConditionUser, true,
            "Medical condition user fetched successfully", StatusCodes.Status200OK));
    }
}