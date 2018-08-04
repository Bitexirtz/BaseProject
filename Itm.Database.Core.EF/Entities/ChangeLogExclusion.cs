using System;
using Itm.Database.Core.Contracts;

namespace Itm.Database.Core.EF.Entities
{
	public class ChangeLogExclusion : IEntity
	{
		public ChangeLogExclusion ()
		{
		}

		public int ID { get; set; }

		public string EntityName { get; set; }

		public string PropertyName { get; set; }
	}
}
