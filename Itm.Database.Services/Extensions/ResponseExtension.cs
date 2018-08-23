using Itm.Database.Core.Exception;
using Itm.Database.Core.Services.ResponseTypes;
using Itm.Log.Core;

namespace Itm.Database.Services.Extensions
{
	internal static class ResponseExtension
	{
		public static void SetError (this IResponse response, System.Exception ex, ILogger logger)
		{
			response.DidError = true;

			var cast = ex as DatabaseException;

			if (cast == null) {
				logger.Error (ex.ToString ());
				response.ErrorMessage = "There was an internal error, please contact to technical support.";
			}
			else {
				logger.Error (ex.Message);
				response.ErrorMessage = ex.Message;
			}
		}
	}
}
