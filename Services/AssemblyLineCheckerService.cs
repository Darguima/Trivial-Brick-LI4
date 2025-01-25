using TrivialBrick.Business;

namespace TrivialBrick.Services;

public class AssemblyLineCheckerService(BLAssemblyLines assemblyLineBL) : IHostedService, IDisposable
{
    private readonly BLAssemblyLines _assemblyLineBL = assemblyLineBL;
    private Timer? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("AssemblyLineCheckerService is starting.");
        _timer = new Timer(CheckAssemblyLines, null, TimeSpan.Zero, TimeSpan.FromSeconds(1)); // Verify every 1 second
        return Task.CompletedTask;
    }

    private async void CheckAssemblyLines(object? state)
    {
        try
        {
            var assemblyLines = await _assemblyLineBL.GetOccupiedAssemblyLines();

            foreach (var line in assemblyLines)
            {

                if (line.Expected_end_time <= DateTime.Now)
                {
                    await _assemblyLineBL.DeallocateAssemblyLine(line);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking assembly lines: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("AssemblyLineCheckerService is stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}