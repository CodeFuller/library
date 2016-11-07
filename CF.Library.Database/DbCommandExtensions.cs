﻿using System;
using System.Data;

namespace CF.Library.Database
{
	/// <summary>
	/// Holder for IDbCommand extension methods.
	/// </summary>
	public static class DbCommandExtensions
	{
		/// <summary>
		/// Prepares IDbCommand for Stored Procedure call.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:ReviewSqlQueriesForSecurityVulnerabilities", Justification = "Command text doesn't contain user input")]
		public static void SetStoredProcedure(this IDbCommand command, string storedProcedureName)
		{
			if (command == null)
			{
				throw new ArgumentNullException(nameof(command));
			}
			if (storedProcedureName == null)
			{
				throw new ArgumentNullException(nameof(storedProcedureName));
			}

			command.CommandText = storedProcedureName;
			command.CommandType = CommandType.StoredProcedure;
		}

		/// <summary>
		/// Adds parameter to the command and sets its value.
		/// </summary>
		public static void AddParameterWithValue(this IDbCommand command, string parameterName, object value)
		{
			if (command == null)
			{
				throw new ArgumentNullException(nameof(command));
			}
			if (parameterName == null)
			{
				throw new ArgumentNullException(nameof(parameterName));
			}

			var parameter = command.CreateParameter();
			parameter.ParameterName = parameterName;
			parameter.Value = value ?? DBNull.Value;
			command.Parameters.Add(parameter);
		}
	}
}
