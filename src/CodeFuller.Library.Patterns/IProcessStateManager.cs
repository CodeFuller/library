namespace CodeFuller.Library.Patterns
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
	/// Interface for managing of process state and switching between states, e.g. stopped, running, paused.
	/// </summary>
	public interface IProcessStateManager
	{
		/// <summary>
		/// Returns current state of the process.
		/// </summary>
		ProcessState State { get; }

		/// <summary>
		/// Switches process to Running state.
		/// </summary>
		void Start();

		/// <summary>
		/// Switches process to Stopped state.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "'Stop' is the best name in current semantics.")]
		void Stop();

		/// <summary>
		/// Switches process to Paused state.
		/// </summary>
		void Pause();
	}
}
