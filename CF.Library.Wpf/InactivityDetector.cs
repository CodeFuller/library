using System;
using System.Runtime.InteropServices;
using CF.Library.Core.Exceptions;
using CF.Library.Core.Facades;

namespace CF.Library.Wpf
{
	internal static class NativeMethods
	{
		public struct LASTINPUTINFO
		{
			public uint cbSize;
			public uint dwTime;
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
	}

	/// <summary>
	/// Implementation for IInactivityDetector.
	/// </summary>
	public class InactivityDetector : IInactivityDetector, IDisposable
	{
		private bool deactivated;

		/// <summary>
		/// Property Injection for IProcessStateManager.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Setter could be used by Unit tests from assembly for which internals are visible.")]
		internal ITimerFacade Timer { get; set; } = new TimerFacade();

		/// <summary>
		/// Max user inactivity duration after which Deactivated is fired.
		/// </summary>
		public TimeSpan InactivityThreshold { get; set; }

		/// <summary>
		/// Event that is fired when user is inactive during InactivityThreshold period.
		/// </summary>
		public event EventHandler<InactivationDetectedEventArgs> Deactivated;

		/// <summary>
		/// Event that is fired after user returns to work after some period of inactivity.
		/// </summary>
		public event EventHandler<ReactivationDetectedEventArgs> Reactivated;

		/// <summary>
		/// Constructor
		/// </summary>
		public InactivityDetector(TimeSpan inactivityThreshold)
		{
			InactivityThreshold = inactivityThreshold;
			Timer.Elapsed += (sender, e) => this.OnTick();
			Timer.Interval = 1000;
		}

		/// <summary>
		/// Starts inactivity detection
		/// </summary>
		public void Start()
		{
			deactivated = false;
			Timer.Start();
		}

		/// <summary>
		/// Stops inactivity detection
		/// </summary>
		public void Stop()
		{
			Timer.Stop();
		}

		/// <summary>
		/// Returns true if detector is in active state.
		/// </summary>
		public bool IsActive => Timer.Enabled && !deactivated;

		private void OnTick()
		{
			var inactivitySpan = GetInactivitySpan();

			if (!deactivated)
			{
				if (inactivitySpan >= InactivityThreshold)
				{
					deactivated = true;
					OnInactivition(inactivitySpan);
				}
			}
			else
			{
				if (inactivitySpan < InactivityThreshold)
				{
					deactivated = false;
					OnReactivation(inactivitySpan);
				}
			}
		}

		/// <summary>
		/// Is called when user is inactive during Threshold period
		/// </summary>
		protected virtual void OnInactivition(TimeSpan inactivitySpan)
		{
			Deactivated?.Invoke(this, new InactivationDetectedEventArgs(inactivitySpan));
		}

		/// <summary>
		/// Is called when user returns to work after some period of inactivity
		/// </summary>
		protected virtual void OnReactivation(TimeSpan inactivitySpan)
		{
			Reactivated?.Invoke(this, new ReactivationDetectedEventArgs(inactivitySpan));
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1404:CallGetLastErrorImmediatelyAfterPInvoke", Justification = "False Positive: GetLastWin32Error() is called after GetLastInputInfo() in case of error, not after TimeSpan.FromMilliseconds()")]
		private static TimeSpan GetInactivitySpan()
		{
			var lastInputInfo = new NativeMethods.LASTINPUTINFO();
			lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
			lastInputInfo.dwTime = 0;

			if (NativeMethods.GetLastInputInfo(ref lastInputInfo))
			{
				return TimeSpan.FromMilliseconds(Environment.TickCount - (int)lastInputInfo.dwTime);
			}
			else
			{
				throw new SystemCallFailedException(Marshal.GetLastWin32Error());
			}
		}

		/// <summary>
		/// Implementation for IDisposable.Dispose()
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases object resources
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", Justification = "False positive. See http://stackoverflow.com/q/34583417/5740031 for details.")]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Timer?.Dispose();
			}
		}
	}
}
