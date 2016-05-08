using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Core.Exceptions;

namespace CF.Scrum.Objects
{
	public enum PersonRole
	{
		Undefined,
		Developer,
		QA,
		BusinessAnalyst,
		ProductOwner,
		ScrumMaster
	}

	public class Person : BaseObject
	{
		//	CF 2DO: Use lazy loading for user data. We often need only id.
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string DisplayName { get; set; }
		public string Email { get; set; }
		/// <summary>
		/// Is used for Scrum provider authentication
		/// </summary>
		public string UserName { get; set; }

		public PersonRole Role { get; set; }

		public Person(string id) : base(id)
		{
		}

		public override string ToString()
		{
			return DisplayName;
		}
	}
}
