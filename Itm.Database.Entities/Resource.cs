using Itm.Database.Core.Entities;

namespace Itm.Database.Entities
{
    public class Resource : AuditableBaseEntity, IAuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Disabled { get; set; }
    }
}
