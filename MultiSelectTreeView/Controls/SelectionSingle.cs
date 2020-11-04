using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace System.Windows.Controls
{
	public class SelectionSingle : ISelectionStrategy
	{
		private readonly MultiSelectTreeView treeView;

		
		public SelectionSingle(MultiSelectTreeView treeView)
		{
			this.treeView = treeView;
		}
		
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
		
		public event EventHandler<PreviewSelectionChangedEventArgs> PreviewSelectionChanged;
		public void ApplyTemplate()
		{
		}
		
		public bool Select(MultiSelectTreeViewItem item)
		{
			if (treeView.SelectedItems.Count == 1 &&
			    treeView.SelectedItems[0] == item.DataContext)
			{
				// Requested to select the single already-selected item. Don't change the selection.
				FocusHelper.Focus(item, true);
				return true;
			}
			else
			{
				return SelectCore(item);
			}
		}

		public bool SelectCore(MultiSelectTreeViewItem item)
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
				return false;
			}

			treeView.SelectedItems.Add(item.DataContext);
			FocusHelper.Focus(item, true);
			return true;
		}

		public bool Deselect(MultiSelectTreeViewItem item, bool bringIntoView = false)
		{
			var e = new PreviewSelectionChangedEventArgs(false, item.DataContext);
			OnPreviewSelectionChanged(e);
			if (e.CancelAny) return false;

			treeView.SelectedItems.Remove(item.DataContext);
			FocusHelper.Focus(item, bringIntoView);
			return true;
		}

		public bool SelectPreviousFromKey()
		{
			List<MultiSelectTreeViewItem> items = MultiSelectTreeView.RecursiveTreeViewItemEnumerable(treeView, false, false).ToList();
			MultiSelectTreeViewItem item = treeView.GetPreviousItem(GetFocusedItem(), items);
			return SelectFromKey(item);
		}

		public bool SelectNextFromKey()
		{
			List<MultiSelectTreeViewItem> items = MultiSelectTreeView.RecursiveTreeViewItemEnumerable(treeView, false, false).ToList();
			MultiSelectTreeViewItem item = treeView.GetNextItem(GetFocusedItem(), items);
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
			return false;
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

		public bool SelectCurrentBySpace()
		{
			var item = GetFocusedItem();
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
			
			FocusHelper.Focus(item, true);
			return true;
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
			FrameworkElement itemContent = (FrameworkElement) item.Template.FindName("PART_Header", item);
			if (itemContent == null) {
				return down ? SelectLastFromKey() : SelectFirstFromKey();
			}

			double offset = treeView.ActualHeight - 2 * ((FrameworkElement)itemContent.Parent).ActualHeight;
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
		
		private bool SelectFromKey(MultiSelectTreeViewItem item)
		{
			if (item == null)
			{
				return false;
			}

			return SelectCore(item);
		}

		private MultiSelectTreeViewItem GetFocusedItem()
		{
			foreach (var item in MultiSelectTreeView.RecursiveTreeViewItemEnumerable(treeView, false, false))
			{
				if (item.IsFocused) return item;
			}
			return null;
		}
		
		protected void OnPreviewSelectionChanged(PreviewSelectionChangedEventArgs e)
		{
			var handler = PreviewSelectionChanged;
			handler?.Invoke(this, e);
		}
	}
}
