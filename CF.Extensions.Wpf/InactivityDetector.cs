﻿using System;
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
	/// <summary>
	/// Detects user inactivity (no keyboard presses and no mouse move/clicks during some period of time)
	/// </summary>
	public class InactivityDetector
	{
		private readonly Timer tickTimer = new Timer();

		private bool deactivated;

		/// <summary>
		/// Max user inactivity duration after which InactivationDetected is fired
		/// </summary>
		public TimeSpan Threshold { get; set; }

		/// <summary>
		/// Event that is fired when user is inactive during Threshold period
		/// </summary>
		public event EventHandler<InactivationDetectedEventArgs> InactivationDetected;
		/// <summary>
		/// Event that is fired after user returns to work after some period of inactivity
		/// </summary>
		public event EventHandler<ReactivationDetectedEventArgs> ReactivationDetected;

		/// <summary>
		/// Constructor
		/// </summary>
		public InactivityDetector(TimeSpan threshold)
		{
			Threshold = threshold;
			tickTimer.Elapsed += (sender, e) => this.OnTick();
			tickTimer.Interval = 1000;
		}

		/// <summary>
		/// Starts inactivity detection
		/// </summary>
		public void Start()
		{
			Logger.Debug("Started inactivity detector");
			deactivated = false;
			tickTimer.Start();
		}

		/// <summary>
		/// Stops inactivity detection
		/// </summary>
		public void Stop()
		{
			Logger.Debug("Stopped inactivity detector");
			tickTimer.Stop();
		}

		/// <summary>
		/// Returns true if inactivity detection is running
		/// </summary>
		public bool IsRunning()
		{
			return tickTimer.Enabled;
		}

		/// <summary>
		/// Returns true if user is active
		/// </summary>
		public bool IsActive()
		{
			return IsRunning() && !deactivated;
		}

		private void OnTick()
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

		/// <summary>
		/// Is called when user is inactive during Threshold period
		/// </summary>
		protected virtual void OnInactivition(TimeSpan inactivitySpan)
		{
			if (InactivationDetected != null)
			{
				InactivationDetected(this, new InactivationDetectedEventArgs(inactivitySpan));
			}
		}
		/// <summary>
		/// Is called when user returns to work after some period of inactivity
		/// </summary>
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
