using AutoMapper;
using Itm.Database.Entities;
using Itm.Models;

namespace Itm.ObjectMap
{
	public class RoleProfile : Profile
	{
		public RoleProfile ()
		{
			CreateMap<RoleModel, Role> ();
			CreateMap<Role, RoleModel> ();
		}
	}
}
