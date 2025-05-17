using Ntier.Shared.Enums;

namespace Ntier.Shared.Dtos;

public record UserDto(string Email, string Password, Role Role);

public record class RegisterDto(string Email, string Password, string Name, DateTimeOffset? DateOfBirth, string? Avatar, float? Height, Gender Gender = Gender.Male);

public record LoginDto(string Email, string Password);

public record UserResponseDto(int Id, string Email, Role Role, string Name, DateTimeOffset? DateOfBirth, string? Avatar, float? Height, Gender Gender, bool IsDeleted, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);

public record class CreateUserDto(string Email, string Name, DateTimeOffset? DateOfBirth, string? Avatar, float? Height, Gender Gender = Gender.Male);
public record class UpdateUserDto(string Name, DateTimeOffset? DateOfBirth, string? Avatar, float? Height, Gender Gender = Gender.Male);