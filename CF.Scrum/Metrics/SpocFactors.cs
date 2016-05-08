using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Core;

namespace CF.Scrum.Metrics
{
	[Serializable]
	public class SpocFactors
	{
		public enum Factor
		{
			[Description("Estimation error")]
			EstimationError,

			[Description("Not accepted work")]
			NotAcceptedWork,

			[Description("Not reported time")]
			NotReportedTime,

			[Description("Not priced work")]
			NotPricedWork,
		}

		private static readonly decimal IdealSpoc = (decimal)8.0;

		private readonly Dictionary<Factor, decimal> factorValues = Enum.GetValues(typeof (Factor)).Cast<Factor>().ToDictionary(e => e, e => (decimal)0);

		public decimal AcceptedPoints { get; set; }
		public decimal Capacity { get; set; }

		public decimal SpocValue
		{ get { return Capacity / AcceptedPoints; } }

		public decimal this[Factor factor]
		{
			get
			{
				return factorValues[factor];
			}
		}

		public void AddEstimationError(EstimationError error)
		{
			factorValues[Factor.EstimationError] += error.AbsoluteError;
		}

		public void AddNotAcceptedWork(decimal spentTime)
		{
			factorValues[Factor.NotAcceptedWork] += spentTime;
		}

		public void AddNotReportedTime(decimal time)
		{
			factorValues[Factor.NotReportedTime] += time;
		}

		public void AddNotPricedWork(decimal time)
		{
			factorValues[Factor.NotPricedWork] += time;
		}

		public void AddAcceptedStoryPoints(decimal stp)
		{
			AcceptedPoints += stp;
		}
		public void AddCapacity(decimal time)
		{
			Capacity += time;
		}

		public decimal FactorShare(Factor f)
		{
			return factorValues[f]/factorValues.Values.Sum();
		}

		public static SpocFactors operator +(SpocFactors f1, SpocFactors f2)
		{
			var result = new SpocFactors()
			{
				AcceptedPoints = f1.AcceptedPoints + f2.AcceptedPoints,
				Capacity = f1.Capacity + f2.Capacity
			};

			foreach (var fv in f1.factorValues.Concat(f2.factorValues))
			{
				result.factorValues[fv.Key] += fv.Value;
			}

			return result;
		}

		public override string ToString()
		{
			var spocExcess = SpocValue - IdealSpoc;

			var str = "";
			str += String.Format("Capacity:\t\t\t{0:0.0} h\n", Capacity);
			str += String.Format("Accepted points:\t\t{0:0.0} stp\n", AcceptedPoints);
			str += String.Format("SPOC:\t\t\t\t{0:0.0} h\n", SpocValue);
			str += String.Format("SPOC Excess:\t\t\t{0:0.0} h\n", spocExcess);
			str += "\n";
			str += "SPOC Factors:\n";
			foreach (var factor in factorValues.OrderByDescending(pair => pair.Value))
			{
				var share = FactorShare(factor.Key);
				str += String.Format("\t{0}:\t{1:0.0} h\t{2:0.0}%\t+{3:0.0} h\n", factor.Key.GetDescription(), factor.Value, 100 * share, share * spocExcess);
			}

			return str;
		}
	}

	public static class SpocFactorsExtensions
	{
		public static SpocFactors Sum(this IEnumerable<SpocFactors> source)
		{
			return source.Aggregate((x, y) => x + y);
		}
	}
}
