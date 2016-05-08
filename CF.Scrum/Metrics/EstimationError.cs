using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Scrum.Metrics
{
	public class EstimationError
	{
		public decimal EstimatedTime { get; set; }

		public decimal SpentTime { get; set; }

		public decimal Value
		{ get { return (SpentTime - EstimatedTime) / EstimatedTime; } }

		public decimal AbsoluteError
		{ get { return SpentTime - EstimatedTime; } }

		public EstimationError()
		{
		}

		public EstimationError(decimal estimatedTime, decimal spentTime)
		{
			EstimatedTime = estimatedTime;
			SpentTime = spentTime;
		}

		public static EstimationError operator +(EstimationError err1, EstimationError err2)
		{
			return new EstimationError()
			{
				EstimatedTime = err1.EstimatedTime + err2.EstimatedTime,
				SpentTime = err1.SpentTime + err2.SpentTime
			};
		}

		public override string ToString()
		{
			return (EstimatedTime != 0) ?	String.Format("{0:+0.0#;-0.0#;0.0}% ({1:n2} -> {2:n2})", 100 * Value, EstimatedTime, SpentTime) :
											String.Format("+∞ ({0:n2} -> {1:n2})", EstimatedTime, SpentTime);
		}
	}

	public static class EstimationErrorExtensions
	{
		public static EstimationError Sum(this IEnumerable<EstimationError> source)
		{
			return source.Aggregate((x, y) => x + y);
		}

		public static EstimationError Sum(this IEnumerable<GenericPersonMetric<EstimationError>> source)
		{
			return source.Aggregate(new EstimationError(), (x, y) => x + y.Value, result => result);
		}
	}
}
