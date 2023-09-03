using Microsoft.AspNetCore.Mvc;

namespace DependencyInjection.Controllers;

[ApiController]
[Route("[controller]")]
public class OperationController : ControllerBase
{
    private readonly OperationService _operationService;
    private readonly IOperationSingleton _operationSingleton;
    private readonly IOperationScoped _operationScoped;
    private readonly IOperationTransient _operationTransient;

    public OperationController(OperationService operationService, IOperationSingleton operationSingleton, IOperationScoped operationScoped, IOperationTransient operationTransient)
    {
        _operationService = operationService;
        _operationSingleton = operationSingleton;
        _operationScoped = operationScoped;
        _operationTransient = operationTransient;
    }

    [HttpGet]
    public ActionResult Get()
    {
        var model = new
        {
            ControllerDependencies = new
            {
                Transient = _operationTransient.OperationId,
                Scoped = _operationScoped.OperationId,
                Singleton = _operationSingleton.OperationId,
            },
            ServiceDependencies = new
            {
                Transient = _operationService.OperationTransient.OperationId,
               // Scoped = _operationService.OperationScoped.OperationId,
                Singleton = _operationService.OperationSingleton.OperationId
            }
        };

        return Ok(model);
    }
}