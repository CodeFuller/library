using System;
using static System.FormattableString;

namespace CF.Library.Patterns
{
	/// <summary>
	/// Possible values of process state.
	/// </summary>
	public enum ProcessState
	{
		/// <summary>
		/// Process is in undefined state.
		/// </summary>
		Undefined,

		/// <summary>
		/// Process is in stopped state.
		/// </summary>
		Stopped,

		/// <summary>
		/// Process is in running state.
		/// </summary>
		Running,

		/// <summary>
		/// Process is in paused state.
		/// </summary>
		Paused,
	}

	/// <summary>
	/// Manages process state and switching between states, e.g. stopped, running, paused.
	/// </summary>
	public class ProcessStateManager : IProcessStateManager
	{
		private ProcessState state;

		/// <summary>
		/// Constructor.
		/// </summary>
		public ProcessStateManager()
		{
			state = ProcessState.Stopped;
		}

		/// <summary>
		/// Switches process to Running state.
		/// </summary>
		public void Start()
		{
			if (state != ProcessState.Stopped && state != ProcessState.Paused)
			{
				throw new InvalidOperationException(Invariant($"Could not start while in '{state}' state"));
			}

			state = ProcessState.Running;
		}

		/// <summary>
		/// Switches process to Stopped state.
		/// </summary>
		public void Stop()
		{
			if (state != ProcessState.Running)
			{
				throw new InvalidOperationException(Invariant($"Could not stop while in '{state}' state"));
			}

			state = ProcessState.Stopped;
		}

		/// <summary>
		/// Switches process to Paused state.
		/// </summary>
		public void Pause()
		{
			if (state != ProcessState.Running)
			{
				throw new InvalidOperationException(Invariant($"Could not pause while in '{state}' state"));
			}

			state = ProcessState.Paused;
		}
	}
}
