using System;
using static System.FormattableString;

namespace CF.Library.Patterns
{
	/// <summary>
	/// Manages process state and switching between states, e.g. stopped, running, paused.
	/// </summary>
	public class ProcessStateManager : IProcessStateManager
	{
		/// <summary>
		/// Returns current state of the process.
		/// </summary>
		public ProcessState State { get; private set; }

		/// <summary>
		/// Constructor.
		/// </summary>
		public ProcessStateManager()
		{
			State = ProcessState.Stopped;
		}

		/// <summary>
		/// Switches process to Running state.
		/// </summary>
		public void Start()
		{
			if (State != ProcessState.Stopped && State != ProcessState.Paused)
			{
				throw new InvalidOperationException(Invariant($"Could not start while in '{State}' state"));
			}

			State = ProcessState.Running;
		}

		/// <summary>
		/// Switches process to Stopped state.
		/// </summary>
		public void Stop()
		{
			if (State != ProcessState.Running)
			{
				throw new InvalidOperationException(Invariant($"Could not stop while in '{State}' state"));
			}

			State = ProcessState.Stopped;
		}

		/// <summary>
		/// Switches process to Paused state.
		/// </summary>
		public void Pause()
		{
			if (State != ProcessState.Running)
			{
				throw new InvalidOperationException(Invariant($"Could not pause while in '{State}' state"));
			}

			State = ProcessState.Paused;
		}
	}
}
