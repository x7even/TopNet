using TopNet.Models;

namespace TopNet.UI;

/// <summary>
/// Renders system metrics to the console
/// </summary>
public class ConsoleRenderer
{
    private const int ProgressBarWidth = 30;
    
    /// <summary>
    /// Renders the system metrics to the console
    /// </summary>
    public void Render(SystemMetrics metrics)
    {
        ConsoleHelper.ClearConsole();
        
        RenderHeader(metrics);
        ConsoleHelper.DrawHorizontalLine();
        
        RenderCpuMetrics(metrics.Cpu);
        ConsoleHelper.DrawHorizontalLine();
        
        RenderMemoryMetrics(metrics.Memory);
        ConsoleHelper.DrawHorizontalLine();
        
        RenderDiskMetrics(metrics.Disk);
        ConsoleHelper.DrawHorizontalLine();
        
        RenderNetworkMetrics(metrics.Network);
        
        // Position cursor at the bottom
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
    }
    
    /// <summary>
    /// Renders the header with system information
    /// </summary>
    private void RenderHeader(SystemMetrics metrics)
    {
        ConsoleHelper.WriteLineColored($"TopNet - System Monitor", ConsoleColor.Cyan);
        Console.WriteLine($"System: {metrics.SystemName}");
        Console.WriteLine($"Uptime: {FormatUptime(metrics.Uptime)}");
        Console.WriteLine($"Time: {metrics.Timestamp:yyyy-MM-dd HH:mm:ss}");
    }
    
    /// <summary>
    /// Renders CPU metrics
    /// </summary>
    private void RenderCpuMetrics(CpuMetrics metrics)
    {
        ConsoleHelper.WriteLineColored("CPU", ConsoleColor.Cyan);
        Console.WriteLine($"Processor: {metrics.ProcessorName}");
        
        Console.Write($"Total CPU Usage: {metrics.TotalUsagePercentage:0.0}% ");
        ConsoleHelper.DrawProgressBar(metrics.TotalUsagePercentage, ProgressBarWidth);
        Console.WriteLine();
        
        Console.WriteLine();
        ConsoleHelper.WriteLineColored("CPU Cores", ConsoleColor.Cyan);
        
        // Determine how many cores to display per row
        int coresPerRow = Math.Max(1, Console.WindowWidth / 40);
        int coreWidth = Console.WindowWidth / coresPerRow;
        
        for (int i = 0; i < metrics.ProcessorCount; i += coresPerRow)
        {
            for (int j = 0; j < coresPerRow && i + j < metrics.ProcessorCount; j++)
            {
                int coreId = i + j;
                Console.SetCursorPosition(j * coreWidth, Console.CursorTop);
                
                if (metrics.CoreUsagePercentages.TryGetValue(coreId, out float usage))
                {
                    Console.Write($"Core {coreId}: {usage:0.0}% ");
                    ConsoleHelper.DrawProgressBar(usage, 15);
                }
            }
            Console.WriteLine();
        }
    }
    
    /// <summary>
    /// Renders memory metrics
    /// </summary>
    private void RenderMemoryMetrics(MemoryMetrics metrics)
    {
        ConsoleHelper.WriteLineColored("Memory", ConsoleColor.Cyan);
        
        Console.Write($"Memory Usage: {metrics.UsagePercentage:0.0}% ");
        ConsoleHelper.DrawProgressBar(metrics.UsagePercentage, ProgressBarWidth);
        Console.WriteLine();
        
        Console.WriteLine($"Total: {ConsoleHelper.FormatByteSize(metrics.TotalPhysicalMemory)}");
        Console.WriteLine($"Used: {ConsoleHelper.FormatByteSize(metrics.UsedPhysicalMemory)}");
        Console.WriteLine($"Available: {ConsoleHelper.FormatByteSize(metrics.AvailablePhysicalMemory)}");
    }
    
    /// <summary>
    /// Renders disk metrics
    /// </summary>
    private void RenderDiskMetrics(DiskMetrics metrics)
    {
        ConsoleHelper.WriteLineColored("Disk", ConsoleColor.Cyan);
        
        Console.WriteLine($"Read: {ConsoleHelper.FormatByteRate(metrics.ReadBytesPerSec)} ({metrics.ReadOperationsPerSec:0.0} ops/s)");
        Console.WriteLine($"Write: {ConsoleHelper.FormatByteRate(metrics.WriteBytesPerSec)} ({metrics.WriteOperationsPerSec:0.0} ops/s)");
        
        Console.WriteLine();
        ConsoleHelper.WriteLineColored("Disk Space", ConsoleColor.Cyan);
        
        foreach (var disk in metrics.Disks)
        {
            Console.Write($"{disk.Name} {disk.UsagePercentage:0.0}% ");
            ConsoleHelper.DrawProgressBar(disk.UsagePercentage, ProgressBarWidth);
            Console.Write($" {ConsoleHelper.FormatByteSize(disk.UsedSpace)}/{ConsoleHelper.FormatByteSize(disk.TotalSize)}");
            Console.WriteLine();
        }
    }
    
    /// <summary>
    /// Renders network metrics
    /// </summary>
    private void RenderNetworkMetrics(NetworkMetrics metrics)
    {
        ConsoleHelper.WriteLineColored("Network", ConsoleColor.Cyan);
        
        Console.WriteLine($"Total Download: {ConsoleHelper.FormatByteRate(metrics.TotalBytesReceivedPerSec)}");
        Console.WriteLine($"Total Upload: {ConsoleHelper.FormatByteRate(metrics.TotalBytesSentPerSec)}");
        
        if (metrics.Interfaces.Count > 0)
        {
            Console.WriteLine();
            ConsoleHelper.WriteLineColored("Network Interfaces", ConsoleColor.Cyan);
            
            foreach (var nic in metrics.Interfaces)
            {
                Console.WriteLine($"{nic.Name}");
                Console.WriteLine($"  Download: {ConsoleHelper.FormatByteRate(nic.BytesReceivedPerSec)}");
                Console.WriteLine($"  Upload: {ConsoleHelper.FormatByteRate(nic.BytesSentPerSec)}");
            }
        }
    }
    
    /// <summary>
    /// Formats the uptime to a human-readable string
    /// </summary>
    private string FormatUptime(TimeSpan uptime)
    {
        if (uptime.TotalDays >= 1)
        {
            return $"{(int)uptime.TotalDays}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";
        }
        
        if (uptime.TotalHours >= 1)
        {
            return $"{(int)uptime.TotalHours}h {uptime.Minutes}m {uptime.Seconds}s";
        }
        
        if (uptime.TotalMinutes >= 1)
        {
            return $"{(int)uptime.TotalMinutes}m {uptime.Seconds}s";
        }
        
        return $"{uptime.Seconds}s";
    }
}
