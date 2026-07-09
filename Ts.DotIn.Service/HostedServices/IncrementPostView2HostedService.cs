using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ts.DotIn.Service.DataInterfaces;
namespace Ts.DotIn.Service.HostedServices
{
    public class IncrementPostView2HostedService : BackgroundService
    {
        private readonly IPostViewCounter postViewCounter;
        private readonly ILogger<IncrementPostView2HostedService> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public IncrementPostView2HostedService(IPostViewCounter postViewCounter,
            ILogger<IncrementPostView2HostedService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.postViewCounter = postViewCounter;
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("DotIn - IncrementPostView Hosted Service running.");

            while (!cancellationToken.IsCancellationRequested)
            {
                var snapshot = postViewCounter.SnapshotAndReset();

                if (snapshot.Count > 0)
                {
                    await ProcessWorkAsync(snapshot).ConfigureAwait(false);
                    logger.LogInformation($"DotIn - Database Processing: Incremented total views.");
                }
                await Task.Delay(TimeSpan.FromMinutes(2.03), cancellationToken).ConfigureAwait(false);
            }

            logger.LogInformation("DotIn - IncrementPostView Hosted Service stopping.");
        }

        private async Task ProcessWorkAsync(Dictionary<int, int> data)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var postService = scope.ServiceProvider.GetRequiredService<IPostService>();
            try
            {
                await postService.IncrementPostView2ForPostIdsAsync(data).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DotIn - Error while incrementing the post view.");
            }
        }
    }
}
