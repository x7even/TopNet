using System.Runtime.InteropServices;
using TopNet.Services;
using TopNet.UI;

namespace TopNet;

/// <summary>
/// Main program class
/// </summary>
public class Program
{
    private static readonly CancellationTokenSource _cancellationTokenSource = new();
    private static readonly MetricsCollector _metricsCollector = new();
    private static readonly ConsoleRenderer _consoleRenderer = new();
    
    /// <summary>
    /// Main entry point
    /// </summary>
    public static async Task Main(string[] args)
    {
        // Set up console
        Console.Title = "TopNet - System Monitor";
        Console.CursorVisible = false;
        
        // Set up cancellation
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            _cancellationTokenSource.Cancel();
        };
        
        // Display welcome message
        Console.WriteLine("TopNet - System Monitor");
        Console.WriteLine("Press Ctrl+C to exit");
        Console.WriteLine();
        Console.WriteLine("Initializing...");
        
        // Wait for initial metrics collection
        await Task.Delay(1000);
        
        try
        {
            // Start the monitoring loop
            await MonitorSystemAsync(_cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            // Normal exit
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            // Clean up
            Console.CursorVisible = true;
            Console.Clear();
            Console.WriteLine("TopNet has exited.");
        }
    }
    
    /// <summary>
    /// Monitors the system metrics in a loop
    /// </summary>
    private static async Task MonitorSystemAsync(CancellationToken cancellationToken)
    {
        // First collection to initialize counters
        var metrics = _metricsCollector.CollectMetrics();
        
        // Wait a moment for counters to stabilize
        await Task.Delay(1000, cancellationToken);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // Collect metrics
                metrics = _metricsCollector.CollectMetrics();
                
                // Render metrics
                _consoleRenderer.Render(metrics);
                
                // Wait for next update
                await Task.Delay(1000, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}
