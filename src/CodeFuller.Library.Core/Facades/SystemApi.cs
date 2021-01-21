using System;
using System.Runtime.InteropServices;
using CodeFuller.Library.Core.Exceptions;

namespace CodeFuller.Library.Core.Facades
{
	public class SystemApi : ISystemApi
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1404:CallGetLastErrorImmediatelyAfterPInvoke", Justification = "False Positive: GetLastWin32Error() is called after GetLastInputInfo() in case of error, not after TimeSpan.FromMilliseconds()")]
		public TimeSpan GetUserInactivitySpan()
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
	}
}
