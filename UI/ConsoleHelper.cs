namespace TopNet.UI;

/// <summary>
/// Helper class for console rendering
/// </summary>
public static class ConsoleHelper
{
    /// <summary>
    /// Writes a string with the specified color
    /// </summary>
    public static void WriteColored(string text, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = originalColor;
    }
    
    /// <summary>
    /// Writes a line with the specified color
    /// </summary>
    public static void WriteLineColored(string text, ConsoleColor color)
    {
        WriteColored(text, color);
        Console.WriteLine();
    }
    
    /// <summary>
    /// Draws a horizontal line
    /// </summary>
    public static void DrawHorizontalLine(char lineChar = '─', ConsoleColor color = ConsoleColor.DarkGray)
    {
        WriteLineColored(new string(lineChar, Console.WindowWidth), color);
    }
    
    /// <summary>
    /// Draws a progress bar
    /// </summary>
    public static void DrawProgressBar(float percentage, int width, ConsoleColor color = ConsoleColor.Green)
    {
        // Ensure percentage is between 0 and 100
        percentage = Math.Max(0, Math.Min(100, percentage));
        
        // Calculate the number of filled characters
        int filledWidth = (int)Math.Round(percentage / 100 * width);
        
        // Draw the progress bar
        Console.Write('[');
        
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = GetProgressBarColor(percentage);
        
        Console.Write(new string('█', filledWidth));
        Console.ForegroundColor = originalColor;
        
        Console.Write(new string(' ', width - filledWidth));
        Console.Write(']');
    }
    
    /// <summary>
    /// Gets the color for a progress bar based on the percentage
    /// </summary>
    private static ConsoleColor GetProgressBarColor(float percentage)
    {
        if (percentage < 60)
            return ConsoleColor.Green;
        if (percentage < 80)
            return ConsoleColor.Yellow;
        return ConsoleColor.Red;
    }
    
    /// <summary>
    /// Formats a byte size to a human-readable string (e.g., "1.5 MB")
    /// </summary>
    public static string FormatByteSize(ulong bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
        int suffixIndex = 0;
        double size = bytes;
        
        while (size >= 1024 && suffixIndex < suffixes.Length - 1)
        {
            size /= 1024;
            suffixIndex++;
        }
        
        return $"{size:0.##} {suffixes[suffixIndex]}";
    }
    
    /// <summary>
    /// Formats a byte rate to a human-readable string (e.g., "1.5 MB/s")
    /// </summary>
    public static string FormatByteRate(float bytesPerSec)
    {
        string[] suffixes = { "B/s", "KB/s", "MB/s", "GB/s", "TB/s" };
        int suffixIndex = 0;
        double rate = bytesPerSec;
        
        while (rate >= 1024 && suffixIndex < suffixes.Length - 1)
        {
            rate /= 1024;
            suffixIndex++;
        }
        
        return $"{rate:0.##} {suffixes[suffixIndex]}";
    }
    
    /// <summary>
    /// Clears the console and resets the cursor position
    /// </summary>
    public static void ClearConsole()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
    }
}
