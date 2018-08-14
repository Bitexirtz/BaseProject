using System;

namespace Itm.Database.Core.Exception
{
	public class DatabaseException : System.Exception
	{
		public DatabaseException ()
			: base ()
		{
		}

		public DatabaseException (string message)
			: base (message)
		{
		}

		public DatabaseException (string message, System.Exception innerException)
			: base (message, innerException)
		{
		}
	}
}
