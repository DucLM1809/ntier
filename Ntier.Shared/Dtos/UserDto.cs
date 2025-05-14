using Ntier.Shared.Enums;

namespace Ntier.Shared.Dtos;

public record UserDto(string Email, string Password, Role Role);

public record class RegisterDto(string Email, string Password, string Name, DateTimeOffset? DateOfBirth, string? Avatar, float? Height, Gender Gender = Gender.Male);

public record LoginDto(string Email, string Password);

public record UserResponseDto(string Email, Role Role);