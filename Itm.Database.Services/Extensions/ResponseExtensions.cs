using System;
using Itm.Database.Core.Exception;
using Itm.Database.Core.Services.ResponseTypes;
using Microsoft.Extensions.Logging;

namespace Itm.Database.Core.EF.Extensions
{
	internal static class ResponseExtensions
	{
		public static void SetError (this IResponse response, System.Exception ex, ILogger logger)
		{
			response.DidError = true;

			var cast = ex as DatabaseException;

			if (cast == null) {
				logger?.LogCritical (ex.ToString ());
				response.ErrorMessage = "There was an internal error, please contact to technical support.";
			}
			else {
				logger?.LogError (ex.Message);
				response.ErrorMessage = ex.Message;
			}
		}
	}
}
