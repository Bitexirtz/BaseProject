﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itm.Database.Core.Contracts
{
	public interface IUserInfo
	{
		Guid ID { get; set; }
		string Name { get; set; }
	}
}
