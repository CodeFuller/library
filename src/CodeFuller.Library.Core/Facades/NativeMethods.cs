using System.Runtime.InteropServices;

namespace CodeFuller.Library.Core.Facades
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
}
