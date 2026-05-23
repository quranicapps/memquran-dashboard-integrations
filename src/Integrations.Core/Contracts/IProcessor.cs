namespace Integrations.Core.Contracts;

public interface IProcessor<TResponse>
{
    Task<TResponse> ProcessAsync();
}