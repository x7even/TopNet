# TopNet - A .NET System Monitor

TopNet is a console-based system monitoring application similar to htop but for Windows, built with .NET 9. It provides real-time monitoring of system resources including CPU, memory, disk, and network usage.

## Features

- **Real-time System Metrics Display**:
  - CPU usage (overall and per-core)
  - Memory usage (used, available, total)
  - Disk I/O (read/write rates)
  - Network usage (upload/download rates)
  - Disk space information

- **UI Features**:
  - Console-based interface with color coding
  - Auto-refreshing display (updates every second)
  - Clean, organized layout with sections for different metrics
  - Header with system summary
  - Detailed per-core CPU metrics
  - Visual progress bars for usage percentages

## Requirements

- Windows operating system
- .NET 9 SDK

## Installation

1. Clone the repository or download the source code
2. Build the project:
   ```
   dotnet build
   ```

## Usage

Run the application:

```
dotnet run
```

Press `Ctrl+C` to exit the application.

## Architecture

The application is structured as follows:

- **Models**: Data structures for system metrics
  - `CpuMetrics`: CPU usage information
  - `MemoryMetrics`: Memory usage information
  - `DiskMetrics`: Disk I/O and space information
  - `NetworkMetrics`: Network usage information
  - `SystemMetrics`: Container for all metrics

- **Services**: Business logic
  - `MetricsCollector`: Collects system metrics using Windows performance counters

- **UI**: User interface components
  - `ConsoleHelper`: Utility methods for console rendering
  - `ConsoleRenderer`: Renders metrics to the console

## Dependencies

- `System.Diagnostics.PerformanceCounter`: For accessing Windows performance counters
- `System.Management`: For additional hardware information

## License

MIT
