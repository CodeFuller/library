﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core
{
	public static class ExceptionExtensions
	{
		/// <summary>
		/// If exc is Aggregate exception and all inner exceptions have the same type - returns first inner exception
		/// Otherwise returns exception itself
		/// </summary>
		public static Exception Aggregate(this Exception exc)
		{
			var aggregateException = exc as AggregateException;
			if (aggregateException == null || aggregateException.InnerExceptions.Count == 0)
			{
				return exc;
			}

			var first = aggregateException.InnerExceptions.First();
			return aggregateException.InnerExceptions.Skip(1).All(e => e.GetType() == first.GetType()) ? first : exc;
		}
	}
}