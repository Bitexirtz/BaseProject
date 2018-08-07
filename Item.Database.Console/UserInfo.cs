using Itm.Database.Core.Entities;

namespace Itm.Database.Console
{
	public class UserInfo : IUserInfo
	{
		public UserInfo ()
		{
			ID = 0;
			Name = "Login-User";
		}

		public int ID { get; set; }
		public string Name { get; set; }
	}
}
