namespace Integrations.Core;

public record ApiResponse<T>(
    T? Data,
    string? Message = null,
    object? Meta = null
);