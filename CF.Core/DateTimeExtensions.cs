﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core
{
	/// <summary>
	/// Holder for DateTime extension methods
	/// </summary>
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Returns mininum of two DateTime objects
		/// </summary>
		public static DateTime Min(DateTime dt1, DateTime dt2)
		{
			return dt1 < dt2 ? dt1 : dt2;
		}

		/// <summary>
		/// Returns maximum of two DateTime objects
		/// </summary>
		public static DateTime Max(DateTime dt1, DateTime dt2)
		{
			return dt1 >= dt2 ? dt1 : dt2;
		}
	}
}
