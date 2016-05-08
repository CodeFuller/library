using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Patterns
{
	public class LinkedEventHandler<TEventArgs> where TEventArgs : EventArgs
	{
		private event EventHandler<TEventArgs> ParentHandler;

		public event EventHandler<TEventArgs> SelfHandler;

		public LinkedEventHandler(LinkedEventHandler<TEventArgs> parent, EventHandler<TEventArgs> selfHandler)
		{
			if (parent != null)
			{
				ParentHandler += parent.OnEvent;
			}

			if (selfHandler != null)
			{
				SelfHandler += selfHandler;
			}
		}
		public LinkedEventHandler(LinkedEventHandler<TEventArgs> parent)
			: this(parent, null)
		{
		}
		public LinkedEventHandler(EventHandler<TEventArgs> selfHandler)
			: this(null, selfHandler)
		{
		}

		public LinkedEventHandler()
			: this(null, null)
		{
		}

		public void TriggerEvent(object sender, TEventArgs e)
		{
			OnEvent(sender, e);
		}

		protected virtual void OnEvent(object sender, TEventArgs e)
		{
			if (SelfHandler != null)
			{
				SelfHandler(sender, e);
			}
			if (ParentHandler != null)
			{
				ParentHandler(sender, e);
			}
		}
	}
}
