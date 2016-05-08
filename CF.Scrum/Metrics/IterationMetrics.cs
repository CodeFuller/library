using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Scrum.Metrics
{
	[Serializable]
	public class IterationMetrics
	{
		public decimal Capacity
		{
			get { return SpocFactors.Capacity; }
		}

		public decimal AcceptedStp
		{
			get { return SpocFactors.AcceptedPoints; }
		}

		public decimal Spoc
		{
			get { return AcceptedStp == 0 ? int.MaxValue : Capacity / AcceptedStp; }
		}

		public decimal FocusFactor
		{
			get { return AcceptedStp == 0 ? 0 : Analyzer.IdealDayDuration / Spoc; }
		}

		public SpocFactors SpocFactors { get; set; }

		public IterationMetrics(SpocFactors spocFactors)
		{
			SpocFactors = spocFactors;
		}

		public static IterationMetrics operator +(IterationMetrics m1, IterationMetrics m2)
		{
			return new IterationMetrics(m1.SpocFactors + m2.SpocFactors);
		}
	}

	public static class IterationMetricsExtensions
	{
		public static IterationMetrics Sum(this IEnumerable<IterationMetrics> source)
		{
			return source.Aggregate((x, y) => x + y);
		}
	}
}
