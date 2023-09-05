using System.ComponentModel.DataAnnotations;

namespace WebApplication1;

public class ServiceConfiguration
{
    public string ContactEmail { get; set; }
    public int LowestPriority { get; set; }
    public int HighestPriority { get; set; }
    [Required]
    public string ApiKey { get; set; }
}