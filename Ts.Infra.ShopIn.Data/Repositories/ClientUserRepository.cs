using Microsoft.EntityFrameworkCore;
using Ts.Infra.ShopIn.Data.Context;
using Ts.ShopIn.Domain.DataTableModels.ClientUserDataTables;
using Ts.ShopIn.Domain.Interfaces;
using Ts.ShopIn.Domain.Models;
namespace Ts.Infra.ShopIn.Data.Repositories
{
    public class ClientUserRepository : IClientUserRepository
    {
        private readonly ShopInDbContext dbContext;

        public ClientUserRepository(ShopInDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<ClientUser>> GetAllAsync(ClientUserDataTableRequest requestModel)
        {
            var queriableEntity = CommonSearch(requestModel);

            // Ordering
            IOrderedQueryable<ClientUser> orderQueriableEntity = null;
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

            return queriableEntity.Skip(requestModel.Start).Take(requestModel.Length).AsNoTracking().ToListAsync();
        }

        public Task<int> GetRecordsFilteredAsync(ClientUserDataTableRequest requestModel)
        {
            return CommonSearch(requestModel).AsNoTracking().CountAsync();
        }

        public Task<int> GetTotalCountAsync()
        {
            return dbContext.Users.AsNoTracking().CountAsync();
        }

        public Task<List<ClientUser>> GetUsersByRolesAsync(IEnumerable<string> roles)
        {
            var users = (from userId in (from user in dbContext.Users
                                         join userRole in dbContext.UserRoles on user.Id equals userRole.UserId
                                         join role in dbContext.Roles on userRole.RoleId equals role.Id
                                         where roles.Contains(role.Name) && user.EmailConfirmed
                                         select user.Id).Distinct()
                         join user in dbContext.Users on userId equals user.Id
                         select user).AsNoTracking().ToListAsync();

            return users;
        }

        private IQueryable<ClientUser> CommonSearch(ClientUserDataTableRequest requestModel)
        {
            var queriableEntity = dbContext.Users.AsQueryable();

            // Global Search
            if (!string.IsNullOrEmpty(requestModel.Search?.Value))
                queriableEntity = queriableEntity.Where(x => x.FirstName.Contains(requestModel.Search.Value)
                || x.LastName.Contains(requestModel.Search.Value) || x.UserName == requestModel.Search.Value);

            return queriableEntity;
        }

        public Task<ClientUser> GetForCartOrderAsync(string id)
        {
            return dbContext.Users
                .Select(x => new ClientUser
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneCode = x.PhoneCode,
                    PhoneNumber = x.PhoneNumber
                })
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
