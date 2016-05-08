using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Patterns;

namespace CF.Scrum.Objects
{
	public abstract class WorkItem : BaseObject
	{

		public string FormattedId { get; set; }
		public string Name { get; set; }

		public bool Blocked { get; set; }

		protected WorkItem(string id)
			: base(id)
		{
		}

		public override string ToString()
		{
			return String.Format("[{0}: {1}]", FormattedId, Name);
		}
	}
}
