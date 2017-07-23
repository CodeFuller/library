using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CF.Library.Wpf
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
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		/// <summary>
		/// Constructs instance of the command for given async action.
		/// </summary>
		public AsyncRelayCommand(Func<Task> commandAction)
		{
			if (commandAction == null)
			{
				throw new ArgumentNullException(nameof(commandAction));
			}

			this.commandAction = commandAction;
		}

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
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
