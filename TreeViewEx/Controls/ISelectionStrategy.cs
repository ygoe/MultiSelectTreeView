namespace System.Windows.Controls
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal interface ISelectionStrategy : IDisposable
    {
        void ApplyTemplate();

        bool SelectCore(TreeViewExItem owner);

        bool UnSelect(TreeViewExItem item);

        bool SelectPreviousFromKey();

        bool SelectNextFromKey();

		bool SelectFirstFromKey();

		bool SelectLastFromKey();

		bool SelectParentFromKey();

		bool SelectCurrentBySpace();

        bool Select(TreeViewExItem treeViewExItem);

		event EventHandler<PreviewSelectionChangedEventArgs> PreviewSelectionChanged;
    }

	public class PreviewSelectionChangedEventArgs : EventArgs
	{
		public bool Selecting { get; private set; }
		public object Item { get; private set; }
		public bool Cancel { get; set; }

		public PreviewSelectionChangedEventArgs(bool selecting, object item)
		{
			Selecting = selecting;
			Item = item;
		}
	}
}
