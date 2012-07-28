namespace System.Windows.Controls
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows.Input;

	/// <summary>
	/// Logic for the multiple selection
	/// </summary>
	public class SelectionMultiple : ISelectionStrategy
	{
		private readonly TreeViewEx treeViewEx;

		private BorderSelectionLogic borderSelectionLogic;

		private object lastShiftRoot;

		private TreeViewExItem selectedPreviewItem;

		public SelectionMultiple(TreeViewEx treeViewEx)
		{
			this.treeViewEx = treeViewEx;
		}

		public Func<object, bool> CanSelect { get; set; }

		#region Properties

		private static bool IsControlKeyDown
		{
			get
			{
				return (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
			}
		}

		private static bool IsShiftKeyDown
		{
			get
			{
				return (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
			}
		}

		private TreeViewExItem SelectedPreviewItem
		{
			get
			{
				return selectedPreviewItem;
			}
			set
			{
				if (selectedPreviewItem != null)
				{
					selectedPreviewItem.IsSelectedPreview = false;
				}

				selectedPreviewItem = value;
				if (selectedPreviewItem != null)
				{
					selectedPreviewItem.IsSelectedPreview = true;
				}
			}
		}

		#endregion

		public void ApplyTemplate()
		{
			borderSelectionLogic = new BorderSelectionLogic(
			   treeViewEx,
			   treeViewEx.Template.FindName("selectionBorder", treeViewEx) as Border,
			   TreeViewEx.RecursiveTreeViewItemEnumerable(treeViewEx, false));
		}

		public void Select(TreeViewExItem item)
		{
			if (CanSelect != null && item != null && !CanSelect(item.DataContext)) return;
			
			if (IsControlKeyDown)
			{
				if (treeViewEx.SelectedItems.Contains(item.DataContext))
				{
					UnSelect(item);

					if (item.DataContext == lastShiftRoot)
					{
						lastShiftRoot = null;
					}
				}
				else
				{
					item.IsSelected = true;
					treeViewEx.SelectedItems.Add(item.DataContext);
					lastShiftRoot = item.DataContext;
				}
				FocusHelper.Focus(item);
				SelectedPreviewItem = null;
			}
			else
			{
				SelectCore(item);
			}
		}

		internal void SelectByRectangle(TreeViewExItem item)
		{
			if (CanSelect != null && item != null && !CanSelect(item.DataContext)) return;

			treeViewEx.SelectedItems.Add(item.DataContext);
			SelectedPreviewItem = null;
			lastShiftRoot = item.DataContext;
		}

		public void SelectCore(TreeViewExItem item)
		{
			if (IsControlKeyDown)
			{
				SelectedPreviewItem = item;
				FocusHelper.Focus(item);
			}
			else if (IsShiftKeyDown && treeViewEx.SelectedItems.Count > 0)
			{
				object firstSelectedItem = lastShiftRoot ?? treeViewEx.SelectedItems.First();
				TreeViewExItem shiftRootItem = treeViewEx.GetTreeViewItemsFor(new List<object> { firstSelectedItem }).First();

				treeViewEx.SelectedItems.Clear();

				foreach (var node in treeViewEx.GetNodesToSelectBetween(shiftRootItem, item))
				{
					treeViewEx.SelectedItems.Add(node.DataContext);
				}

				SelectedPreviewItem = null;
				FocusHelper.Focus(item);
			}
			else
			{
				treeViewEx.SelectedItems.Clear();
				treeViewEx.SelectedItems.Add(item.DataContext);
				SelectedPreviewItem = null;
				FocusHelper.Focus(item);
				lastShiftRoot = item.DataContext;
			}

			LastSelectedItem = item;
		}

		public void SelectCurrentBySpace()
		{
			if (SelectedPreviewItem != null)
			{
				if (CanSelect != null && !CanSelect(SelectedPreviewItem.DataContext)) return;

				// There is a selection preview item that was chosen by Ctrl+Arrow key
				var item = SelectedPreviewItem;
				if (treeViewEx.SelectedItems.Contains(item.DataContext))
				{
					// With Ctrl key, toggle this item selection.
					// Without Ctrl key, always select it.
					if (IsControlKeyDown)
					{
						UnSelect(item);
						item.IsSelected = false;
					}
				}
				else
				{
					item.IsSelected = true;
					treeViewEx.SelectedItems.Add(item.DataContext);
				}
				FocusHelper.Focus(item);
			}
			else
			{
				// An item was chosen without the Ctrl key
				Select(treeViewEx.GetFocusedItem(TreeViewEx.RecursiveTreeViewItemEnumerable(treeViewEx, true)));
			}
		}

		public void SelectNextFromKey()
		{
			List<TreeViewExItem> items = TreeViewEx.RecursiveTreeViewItemEnumerable(treeViewEx, true).ToList();
			TreeViewExItem item;
			if (IsControlKeyDown && SelectedPreviewItem != null)
			{
				item = treeViewEx.GetNextItem(SelectedPreviewItem, items);
			}
			else
			{
				item = treeViewEx.GetNextItem(treeViewEx.GetFocusedItem(items), items);
			}

			if (item == null)
			{
				return;
			}

			SelectCore(item);
		}

		public void SelectPreviousFromKey()
		{
			List<TreeViewExItem> items = TreeViewEx.RecursiveTreeViewItemEnumerable(treeViewEx, true).ToList();
			TreeViewExItem item;
			if (IsControlKeyDown && SelectedPreviewItem != null)
			{
				item = treeViewEx.GetPreviousItem(SelectedPreviewItem, items);
			}
			else
			{
				item = treeViewEx.GetPreviousItem(treeViewEx.GetFocusedItem(items), items);
			}

			if (item == null)
			{
				return;
			}

			SelectCore(item);
		}

		public void UnSelect(TreeViewExItem item)
		{
			treeViewEx.SelectedItems.Remove(item.DataContext);
		}

		public void Dispose()
		{
			if (borderSelectionLogic != null)
			{
				borderSelectionLogic.Dispose();
				borderSelectionLogic = null;
			}

			GC.SuppressFinalize(this);
		}

		#region ISelectionStrategy Members


		public TreeViewExItem LastSelectedItem
		{
			get;
			private set;
		}

		public void OnLostFocus()
		{
			SelectedPreviewItem = null;
		}

		#endregion
	}
}
