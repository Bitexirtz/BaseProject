using AutoMapper;
using Itm.Database.Entities;
using Itm.Models;

namespace Itm.ObjectMap
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<ResourceModel, Resource>();
            CreateMap<Resource, ResourceModel>();
        }
    }
}
