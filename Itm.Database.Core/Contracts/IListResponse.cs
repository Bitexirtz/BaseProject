using System.Collections.Generic;

namespace Itm.Database.Core.Contracts
{
	public interface IListResponse<TModel> : IResponse
	{
		IEnumerable<TModel> Model { get; set; }
	}
}
