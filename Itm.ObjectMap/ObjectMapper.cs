using System.Reflection;
using AutoMapper;

namespace Itm.ObjectMap
{
	public class ObjectMapper : Mapper
	{
		public ObjectMapper () : base (
			 new MapperConfiguration (cfg =>
			 {
				 cfg.AddProfiles (Assembly.GetExecutingAssembly ());
			 })
			)
		{
		}
	}
}
