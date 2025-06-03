using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ntier.Business.Exceptions;
using Ntier.DataAccess.Repositories.Interfaces;
using Ntier.Shared.Dtos;
using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public class MedicalConditionUserService : IMedicalConditionUserService
{
    private readonly ILogger<MedicalConditionUserService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public MedicalConditionUserService(ILogger<MedicalConditionUserService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<MedicalConditionUser>> GetFilteredMedicalConditionUsersAsync(
        QueryParameters queryParameters, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching filtered medical condition users with parameters: {QueryParameters}",
            queryParameters);

        try
        {
            var medicalConditionUsers = await _unitOfWork.MedicalConditionUsers.GetAsync(null, queryParameters);


            return await medicalConditionUsers.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "An error occurred while fetching filtered medical condition users with parameters: {QueryParameters}",
                queryParameters);
            throw;
        }
    }

    public async Task<List<MedicalConditionUser>> AddMedicalConditionUserAsync(
        MedicalConditionUserDto medicalConditionUserDto,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new medical condition user with data: {MedicalConditionUserDto}",
            medicalConditionUserDto);

        try
        {
            var existingUser = await _unitOfWork.Users.GetByIdAsync(medicalConditionUserDto.UserId);
            if (existingUser == null)
            {
                _logger.LogWarning("User with ID: {UserId} not found.", medicalConditionUserDto.UserId);
                throw new BadRequestException($"User with ID: {medicalConditionUserDto.UserId} not found.");
            }

            var normalizedMedicalConditionUsers = new List<MedicalConditionUser>();

            foreach (var medicalConditionUser in medicalConditionUserDto.MedicalConditionIds)
                normalizedMedicalConditionUsers.Add(new MedicalConditionUser
                    {
                        UserId = medicalConditionUserDto.UserId,
                        MedicalConditionId = medicalConditionUser
                    }
                );

            await _unitOfWork.MedicalConditionUsers.AddRange(normalizedMedicalConditionUsers);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return normalizedMedicalConditionUsers;
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                "An error occurred while adding new medical condition user with data: {MedicalConditionUserDto}",
                medicalConditionUserDto);
            throw;
        }
    }

    public async Task DeleteMedicalConditionUserAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting medical condition user with ID: {MedicalConditionUserId}", id);

        try
        {
            var medicalConditionUser = _unitOfWork.MedicalConditionUsers.GetByIdAsync(id);

            if (medicalConditionUser == null)
            {
                _logger.LogWarning("Medical condition user with ID: {MedicalConditionUserId} not found.", id);
                throw new NotFoundException($"Medical condition user with ID: {id} not found.");
            }

            await _unitOfWork.MedicalConditionUsers.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted medical condition user with ID: {MedicalConditionUserId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "An error occurred while deleting medical condition user with ID: {MedicalConditionUserId}",
                id);
            throw;
        }
    }

    public async Task<MedicalConditionUser> GetMedicalConditionUserByIdAsync(int id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching medical condition user with ID: {MedicalConditionUserId}", id);

        try
        {
            var medicalConditionUser = await _unitOfWork.MedicalConditionUsers.GetByIdAsync(id);

            if (medicalConditionUser == null)
            {
                _logger.LogWarning("Medical condition user with ID: {MedicalConditionUserId} not found.", id);
                throw new NotFoundException($"Medical condition user with ID: {id} not found.");
            }

            return medicalConditionUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "An error occurred while fetching medical condition user with ID: {MedicalConditionUserId}",
                id);
            throw;
        }
    }
}