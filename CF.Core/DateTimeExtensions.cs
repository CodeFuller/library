using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core
{
	public static class DateTimeExtensions
	{
		public static DateTime Min(DateTime dt1, DateTime dt2)
		{
			return dt1 < dt2 ? dt1 : dt2;
		}

		public static DateTime Max(DateTime dt1, DateTime dt2)
		{
			return dt1 >= dt2 ? dt1 : dt2;
		}
	}
}
