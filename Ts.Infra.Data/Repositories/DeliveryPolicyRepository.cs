using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class DeliveryPolicyRepository : GenericRepository<DeliveryPolicy>, IDeliveryPolicyRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public DeliveryPolicyRepository(TsIdentityDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
