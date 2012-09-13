namespace System.Windows.Controls
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal interface ISelectionStrategy : IDisposable
    {
        void ApplyTemplate();

        bool SelectCore(MultiSelectTreeViewItem owner);

        bool Deselect(MultiSelectTreeViewItem item, bool bringIntoView = false);

        bool SelectPreviousFromKey();

        bool SelectNextFromKey();

		bool SelectFirstFromKey();

		bool SelectLastFromKey();

		bool SelectPageUpFromKey();

		bool SelectPageDownFromKey();

		bool SelectAllFromKey();

		bool SelectParentFromKey();

		bool SelectCurrentBySpace();

        bool Select(MultiSelectTreeViewItem treeViewItem);

		event EventHandler<PreviewSelectionChangedEventArgs> PreviewSelectionChanged;
    }

	public class PreviewSelectionChangedEventArgs : EventArgs
	{
		public bool Selecting { get; private set; }
		public object Item { get; private set; }
		public bool CancelThis { get; set; }
		public bool CancelAll { get; set; }

		public bool CancelAny { get { return CancelThis || CancelAll; } }

		public PreviewSelectionChangedEventArgs(bool selecting, object item)
		{
#if DEBUG
			// Make sure we don't confuse MultiSelectTreeViewItems and their DataContexts while development
			if (item is MultiSelectTreeViewItem)
				throw new ArgumentException("The selection preview event was passed a MultiSelectTreeViewItem instance. Only their DataContext instances must be used here!");
#endif

			Selecting = selecting;
			Item = item;
		}
	}
}
