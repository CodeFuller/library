using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Scrum.Objects;

namespace CF.Scrum.Reports
{
	[Serializable]
	public class ReportedPerson : IEquatable<ReportedPerson>
	{
		private const string AlienName = "Alien";

		public string FullName { get; set; }

		public static ReportedPerson Alien
		{
			get { return new ReportedPerson() { FullName = AlienName }; }
		}

		public bool IsAlien
		{
			get { return this.Equals(Alien); }
		}

		private ReportedPerson()
		{
		}

		public ReportedPerson(Person person)
		{
			FullName = person.DisplayName;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ReportedPerson);
		}

		public bool Equals(ReportedPerson cmp)
		{
			return cmp != null && FullName.Equals(cmp.FullName);
		}

		public override int GetHashCode()
		{
			return FullName.GetHashCode();
		}
	}
}
