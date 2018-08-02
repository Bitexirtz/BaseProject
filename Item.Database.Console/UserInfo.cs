using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Itm.Database.Core.Contracts;

namespace Itm.Database.Console
{
	public class UserInfo : IUserInfo
	{
		public UserInfo ()
		{
			ID = 0;
			Name = "Mark";
		}

		public int ID { get; set; }
		public string Name { get; set; }
	}
}
