using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Controls
{
	/// <summary>
	/// Implements the logic for the multiple selection strategy.
	/// </summary>
	public class SelectionMultiple : ISelectionStrategy
	{
		public event EventHandler<PreviewSelectionChangedEventArgs> PreviewSelectionChanged;

		private readonly MultiSelectTreeView treeView;
		private BorderSelectionLogic borderSelectionLogic;
		private object lastShiftRoot;

		public SelectionMultiple(MultiSelectTreeView treeView)
		{
			this.treeView = treeView;
		}

		#region Properties

		public bool LastCancelAll { get; private set; }
		
		internal static bool IsControlKeyDown
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
			   treeView,
			   treeView.Template.FindName("selectionBorder", treeView) as Border,
			   treeView.Template.FindName("scrollViewer", treeView) as ScrollViewer,
			   treeView.Template.FindName("content", treeView) as ItemsPresenter,
			   MultiSelectTreeView.RecursiveTreeViewItemEnumerable(treeView, false, false));
		}

		public bool Select(MultiSelectTreeViewItem item)
		{
			if (IsControlKeyDown)
			{
				if (treeView.SelectedItems.Contains(item.DataContext))
				{
					return Deselect(item, true);
				}
				else
				{
					var e = new PreviewSelectionChangedEventArgs(true, item.DataContext);
					OnPreviewSelectionChanged(e);
					if (e.CancelAny)
					{
						FocusHelper.Focus(item, true);
						return false;
					}

					return SelectCore(item);
				}
			}
			else
			{
				if (treeView.SelectedItems.Count == 1 &&
					treeView.SelectedItems[0] == item.DataContext)
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

		internal bool SelectByRectangle(MultiSelectTreeViewItem item)
		{
			var e = new PreviewSelectionChangedEventArgs(true, item.DataContext);
			OnPreviewSelectionChanged(e);
			if (e.CancelAny)
			{
				lastShiftRoot = item.DataContext;
				return false;
			}

			if (!treeView.SelectedItems.Contains(item.DataContext))
			{
				treeView.SelectedItems.Add(item.DataContext);
			}
			lastShiftRoot = item.DataContext;
			return true;
		}

		internal bool DeselectByRectangle(MultiSelectTreeViewItem item)
		{
			var e = new PreviewSelectionChangedEventArgs(false, item.DataContext);
			OnPreviewSelectionChanged(e);
			if (e.CancelAny)
			{
				lastShiftRoot = item.DataContext;
				return false;
			}

			treeView.SelectedItems.Remove(item.DataContext);
			if (item.DataContext == lastShiftRoot)
			{
				lastShiftRoot = null;
			}
			return true;
		}

		public bool SelectCore(MultiSelectTreeViewItem item)
		{
			if (IsControlKeyDown)
			{
				if (!treeView.SelectedItems.Contains(item.DataContext))
				{
					treeView.SelectedItems.Add(item.DataContext);
				}
				lastShiftRoot = item.DataContext;
			}
			else if (IsShiftKeyDown && treeView.SelectedItems.Count > 0)
			{
				object firstSelectedItem = lastShiftRoot ?? treeView.SelectedItems.First();
				MultiSelectTreeViewItem shiftRootItem = treeView.GetTreeViewItemsFor(new List<object> { firstSelectedItem }).First();

				var newSelection = treeView.GetNodesToSelectBetween(shiftRootItem, item).Select(n => n.DataContext).ToList();
				// Make a copy of the list because we're modifying it while enumerating it
				var selectedItems = new object[treeView.SelectedItems.Count];
				treeView.SelectedItems.CopyTo(selectedItems, 0);
				// Remove all items no longer selected
				foreach (var selItem in selectedItems.Where(i => !newSelection.Contains(i)))
				{
					var e = new PreviewSelectionChangedEventArgs(false, selItem);
					OnPreviewSelectionChanged(e);
					if (e.CancelAll)
					{
						FocusHelper.Focus(item);
						return false;
					}
					if (!e.CancelThis)
					{
						treeView.SelectedItems.Remove(selItem);
					}
				}
				// Add new selected items
				foreach (var newItem in newSelection.Where(i => !selectedItems.Contains(i)))
				{
					var e = new PreviewSelectionChangedEventArgs(true, newItem);
					OnPreviewSelectionChanged(e);
					if (e.CancelAll)
					{
						FocusHelper.Focus(item, true);
						return false;
					}
					if (!e.CancelThis)
					{
						treeView.SelectedItems.Add(newItem);
					}
				}
			}
			else
			{
				if (treeView.SelectedItems.Count > 0)
				{
					foreach (var selItem in new ArrayList(treeView.SelectedItems))
					{
						var e2 = new PreviewSelectionChangedEventArgs(false, selItem);
						OnPreviewSelectionChanged(e2);
						if (e2.CancelAll)
						{
							FocusHelper.Focus(item);
							lastShiftRoot = item.DataContext;
							return false;
						}
						if (!e2.CancelThis)
						{
							treeView.SelectedItems.Remove(selItem);
						}
					}
				}
				
				var e = new PreviewSelectionChangedEventArgs(true, item.DataContext);
				OnPreviewSelectionChanged(e);
				if (e.CancelAny)
				{
					FocusHelper.Focus(item, true);
					lastShiftRoot = item.DataContext;
					return false;
				}

				treeView.SelectedItems.Add(item.DataContext);
				lastShiftRoot = item.DataContext;
			}

			FocusHelper.Focus(item, true);
			return true;
		}

		public bool SelectCurrentBySpace()
		{
			// Another item was focused by Ctrl+Arrow key
			var item = GetFocusedItem();
			if (treeView.SelectedItems.Contains(item.DataContext))
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
				if (e.CancelAny)
				{
					FocusHelper.Focus(item, true);
					return false;
				}

				item.IsSelected = true;
				if (!treeView.SelectedItems.Contains(item.DataContext))
				{
					treeView.SelectedItems.Add(item.DataContext);
				}
			}
			FocusHelper.Focus(item, true);
			return true;
		}

		private MultiSelectTreeViewItem GetFocusedItem()
		{
			foreach (var item in MultiSelectTreeView.RecursiveTreeViewItemEnumerable(treeView, false, false))
			{
				if (item.IsFocused) return item;
			}
			return null;
		}

		private bool SelectFromKey(MultiSelectTreeViewItem item)
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
			List<MultiSelectTreeViewItem> items = MultiSelectTreeView.RecursiveTreeViewItemEnumerable(treeView, false, false).ToList();
			MultiSelectTreeViewItem item = treeView.GetNextItem(GetFocusedItem(), items);
			return SelectFromKey(item);
		}

		public bool SelectPreviousFromKey()
		{
			List<MultiSelectTreeViewItem> items = MultiSelectTreeView.RecursiveTreeViewItemEnumerable(treeView, false, false).ToList();
			MultiSelectTreeViewItem item = treeView.GetPreviousItem(GetFocusedItem(), items);
			return SelectFromKey(item);
		}

		public bool SelectFirstFromKey()
		{
			List<MultiSelectTreeViewItem> items = MultiSelectTreeView.RecursiveTreeViewItemEnumerable(treeView, false, false).ToList();
			MultiSelectTreeViewItem item = treeView.GetFirstItem(items);
			return SelectFromKey(item);
		}

		public bool SelectLastFromKey()
		{
			List<MultiSelectTreeViewItem> items = MultiSelectTreeView.RecursiveTreeViewItemEnumerable(treeView, false, false).ToList();
			MultiSelectTreeViewItem item = treeView.GetLastItem(items);
			return SelectFromKey(item);
		}

		private bool SelectPageUpDown(bool down)
		{
			List<MultiSelectTreeViewItem> items = MultiSelectTreeView.RecursiveTreeViewItemEnumerable(treeView, false, false).ToList();
			MultiSelectTreeViewItem item = GetFocusedItem();
			if (item == null)
			{
				return down ? SelectLastFromKey() : SelectFirstFromKey();
			}

			double targetY = item.TransformToAncestor(treeView).Transform(new Point()).Y;
			FrameworkElement itemContent = (FrameworkElement) item.Template.FindName("headerBorder", item);
			double offset = treeView.ActualHeight - 2 * itemContent.ActualHeight;
			if (!down) offset = -offset;
			targetY += offset;
			while (true)
			{
				var newItem = down ? treeView.GetNextItem(item, items) : treeView.GetPreviousItem(item, items);
				if (newItem == null) break;
				item = newItem;
				double itemY = item.TransformToAncestor(treeView).Transform(new Point()).Y;
				if (down && itemY > targetY ||
					!down && itemY < targetY)
				{
					break;
				}
			}
			return SelectFromKey(item);
		}

		public bool SelectPageUpFromKey()
		{
			return SelectPageUpDown(false);
		}

		public bool SelectPageDownFromKey()
		{
			return SelectPageUpDown(true);
		}

		public bool SelectAllFromKey()
		{
			var items = MultiSelectTreeView.RecursiveTreeViewItemEnumerable(treeView, false, false).ToList();
			// Add new selected items
			foreach (var item in items.Where(i => !treeView.SelectedItems.Contains(i.DataContext)))
			{
				var e = new PreviewSelectionChangedEventArgs(true, item.DataContext);
				OnPreviewSelectionChanged(e);
				if (e.CancelAll)
				{
					return false;
				}
				if (!e.CancelThis)
				{
					treeView.SelectedItems.Add(item.DataContext);
				}
			}
			return true;
		}

		public bool SelectParentFromKey()
		{
			DependencyObject parent = GetFocusedItem();
			while (parent != null)
			{
				parent = VisualTreeHelper.GetParent(parent);
				if (parent is MultiSelectTreeViewItem) break;
			}
			return SelectFromKey(parent as MultiSelectTreeViewItem);
		}

		public bool Deselect(MultiSelectTreeViewItem item, bool bringIntoView = false)
		{
			var e = new PreviewSelectionChangedEventArgs(false, item.DataContext);
			OnPreviewSelectionChanged(e);
			if (e.CancelAny) return false;

			treeView.SelectedItems.Remove(item.DataContext);
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
				LastCancelAll = e.CancelAll;
			}
		}
	}
}
