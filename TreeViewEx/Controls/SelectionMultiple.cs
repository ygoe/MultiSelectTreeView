namespace System.Windows.Controls
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows.Input;
	using System.Windows.Media;

	/// <summary>
	/// Logic for the multiple selection
	/// </summary>
	public class SelectionMultiple : ISelectionStrategy
	{
		public event EventHandler<PreviewSelectionChangedEventArgs> PreviewSelectionChanged;

		private readonly TreeViewEx treeViewEx;
		private BorderSelectionLogic borderSelectionLogic;
		private object lastShiftRoot;

		public SelectionMultiple(TreeViewEx treeViewEx)
		{
			this.treeViewEx = treeViewEx;
		}

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

		#endregion

		public void ApplyTemplate()
		{
			borderSelectionLogic = new BorderSelectionLogic(
			   treeViewEx,
			   treeViewEx.Template.FindName("selectionBorder", treeViewEx) as Border,
			   treeViewEx.Template.FindName("scrollViewer", treeViewEx) as ScrollViewer,
			   treeViewEx.Template.FindName("content", treeViewEx) as ItemsPresenter,
			   TreeViewEx.RecursiveTreeViewItemEnumerable(treeViewEx, false, false));
		}

		public bool Select(TreeViewExItem item)
		{
			if (IsControlKeyDown)
			{
				if (treeViewEx.SelectedItems.Contains(item.DataContext))
				{
					return Deselect(item, true);
				}
				else
				{
					var e = new PreviewSelectionChangedEventArgs(true, item.DataContext);
					OnPreviewSelectionChanged(e);
					if (e.Cancel)
					{
						FocusHelper.Focus(item, true);
						return false;
					}

					return SelectCore(item);
				}
			}
			else
			{
				if (treeViewEx.SelectedItems.Count == 1 &&
					treeViewEx.SelectedItems[0] == item.DataContext)
				{
					// Requested to select the single already-selected item. Don't change the selection.
					FocusHelper.Focus(item, true);
					lastShiftRoot = item.DataContext;
					return true;
				}
				else
				{
					return SelectCore(item);
				}
			}
		}

		internal bool SelectByRectangle(TreeViewExItem item)
		{
			var e = new PreviewSelectionChangedEventArgs(true, item.DataContext);
			OnPreviewSelectionChanged(e);
			if (e.Cancel)
			{
				lastShiftRoot = item.DataContext;
				return false;
			}

			if (!treeViewEx.SelectedItems.Contains(item.DataContext))
			{
				treeViewEx.SelectedItems.Add(item.DataContext);
			}
			lastShiftRoot = item.DataContext;
			return true;
		}

		public bool SelectCore(TreeViewExItem item)
		{
			if (IsControlKeyDown)
			{
				if (!treeViewEx.SelectedItems.Contains(item.DataContext))
				{
					treeViewEx.SelectedItems.Add(item.DataContext);
				}
				lastShiftRoot = item.DataContext;
			}
			else if (IsShiftKeyDown && treeViewEx.SelectedItems.Count > 0)
			{
				object firstSelectedItem = lastShiftRoot ?? treeViewEx.SelectedItems.First();
				TreeViewExItem shiftRootItem = treeViewEx.GetTreeViewItemsFor(new List<object> { firstSelectedItem }).First();

				var newSelection = treeViewEx.GetNodesToSelectBetween(shiftRootItem, item).Select(n => n.DataContext).ToList();
				var selectedItems = new object[treeViewEx.SelectedItems.Count];
				// Make a copy of the list because we're modifying it while enumerating it
				treeViewEx.SelectedItems.CopyTo(selectedItems, 0);
				// Remove all items no longer selected
				foreach (var selItem in selectedItems.Where(i => !newSelection.Contains(i)))
				{
					var e = new PreviewSelectionChangedEventArgs(false, selItem);
					OnPreviewSelectionChanged(e);
					if (e.Cancel)
					{
						FocusHelper.Focus(item);
						return false;
					}

					treeViewEx.SelectedItems.Remove(selItem);
				}
				// Add new selected items
				foreach (var newItem in newSelection.Where(i => !selectedItems.Contains(i)))
				{
					var e = new PreviewSelectionChangedEventArgs(true, newItem);
					OnPreviewSelectionChanged(e);
					if (e.Cancel)
					{
						FocusHelper.Focus(item, true);
						return false;
					}

					treeViewEx.SelectedItems.Add(newItem);
				}
			}
			else
			{
				if (treeViewEx.SelectedItems.Count > 0)
				{
					foreach (var selItem in treeViewEx.SelectedItems)
					{
						var e2 = new PreviewSelectionChangedEventArgs(false, selItem);
						OnPreviewSelectionChanged(e2);
						if (e2.Cancel)
						{
							FocusHelper.Focus(item);
							lastShiftRoot = item.DataContext;
							return false;
						}
					}
					
					treeViewEx.SelectedItems.Clear();
				}
				
				var e = new PreviewSelectionChangedEventArgs(true, item.DataContext);
				OnPreviewSelectionChanged(e);
				if (e.Cancel)
				{
					FocusHelper.Focus(item, true);
					lastShiftRoot = item.DataContext;
					return false;
				}

				treeViewEx.SelectedItems.Add(item.DataContext);
				lastShiftRoot = item.DataContext;
			}

			FocusHelper.Focus(item, true);
			return true;
		}

		public bool SelectCurrentBySpace()
		{
			// Another item was focused by Ctrl+Arrow key
			var item = GetFocusedItem();
			if (treeViewEx.SelectedItems.Contains(item.DataContext))
			{
				// With Ctrl key, toggle this item selection (deselect now).
				// Without Ctrl key, always select it (is already selected).
				if (IsControlKeyDown)
				{
					if (!Deselect(item, true)) return false;
					item.IsSelected = false;
				}
			}
			else
			{
				var e = new PreviewSelectionChangedEventArgs(true, item.DataContext);
				OnPreviewSelectionChanged(e);
				if (e.Cancel)
				{
					FocusHelper.Focus(item, true);
					return false;
				}

				item.IsSelected = true;
				if (!treeViewEx.SelectedItems.Contains(item.DataContext))
				{
					treeViewEx.SelectedItems.Add(item.DataContext);
				}
			}
			FocusHelper.Focus(item, true);
			return true;
		}

		private TreeViewExItem GetFocusedItem()
		{
			foreach (var item in TreeViewEx.RecursiveTreeViewItemEnumerable(treeViewEx, false, false))
			{
				if (item.IsFocused) return item;
			}
			return null;
		}

		private bool SelectFromKey(TreeViewExItem item)
		{
			if (item == null)
			{
				return false;
			}

			// If Ctrl is pressed just focus it, so it can be selected by Space. Otherwise select it.
			if (IsControlKeyDown)
			{
				FocusHelper.Focus(item, true);
				return true;
			}
			else
			{
				return SelectCore(item);
			}
		}

		public bool SelectNextFromKey()
		{
			List<TreeViewExItem> items = TreeViewEx.RecursiveTreeViewItemEnumerable(treeViewEx, false, false).ToList();
			TreeViewExItem item = treeViewEx.GetNextItem(GetFocusedItem(), items);
			return SelectFromKey(item);
		}

		public bool SelectPreviousFromKey()
		{
			List<TreeViewExItem> items = TreeViewEx.RecursiveTreeViewItemEnumerable(treeViewEx, false, false).ToList();
			TreeViewExItem item = treeViewEx.GetPreviousItem(GetFocusedItem(), items);
			return SelectFromKey(item);
		}

		public bool SelectFirstFromKey()
		{
			List<TreeViewExItem> items = TreeViewEx.RecursiveTreeViewItemEnumerable(treeViewEx, false, false).ToList();
			TreeViewExItem item = treeViewEx.GetFirstItem(items);
			return SelectFromKey(item);
		}

		public bool SelectLastFromKey()
		{
			List<TreeViewExItem> items = TreeViewEx.RecursiveTreeViewItemEnumerable(treeViewEx, false, false).ToList();
			TreeViewExItem item = treeViewEx.GetLastItem(items);
			return SelectFromKey(item);
		}

		public bool SelectParentFromKey()
		{
			DependencyObject parent = GetFocusedItem();
			while (parent != null)
			{
				parent = VisualTreeHelper.GetParent(parent);
				if (parent is TreeViewExItem) break;
			}
			return SelectFromKey(parent as TreeViewExItem);
		}

		public bool Deselect(TreeViewExItem item, bool bringIntoView = false)
		{
			var e = new PreviewSelectionChangedEventArgs(false, item.DataContext);
			OnPreviewSelectionChanged(e);
			if (e.Cancel) return false;

			treeViewEx.SelectedItems.Remove(item.DataContext);
			if (item.DataContext == lastShiftRoot)
			{
				lastShiftRoot = null;
			}
			FocusHelper.Focus(item, bringIntoView);
			return true;
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

		protected void OnPreviewSelectionChanged(PreviewSelectionChangedEventArgs e)
		{
			var handler = PreviewSelectionChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}
