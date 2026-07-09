using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ts.DotIn.Service.DataInterfaces;
namespace Ts.DotIn.Service.HostedServices
{
    public class IncrementStoryView2HostedService : BackgroundService
    {
        private readonly IStoryViewCounter storyViewCounter;
        private readonly ILogger<IncrementStoryView2HostedService> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public IncrementStoryView2HostedService(IStoryViewCounter storyViewCounter,
            ILogger<IncrementStoryView2HostedService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.storyViewCounter = storyViewCounter;
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("DotIn - IncrementStoryView Hosted Service running.");

            while (!cancellationToken.IsCancellationRequested)
            {
                var snapshot = storyViewCounter.SnapshotAndReset();

                if (snapshot.Count > 0)
                {
                    await ProcessWorkAsync(snapshot).ConfigureAwait(false);
                    logger.LogInformation($"DotIn - Database Processing: Incremented total views.");
                }
                await Task.Delay(TimeSpan.FromMinutes(2.02), cancellationToken).ConfigureAwait(false);
            }

            logger.LogInformation("DotIn - IncrementStoryView Hosted Service stopping.");
        }

        private async Task ProcessWorkAsync(Dictionary<int, int> data)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var storyService = scope.ServiceProvider.GetRequiredService<IStoryService>();
            try
            {
                await storyService.IncrementStoryView2ForStoryIdsAsync(data).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DotIn - Error while incrementing the story view.");
            }
        }
    }
}
