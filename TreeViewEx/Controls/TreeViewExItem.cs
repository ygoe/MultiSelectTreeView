namespace System.Windows.Controls
{
	#region

	using System.Windows.Automation.Peers;
	using System.Windows.Input;
	using System.Windows.Media;

	#endregion

	public class TreeViewExItem : HeaderedItemsControl
	{
		#region Constants and Fields

		public static DependencyProperty BackgroundFocusedProperty = DependencyProperty.Register(
			"BackgroundFocused",
			typeof(Brush),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(SystemColors.HighlightBrush, null));

		public static DependencyProperty BackgroundSelectedPreviewProperty = DependencyProperty.Register(
			"BackgroundSelectedPreview",
			typeof(Brush),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(Brushes.DarkGray, null));

		public static DependencyProperty BackgroundSelectedHoveredProperty = DependencyProperty.Register(
			"BackgroundSelectedHovered",
			typeof(Brush),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(Brushes.DarkGray, null));

		public static DependencyProperty BackgroundSelectedProperty = DependencyProperty.Register(
			"BackgroundSelected",
			typeof(Brush),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(Brushes.LightGray, null));

		public static DependencyProperty BackgroundHoveredProperty = DependencyProperty.Register(
			"BackgroundHovered",
			typeof(Brush),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(Brushes.LightGray, null));

		public static DependencyProperty BackgroundInactiveProperty = DependencyProperty.Register(
			"BackgroundInactive",
			typeof(Brush),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(Brushes.LightGray, null));

		public static DependencyProperty BorderBrushHoveredProperty = DependencyProperty.Register(
			"BorderBrushHovered",
			typeof(Brush),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(Brushes.Transparent, null));

		public static DependencyProperty BorderBrushFocusedProperty = DependencyProperty.Register(
			"BorderBrushFocused",
			typeof(Brush),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(Brushes.Transparent, null));

		public static DependencyProperty BorderBrushInactiveProperty = DependencyProperty.Register(
			"BorderBrushInactive",
			typeof(Brush),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(Brushes.Black, null));

		public static DependencyProperty BorderBrushSelectedPreviewProperty = DependencyProperty.Register(
			"BorderBrushSelectedPreview",
			typeof(Brush),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(Brushes.Black, null));

		public static DependencyProperty BorderBrushSelectedProperty = DependencyProperty.Register(
			"BorderBrushSelected",
			typeof(Brush),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(Brushes.Transparent, null));

		public static DependencyProperty IsExpandedProperty = DependencyProperty.Register(
			"IsExpanded",
			typeof(bool),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(false, null));

		public static DependencyProperty IsEditableProperty = DependencyProperty.Register(
			"IsEditable",
			typeof(bool),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(true, null));

		public static new DependencyProperty IsVisibleProperty = DependencyProperty.Register(
			"IsVisible",
			typeof(bool),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(true, null));

		public static DependencyProperty IsSelectedProperty = DependencyProperty.Register(
			"IsSelected",
			typeof(bool),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(OnIsSelectedChanged)));

		public static DependencyProperty IsSelectedPreviewProperty = DependencyProperty.Register(
			"IsSelectedPreview",
			typeof(bool),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(false, null));

		public static DependencyProperty IsEditingProperty = DependencyProperty.Register(
			"IsEditing",
			typeof(bool),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(false, null));

		public static DependencyProperty ContentTemplateEditProperty = DependencyProperty.Register(
			"ContentTemplateEdit",
			typeof(DataTemplate),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(null, null));

		public static DependencyProperty DisplayNameProperty = DependencyProperty.Register(
			"DisplayName",
			typeof(string),
			typeof(TreeViewExItem),
			new FrameworkPropertyMetadata(null));

		#endregion

		#region Constructors and Destructors

		static TreeViewExItem()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(TreeViewExItem),
				new FrameworkPropertyMetadata(typeof(TreeViewExItem)));
		}

		public TreeViewExItem()
		{
		}

		#endregion

		#region Public Properties

		public DataTemplate ContentTemplateEdit
		{
			get
			{
				return (DataTemplate) GetValue(ContentTemplateEditProperty);
			}
			set
			{
				SetValue(ContentTemplateEditProperty, value);
			}
		}

		public Brush BackgroundFocused
		{
			get
			{
				return (Brush) GetValue(BackgroundFocusedProperty);
			}
			set
			{
				SetValue(BackgroundFocusedProperty, value);
			}
		}

		public Brush BackgroundSelected
		{
			get
			{
				return (Brush) GetValue(BackgroundSelectedProperty);
			}
			set
			{
				SetValue(BackgroundSelectedProperty, value);
			}
		}

		public Brush BackgroundSelectedPreview
		{
			get
			{
				return (Brush) GetValue(BackgroundSelectedPreviewProperty);
			}
			set
			{
				SetValue(BackgroundSelectedPreviewProperty, value);
			}
		}

		public Brush BackgroundSelectedHovered
		{
			get
			{
				return (Brush) GetValue(BackgroundSelectedHoveredProperty);
			}
			set
			{
				SetValue(BackgroundSelectedHoveredProperty, value);
			}
		}

		public Brush BackgroundHovered
		{
			get
			{
				return (Brush) GetValue(BackgroundHoveredProperty);
			}
			set
			{
				SetValue(BackgroundHoveredProperty, value);
			}
		}

		public Brush BackgroundInactive
		{
			get
			{
				return (Brush) GetValue(BackgroundInactiveProperty);
			}
			set
			{
				SetValue(BackgroundInactiveProperty, value);
			}
		}

		public Brush BorderBrushInactive
		{
			get
			{
				return (Brush) GetValue(BorderBrushInactiveProperty);
			}
			set
			{
				SetValue(BorderBrushInactiveProperty, value);
			}
		}

		public Brush BorderBrushHovered
		{
			get
			{
				return (Brush) GetValue(BorderBrushHoveredProperty);
			}
			set
			{
				SetValue(BorderBrushHoveredProperty, value);
			}
		}

		public Brush BorderBrushFocused
		{
			get
			{
				return (Brush) GetValue(BorderBrushFocusedProperty);
			}
			set
			{
				SetValue(BorderBrushFocusedProperty, value);
			}
		}

		public Brush BorderBrushSelected
		{
			get
			{
				return (Brush) GetValue(BorderBrushSelectedProperty);
			}
			set
			{
				SetValue(BorderBrushSelectedProperty, value);
			}
		}

		public Brush BorderBrushSelectedPreview
		{
			get
			{
				return (Brush) GetValue(BorderBrushSelectedPreviewProperty);
			}
			set
			{
				SetValue(BorderBrushSelectedPreviewProperty, value);
			}
		}

		public bool IsExpanded
		{
			get
			{
				return (bool) GetValue(IsExpandedProperty);
			}
			set
			{
				SetValue(IsExpandedProperty, value);
			}
		}

		public bool IsEditable
		{
			get
			{
				return (bool) GetValue(IsEditableProperty);
			}
			set
			{
				SetValue(IsEditableProperty, value);
			}
		}

		public new bool IsVisible
		{
			get
			{
				return (bool) GetValue(IsVisibleProperty);
			}
			set
			{
				SetValue(IsVisibleProperty, value);
			}
		}

		public bool IsEditing
		{
			get
			{
				return (bool) GetValue(IsEditingProperty);
			}
			set
			{
				SetValue(IsEditingProperty, value);
			}
		}

		public bool IsSelected
		{
			get
			{
				return (bool) GetValue(IsSelectedProperty);
			}
			set
			{
				//System.Diagnostics.Debug.WriteLine("IsSelected of " + DataContext + " is " + value + " from " + ParentItemsControl.GetHashCode());
				SetValue(IsSelectedProperty, value);
			}
		}

		public bool IsSelectedPreview
		{
			get
			{
				return (bool) GetValue(IsSelectedPreviewProperty);
			}
			set
			{
				//Debug.Log("IsSelectedPreview of " + DataContext + " is " + value + " from " + ParentItemsControl.GetHashCode());
				SetValue(IsSelectedPreviewProperty, value);
			}
		}

		public string DisplayName
		{
			get
			{
				return (string) GetValue(DisplayNameProperty);
			}
			set
			{
				SetValue(DisplayNameProperty, value);
			}
		}

		#endregion

		#region Properties

		internal TreeViewEx ParentTreeView
		{
			get
			{
				for (ItemsControl itemsControl = ParentItemsControl;
					itemsControl != null;
					itemsControl = ItemsControlFromItemContainer(itemsControl))
				{
					TreeViewEx treeView = itemsControl as TreeViewEx;
					if (treeView != null)
					{
						return treeView;
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

		#endregion

		#region Public Methods

		public override string ToString()
		{
			if (DataContext != null)
			{
				return string.Format("{0} ({1})", DataContext, base.ToString());
			}

			return base.ToString();
		}

		#endregion

		#region Protected Methods

		protected static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TreeViewExItem item = d as TreeViewExItem;
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

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (!e.Handled)
			{
				Key key = e.Key;
				switch (key)
				{
					case Key.Left:
					case Key.Right:
					case Key.Up:
					case Key.Down:
					case Key.Add:
					case Key.Subtract:
					case Key.Space:
						if (ParentTreeView.Selection.LastSelectedItem != null)
							ParentTreeView.Selection.LastSelectedItem.BringIntoView(new Rect(1, 1, 1, 1));
						break;
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

			base.OnPropertyChanged(e);
		}

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new TreeViewExItem();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is TreeViewExItem;
		}

		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TreeViewExItemAutomationPeer(this);
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			if (ParentTreeView.SelectedItems.Contains(DataContext))
			{
				IsSelected = true;
			}
		}

		protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
		{
			base.OnMouseDoubleClick(e);
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
						e.Handled = true;
						break;
					case Key.Right:
						if (CanExpand)
						{
							IsExpanded = true;
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
						if (ContentTemplateEdit != null && IsFocused && IsEditable)
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
						FocusHelper.Focus(this);
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
			FocusHelper.Focus(this);
			IsEditing = false;
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			ParentTreeView.LastFocusedItem = this;
			//System.Diagnostics.Debug.WriteLine("TreeViewExItem.OnGotFocus(), DisplayName = " + DisplayName);
		}

		protected override void OnLostFocus(RoutedEventArgs e)
		{
			base.OnLostFocus(e);
			IsEditing = false;
			//System.Diagnostics.Debug.WriteLine("TreeViewExItem.OnLostFocus(), DisplayName = " + DisplayName);
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			base.OnMouseDown(e);
			ParentTreeView.Selection.Select(this);
			e.Handled = true;
		}
		#endregion

		#region Internal Methods
		internal void InvokeMouseDown()
		{
			var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Right);
			e.RoutedEvent = Mouse.MouseDownEvent;
			this.OnMouseDown(e);
		}
		#endregion
	}
}