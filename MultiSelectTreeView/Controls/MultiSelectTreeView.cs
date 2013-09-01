using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Controls
{
	public class MultiSelectTreeView : ItemsControl
	{
		#region Constants and Fields

		public event EventHandler<PreviewSelectionChangedEventArgs> PreviewSelectionChanged;
		
		// TODO: Provide more details. Fire once for every single change and once for all groups of changes, with different flags
		public event EventHandler SelectionChanged;

		public static readonly DependencyProperty LastSelectedItemProperty;

		public static DependencyProperty BackgroundSelectionRectangleProperty = DependencyProperty.Register(
			"BackgroundSelectionRectangle",
			typeof(Brush),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(0x60, 0x33, 0x99, 0xFF)), null));

		public static DependencyProperty BorderBrushSelectionRectangleProperty = DependencyProperty.Register(
			"BorderBrushSelectionRectangle",
			typeof(Brush),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(0x33, 0x99, 0xFF)), null));

		public static DependencyProperty HoverHighlightingProperty = DependencyProperty.Register(
			"HoverHighlighting",
			typeof(bool),
			typeof(MultiSelectTreeView),
			new FrameworkPropertyMetadata(true, null));

		public static DependencyProperty VerticalRulersProperty = DependencyProperty.Register(
			"VerticalRulers",
			typeof(bool),
			typeof(MultiSelectTreeView),
			new FrameworkPropertyMetadata(false, null));

		public static DependencyProperty ItemIndentProperty = DependencyProperty.Register(
			"ItemIndent",
			typeof(int),
			typeof(MultiSelectTreeView),
			new FrameworkPropertyMetadata(13, null));

		public static DependencyProperty IsKeyboardModeProperty = DependencyProperty.Register(
			"IsKeyboardMode",
			typeof(bool),
			typeof(MultiSelectTreeView),
			new FrameworkPropertyMetadata(false, null));

		public static DependencyPropertyKey LastSelectedItemPropertyKey = DependencyProperty.RegisterReadOnly(
			"LastSelectedItem",
			typeof(object),
			typeof(MultiSelectTreeView),
			new FrameworkPropertyMetadata(null));

		public static DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
			"SelectedItems",
			typeof(IList),
			typeof(MultiSelectTreeView),
			new FrameworkPropertyMetadata(
				null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemsPropertyChanged));

		public static DependencyProperty AllowEditItemsProperty = DependencyProperty.Register(
			"AllowEditItems",
			typeof(bool),
			typeof(MultiSelectTreeView),
			new FrameworkPropertyMetadata(false, null));

		#endregion

		#region Constructors and Destructors

		static MultiSelectTreeView()
		{
			LastSelectedItemProperty = LastSelectedItemPropertyKey.DependencyProperty;
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectTreeView), new FrameworkPropertyMetadata(typeof(MultiSelectTreeView)));
		}

		public MultiSelectTreeView()
		{
			SelectedItems = new ObservableCollection<object>();
			Selection = new SelectionMultiple(this);
			Selection.PreviewSelectionChanged += (s, e) => { OnPreviewSelectionChanged(e); };
		}

		#endregion

		#region Public Properties

		public Brush BackgroundSelectionRectangle
		{
			get
			{
				return (Brush) GetValue(BackgroundSelectionRectangleProperty);
			}
			set
			{
				SetValue(BackgroundSelectionRectangleProperty, value);
			}
		}

		public Brush BorderBrushSelectionRectangle
		{
			get
			{
				return (Brush) GetValue(BorderBrushSelectionRectangleProperty);
			}
			set
			{
				SetValue(BorderBrushSelectionRectangleProperty, value);
			}
		}

		public bool HoverHighlighting
		{
			get
			{
				return (bool) GetValue(HoverHighlightingProperty);
			}
			set
			{
				SetValue(HoverHighlightingProperty, value);
			}
		}

		public bool VerticalRulers
		{
			get
			{
				return (bool) GetValue(VerticalRulersProperty);
			}
			set
			{
				SetValue(VerticalRulersProperty, value);
			}
		}

		public int ItemIndent
		{
			get
			{
				return (int) GetValue(ItemIndentProperty);
			}
			set
			{
				SetValue(ItemIndentProperty, value);
			}
		}

		[Browsable(false)]
		public bool IsKeyboardMode
		{
			get
			{
				return (bool) GetValue(IsKeyboardModeProperty);
			}
			set
			{
				SetValue(IsKeyboardModeProperty, value);
			}
		}

		public bool AllowEditItems
		{
			get
			{
				return (bool) GetValue(AllowEditItemsProperty);
			}
			set
			{
				SetValue(AllowEditItemsProperty, value);
			}
		}

		/// <summary>
		///    Gets the last selected item.
		/// </summary>
		public object LastSelectedItem
		{
			get
			{
				return GetValue(LastSelectedItemProperty);
			}
			private set
			{
				SetValue(LastSelectedItemPropertyKey, value);
			}
		}

		private MultiSelectTreeViewItem lastFocusedItem;
		/// <summary>
		/// Gets the last focused item.
		/// </summary>
		internal MultiSelectTreeViewItem LastFocusedItem
		{
			get
			{
				return lastFocusedItem;
			}
			set
			{
				// Only the last focused MultiSelectTreeViewItem may have IsTabStop = true
				// so that the keyboard focus only stops a single time for the MultiSelectTreeView control.
				if (lastFocusedItem != null)
				{
					lastFocusedItem.IsTabStop = false;
				}
				lastFocusedItem = value;
				if (lastFocusedItem != null)
				{
					lastFocusedItem.IsTabStop = true;
				}
				// The MultiSelectTreeView control only has the tab stop if none of its items has it.
				IsTabStop = lastFocusedItem == null;
			}
		}

		/// <summary>
		/// Gets or sets a list of selected items and can be bound to another list. If the source list
		/// implements <see cref="INotifyPropertyChanged"/> the changes are automatically taken over.
		/// </summary>
		public IList SelectedItems
		{
			get
			{
				return (IList) GetValue(SelectedItemsProperty);
			}
			set
			{
				SetValue(SelectedItemsProperty, value);
			}
		}

		internal ISelectionStrategy Selection { get; private set; }

		#endregion

		#region Public Methods and Operators

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			Selection.ApplyTemplate();

			Unloaded += OnUnLoaded;
		}

		public bool ClearSelection()
		{
			if (SelectedItems.Count > 0)
			{
				foreach (var selItem in SelectedItems)
				{
					var e = new PreviewSelectionChangedEventArgs(false, selItem);
					OnPreviewSelectionChanged(e);
					if (e.CancelAny)
					{
						return false;
					}
				}

				SelectedItems.Clear();
			}
			return true;
		}

		public void FocusItem(object item, bool bringIntoView = false)
		{
			MultiSelectTreeViewItem node = GetTreeViewItemsFor(new List<object> { item }).FirstOrDefault();
			if (node != null)
			{
				FocusHelper.Focus(node, bringIntoView);
			}
		}

		public void BringItemIntoView(object item)
		{
			MultiSelectTreeViewItem node = GetTreeViewItemsFor(new List<object> { item }).First();
			FrameworkElement itemContent = (FrameworkElement) node.Template.FindName("headerBorder", node);
			itemContent.BringIntoView();
		}

		public bool SelectNextItem()
		{
			return Selection.SelectNextFromKey();
		}

		public bool SelectPreviousItem()
		{
			return Selection.SelectPreviousFromKey();
		}

		public bool SelectFirstItem()
		{
			return Selection.SelectFirstFromKey();
		}

		public bool SelectLastItem()
		{
			return Selection.SelectLastFromKey();
		}

		public bool SelectAllItems()
		{
			return Selection.SelectAllFromKey();
		}

		public bool SelectParentItem()
		{
			return Selection.SelectParentFromKey();
		}

		#endregion

		#region Methods

		internal bool ClearSelectionByRectangle()
		{
			foreach (var item in new ArrayList(SelectedItems))
			{
				var e = new PreviewSelectionChangedEventArgs(false, item);
				OnPreviewSelectionChanged(e);
				if (e.CancelAny) return false;
			}
			
			SelectedItems.Clear();
			return true;
		}

		internal MultiSelectTreeViewItem GetNextItem(MultiSelectTreeViewItem item, List<MultiSelectTreeViewItem> items)
		{
			int indexOfCurrent = item != null ? items.IndexOf(item) : -1;
			for (int i = indexOfCurrent + 1; i < items.Count; i++)
			{
				if (items[i].IsVisible)
				{
					return items[i];
				}
			}
			return null;
		}

		internal MultiSelectTreeViewItem GetPreviousItem(MultiSelectTreeViewItem item, List<MultiSelectTreeViewItem> items)
		{
			int indexOfCurrent = item != null ? items.IndexOf(item) : -1;
			for (int i = indexOfCurrent - 1; i >= 0; i--)
			{
				if (items[i].IsVisible)
				{
					return items[i];
				}
			}
			return null;
		}

		internal MultiSelectTreeViewItem GetFirstItem(List<MultiSelectTreeViewItem> items)
		{
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].IsVisible)
				{
					return items[i];
				}
			}
			return null;
		}

		internal MultiSelectTreeViewItem GetLastItem(List<MultiSelectTreeViewItem> items)
		{
			for (int i = items.Count - 1; i >= 0; i--)
			{
				if (items[i].IsVisible)
				{
					return items[i];
				}
			}
			return null;
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new MultiSelectTreeViewItem();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is MultiSelectTreeViewItem;
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new MultiSelectTreeViewAutomationPeer(this);
		}

		private static void OnSelectedItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			MultiSelectTreeView treeView = (MultiSelectTreeView) d;
			if (e.OldValue != null)
			{
				INotifyCollectionChanged collection = e.OldValue as INotifyCollectionChanged;
				if (collection != null)
				{
					collection.CollectionChanged -= treeView.OnSelectedItemsChanged;
				}
			}

			if (e.NewValue != null)
			{
				INotifyCollectionChanged collection = e.NewValue as INotifyCollectionChanged;
				if (collection != null)
				{
					collection.CollectionChanged += treeView.OnSelectedItemsChanged;
				}
			}
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Remove:
				case NotifyCollectionChangedAction.Replace:
					if (e.OldItems != null)
					{
						foreach (var item in e.OldItems)
						{
							SelectedItems.Remove(item);
							// Don't preview and ask, it is already gone so it must be removed from
							// the SelectedItems list
						}
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					// If the items list has considerably changed, the selection is probably
					// useless anyway, clear it entirely.
					SelectedItems.Clear();
					break;
			}
			
			base.OnItemsChanged(e);
		}

		internal static IEnumerable<MultiSelectTreeViewItem> RecursiveTreeViewItemEnumerable(ItemsControl parent, bool includeInvisible)
		{
			return RecursiveTreeViewItemEnumerable(parent, includeInvisible, true);
		}

		internal static IEnumerable<MultiSelectTreeViewItem> RecursiveTreeViewItemEnumerable(ItemsControl parent, bool includeInvisible, bool includeDisabled)
		{
			foreach (var item in parent.Items)
			{
				MultiSelectTreeViewItem tve = (MultiSelectTreeViewItem) parent.ItemContainerGenerator.ContainerFromItem(item);
				if (tve == null)
				{
					// Container was not generated, therefore it is probably not visible, so we can ignore it.
					continue;
				}
				if (!includeInvisible && !tve.IsVisible)
				{
					continue;
				}
				if (!includeDisabled && !tve.IsEnabled)
				{
					continue;
				}

				yield return tve;
				if (includeInvisible || tve.IsExpanded)
				{
					foreach (var childItem in RecursiveTreeViewItemEnumerable(tve, includeInvisible, includeDisabled))
					{
						yield return childItem;
					}
				}
			}
		}

		internal IEnumerable<MultiSelectTreeViewItem> GetNodesToSelectBetween(MultiSelectTreeViewItem firstNode, MultiSelectTreeViewItem lastNode)
		{
			var allNodes = RecursiveTreeViewItemEnumerable(this, false, false).ToList();
			var firstIndex = allNodes.IndexOf(firstNode);
			var lastIndex = allNodes.IndexOf(lastNode);

			if (firstIndex >= allNodes.Count)
			{
				throw new InvalidOperationException(
				   "First node index " + firstIndex + "greater or equal than count " + allNodes.Count + ".");
			}

			if (lastIndex >= allNodes.Count)
			{
				throw new InvalidOperationException(
				   "Last node index " + lastIndex + " greater or equal than count " + allNodes.Count + ".");
			}

			var nodesToSelect = new List<MultiSelectTreeViewItem>();

			if (lastIndex == firstIndex)
			{
				return new List<MultiSelectTreeViewItem> { firstNode };
			}

			if (lastIndex > firstIndex)
			{
				for (int i = firstIndex; i <= lastIndex; i++)
				{
					if (allNodes[i].IsVisible)
					{
						nodesToSelect.Add(allNodes[i]);
					}
				}
			}
			else
			{
				for (int i = firstIndex; i >= lastIndex; i--)
				{
					if (allNodes[i].IsVisible)
					{
						nodesToSelect.Add(allNodes[i]);
					}
				}
			}

			return nodesToSelect;
		}

		/// <summary>
		/// Finds the treeview item for each of the specified data items.
		/// </summary>
		/// <param name="dataItems">List of data items to search for.</param>
		/// <returns></returns>
		internal IEnumerable<MultiSelectTreeViewItem> GetTreeViewItemsFor(IEnumerable dataItems)
		{
			if (dataItems == null)
			{
				yield break;
			}

			foreach (var dataItem in dataItems)
			{
				foreach (var treeViewItem in RecursiveTreeViewItemEnumerable(this, true))
				{
					if (treeViewItem.DataContext == dataItem)
					{
						yield return treeViewItem;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets all data items referenced in all treeview items of the entire control.
		/// </summary>
		/// <returns></returns>
		internal IEnumerable GetAllDataItems()
		{
			foreach (var treeViewItem in RecursiveTreeViewItemEnumerable(this, true))
			{
				yield return treeViewItem.DataContext;
			}
		}

		// this eventhandler reacts on the firing control to, in order to update the own status
		private void OnSelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
#if DEBUG
					// Make sure we don't confuse MultiSelectTreeViewItems and their DataContexts while development
					if (e.NewItems.OfType<MultiSelectTreeViewItem>().Any())
						throw new ArgumentException("A MultiSelectTreeViewItem instance was added to the SelectedItems collection. Only their DataContext instances must be added to this list!");
#endif
					object last = null;
					foreach (var item in GetTreeViewItemsFor(e.NewItems))
					{
						if (!item.IsSelected)
						{
							item.IsSelected = true;
						}

						last = item.DataContext;
					}

					LastSelectedItem = last;
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (var item in GetTreeViewItemsFor(e.OldItems))
					{
						item.IsSelected = false;
						if (item.DataContext == LastSelectedItem)
						{
							if (SelectedItems.Count > 0)
							{
								LastSelectedItem = SelectedItems[SelectedItems.Count - 1];
							}
							else
							{
								LastSelectedItem = null;
							}
						}
					}

					break;
				case NotifyCollectionChangedAction.Reset:
					foreach (var item in RecursiveTreeViewItemEnumerable(this, true))
					{
						if (item.IsSelected)
						{
							item.IsSelected = false;
						}
					}

					LastSelectedItem = null;
					break;
				default:
					throw new InvalidOperationException();
			}

			OnSelectionChanged();
		}

		private void OnUnLoaded(object sender, RoutedEventArgs e)
		{
			Unloaded -= OnUnLoaded;
			if (Selection != null)
				Selection.Dispose();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (!e.Handled)
			{
				// Basically, this should not be needed anymore. It allows selecting an item with
				// the keyboard when the MultiSelectTreeView control has the focus. If there were already
				// items when the control was focused, an item has already been focused (and
				// subsequent key presses won't land here but at the item).
				Key key = e.Key;
				switch (key)
				{
					case Key.Up:
						// Select last item
						var lastNode = RecursiveTreeViewItemEnumerable(this, false).LastOrDefault();
						if (lastNode != null)
						{
							Selection.Select(lastNode);
							e.Handled = true;
						}
						break;
					case Key.Down:
						// Select first item
						var firstNode = RecursiveTreeViewItemEnumerable(this, false).FirstOrDefault();
						if (firstNode != null)
						{
							Selection.Select(firstNode);
							e.Handled = true;
						}
						break;
				}
			}
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);
			if (!IsKeyboardMode)
			{
				IsKeyboardMode = true;
				//System.Diagnostics.Debug.WriteLine("Changing to keyboard mode from PreviewKeyDown");
			}
		}

		protected override void OnPreviewKeyUp(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);
			if (!IsKeyboardMode)
			{
				IsKeyboardMode = true;
				//System.Diagnostics.Debug.WriteLine("Changing to keyboard mode from PreviewKeyUp");
			}
		}

		protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseDown(e);
			if (IsKeyboardMode)
			{
				IsKeyboardMode = false;
				//System.Diagnostics.Debug.WriteLine("Changing to mouse mode");
			}
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine("MultiSelectTreeView.OnGotFocus()");
			//System.Diagnostics.Debug.WriteLine(Environment.StackTrace);

			base.OnGotFocus(e);

			// If the MultiSelectTreeView control has gotten the focus, it needs to pass it to an
			// item instead. If there was an item focused before, return to that. Otherwise just
			// focus this first item in the list if any. If there are no items at all, the
			// MultiSelectTreeView control just keeps the focus.
			// In any case, the focussing must occur when the current event processing is finished,
			// i.e. be queued in the dispatcher. Otherwise the TreeView could keep its focus
			// because other focus things are still going on and interfering this final request.

			var lastFocusedItem = LastFocusedItem;
			if (lastFocusedItem != null)
			{
				Dispatcher.BeginInvoke((Action) (() => FocusHelper.Focus(lastFocusedItem)));
			}
			else
			{
				var firstNode = RecursiveTreeViewItemEnumerable(this, false).FirstOrDefault();
				if (firstNode != null)
				{
					Dispatcher.BeginInvoke((Action) (() => FocusHelper.Focus(firstNode)));
				}
			}
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);

			// This happens when a mouse button was pressed in an area which is not covered by an
			// item. Then, it should be focused which in turn passes on the focus to an item.
			Focus();
		}

		protected void OnPreviewSelectionChanged(PreviewSelectionChangedEventArgs e)
		{
			var handler = PreviewSelectionChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		protected void OnSelectionChanged()
		{
			var handler = SelectionChanged;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		#endregion
	}
}