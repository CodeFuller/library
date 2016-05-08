using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Scrum.Objects;

namespace CF.Scrum.Metrics
{
	public class GenericPersonMetric<T>
	{
		public Person Person { get; set; }

		public T Value { get; set; }

		public GenericPersonMetric(Person person, T value)
		{
			Person = person;
			Value = value;
		}
	}

	public class PersonMetric : GenericPersonMetric<decimal>
	{
		public PersonMetric(Person person, decimal value) : base(person, value)
		{
		}
	}
}
