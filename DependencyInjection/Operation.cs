namespace DependencyInjection;

public class Operation : IOperationTransient, IOperationScoped, IOperationSingleton
{
    public Operation()
    {
        OperationId = Guid.NewGuid();
    }

    public Guid OperationId { get; }
}