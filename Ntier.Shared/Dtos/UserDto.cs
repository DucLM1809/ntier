namespace Ntier.Shared.Dtos
{
    public record UserDto(string Email, string Password, string Role);

    public record UserResponseDto(string Email, string Role);
}
