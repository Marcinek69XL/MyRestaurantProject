using Microsoft.Extensions.Options;

namespace WebApplication1;

public class SampleService
{
    private readonly ServiceConfiguration _serviceConfOptions;

    public SampleService(IOptions<ServiceConfiguration> serviceConfOptions)
    {
        _serviceConfOptions = serviceConfOptions.Value;
    }

    public ServiceConfiguration GetOptions()
    {
        return _serviceConfOptions;
    }
}