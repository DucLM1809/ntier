using Ntier.Shared.Enums;

namespace Ntier.Shared.Dtos;

public record UserDto(string Email, string Password, Role Role);

public record LoginDto(string Email, string Password);

public record UserResponseDto(string Email, Role Role);