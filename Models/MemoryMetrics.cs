namespace TopNet.Models;

/// <summary>
/// Represents memory usage metrics
/// </summary>
public class MemoryMetrics
{
    /// <summary>
    /// Gets or sets the total physical memory in bytes
    /// </summary>
    public ulong TotalPhysicalMemory { get; set; }
    
    /// <summary>
    /// Gets or sets the available physical memory in bytes
    /// </summary>
    public ulong AvailablePhysicalMemory { get; set; }
    
    /// <summary>
    /// Gets or sets the used physical memory in bytes
    /// </summary>
    public ulong UsedPhysicalMemory => TotalPhysicalMemory - AvailablePhysicalMemory;
    
    /// <summary>
    /// Gets the memory usage as a percentage (0-100)
    /// </summary>
    public float UsagePercentage => TotalPhysicalMemory > 0 
        ? (float)UsedPhysicalMemory / TotalPhysicalMemory * 100 
        : 0;
}
