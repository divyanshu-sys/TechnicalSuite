using Ts.Domain.DataTableModels.PostOfficeDataTables;
using Ts.Domain.Models;
namespace Ts.Domain.Interfaces
{
    public interface IPostOfficeRepository : IGenericRepository<PostOffice>
    {
        Task<List<PostOffice>> GetAllAsync(PostOfficeDataTableRequest requestModel);

        Task<int> GetRecordsFilteredAsync(PostOfficeDataTableRequest requestModel);

        Task<List<PostOffice>> GetAllByDistrictIdAsync(int districtId);
    }
}
