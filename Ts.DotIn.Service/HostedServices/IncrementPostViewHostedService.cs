using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ts.DotIn.Service.DataInterfaces;
namespace Ts.DotIn.Service.HostedServices
{
    [Obsolete($"This hosted service is deprecated. Please use {nameof(IncrementPostView2HostedService)} instead.")]
    public class IncrementPostViewHostedService : BackgroundService
    {
        private readonly IPostQueue postQueue;
        private readonly ILogger<IncrementPostViewHostedService> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public IncrementPostViewHostedService(IPostQueue postQueue,
            ILogger<IncrementPostViewHostedService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.postQueue = postQueue;
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("DotIn - IncrementPostView Hosted Service running.");

            while (!cancellationToken.IsCancellationRequested)
            {
                if (!postQueue.IsEmpty)
                {
                    var ids = postQueue.DequeueAll();
                    await ProcessWorkAsync(ids).ConfigureAwait(false);
                    logger.LogInformation($"DotIn - Database Processing: Incremented total views.");
                }
                await Task.Delay(TimeSpan.FromMinutes(2.03), cancellationToken).ConfigureAwait(false);
            }

            logger.LogInformation("DotIn - IncrementPostView Hosted Service stopping.");
        }

        private async Task ProcessWorkAsync(IEnumerable<int> ids)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var postService = scope.ServiceProvider.GetRequiredService<IPostService>();
            try
            {
                await postService.IncrementPostViewForPostIdsAsync(ids).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DotIn - Error while incrementing the post view.");
            }
        }
    }
}
