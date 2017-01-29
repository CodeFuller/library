namespace CF.Library.Patterns
{
	/// <summary>
	/// Interface for managing of process state and switching between states, e.g. stopped, running, paused.
	/// </summary>
	public interface IProcessStateManager
	{
		/// <summary>
		/// Switches process to Running state.
		/// </summary>
		void Start();

		/// <summary>
		/// Switches process to Stopped state.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = "Stop name is the best in current semantics.")]
		void Stop();

		/// <summary>
		/// Switches process to Paused state.
		/// </summary>
		void Pause();
	}
}
