using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ts.ShopIn.Service.DataInterfaces;
namespace Ts.ShopIn.Service.HostedServices
{
    public class IncrementBlogAndProductDetailView2HostedService : BackgroundService
    {
        private readonly IBlogViewCounter blogViewCounter;
        private readonly IProductDetailViewCounter productDetailViewCounter;
        private readonly ILogger<IncrementBlogAndProductDetailView2HostedService> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public IncrementBlogAndProductDetailView2HostedService(IBlogViewCounter blogViewCounter,
            IProductDetailViewCounter productDetailViewCounter,
            ILogger<IncrementBlogAndProductDetailView2HostedService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            this.blogViewCounter = blogViewCounter;
            this.productDetailViewCounter = productDetailViewCounter;
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("ShopDotIn - IncrementBlogAndProductDetailView Hosted Service running.");

            while (!cancellationToken.IsCancellationRequested)
            {
                var snapshotBlog = blogViewCounter.SnapshotAndReset();

                if (snapshotBlog.Count > 0)
                {
                    await ProcessBlogWorkAsync(snapshotBlog).ConfigureAwait(false);
                    logger.LogInformation($"ShopDotIn - Database Processing: Incremented blog total views.");
                }

                var snapshotProductDetail = productDetailViewCounter.SnapshotAndReset();

                if (snapshotProductDetail.Count > 0)
                {
                    await ProcessProductDetailWorkAsync(snapshotProductDetail).ConfigureAwait(false);
                    logger.LogInformation($"ShopDotIn - Database Processing: Incremented product detail total views.");
                }

                await Task.Delay(TimeSpan.FromMinutes(2.04), cancellationToken).ConfigureAwait(false);
            }

            logger.LogInformation("ShopDotIn - IncrementBlogAndProductDetailView Hosted Service stopping.");
        }

        private async Task ProcessBlogWorkAsync(Dictionary<int, int> data)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var blogService = scope.ServiceProvider.GetRequiredService<IBlogService>();
            try
            {
                await blogService.IncrementBlogView2ForBlogIdsAsync(data).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ShopDotIn - Error while incrementing the blog view.");
            }
        }

        private async Task ProcessProductDetailWorkAsync(Dictionary<int, int> data)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var productDetailService = scope.ServiceProvider.GetRequiredService<IProductDetailService>();
            try
            {
                await productDetailService.IncrementProductDetailView2ForProductDetailIdsAsync(data).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ShopDotIn - Error while incrementing the product detail view.");
            }
        }
    }
}
