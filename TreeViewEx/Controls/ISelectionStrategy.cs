namespace System.Windows.Controls
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal interface ISelectionStrategy : IDisposable
    {
        void ApplyTemplate();

        void SelectCore(TreeViewExItem owner);

        void UnSelect(TreeViewExItem item);

        void SelectPreviousFromKey();

        void SelectNextFromKey();

        void SelectCurrentBySpace();

        void Select(TreeViewExItem treeViewExItem);

        TreeViewExItem LastSelectedItem { get; }

		Func<object, bool> CanSelect { get; set; }

		void OnLostFocus();
    }
}
