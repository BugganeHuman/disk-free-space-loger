namespace disk_free_space_loger;

using System.IO;
using System.Numerics;
using Microsoft.Extensions.Hosting;

public class Worker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            string? path = Path.GetPathRoot(AppContext.BaseDirectory);
            if (path is not null)
            {
                var drive = new DriveInfo(path);
                string diskName = drive.Name;
                BigInteger freeSpace = drive.AvailableFreeSpace / 1024 / 1024 / 1024;
                string timeNow = DateTime.Now.ToString("dd.MM.yyyy  HH.mm");
                File.AppendAllText("free_space_logs.txt", $"{timeNow} | disk {diskName} |" +
                                   $" free space = {freeSpace}GB\n");
            }

            await Task.Delay(300000, cancellationToken);
        }
    }
}