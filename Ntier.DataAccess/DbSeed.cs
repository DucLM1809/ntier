using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Ntier.Shared.Dtos;
using Ntier.Shared.Models;

namespace Ntier.DataAccess;

public static class DbSeed
{
    public static void SeedFood(DataContext context, ILogger logger)
    {
        var json = File.ReadAllText("../Ntier.DataAccess/Data/Nutrients.json");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        var foods = JsonSerializer.Deserialize<List<FoodDto>>(json, options);

        if (!context.Foods.Any())
            try
            {
                logger.LogInformation("Seeding food data...");

                // Normalize food data
                var normalizedFoods = new List<Food>();

                // Normalize food nutrients data
                var normalizedNutrients = new List<Nutrient>();

                foreach (var food in foods!)
                {
                    normalizedFoods.Add(new Food
                    {
                        Name = food.Description,
                        Description = food.Description,
                        Source = "fdc (FNDDS)"
                    });

                    // Check if nutrient is not existed then add to list
                    foreach (var foodNutrient in food.FoodNutrients)
                        if (!normalizedNutrients.Any(n => n.Name == foodNutrient.Name))
                            normalizedNutrients.Add(new Nutrient
                            {
                                Name = foodNutrient.Name,
                                Unit = foodNutrient.UnitName
                            });
                }

                context.Foods.AddRange(normalizedFoods!);
                context.Nutrients.AddRange(normalizedNutrients);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError("Error seeding food data: {Message}", ex.Message);
                logger.LogError("Stack Trace: {StackTrace}", ex.StackTrace);

                throw;
            }
        else
            logger.LogInformation("Food data already seeded.");

        if (!context.FoodNutrients.Any())
            try
            {
                logger.LogInformation("Seeding food nutrient data...");


                // Normalize food nutrients data
                var normalizedFoodNutrients = new List<FoodNutrient>();

                foreach (var food in foods!)
                {
                    var existingFood = context.Foods.FirstOrDefault(f => f.Name == food.Description);
                    var existingNutrients = context.Nutrients.ToList();

                    foreach (var foodNutrient in food.FoodNutrients)
                    {
                        var existingNutrient = existingNutrients.FirstOrDefault(n => n.Name == foodNutrient.Name);

                        if (existingFood != null && existingNutrient != null)
                            normalizedFoodNutrients.Add(new FoodNutrient
                            {
                                FoodId = existingFood.Id,
                                NutrientId = existingNutrient.Id,
                                Amount = foodNutrient.Amount
                            });
                    }
                }

                context.FoodNutrients.AddRange(normalizedFoodNutrients!);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError("Error seeding food nutrient data: {Message}", ex.Message);
                logger.LogError("Stack Trace: {StackTrace}", ex.StackTrace);

                throw;
            }
        else
            logger.LogInformation("Food nutrient data already seeded.");
    }

    public static void SeedMedicalConditions(DataContext context, ILogger logger)
    {
        logger.LogInformation("Seeding medical conditions...");

        var json = File.ReadAllText("../Ntier.DataAccess/Data/DietaryRestrictions.json");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        var dietaryRestrictions = JsonSerializer.Deserialize<List<DietaryRestrictionDto>>(json, options);

        var normalizedMedicalConditions = new List<MedicalCondition>();

        if (dietaryRestrictions != null)
            foreach (var dietaryRestriction in dietaryRestrictions)
                normalizedMedicalConditions.Add(new MedicalCondition
                {
                    Name = dietaryRestriction.MedicalCondition.Name,
                    Description = dietaryRestriction.MedicalCondition.Description
                });

        if (!context.MedicalConditions.Any())
            try
            {
                context.MedicalConditions.AddRange(normalizedMedicalConditions);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError("Error seeding medical conditions: {Message}", e.Message);
                logger.LogError("Stack Trace: {StackTrace}", e.StackTrace);
                throw;
            }
        else
            logger.LogInformation("Medical conditions already seeded.");
    }

    public static void SeedDietRestriction(DataContext context, ILogger logger)
    {
        logger.LogInformation("Seeding medical conditions...");

        var json = File.ReadAllText("../Ntier.DataAccess/Data/DietaryRestrictions.json");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };

        var dietaryRestrictions = JsonSerializer.Deserialize<List<DietaryRestrictionDto>>(json, options);

        var normalizedDietRestrictions = new List<DietRestriction>();

        if (dietaryRestrictions != null)
            foreach (var dietaryRestriction in dietaryRestrictions)
                normalizedDietRestrictions.Add(new DietRestriction
                {
                    High = string.Join(", ", dietaryRestriction.DietaryRestrictions.High),
                    Low = string.Join(", ", dietaryRestriction.DietaryRestrictions.Low),
                    Avoid = string.Join(", ", dietaryRestriction.DietaryRestrictions.Avoid)
                });

        if (!context.DietRestrictions.Any())
            try
            {
                context.DietRestrictions.AddRange(normalizedDietRestrictions);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                logger.LogError("Error seeding diet restrictions: {Message}", e.Message);
                logger.LogError("Stack Trace: {StackTrace}", e.StackTrace);
                throw;
            }
        else
            logger.LogInformation("Diet restrictions already seeded.");
    }
}