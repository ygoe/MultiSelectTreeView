using System.Threading;
using System.Windows.Threading;

namespace System.Windows.Controls
{
    /// <summary>
    /// Helper methods to focus.
    /// </summary>
    public static class FocusHelper
    {
        #region Public methods

        public static void Focus(EditTextBox element)
        {
            //System.Diagnostics.Debug.WriteLine("Focus textbox with helper:" + element.Text);
            FocusCore(element);
			element.BringIntoView();
		}

        public static void Focus(MultiSelectTreeViewItem element, bool bringIntoView = false)
        {
			//System.Diagnostics.Debug.WriteLine("FocusHelper focusing " + (bringIntoView ? "[into view] " : "") + element.DataContext);
            FocusCore(element);

			if (bringIntoView)
			{
				FrameworkElement itemContent = (FrameworkElement) element.Template.FindName("headerBorder", element);
				itemContent.BringIntoView();
			}
		}

        public static void Focus(MultiSelectTreeView element)
        {
            //System.Diagnostics.Debug.WriteLine("Focus Tree with helper");
            FocusCore(element);
			element.BringIntoView();
		}

        private static void FocusCore(FrameworkElement element)
        {
            if (!element.Focus())
            {
                element.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() => element.Focus()));
            }

#if DEBUG
            // no good idea, seems to block sometimes
            int i = 0;
            while (i < 5)
            {
                if (element.IsFocused) return;
                Thread.Sleep(100);
                i++;
            }
            if (i >= 5)
            {
            }
#endif
		}

		#endregion Public methods
	}
}