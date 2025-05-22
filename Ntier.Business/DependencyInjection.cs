using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Ntier.Business.Mappings;
using Ntier.Business.Service;
using Ntier.Business.Validators;

namespace Ntier.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFoodService, FoodService>();
        services.AddScoped<INutrientService, NutrientService>();
        services.AddScoped<IFoodNutrientService, FoodNutrientService>();
        services.AddScoped<IMedicalConditionUserService, MedicalConditionUserService>();

        // Register AutoMapper
        services.AddAutoMapper(typeof(UserProfile));

        // Register FluentValidation
        services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateUserDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<MedicalConditionUserDtoValidator>();

        return services;
    }
}