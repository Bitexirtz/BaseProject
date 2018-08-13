using System;

namespace Itm.Models
{
	public class UserModel
	{
		public UserModel()
		{
		}

		public UserModel(int userID)
		{
			ID = userID;
		}

		public int ID { get; set; }

		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public string UserName { get; set; }
		public string Password { get; set; }

		public DateTime? BirthDate { get; set; }


	}
}
