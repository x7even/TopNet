namespace TopNet.Models;

/// <summary>
/// Represents a complete snapshot of system metrics
/// </summary>
public class SystemMetrics
{
    /// <summary>
    /// Gets or sets the CPU metrics
    /// </summary>
    public CpuMetrics Cpu { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the memory metrics
    /// </summary>
    public MemoryMetrics Memory { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the disk metrics
    /// </summary>
    public DiskMetrics Disk { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the network metrics
    /// </summary>
    public NetworkMetrics Network { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the timestamp when these metrics were collected
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Gets or sets the system uptime
    /// </summary>
    public TimeSpan Uptime { get; set; }
    
    /// <summary>
    /// Gets or sets the system name
    /// </summary>
    public string? SystemName { get; set; }
}
