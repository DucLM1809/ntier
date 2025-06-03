using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ntier.Business.Exceptions;
using Ntier.DataAccess.Repositories.Interfaces;
using Ntier.Shared.Models;

namespace Ntier.Business.Service;

public class FoodService : IFoodService
{
    private readonly ILogger<FoodService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public FoodService(ILogger<FoodService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Food>> GetFilteredFoodsAsync(QueryParameters queryParameters,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching filtered foods with parameters: {QueryParameters}", queryParameters);

        try
        {
            var foods = await _unitOfWork.Foods.GetAsync(null, queryParameters);


            return await foods.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching filtered foods with parameters: {QueryParameters}",
                queryParameters);
            throw;
        }
    }

    public async Task<Food> GetFoodByIdAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching food with ID: {FoodId}", id);

        try
        {
            var food = await _unitOfWork.Foods.GetByIdAsync(id);

            if (food == null)
            {
                _logger.LogWarning("Food with ID: {FoodId} not found.", id);
                throw new NotFoundException($"Food with ID: {id} not found.");
            }

            return food;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching food with ID: {FoodId}", id);
            throw;
        }
    }

    public async Task<Food> AddFoodAsync(Food food, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new food: {Food}", food);

        try
        {
            await _unitOfWork.Foods.AddAsync(food);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully added food with ID: {FoodId}", food.Id);

            return food;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding food: {Food}", food);
            throw;
        }
    }

    public async Task<Food> UpdateFoodAsync(int id, Food food, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating food with ID: {FoodId}", id);

        try
        {
            var existingFood = await _unitOfWork.Foods.GetByIdAsync(id);

            if (existingFood == null)
            {
                _logger.LogWarning("Food with ID: {FoodId} not found.", id);
                throw new NotFoundException($"Food with ID: {id} not found.");
            }

            food.Id = id;
            await _unitOfWork.Foods.UpdateAsync(food);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully updated food with ID: {FoodId}", id);

            return food;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating food with ID: {FoodId}", id);
            throw;
        }
    }

    public async Task DeleteFoodAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting food with ID: {FoodId}", id);

        try
        {
            var existingFood = await _unitOfWork.Foods.GetByIdAsync(id);

            if (existingFood == null)
            {
                _logger.LogWarning("Food with ID: {FoodId} not found.", id);
                throw new NotFoundException($"Food with ID: {id} not found.");
            }

            await _unitOfWork.Foods.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted food with ID: {FoodId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting food with ID: {FoodId}", id);
            throw;
        }
    }
}