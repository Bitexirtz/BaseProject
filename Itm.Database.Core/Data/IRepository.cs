using System.Threading.Tasks;

namespace Itm.Database.Core.Data
{
	public interface IRepository
	{
		int CommitChanges ();

		Task<int> CommitChangesAsync ();
	}
}
