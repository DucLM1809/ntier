using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ntier.Business.Exceptions;
using Ntier.DataAccess.Repositories.Interfaces;
using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public class NutrientService : INutrientService
{
    private readonly ILogger<NutrientService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public NutrientService(ILogger<NutrientService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Nutrient>> GetAllNutrientsAsync(QueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all nutrients with parameters: {QueryParameters}", queryParameters);

        try
        {
            var nutrients = await _unitOfWork.Nutrients.GetAsync(null, queryParameters);


            return await nutrients.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching all nutrients with parameters: {QueryParameters}",
                queryParameters);
            throw;
        }
    }

    public async Task<Nutrient> GetNutrientByIdAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching nutrient with ID: {NutrientId}", id);

        try
        {
            var nutrient = await _unitOfWork.Nutrients.GetByIdAsync(id);

            if (nutrient == null)
            {
                _logger.LogWarning("Nutrient with ID: {NutrientId} not found.", id);
                throw new NotFoundException($"Nutrient with ID: {id} not found.");
            }

            return nutrient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching nutrient with ID: {NutrientId}", id);
            throw;
        }
    }

    public async Task<Nutrient> AddNutrientAsync(Nutrient nutrient, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new nutrient: {Nutrient}", nutrient);

        try
        {
            var addedNutrient = await _unitOfWork.Nutrients.AddAsync(nutrient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added nutrient with ID: {NutrientId}", addedNutrient.Id);

            return addedNutrient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding nutrient: {Nutrient}", nutrient);
            throw;
        }
    }

    public async Task<Nutrient> UpdateNutrientAsync(Nutrient nutrient, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating nutrient: {Nutrient}", nutrient);

        try
        {
            await _unitOfWork.Nutrients.UpdateAsync(nutrient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return nutrient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating nutrient: {Nutrient}", nutrient);
            throw;
        }
    }

    public async Task DeleteNutrientAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting nutrient with ID: {NutrientId}", id);

        try
        {
            var nutrient = await _unitOfWork.Nutrients.GetByIdAsync(id);

            if (nutrient == null)
            {
                _logger.LogWarning("Nutrient with ID: {NutrientId} not found.", id);
                throw new NotFoundException($"Nutrient with ID: {id} not found.");
            }

            await _unitOfWork.Nutrients.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted nutrient with ID: {NutrientId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting nutrient with ID: {NutrientId}", id);
            throw;
        }
    }
}