﻿namespace DB2DB
{
    public class Worker : BackgroundService
    {
        private readonly BackUpDatabase _database = new BackUpDatabase("SERVICEDB1", "SERVICEDB2");
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        { 
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var data = await _database.BackUp();
                    _logger.LogWarning("BackUp Await tamamlandı : " + data);

                    await Task.Delay(TimeSpan.FromMilliseconds(5000), stoppingToken);
                    _logger.LogWarning("5 saniye bekeleme tamamlandı");
                }
            }
            catch (TaskCanceledException)
            {
                // When the stopping token is canceled, for example, a call made from services.msc,
                // we shouldn't exit with a non-zero exit code. In other words, this is expected...
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                // Terminates this process and returns an exit code to the operating system.
                // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
                // performs one of two scenarios:
                // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
                // 2. When set to "StopHost": will cleanly stop the host, and log errors.
                //
                // In order for the Windows Service Management system to leverage configured
                // recovery options, we need to terminate the process with a non-zero exit code.
                Environment.Exit(1);
            }
        }
    }
}