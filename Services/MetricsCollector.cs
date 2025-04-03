using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using TopNet.Models;

namespace TopNet.Services;

/// <summary>
/// Service for collecting system metrics
/// </summary>
public class MetricsCollector
{
    // Performance counters
    private PerformanceCounter? _cpuCounter;
    private Dictionary<int, PerformanceCounter> _cpuCoreCounters = new();
    private PerformanceCounter? _diskReadCounter;
    private PerformanceCounter? _diskWriteCounter;
    private PerformanceCounter? _diskReadOpsCounter;
    private PerformanceCounter? _diskWriteOpsCounter;
    private Dictionary<string, (PerformanceCounter Sent, PerformanceCounter Received)> _networkCounters = new();
    
    // Previous values for rate calculations
    private DateTime _lastCollectionTime = DateTime.Now;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="MetricsCollector"/> class
    /// </summary>
    public MetricsCollector()
    {
        InitializeCounters();
    }
    
    /// <summary>
    /// Initializes the performance counters
    /// </summary>
    private void InitializeCounters()
    {
        try
        {
            // CPU total counter
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            
            // CPU core counters
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                _cpuCoreCounters[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
            }
            
            // Disk counters
            _diskReadCounter = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            _diskWriteCounter = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
            _diskReadOpsCounter = new PerformanceCounter("PhysicalDisk", "Disk Reads/sec", "_Total");
            _diskWriteOpsCounter = new PerformanceCounter("PhysicalDisk", "Disk Writes/sec", "_Total");
            
            // Network counters
            var networkInterfaces = GetNetworkInterfaces();
            foreach (var nic in networkInterfaces)
            {
                try
                {
                    var sentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", nic);
                    var receivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", nic);
                    _networkCounters[nic] = (sentCounter, receivedCounter);
                }
                catch
                {
                    // Skip interfaces that don't have counters
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing performance counters: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Gets the names of network interfaces
    /// </summary>
    private List<string> GetNetworkInterfaces()
    {
        var interfaces = new List<string>();
        
        try
        {
            var category = new PerformanceCounterCategory("Network Interface");
            interfaces.AddRange(category.GetInstanceNames());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting network interfaces: {ex.Message}");
        }
        
        return interfaces;
    }
    
    /// <summary>
    /// Collects all system metrics
    /// </summary>
    public SystemMetrics CollectMetrics()
    {
        var metrics = new SystemMetrics
        {
            Timestamp = DateTime.Now,
            SystemName = Environment.MachineName,
            Uptime = GetSystemUptime()
        };
        
        CollectCpuMetrics(metrics.Cpu);
        CollectMemoryMetrics(metrics.Memory);
        CollectDiskMetrics(metrics.Disk);
        CollectNetworkMetrics(metrics.Network);
        
        _lastCollectionTime = DateTime.Now;
        
        return metrics;
    }
    
    /// <summary>
    /// Collects CPU metrics
    /// </summary>
    private void CollectCpuMetrics(CpuMetrics metrics)
    {
        try
        {
            metrics.ProcessorCount = Environment.ProcessorCount;
            metrics.ProcessorName = GetProcessorName();
            
            // Get total CPU usage
            if (_cpuCounter != null)
            {
                metrics.TotalUsagePercentage = _cpuCounter.NextValue();
            }
            
            // Get per-core CPU usage
            foreach (var (coreId, counter) in _cpuCoreCounters)
            {
                metrics.CoreUsagePercentages[coreId] = counter.NextValue();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error collecting CPU metrics: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Gets the processor name
    /// </summary>
    private string GetProcessorName()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_Processor");
            foreach (var obj in searcher.Get())
            {
                return obj["Name"]?.ToString() ?? "Unknown";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting processor name: {ex.Message}");
        }
        
        return "Unknown";
    }
    
    /// <summary>
    /// Collects memory metrics
    /// </summary>
    private void CollectMemoryMetrics(MemoryMetrics metrics)
    {
        try
        {
            using var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");
            foreach (var obj in searcher.Get())
            {
                // Values are in KB, convert to bytes
                if (obj["TotalVisibleMemorySize"] != null)
                {
                    metrics.TotalPhysicalMemory = Convert.ToUInt64(obj["TotalVisibleMemorySize"]) * 1024;
                }
                
                if (obj["FreePhysicalMemory"] != null)
                {
                    metrics.AvailablePhysicalMemory = Convert.ToUInt64(obj["FreePhysicalMemory"]) * 1024;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error collecting memory metrics: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Collects disk metrics
    /// </summary>
    private void CollectDiskMetrics(DiskMetrics metrics)
    {
        try
        {
            // Collect disk I/O metrics
            if (_diskReadCounter != null)
            {
                metrics.ReadBytesPerSec = _diskReadCounter.NextValue();
            }
            
            if (_diskWriteCounter != null)
            {
                metrics.WriteBytesPerSec = _diskWriteCounter.NextValue();
            }
            
            if (_diskReadOpsCounter != null)
            {
                metrics.ReadOperationsPerSec = _diskReadOpsCounter.NextValue();
            }
            
            if (_diskWriteOpsCounter != null)
            {
                metrics.WriteOperationsPerSec = _diskWriteOpsCounter.NextValue();
            }
            
            // Collect disk space metrics
            foreach (var drive in DriveInfo.GetDrives().Where(d => d.IsReady))
            {
                metrics.Disks.Add(new DiskInfo
                {
                    Name = drive.Name,
                    TotalSize = (ulong)drive.TotalSize,
                    FreeSpace = (ulong)drive.AvailableFreeSpace
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error collecting disk metrics: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Collects network metrics
    /// </summary>
    private void CollectNetworkMetrics(NetworkMetrics metrics)
    {
        try
        {
            foreach (var (name, (sentCounter, receivedCounter)) in _networkCounters)
            {
                var interfaceMetrics = new NetworkInterfaceMetrics
                {
                    Name = name,
                    Description = name,
                    BytesSentPerSec = sentCounter.NextValue(),
                    BytesReceivedPerSec = receivedCounter.NextValue()
                };
                
                metrics.Interfaces.Add(interfaceMetrics);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error collecting network metrics: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Gets the system uptime
    /// </summary>
    private TimeSpan GetSystemUptime()
    {
        try
        {
            using var uptime = new PerformanceCounter("System", "System Up Time");
            uptime.NextValue(); // First call always returns 0
            return TimeSpan.FromSeconds(uptime.NextValue());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting system uptime: {ex.Message}");
            return TimeSpan.Zero;
        }
    }
}
