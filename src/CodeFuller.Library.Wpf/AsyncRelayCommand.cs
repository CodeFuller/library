using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CodeFuller.Library.Wpf
{
	/// <summary>
	/// Asynchronous version of RelayCommand.
	/// </summary>
	public class AsyncRelayCommand : ICommand
	{
		private readonly Func<Task> commandAction;

		/// <summary>
		/// Occurs when changes occur that affect whether or not the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncRelayCommand"/> class for a given async action.
		/// </summary>
		/// <param name="commandAction">Action to execute for the command.</param>
		public AsyncRelayCommand(Func<Task> commandAction)
		{
			this.commandAction = commandAction ?? throw new ArgumentNullException(nameof(commandAction));
		}

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		/// <returns>True if the command could be executed; otherwise, false.</returns>
		public bool CanExecute(object parameter)
		{
			return true;
		}

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">This parameter will always be ignored.</param>
		public async void Execute(object parameter)
		{
			if (CanExecute(parameter))
			{
				await commandAction();
			}
		}
	}
}
