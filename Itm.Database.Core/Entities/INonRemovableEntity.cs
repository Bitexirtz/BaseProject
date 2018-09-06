namespace Itm.Database.Core.Entities
{
	public interface INonRemovableEntity
	{
		bool Disabled { get; set; }
	}
}
