using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CF.Patterns;

namespace CF.Scrum.Objects
{
	public class Team : BaseObject
	{
		private AsyncLazy<List<Person>> members;

		public string Name { get; set; }

		public Team(string id) : base(id)
		{
		}

		public async Task<List<Person>> GetMembers()
		{
			return await members;
		}

		/// <summary>
		/// Instant members initializer
		/// </summary>
		public void SetMembers(List<Person> members)
		{
			this.members = new AsyncLazy<List<Person>>(() => members);
		}

		/// <summary>
		/// Lazy members initializer
		/// </summary>
		public void SetMembers(Func<Task<List<Person>>> valueFactory)
		{
			members = new AsyncLazy<List<Person>>(valueFactory);
		}
	}
}
