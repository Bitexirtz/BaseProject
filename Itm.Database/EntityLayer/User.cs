using System;
using Itm.Database.Core.Contracts;

namespace Itm.Database.EntityLayer
{
	public class User : IAuditableEntity
	{
		public User ()
		{
		}

		public User (int? userID)
		{
			UserID = userID;
		}

		public int? UserID { get; set; }

		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public DateTime? BirthDate { get; set; }

		public string CreationUser { get; set; }

		public DateTime? CreationDateTime { get; set; }

		public string LastUpdateUser { get; set; }

		public DateTime? LastUpdateDateTime { get; set; }

		public Byte[] Timestamp { get; set; }
	}
}
