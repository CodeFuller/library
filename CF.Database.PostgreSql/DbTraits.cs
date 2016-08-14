﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Extensions.Database;
using CF.Core.Exceptions.Database;
using Npgsql;
using static System.FormattableString;
using static CF.Core.FormattableStringExtensions;

namespace CF.Database.PostgreSql
{
	public class DbTraits : IDbTraits
	{
		/// <summary>
		/// Implementation of ISpecificDbAdapter.PrepareConnection()
		/// </summary>
		public void PrepareConnection(IDbConnection connection)
		{
		}

		/// <summary>
		/// Implementation of ISpecificDbAdapter.GetParameterId()
		/// </summary>
		public string GetParameterId(int index, string name)
		{
			return Invariant($":{name}");
		}

		/// <summary>
		/// Implementation of ISpecificDbAdapter.SerializeValue()
		/// </summary>
		public object SerializeValue(object value)
		{
			Uri uri = value as Uri;
			if (uri != null)
			{
				return uri.OriginalString;
			}

			return value;
		}

		/// <summary>
		/// Implementation of ISpecificDbAdapter.DeserializeValue()
		/// </summary>
		public object DeserializeValue(object data, Type outputType)
		{
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			if (outputType == typeof(Uri))
			{
				var str = data as String;
				if (str == null)
				{
					throw new UnexpectedDbDataTypeException();
				}
				return new Uri(str);
			}

			if (data.GetType() != outputType)
			{
				throw new UnexpectedDbDataTypeException();
			}

			return data;
		}

		/// <summary>
		/// Implementation of ISpecificDbAdapter.MapEnum()
		/// </summary>
		public void MapEnum<TEnum>(IDbConnection connection) where TEnum : struct
		{
			NpgsqlConnection pgSqlConnection = (NpgsqlConnection)connection;
			if (pgSqlConnection == null)
			{
				throw new ArgumentException(Current($"Connection should has type of {typeof(NpgsqlConnection)}"));
			}

			pgSqlConnection.MapEnum<TEnum>();
		}
	}
}