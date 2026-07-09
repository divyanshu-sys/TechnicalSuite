using Microsoft.EntityFrameworkCore;
using Ts.Domain.DataTableModels.ApplicationUserDataTables;
using Ts.Domain.Interfaces;
using Ts.Domain.Models;
using Ts.Infra.Data.Context;
namespace Ts.Infra.Data.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly TsIdentityDbContext dbContext;

        public ApplicationUserRepository(TsIdentityDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<ApplicationUser>> GetAllAsync(ApplicationUserDataTableRequest requestModel)
        {
            var queriableEntity = CommonSearch(requestModel);

            // Ordering
            IOrderedQueryable<ApplicationUser> orderQueriableEntity = null;
            if (requestModel.OrderList.Any())
            {
                foreach (var item in requestModel.OrderList)
                {
                    if (item.OrderBy != null)
                        if (item.IsAsc)
                        {
                            if (orderQueriableEntity == null)
                            {
                                if (item.OrderBy.FirstName)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.FirstName);
                                else if (item.OrderBy.LastName)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.LastName);
                                else if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.CreatedOn);
                                else if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = queriableEntity.OrderBy(x => x.UpdatedOn);
                            }
                            else
                            {
                                if (item.OrderBy.FirstName)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.FirstName);
                                else if (item.OrderBy.LastName)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.LastName);
                                else if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.CreatedOn);
                                else if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenBy(x => x.UpdatedOn);
                            }
                        }
                        else
                        {
                            if (orderQueriableEntity == null)
                            {
                                if (item.OrderBy.FirstName)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.FirstName);
                                else if (item.OrderBy.LastName)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.LastName);
                                else if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.CreatedOn);
                                else if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = queriableEntity.OrderByDescending(x => x.UpdatedOn);
                            }
                            else
                            {
                                if (item.OrderBy.FirstName)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.FirstName);
                                else if (item.OrderBy.LastName)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.LastName);
                                else if (item.OrderBy.CreatedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.CreatedOn);
                                else if (item.OrderBy.UpdatedOn)
                                    orderQueriableEntity = orderQueriableEntity.ThenByDescending(x => x.UpdatedOn);
                            }
                        }
                }
            }
            if (orderQueriableEntity != null)
                queriableEntity = orderQueriableEntity.AsQueryable();

            return queriableEntity.Skip(requestModel.Start).Take(requestModel.Length).ToListAsync();
        }

        public Task<int> GetRecordsFilteredAsync(ApplicationUserDataTableRequest requestModel)
        {
            return CommonSearch(requestModel).CountAsync();
        }

        public Task<int> GetTotalCountAsync()
        {
            return dbContext.Users.CountAsync();
        }

        public Task<List<ApplicationUser>> GetUsersByRolesAsync(IEnumerable<string> roles)
        {
            var users = (from userId in (from user in dbContext.Users
                                         join userRole in dbContext.UserRoles on user.Id equals userRole.UserId
                                         join role in dbContext.Roles on userRole.RoleId equals role.Id
                                         where roles.Contains(role.Name) && user.EmailConfirmed
                                         select user.Id).Distinct()
                         join user in dbContext.Users on userId equals user.Id
                         select user).ToListAsync();

            return users;
        }

        public Task<List<ApplicationUser>> GetUsersByUserIdsAsync(IEnumerable<string> userIds)
        {
            return dbContext.Users.Where(x => userIds.Contains(x.Id)).ToListAsync();
        }

        private IQueryable<ApplicationUser> CommonSearch(ApplicationUserDataTableRequest requestModel)
        {
            var queriableEntity = dbContext.Users.AsQueryable();

            // Global Search
            if (!string.IsNullOrEmpty(requestModel.Search?.Value))
                queriableEntity = queriableEntity.Where(x => x.FirstName.Contains(requestModel.Search.Value)
                || x.LastName.Contains(requestModel.Search.Value) || x.UserName == requestModel.Search.Value);

            return queriableEntity;
        }
    }
}
