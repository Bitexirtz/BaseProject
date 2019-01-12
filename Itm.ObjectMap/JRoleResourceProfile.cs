using AutoMapper;
using Itm.Database.Entities;
using Itm.Models;

namespace Itm.ObjectMap
{
    public class JRoleResourceProfile : Profile
    {
        public JRoleResourceProfile()
        {
            CreateMap<JRoleResource, RoleModel>();

            CreateMap<JRoleResource, ResourceModel>();
        }
    }
}
