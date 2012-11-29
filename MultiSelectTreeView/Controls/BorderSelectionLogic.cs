using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace System.Windows.Controls
{
	internal class BorderSelectionLogic : IDisposable
	{
		#region Private fields

		private MultiSelectTreeView treeView;
		private readonly Border border;
		private readonly ScrollViewer scrollViewer;
		private readonly ItemsPresenter content;
		private readonly IEnumerable<MultiSelectTreeViewItem> items;
		
		private bool isFirstMove;
		private bool mouseDown;
		private Point startPoint;
		private DateTime lastScrollTime;
		private HashSet<object> initialSelection;

		#endregion Private fields

		#region Constructor

		public BorderSelectionLogic(MultiSelectTreeView treeView, Border selectionBorder, ScrollViewer scrollViewer, ItemsPresenter content, IEnumerable<MultiSelectTreeViewItem> items)
		{
			if (treeView == null)
			{
				throw new ArgumentNullException("treeView");
			}
			if (selectionBorder == null)
			{
				throw new ArgumentNullException("selectionBorder");
			}
			if (scrollViewer == null)
			{
				throw new ArgumentNullException("scrollViewer");
			}
			if (content == null)
			{
				throw new ArgumentNullException("content");
			}
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}

			this.treeView = treeView;
			this.border = selectionBorder;
			this.scrollViewer = scrollViewer;
			this.content = content;
			this.items = items;

			treeView.MouseDown += OnMouseDown;
			treeView.MouseMove += OnMouseMove;
			treeView.MouseUp += OnMouseUp;
			treeView.KeyDown += OnKeyDown;
			treeView.KeyUp += OnKeyUp;
		}

		#endregion Constructor

		#region Public methods

		public void Dispose()
		{
			if (treeView != null)
			{
				treeView.MouseDown -= OnMouseDown;
				treeView.MouseMove -= OnMouseMove;
				treeView.MouseUp -= OnMouseUp;
				treeView.KeyDown -= OnKeyDown;
				treeView.KeyUp -= OnKeyUp;
				treeView = null;
			}
			GC.SuppressFinalize(this);
		}

		#endregion Public methods

		#region Methods

		private void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			mouseDown = true;
			startPoint = Mouse.GetPosition(content);

			// Debug.WriteLine("Initialize drwawing");
			isFirstMove = true;
			// Capture the mouse right now so that the MouseUp event will not be missed
			Mouse.Capture(treeView);

			initialSelection = new HashSet<object>(treeView.SelectedItems.Cast<object>());
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			if (mouseDown)
			{
				if (DateTime.UtcNow > lastScrollTime.AddMilliseconds(100))
				{
					Point currentPointWin = Mouse.GetPosition(scrollViewer);
					if (currentPointWin.Y < 16)
					{
						scrollViewer.LineUp();
						scrollViewer.UpdateLayout();
						lastScrollTime = DateTime.UtcNow;
					}
					if (currentPointWin.Y > scrollViewer.ActualHeight - 16)
					{
						scrollViewer.LineDown();
						scrollViewer.UpdateLayout();
						lastScrollTime = DateTime.UtcNow;
					}
				}

				Point currentPoint = Mouse.GetPosition(content);
				double width = currentPoint.X - startPoint.X + 1;
				double height = currentPoint.Y - startPoint.Y + 1;
				double left = startPoint.X;
				double top = startPoint.Y;

				if (isFirstMove)
				{
					if (Math.Abs(width) <= SystemParameters.MinimumHorizontalDragDistance &&
						Math.Abs(height) <= SystemParameters.MinimumVerticalDragDistance)
					{
						return;
					}

					isFirstMove = false;
					if (!SelectionMultiple.IsControlKeyDown)
					{
						if (!treeView.ClearSelectionByRectangle())
						{
							EndAction();
							return;
						}
					}
				}

				// Debug.WriteLine(string.Format("Drawing: {0};{1};{2};{3}",startPoint.X,startPoint.Y,width,height));
				if (width < 1)
				{
					width = Math.Abs(width - 1) + 1;
					left = startPoint.X - width + 1;
				}

				if (height < 1)
				{
					height = Math.Abs(height - 1) + 1;
					top = startPoint.Y - height + 1;
				}

				border.Width = width;
				Canvas.SetLeft(border, left);
				border.Height = height;
				Canvas.SetTop(border, top);

				border.Visibility = Visibility.Visible;

				double right = left + width - 1;
				double bottom = top + height - 1;

				// Debug.WriteLine(string.Format("left:{1};right:{2};top:{3};bottom:{4}", null, left, right, top, bottom));
				SelectionMultiple selection = (SelectionMultiple) treeView.Selection;
				bool foundFocusItem = false;
				foreach (var item in items)
				{
					FrameworkElement itemContent = (FrameworkElement) item.Template.FindName("headerBorder", item);
					Point p = itemContent.TransformToAncestor(content).Transform(new Point());
					double itemLeft = p.X;
					double itemRight = p.X + itemContent.ActualWidth - 1;
					double itemTop = p.Y;
					double itemBottom = p.Y + itemContent.ActualHeight - 1;

					// Debug.WriteLine(string.Format("element:{0};itemleft:{1};itemright:{2};itemtop:{3};itembottom:{4}",item.DataContext,itemLeft,itemRight,itemTop,itemBottom));
					
					// Compute the current input states for determining the new selection state of the item
					bool intersect = !(itemLeft > right || itemRight < left || itemTop > bottom || itemBottom < top);
					bool initialSelected = initialSelection != null && initialSelection.Contains(item.DataContext);
					bool ctrl = SelectionMultiple.IsControlKeyDown;
					
					// Decision matrix:
					// If the Ctrl key is pressed, each intersected item will be toggled from its initial selection.
					// Without the Ctrl key, each intersected item is selected, others are deselected.
					//
					// newSelected
					// ─────────┬───────────────────────
					//          │ intersect
					//          │  0        │  1
					//          ├───────────┴───────────
					//          │ initial
					//          │  0  │  1  │  0  │  1
					// ─────────┼─────┼─────┼─────┼─────
					// ctrl  0  │  0  │  0  │  1  │  1   = intersect
					// ─────────┼─────┼─────┼─────┼─────
					//       1  │  0  │  1  │  1  │  0   = intersect XOR initial
					//
					bool newSelected = intersect ^ (initialSelected && ctrl);

					// The new selection state for this item has been determined. Apply it.
					if (newSelected)
					{
						// The item shall be selected
						if (!treeView.SelectedItems.Contains(item.DataContext))
						{
							// The item is not currently selected. Try to select it.
							if (!selection.SelectByRectangle(item))
							{
								if (selection.LastCancelAll)
								{
									EndAction();
									return;
								}
							}
						}
					}
					else
					{
						// The item shall be deselected
						if (treeView.SelectedItems.Contains(item.DataContext))
						{
							// The item is currently selected. Try to deselect it.
							if (!selection.DeselectByRectangle(item))
							{
								if (selection.LastCancelAll)
								{
									EndAction();
									return;
								}
							}
						}
					}

					// Always focus and bring into view the item under the mouse cursor
					if (!foundFocusItem &&
						currentPoint.X >= itemLeft && currentPoint.X <= itemRight &&
						currentPoint.Y >= itemTop && currentPoint.Y <= itemBottom)
					{
						FocusHelper.Focus(item, true);
						scrollViewer.UpdateLayout();
						foundFocusItem = true;
					}
				}

				if (e != null)
				{
					e.Handled = true;
				}
			}
		}

		private void OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			EndAction();
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			// The mouse move handler reads the Ctrl key so is dependent on it.
			// If the key state has changed, the selection needs to be updated.
			OnMouseMove(null, null);
		}

		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			// The mouse move handler reads the Ctrl key so is dependent on it.
			// If the key state has changed, the selection needs to be updated.
			OnMouseMove(null, null);
		}

		private void EndAction()
		{
			Mouse.Capture(null);
			mouseDown = false;
			border.Visibility = Visibility.Collapsed;
			initialSelection = null;

			// Debug.WriteLine("End drwawing");
		}

		#endregion Methods
	}
}