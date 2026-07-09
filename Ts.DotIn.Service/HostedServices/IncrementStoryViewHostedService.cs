using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ts.DotIn.Service.DataInterfaces;
namespace Ts.DotIn.Service.HostedServices
{
    [Obsolete($"This hosted service is deprecated. Please use {nameof(IncrementStoryView2HostedService)} instead.")]
    public class IncrementStoryViewHostedService : BackgroundService
    {
        private readonly IStoryQueue storyQueue;
        private readonly ILogger<IncrementStoryViewHostedService> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public IncrementStoryViewHostedService(IStoryQueue storyQueue,
            ILogger<IncrementStoryViewHostedService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.storyQueue = storyQueue;
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("DotIn - IncrementStoryView Hosted Service running.");

            while (!cancellationToken.IsCancellationRequested)
            {
                if (!storyQueue.IsEmpty)
                {
                    var ids = storyQueue.DequeueAll();
                    await ProcessWorkAsync(ids).ConfigureAwait(false);
                    logger.LogInformation($"DotIn - Database Processing: Incremented total views.");
                }
                await Task.Delay(TimeSpan.FromMinutes(2.02), cancellationToken).ConfigureAwait(false);
            }

            logger.LogInformation("DotIn - IncrementStoryView Hosted Service stopping.");
        }

        private async Task ProcessWorkAsync(IEnumerable<int> ids)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var storyService = scope.ServiceProvider.GetRequiredService<IStoryService>();
            try
            {
                await storyService.IncrementStoryViewForStoryIdsAsync(ids).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DotIn - Error while incrementing the story view.");
            }
        }
    }
}
