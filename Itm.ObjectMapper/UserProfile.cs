using AutoMapper;
using Itm.Database.Entities;
using Itm.Models;

namespace Itm.ObjectMapper
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<User, UserModel>();
			CreateMap<UserModel, User>();
		}
	}
}
