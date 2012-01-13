using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.DAL
{
	public interface ICacheRepository<T, T1> : IBaseRepository<T, T1>
	{
		void Reset();
	}
}
