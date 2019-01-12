using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Itm.Database.Context;
using Itm.Database.Core.Entities;
using Itm.Database.Core.Exception;
using Itm.Database.Core.Services.ResponseTypes;
using Itm.Database.Entities;
using Itm.Database.Repositories;
using Itm.Database.Services.Extensions;
using Itm.Log.Core;
using Itm.Models;
using Microsoft.EntityFrameworkCore;

namespace Itm.Database.Services
{
    internal class ResourceService : BaseService, IResourceService
    {
        private IResourceRepository _resourceRepository { get; }

        public ResourceService(ILogger logger, IMapper mapper, IAppUser userInfo, AppDbContext dbContext)
            : base(logger, mapper, userInfo, dbContext)
        {
            _resourceRepository = new ResourceRepository (userInfo, DbContext);
        }

        public async Task<IListResponse<ResourceModel>> GetResourcesAsync(int pageSize = 0, int pageNumber = 0)
        {
            Logger.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new ListResponse<ResourceModel>();

            try
            {
                response.Model = await _resourceRepository.GetAll(pageSize, pageNumber).Select(o => Mapper.Map<ResourceModel>(o)).ToListAsync();
            }
            catch (Exception ex)
            {
                response.SetError(ex, Logger);
            }

            return response;
        }

        public async Task<ISingleResponse<ResourceModel>> GetResourceByIDAsync(int resourceID)
        {
            Logger.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new SingleResponse<ResourceModel>();

            try
            {
                var userDetails = await _resourceRepository.GetByIDAsync(resourceID);

                response.Model = Mapper.Map<ResourceModel>(userDetails);
            }
            catch (Exception ex)
            {
                response.SetError(ex, Logger);
            }

            return response;
        }

        public async Task<ISingleResponse<ResourceModel>> AddResourceAsync(ResourceModel details)
        {
            Logger.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));
            var response = new SingleResponse<ResourceModel>();

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {

                    var resource = Mapper.Map<Resource>(details);
                    await _resourceRepository.AddAsync(resource);

                    transaction.Commit();
                    response.Model = Mapper.Map<ResourceModel>(resource);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

            return response;
        }

        public async Task<ISingleResponse<ResourceModel>> UpdateResourceAsync(ResourceModel updates)
        {
            Logger.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new SingleResponse<ResourceModel>();

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {

                    Resource resource = await _resourceRepository.GetByIDAsync(updates.ID);
                    if (resource == null)
                    {
                        throw new DatabaseException("Resource record not found.");
                    }

                    Mapper.Map(updates, resource);

                    await _resourceRepository.UpdateAsync(resource);

                    transaction.Commit();
                    response.Model = Mapper.Map<ResourceModel>(resource);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.SetError(ex, Logger);
                }
            }

            return response;
        }

        public async Task<ISingleResponse<ResourceModel>> RemoveResourceAsync(int resourceID)
        {
            Logger.Info(CreateInvokedMethodLog(MethodBase.GetCurrentMethod().ReflectedType.FullName));

            var response = new SingleResponse<ResourceModel>();

            try
            {
                // Retrieve user by id
                Resource resource = await _resourceRepository.GetByIDAsync(resourceID);
                if (resource == null)
                {
                    throw new DatabaseException("User record not found.");
                }

                await _resourceRepository.DeleteAsync(resource);
                response.Model = Mapper.Map<ResourceModel>(resource);
            }
            catch (Exception ex)
            {
                response.SetError(ex, Logger);
            }

            return response;
        }
    }
}
