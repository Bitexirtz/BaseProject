using System.Threading.Tasks;

namespace Itm.Database.Core.Services
{
	public interface IRepository
	{
		int CommitChanges ();

		Task<int> CommitChangesAsync ();
	}
}
