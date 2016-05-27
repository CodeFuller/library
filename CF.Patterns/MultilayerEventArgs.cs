using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Patterns
{
	public class MultilayerEventArgs : EventArgs
	{
		public bool Handled { get; set; }
	}
}
