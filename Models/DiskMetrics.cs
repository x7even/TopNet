namespace TopNet.Models;

/// <summary>
/// Represents disk I/O metrics
/// </summary>
public class DiskMetrics
{
    /// <summary>
    /// Gets or sets the disk read rate in bytes per second
    /// </summary>
    public float ReadBytesPerSec { get; set; }
    
    /// <summary>
    /// Gets or sets the disk write rate in bytes per second
    /// </summary>
    public float WriteBytesPerSec { get; set; }
    
    /// <summary>
    /// Gets or sets the disk read operations per second
    /// </summary>
    public float ReadOperationsPerSec { get; set; }
    
    /// <summary>
    /// Gets or sets the disk write operations per second
    /// </summary>
    public float WriteOperationsPerSec { get; set; }
    
    /// <summary>
    /// Gets or sets the disk information including drive letters and space
    /// </summary>
    public List<DiskInfo> Disks { get; set; } = new();
}

/// <summary>
/// Represents information about a disk drive
/// </summary>
public class DiskInfo
{
    /// <summary>
    /// Gets or sets the drive name (e.g., "C:")
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Gets or sets the total size of the drive in bytes
    /// </summary>
    public ulong TotalSize { get; set; }
    
    /// <summary>
    /// Gets or sets the free space on the drive in bytes
    /// </summary>
    public ulong FreeSpace { get; set; }
    
    /// <summary>
    /// Gets the used space on the drive in bytes
    /// </summary>
    public ulong UsedSpace => TotalSize - FreeSpace;
    
    /// <summary>
    /// Gets the usage percentage of the drive (0-100)
    /// </summary>
    public float UsagePercentage => TotalSize > 0 
        ? (float)UsedSpace / TotalSize * 100 
        : 0;
}
