using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;

namespace ToDoAPI.HostedServices;

public sealed class RefreshTokenCleanerBackgroundService : BackgroundService
{
	private readonly ILogger<RefreshTokenCleanerBackgroundService> _logger;
	private IServiceScope _scope;
	private AppDbContext _dbContext;

	public RefreshTokenCleanerBackgroundService(IServiceProvider serviceProvider,
		ILogger<RefreshTokenCleanerBackgroundService> logger)
	{
		_logger = logger;
		_scope = serviceProvider.CreateScope();
		_dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

			var result = await _dbContext.RefreshTokens.Where(x => DateTime.Now >= x.Expires)
				.ExecuteDeleteAsync(stoppingToken);

			if (result == 0)
			{
				_logger.LogInformation("No outdated refresh tokens found");
			}
			else
			{
				_logger.LogInformation($"Refresh tokens cleaned: {result}");
			}
		}
	}

	public override void Dispose()
	{
		base.Dispose();

		_scope.Dispose();
	}
}