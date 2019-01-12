using System.Threading.Tasks;
using Itm.Database.Core.Services;
using Itm.Database.Core.Services.ResponseTypes;
using Itm.Models;

namespace Itm.Database.Services
{
    internal interface IResourceService : IService
    {
        Task<IListResponse<ResourceModel>> GetResourcesAsync(int pageSize = 0, int pageNumber = 0);

        Task<ISingleResponse<ResourceModel>> GetResourceByIDAsync(int roleID);

        Task<ISingleResponse<ResourceModel>> UpdateResourceAsync(ResourceModel updates);

        Task<ISingleResponse<ResourceModel>> AddResourceAsync(ResourceModel details);

        Task<ISingleResponse<ResourceModel>> RemoveResourceAsync(int roleID);
    }
}
