using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using TrivialBrick.Business;

public class AssemblyLineCheckerService : IHostedService, IDisposable
{
    private readonly BLAssemblyLines _assemblyLineBL;
    private Timer? _timer;

    public AssemblyLineCheckerService(BLAssemblyLines assemblyLineBL)
    {
        _assemblyLineBL = assemblyLineBL;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("AssemblyLineCheckerService is starting.");
        _timer = new Timer(CheckAssemblyLines, null, TimeSpan.Zero, TimeSpan.FromSeconds(5)); // Verify every 5 seconds
        return Task.CompletedTask;
    }

    private async void CheckAssemblyLines(object? state)
    {

        try
        {
            var assemblyLines = await _assemblyLineBL.GetOcupiedAssemblyLines();

            foreach (var line in assemblyLines)
            {
               
                if (line.Expected_end_time <= DateTime.Now)
                {
                    await _assemblyLineBL.DesalocateAssemblyLine(line);
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