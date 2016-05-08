using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Runtime.InteropServices;
using CF.Core.Exceptions;
using CF.Extensions;

namespace CF.Extensions.Wpf
{
	public class InactivityDetector
	{
		private readonly Timer tickTimer = new Timer();

		private bool deactivated;

		public TimeSpan Threshold { get; set; }

		public event EventHandler<InactivationDetectedEventArgs> InactivationDetected;
		public event EventHandler<ReactivationDetectedEventArgs> ReactivationDetected;

		public InactivityDetector(TimeSpan threshold)
		{
			Threshold = threshold;
			tickTimer.Elapsed += (sender, e) => this.OnTick();
			tickTimer.Interval = 1000;
		}

		public void Start()
		{
			Logger.Debug("Started inactivity detector");
			deactivated = false;
			tickTimer.Start();
		}

		public void Stop()
		{
			Logger.Debug("Stopped inactivity detector");
			tickTimer.Stop();
		}

		public bool IsRunning()
		{
			return tickTimer.Enabled;
		}

		public bool IsActive()
		{
			return IsRunning() && !deactivated;
		}

		public void OnTick()
		{
			try
			{
				var inactivitySpan = GetInactivitySpan();

				if (!deactivated)
				{
					if (inactivitySpan >= Threshold)
					{
						deactivated = true;
						OnInactivition(inactivitySpan);
					}
				}
				else
				{
					if (inactivitySpan < Threshold)
					{
						deactivated = false;
						OnReactivation(inactivitySpan);
					}
				}
			}
			catch (Exception e)
			{
				Logger.Error(e);
				throw;
			}
		}

		protected virtual void OnInactivition(TimeSpan inactivitySpan)
		{
			if (InactivationDetected != null)
			{
				InactivationDetected(this, new InactivationDetectedEventArgs(inactivitySpan));
			}
		}
		protected virtual void OnReactivation(TimeSpan inactivitySpan)
		{
			if (ReactivationDetected != null)
			{
				ReactivationDetected(this, new ReactivationDetectedEventArgs(inactivitySpan));
			}
		}

		struct LASTINPUTINFO
		{
			public uint cbSize;
			public uint dwTime;
		}

		[DllImport("user32.dll")]
		static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

		[DllImport("kernel32.dll")]
		static extern uint GetLastError();

		private static TimeSpan GetInactivitySpan()
		{
			var lastInputInfo = new LASTINPUTINFO();
			lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
			lastInputInfo.dwTime = 0;

			if (GetLastInputInfo(ref lastInputInfo))
			{
				return TimeSpan.FromMilliseconds(Environment.TickCount - (int)lastInputInfo.dwTime);
			}
			else
			{
				throw new SystemCallFailedException((int)GetLastError());
			}
		}
	}
}
