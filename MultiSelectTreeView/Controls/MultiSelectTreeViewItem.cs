using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Controls
{
	public class MultiSelectTreeViewItem : HeaderedItemsControl
	{
		#region Dependency properties

		#region Brushes

		public static DependencyProperty BackgroundFocusedProperty = DependencyProperty.Register(
			"BackgroundFocused",
			typeof(Brush),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(SystemColors.HighlightBrush, null));

		public static DependencyProperty BackgroundSelectedHoveredProperty = DependencyProperty.Register(
			"BackgroundSelectedHovered",
			typeof(Brush),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(Brushes.DarkGray, null));

		public static DependencyProperty BackgroundSelectedProperty = DependencyProperty.Register(
			"BackgroundSelected",
			typeof(Brush),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(SystemColors.HighlightBrush, null));

		public static DependencyProperty ForegroundSelectedProperty = DependencyProperty.Register(
			"ForegroundSelected",
			typeof(Brush),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(SystemColors.HighlightTextBrush, null));

		public static DependencyProperty BackgroundHoveredProperty = DependencyProperty.Register(
			"BackgroundHovered",
			typeof(Brush),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(Brushes.LightGray, null));

		public static DependencyProperty BackgroundInactiveProperty = DependencyProperty.Register(
			"BackgroundInactive",
			typeof(Brush),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(SystemColors.ControlBrush, null));

		public static DependencyProperty ForegroundInactiveProperty = DependencyProperty.Register(
			"ForegroundInactive",
			typeof(Brush),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, null));

		public static DependencyProperty BorderBrushHoveredProperty = DependencyProperty.Register(
			"BorderBrushHovered",
			typeof(Brush),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(Brushes.Transparent, null));

		public static DependencyProperty BorderBrushFocusedProperty = DependencyProperty.Register(
			"BorderBrushFocused",
			typeof(Brush),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(Brushes.Transparent, null));

		public static DependencyProperty BorderBrushInactiveProperty = DependencyProperty.Register(
			"BorderBrushInactive",
			typeof(Brush),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(Brushes.Black, null));

		public static DependencyProperty BorderBrushSelectedProperty = DependencyProperty.Register(
			"BorderBrushSelected",
			typeof(Brush),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(Brushes.Transparent, null));

		#endregion Brushes

		#region Others

		public static DependencyProperty IsExpandedProperty = DependencyProperty.Register(
			"IsExpanded",
			typeof(bool),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(false));

		public static DependencyProperty IsEditableProperty = DependencyProperty.Register(
			"IsEditable",
			typeof(bool),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(true));

		public static new DependencyProperty IsVisibleProperty = DependencyProperty.Register(
			"IsVisible",
			typeof(bool),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(true));

		public static DependencyProperty IsSelectedProperty = DependencyProperty.Register(
			"IsSelected",
			typeof(bool),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIsSelectedChanged)));

		public static DependencyProperty IsEditingProperty = DependencyProperty.Register(
			"IsEditing",
			typeof(bool),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(false));

		public static DependencyProperty ContentTemplateEditProperty = DependencyProperty.Register(
			"ContentTemplateEdit",
			typeof(DataTemplate),
			typeof(MultiSelectTreeViewItem));

		public static DependencyProperty DisplayNameProperty = DependencyProperty.Register(
			"DisplayName",
			typeof(string),
			typeof(MultiSelectTreeViewItem));

		public static DependencyProperty HoverHighlightingProperty = DependencyProperty.Register(
			"HoverHighlighting",
			typeof(bool),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(false));

		public static DependencyProperty ItemIndentProperty = DependencyProperty.Register(
			"ItemIndent",
			typeof(int),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(13));

		public static DependencyProperty IsKeyboardModeProperty = DependencyProperty.Register(
			"IsKeyboardMode",
			typeof(bool),
			typeof(MultiSelectTreeViewItem),
			new FrameworkPropertyMetadata(false));

		public static DependencyProperty RemarksProperty = DependencyProperty.Register(
			"Remarks",
			typeof(string),
			typeof(MultiSelectTreeViewItem));

		public static DependencyProperty RemarksTemplateProperty = DependencyProperty.Register(
			"RemarksTemplate",
			typeof(DataTemplate),
			typeof(MultiSelectTreeViewItem));

		#endregion Others

		#endregion Dependency properties

		#region Constructors and Destructors

		static MultiSelectTreeViewItem()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(MultiSelectTreeViewItem),
				new FrameworkPropertyMetadata(typeof(MultiSelectTreeViewItem)));
		}

		public MultiSelectTreeViewItem()
		{
		}

		#endregion

		#region Public Properties

		#region Brushes

		public Brush BackgroundFocused
		{
			get { return (Brush) GetValue(BackgroundFocusedProperty); }
			set { SetValue(BackgroundFocusedProperty, value); }
		}

		public Brush BackgroundSelected
		{
			get { return (Brush) GetValue(BackgroundSelectedProperty); }
			set { SetValue(BackgroundSelectedProperty, value); }
		}

		public Brush ForegroundSelected
		{
			get { return (Brush) GetValue(ForegroundSelectedProperty); }
			set { SetValue(ForegroundSelectedProperty, value); }
		}

		public Brush BackgroundSelectedHovered
		{
			get { return (Brush) GetValue(BackgroundSelectedHoveredProperty); }
			set { SetValue(BackgroundSelectedHoveredProperty, value); }
		}

		public Brush BackgroundHovered
		{
			get { return (Brush) GetValue(BackgroundHoveredProperty); }
			set { SetValue(BackgroundHoveredProperty, value); }
		}

		public Brush BackgroundInactive
		{
			get { return (Brush) GetValue(BackgroundInactiveProperty); }
			set { SetValue(BackgroundInactiveProperty, value); }
		}

		public Brush ForegroundInactive
		{
			get { return (Brush) GetValue(ForegroundInactiveProperty); }
			set { SetValue(ForegroundInactiveProperty, value); }
		}

		public Brush BorderBrushInactive
		{
			get { return (Brush) GetValue(BorderBrushInactiveProperty); }
			set { SetValue(BorderBrushInactiveProperty, value); }
		}

		public Brush BorderBrushHovered
		{
			get { return (Brush) GetValue(BorderBrushHoveredProperty); }
			set { SetValue(BorderBrushHoveredProperty, value); }
		}

		public Brush BorderBrushFocused
		{
			get { return (Brush) GetValue(BorderBrushFocusedProperty); }
			set { SetValue(BorderBrushFocusedProperty, value); }
		}

		public Brush BorderBrushSelected
		{
			get { return (Brush) GetValue(BorderBrushSelectedProperty); }
			set { SetValue(BorderBrushSelectedProperty, value); }
		}

		#endregion Brushes

		#region Others

		public DataTemplate ContentTemplateEdit
		{
			get { return (DataTemplate) GetValue(ContentTemplateEditProperty); }
			set { SetValue(ContentTemplateEditProperty, value); }
		}

		public bool IsExpanded
		{
			get { return (bool) GetValue(IsExpandedProperty); }
			set { SetValue(IsExpandedProperty, value); }
		}

		public bool IsEditable
		{
			get { return (bool) GetValue(IsEditableProperty); }
			set { SetValue(IsEditableProperty, value); }
		}

		public new bool IsVisible
		{
			get { return (bool) GetValue(IsVisibleProperty); }
			set { SetValue(IsVisibleProperty, value); }
		}

		public bool IsEditing
		{
			get { return (bool) GetValue(IsEditingProperty); }
			set { SetValue(IsEditingProperty, value); }
		}

		public bool IsSelected
		{
			get { return (bool) GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}

		public string DisplayName
		{
			get { return (string) GetValue(DisplayNameProperty); }
			set { SetValue(DisplayNameProperty, value); }
		}

		public bool HoverHighlighting
		{
			get { return (bool) GetValue(HoverHighlightingProperty); }
			set { SetValue(HoverHighlightingProperty, value); }
		}

		public int ItemIndent
		{
			get { return (int) GetValue(ItemIndentProperty); }
			set { SetValue(ItemIndentProperty, value); }
		}

		public bool IsKeyboardMode
		{
			get { return (bool) GetValue(IsKeyboardModeProperty); }
			set { SetValue(IsKeyboardModeProperty, value); }
		}

		public string Remarks
		{
			get { return (string) GetValue(RemarksProperty); }
			set { SetValue(RemarksProperty, value); }
		}

		public DataTemplate RemarksTemplate
		{
			get { return (DataTemplate) GetValue(RemarksTemplateProperty); }
			set { SetValue(RemarksTemplateProperty, value); }
		}

		#endregion Others

		#endregion

		#region Non-public properties

		private MultiSelectTreeView lastParentTreeView;

		internal MultiSelectTreeView ParentTreeView
		{
			get
			{
				for (ItemsControl itemsControl = ParentItemsControl;
					itemsControl != null;
					itemsControl = ItemsControlFromItemContainer(itemsControl))
				{
					MultiSelectTreeView treeView = itemsControl as MultiSelectTreeView;
					if (treeView != null)
					{
						return lastParentTreeView = treeView;
					}
				}
				return null;
			}
		}

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

		private bool CanExpand
		{
			get
			{
				return HasItems;
			}
		}

		private bool CanExpandOnInput
		{
			get
			{
				return CanExpand && IsEnabled;
			}
		}

		private ItemsControl ParentItemsControl
		{
			get
			{
				return ItemsControlFromItemContainer(this);
			}
		}

		#endregion Non-public properties

		#region Public methods

		public override string ToString()
		{
			if (DataContext != null)
			{
				return string.Format("{0} ({1})", DataContext, base.ToString());
			}

			return base.ToString();
		}

		#endregion Public methods

		#region Protected methods

		protected static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// The item has been selected through its IsSelected property. Update the SelectedItems
			// list accordingly (this is the authoritative collection). No PreviewSelectionChanged
			// event is fired - the item is already selected.
			MultiSelectTreeViewItem item = d as MultiSelectTreeViewItem;
			if (item != null)
			{
				if ((bool) e.NewValue)
				{
					if (!item.ParentTreeView.SelectedItems.Contains(item.DataContext))
					{
						item.ParentTreeView.SelectedItems.Add(item.DataContext);
					}
				}
				else
				{
					item.ParentTreeView.SelectedItems.Remove(item.DataContext);
				}
			}
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			if (ParentTreeView == null) return;

			//System.Diagnostics.Debug.WriteLine("P(" + ParentTreeView.Name + "): " + e.Property + " " + e.NewValue);
			if (e.Property.Name == "IsEditing")
			{
				if ((bool) e.NewValue == false)
				{
					StopEditing();
				}
			}

			// Bring newly expanded child nodes into view if they'd be outside of the current view
			if (e.Property.Name == "IsExpanded")
			{
				if ((bool) e.NewValue == true)
				{
					if (VisualChildrenCount > 0)
					{
						((FrameworkElement) GetVisualChild(VisualChildrenCount - 1)).BringIntoView();
					}
				}
			}

			base.OnPropertyChanged(e);
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
			return new MultiSelectTreeViewItemAutomationPeer(this);
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			if (ParentTreeView != null && ParentTreeView.SelectedItems.Contains(DataContext))
			{
				IsSelected = true;
			}
		}

		protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
		{
			base.OnMouseDoubleClick(e);

			FrameworkElement itemContent = (FrameworkElement) this.Template.FindName("headerBorder", this);
			if (!itemContent.IsMouseOver)
			{
				// A (probably disabled) child item was really clicked, do nothing here
				return;
			}

			if (IsKeyboardFocused) IsExpanded = !IsExpanded;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (!e.Handled)
			{
				Key key = e.Key;
				switch (key)
				{
					case Key.Left:
						if (IsExpanded)
						{
							IsExpanded = false;
						}
						else
						{
							ParentTreeView.Selection.SelectParentFromKey();
						}
						e.Handled = true;
						break;
					case Key.Right:
						if (CanExpand)
						{
							if (!IsExpanded)
							{
								IsExpanded = true;
							}
							else
							{
								ParentTreeView.Selection.SelectNextFromKey();
							}
						}
						e.Handled = true;
						break;
					case Key.Up:
						ParentTreeView.Selection.SelectPreviousFromKey();
						e.Handled = true;
						break;
					case Key.Down:
						ParentTreeView.Selection.SelectNextFromKey();
						e.Handled = true;
						break;
					case Key.Home:
						ParentTreeView.Selection.SelectFirstFromKey();
						e.Handled = true;
						break;
					case Key.End:
						ParentTreeView.Selection.SelectLastFromKey();
						e.Handled = true;
						break;
					case Key.PageUp:
						ParentTreeView.Selection.SelectPageUpFromKey();
						e.Handled = true;
						break;
					case Key.PageDown:
						ParentTreeView.Selection.SelectPageDownFromKey();
						e.Handled = true;
						break;
					case Key.A:
						if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
						{
							ParentTreeView.Selection.SelectAllFromKey();
							e.Handled = true;
						}
						break;
					case Key.Add:
						if (CanExpandOnInput && !IsExpanded)
						{
							IsExpanded = true;
						}
						e.Handled = true;
						break;
					case Key.Subtract:
						if (CanExpandOnInput && IsExpanded)
						{
							IsExpanded = false;
						}
						e.Handled = true;
						break;
					case Key.F2:
						if (ParentTreeView.AllowEditItems && ContentTemplateEdit != null && IsFocused && IsEditable)
						{
							IsEditing = true;
						}
						e.Handled = true;
						break;
					case Key.Escape:
						StopEditing();
						e.Handled = true;
						break;
					case Key.Return:
						FocusHelper.Focus(this, true);
						IsEditing = false;
						e.Handled = true;
						break;
					case Key.Space:
						ParentTreeView.Selection.SelectCurrentBySpace();
						e.Handled = true;
						break;
				}
			}
		}

		private void StopEditing()
		{
			FocusHelper.Focus(this, true);
			IsEditing = false;
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			// Do not call the base method because it would bring all of its children into view on
			// selecting which is not the desired behaviour.
			//base.OnGotFocus(e);
			ParentTreeView.LastFocusedItem = this;
			//System.Diagnostics.Debug.WriteLine("MultiSelectTreeViewItem.OnGotFocus(), DisplayName = " + DisplayName);
			//System.Diagnostics.Debug.WriteLine(Environment.StackTrace);
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			IsEditing = false;
			//System.Diagnostics.Debug.WriteLine("MultiSelectTreeViewItem.OnLostFocus(), DisplayName = " + DisplayName);
			//System.Diagnostics.Debug.WriteLine(Environment.StackTrace);
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			//System.Diagnostics.Debug.WriteLine("MultiSelectTreeViewItem.OnMouseDown(Item = " + this.DisplayName + ", Button = " + e.ChangedButton + ")");
			base.OnMouseDown(e);

			FrameworkElement itemContent = (FrameworkElement) this.Template.FindName("headerBorder", this);
			if (!itemContent.IsMouseOver)
			{
				// A (probably disabled) child item was really clicked, do nothing here
				return;
			}

			if (e.ChangedButton == MouseButton.Left)
			{
				ParentTreeView.Selection.Select(this);
				e.Handled = true;
			}
			if (e.ChangedButton == MouseButton.Right)
			{
				if (!IsSelected)
				{
					ParentTreeView.Selection.Select(this);
				}
				e.Handled = true;
			}
		}

		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			MultiSelectTreeView parentTV;

			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Remove:
					// Remove all items from the SelectedItems list that have been removed from the
					// Items list
					parentTV = ParentTreeView;
					if (parentTV == null)
						parentTV = lastParentTreeView;
					if (parentTV != null)
					{
						foreach (var item in e.OldItems)
						{
							parentTV.SelectedItems.Remove(item);
							// Don't preview and ask, it is already gone so it must be removed from
							// the SelectedItems list
						}
					}
					break;
				case NotifyCollectionChangedAction.Reset:
					// Remove all items from the SelectedItems list that are no longer in the Items
					// list
					parentTV = ParentTreeView;
					if (parentTV == null)
						parentTV = lastParentTreeView;
					if (parentTV != null)
					{
						var selection = new object[parentTV.SelectedItems.Count];
						parentTV.SelectedItems.CopyTo(selection, 0);
						HashSet<object> dataItems = new HashSet<object>(parentTV.GetAllDataItems().Cast<object>());
						foreach (var item in selection)
						{
							if (!dataItems.Contains(item))
							{
								parentTV.SelectedItems.Remove(item);
								// Don't preview and ask, it is already gone so it must be removed
								// from the SelectedItems list
							}
						}
					}
					break;
			}

			base.OnItemsChanged(e);
		}

		#endregion Protected methods

		#region Internal methods

		internal void InvokeMouseDown()
		{
			var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Right);
			e.RoutedEvent = Mouse.MouseDownEvent;
			this.OnMouseDown(e);
		}

		#endregion Internal methods
	}
}