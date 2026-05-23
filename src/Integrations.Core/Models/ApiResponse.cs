namespace Integrations.Core.Models;

public record ApiResponse<T>(
    T? Data,
    string? Message = null,
    object? Meta = null
);