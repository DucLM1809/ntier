namespace Ntier.Shared.Models;

public record ApiResponse<T>(T Data, bool Success, string Message, int StatusCode);
