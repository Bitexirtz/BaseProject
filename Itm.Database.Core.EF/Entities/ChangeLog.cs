using System;
using Itm.Database.Core.Contracts;

namespace Itm.Database.Core.EF.Entities
{
	public class ChangeLog : IEntity
	{
		public ChangeLog ()
		{
		}

		public int ID { get; set; }

		public string ClassName { get; set; }

		public string PropertyName { get; set; }

		public string Key { get; set; }

		public string OriginalValue { get; set; }

		public string CurrentValue { get; set; }

		public string UserName { get; set; }

		public DateTime? ChangeDate { get; set; }

		public int RowID { get; set; }
	}
}
