using System;
using System.Windows.Input;

namespace Client
{
	class WaitCursor : IDisposable
	{
		public WaitCursor()
		{
			Mouse.OverrideCursor = Cursors.Wait;
		}

		public void Dispose()
		{
			Mouse.OverrideCursor = Cursors.Arrow;
		}
	}
}
