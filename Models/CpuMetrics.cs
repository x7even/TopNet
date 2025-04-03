namespace TopNet.Models;

/// <summary>
/// Represents CPU metrics including overall and per-core usage
/// </summary>
public class CpuMetrics
{
    /// <summary>
    /// Gets or sets the overall CPU usage as a percentage (0-100)
    /// </summary>
    public float TotalUsagePercentage { get; set; }
    
    /// <summary>
    /// Gets or sets the per-core CPU usage as percentages (0-100)
    /// </summary>
    public Dictionary<int, float> CoreUsagePercentages { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the number of logical processors (cores)
    /// </summary>
    public int ProcessorCount { get; set; }
    
    /// <summary>
    /// Gets or sets the processor name
    /// </summary>
    public string? ProcessorName { get; set; }
}
