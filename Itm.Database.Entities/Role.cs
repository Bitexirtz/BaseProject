using System.Collections.Generic;
using Itm.Database.Core.Entities;

namespace Itm.Database.Entities
{
    public class Role : AuditableBaseEntity, IAuditableEntity, INonRemovableEntity
	{
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Disabled { get; set; }

		public virtual ICollection<JUserRole> UserRole { get; set; }
	}
}
