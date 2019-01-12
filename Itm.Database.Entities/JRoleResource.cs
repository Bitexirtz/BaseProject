using Itm.Database.Core.Entities;

namespace Itm.Database.Entities
{
	public class JRoleResource : IEntity
	{
		public int RoleID { get; set; }
		public Role Role { get; set; }

		public int ResourceID { get; set; }
		public Resource Resource { get; set; }
	}
}
