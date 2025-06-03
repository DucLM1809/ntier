using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ntier.Business.Exceptions;
using Ntier.DataAccess.Repositories.Interfaces;
using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public class FoodNutrientService : IFoodNutrientService
{
    private readonly ILogger<FoodNutrientService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public FoodNutrientService(ILogger<FoodNutrientService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<FoodNutrient>> GetFilteredFoodNutrientsAsync(QueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching filtered food nutrients with parameters: {QueryParameters}", queryParameters);

        try
        {
            var foodNutrients = await _unitOfWork.FoodNutrients.GetAsync(null, queryParameters,
                new List<Expression<Func<FoodNutrient, object>>>
                {
                    fn => fn.Food
                }
            );

            return await foodNutrients.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "An error occurred while fetching filtered food nutrients with parameters: {QueryParameters}",
                queryParameters);
            throw;
        }
    }

    public async Task<FoodNutrient> GetFoodNutrientByIdAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching food nutrient with ID: {FoodNutrientId}", id);

        try
        {
            var foodNutrient = await _unitOfWork.FoodNutrients.GetByIdAsync(id);

            if (foodNutrient == null)
            {
                _logger.LogWarning("Food nutrient with ID: {FoodNutrientId} not found.", id);
                throw new BadRequestException($"Food nutrient with ID: {id} not found.");
            }

            return foodNutrient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching food nutrient with ID: {FoodNutrientId}", id);
            throw;
        }
    }

    public async Task<FoodNutrient> AddFoodNutrientAsync(FoodNutrient foodNutrient, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new food nutrient: {FoodNutrient}", foodNutrient);

        try
        {
            var addedFoodNutrient = await _unitOfWork.FoodNutrients.AddAsync(foodNutrient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added food nutrient with ID: {FoodNutrientId}",
                addedFoodNutrient.Id);

            return addedFoodNutrient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding food nutrient: {FoodNutrient}", foodNutrient);
            throw;
        }
    }

    public async Task<FoodNutrient> UpdateFoodNutrientAsync(int id, FoodNutrient foodNutrient,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating food nutrient with ID: {FoodNutrientId}", id);

        try
        {
            var existingFoodNutrient = await _unitOfWork.FoodNutrients.GetByIdAsync(id);

            if (existingFoodNutrient == null)
            {
                _logger.LogWarning("Food nutrient with ID: {FoodNutrientId} not found.", id);
                throw new NotFoundException($"Food nutrient with ID: {id} not found.");
            }

            await _unitOfWork.FoodNutrients.UpdateAsync(foodNutrient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated food nutrient with ID: {FoodNutrientId}", id);

            return existingFoodNutrient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating food nutrient with ID: {FoodNutrientId}", id);
            throw;
        }
    }

    public async Task DeleteFoodNutrientAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting food nutrient with ID: {FoodNutrientId}", id);

        try
        {
            var existingFoodNutrient = await _unitOfWork.FoodNutrients.GetByIdAsync(id);

            if (existingFoodNutrient == null)
            {
                _logger.LogWarning("Food nutrient with ID: {FoodNutrientId} not found.", id);
                throw new NotFoundException($"Food nutrient with ID: {id} not found.");
            }

            await _unitOfWork.FoodNutrients.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted food nutrient with ID: {FoodNutrientId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting food nutrient with ID: {FoodNutrientId}", id);
            throw;
        }
    }
}