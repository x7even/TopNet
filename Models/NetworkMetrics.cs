namespace TopNet.Models;

/// <summary>
/// Represents network usage metrics
/// </summary>
public class NetworkMetrics
{
    /// <summary>
    /// Gets or sets the network interfaces with their metrics
    /// </summary>
    public List<NetworkInterfaceMetrics> Interfaces { get; set; } = new();
    
    /// <summary>
    /// Gets the total bytes sent per second across all interfaces
    /// </summary>
    public float TotalBytesSentPerSec => Interfaces.Sum(i => i.BytesSentPerSec);
    
    /// <summary>
    /// Gets the total bytes received per second across all interfaces
    /// </summary>
    public float TotalBytesReceivedPerSec => Interfaces.Sum(i => i.BytesReceivedPerSec);
}

/// <summary>
/// Represents metrics for a specific network interface
/// </summary>
public class NetworkInterfaceMetrics
{
    /// <summary>
    /// Gets or sets the name of the network interface
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the description of the network interface
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Gets or sets the bytes sent per second
    /// </summary>
    public float BytesSentPerSec { get; set; }
    
    /// <summary>
    /// Gets or sets the bytes received per second
    /// </summary>
    public float BytesReceivedPerSec { get; set; }
}
