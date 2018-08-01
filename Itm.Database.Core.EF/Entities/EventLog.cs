using System;
using Itm.Database.Core.Contracts;

namespace Itm.Database.Core.EF.Entities
{
	public class EventLog : IEntity
	{
		public EventLog ()
		{
		}

		public Guid EventLogID { get; set; }

		public int EventType { get; set; }

		public string Key { get; set; }

		public string Message { get; set; }

		public DateTime? EntryDate { get; set; }

		public int RowID { get; set; }
	}
}
