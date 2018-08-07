using System;

namespace Itm.Database.Core.Exception
{
	public class DatabaseException : System.Exception
	{
		public DatabaseException ()
			: base ()
		{
		}

		public DatabaseException (String message)
			: base (message)
		{
		}

		public DatabaseException (String message, System.Exception innerException)
			: base (message, innerException)
		{
		}
	}
}
