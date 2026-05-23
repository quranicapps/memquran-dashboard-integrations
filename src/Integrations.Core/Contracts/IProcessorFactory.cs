namespace Integrations.Core.Contracts;

public interface IProcessorFactory<out T>
{
    IEnumerable<T> CreateProcessors();
}