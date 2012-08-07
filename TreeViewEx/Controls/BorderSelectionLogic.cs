namespace System.Windows.Controls
{
	#region

	using System.Collections.Generic;
	using System.Windows.Input;

	#endregion

	internal class BorderSelectionLogic : IDisposable
	{
		#region Constants and Fields

		private readonly Border border;

		private readonly IEnumerable<TreeViewExItem> items;

		private TreeViewEx content;

		private bool isFirstMove;

		private bool mouseDown;

		private Point startPoint;

		#endregion

		#region Constructors and Destructors

		public BorderSelectionLogic(TreeViewEx content, Border selectionBorder, IEnumerable<TreeViewExItem> items)
		{
			if (content == null)
			{
				throw new ArgumentNullException("treeView");
			}

			if (selectionBorder == null)
			{
				throw new ArgumentNullException("selectionBorder");
			}

			if (selectionBorder == null)
			{
				throw new ArgumentNullException("items");
			}

			this.content = content;
			border = selectionBorder;
			this.items = items;

			content.MouseDown += OnMouseDown;
			content.MouseMove += OnMouseMove;
			content.MouseUp += OnMouseUp;
		}

		#endregion

		#region Public Methods and Operators

		public void Dispose()
		{
			if (content != null)
			{
				content.MouseDown -= OnMouseDown;
				content.MouseMove -= OnMouseMove;
				content.MouseUp -= OnMouseUp;
				content = null;
			}

			GC.SuppressFinalize(this);
		}

		#endregion

		#region Methods

		private void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			mouseDown = true;
			startPoint = Mouse.GetPosition(content);

			// Debug.WriteLine("Initialize drwawing");
			isFirstMove = true;
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			if (mouseDown)
			{
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
					if (!content.ClearSelectionByRectangle())
					{
						EndAction();
						return;
					}
					Mouse.Capture(content);
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
				foreach (var item in items)
				{
					FrameworkElement itemContent = (FrameworkElement) item.Template.FindName("headerBorder", item);
					Point p = itemContent.TransformToAncestor(content).Transform(new Point());
					double itemLeft = p.X;
					double itemRight = p.X + itemContent.ActualWidth - 1;
					double itemTop = p.Y;
					double itemBottom = p.Y + itemContent.ActualHeight - 1;

					// Debug.WriteLine(string.Format("element:{0};itemleft:{1};itemright:{2};itemtop:{3};itembottom:{4}",item.DataContext,itemLeft,itemRight,itemTop,itemBottom));
					if (!(itemLeft > right || itemRight < left || itemTop > bottom || itemBottom < top))
					{
						// The item and the selection border intersect
						if (!content.SelectedItems.Contains(item.DataContext))
						{
							// The item is not currently selected. Try to select it.
							if (!((SelectionMultiple) content.Selection).SelectByRectangle(item))
							{
								//EndAction();
								return;
							}
						}
						// Debug.WriteLine("Is selected: " + item);
					}
					else
					{
						// The item and the selection border do not intersect
						if (content.SelectedItems.Contains(item.DataContext))
						{
							// The item is currently selected. Try to deselect it.
							if (!content.Selection.UnSelect(item))
							{
								//EndAction();
								return;
							}
						}
					}
				}

				e.Handled = true;
			}
		}

		private void OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			EndAction();
		}

		private void EndAction()
		{
			Mouse.Capture(null);
			mouseDown = false;
			border.Visibility = Visibility.Collapsed;

			// Debug.WriteLine("End drwawing");
		}

		#endregion
	}
}