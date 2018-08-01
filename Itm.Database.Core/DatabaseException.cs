using System;

namespace Itm.Database.Core
{
	public class DatabaseException : Exception
	{
		public DatabaseException ()
			: base ()
		{
		}

		public DatabaseException (String message)
			: base (message)
		{
		}

		public DatabaseException (String message, Exception innerException)
			: base (message, innerException)
		{
		}
	}
}
