using AutoMapper;
using Itm.Database.Entities;
using Itm.Models;
using Itm.Database.ObjectMapper.Extensions;

namespace Itm.Database.ObjectMapper
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<UserModel, User> ();
			CreateMap<UserModel, UserCredential>();

			//CreateMap<UserModel, User> ()
			//	.ForMember<UserCredential> (
			//		dest => dest.UserCredential,
			//		opt => opt.MapFrom (src => new UserCredential
			//		{
			//			 UserName = src.UserName,
			//			 Password = src.Password
			//		})
			//	);


			CreateMap<User, UserModel>()
				.ForMember(
					dest => dest.UserName,
					opt => opt.MapFrom(src => src.UserCredential.UserName)
				)
				.ForMember(
					dest => dest.Password,
					opt => opt.MapFrom(src => src.UserCredential.Password)
				);
		}
	}
}
