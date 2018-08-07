namespace Itm.Database.Core.Services.ResponseTypes
{
	public interface ISingleResponse<TModel> : IResponse
	{
		TModel Model { get; set; }
	}
}
