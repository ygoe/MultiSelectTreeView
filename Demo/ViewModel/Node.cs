using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Demo.ViewModel
{
	/// <summary>
	/// View model for testing
	/// </summary>
	public class Node : INotifyPropertyChanged
	{
		#region Data

		private static readonly Node DummyChild = new Node();

		private readonly ObservableCollection<Node> children;
		private readonly Node parent;

		private bool isExpanded;
		private bool isSelected;
		private bool isEditable;
		private bool isEditing;
		private bool isEnabled = true;
		private bool isVisible = true;

		#endregion Data

		#region Constructor

		public Node(Node parent, bool lazyLoadChildren)
		{
			this.parent = parent;

			children = new ObservableCollection<Node>();

			if (lazyLoadChildren)
				children.Add(DummyChild);
		}

		// This is used to create the DummyChild instance.
		private Node()
		{
		}

		#endregion Constructor

		#region Public properties

		/// <summary>
		/// Returns the logical child items of this object.
		/// </summary>
		public ObservableCollection<Node> Children
		{
			get { return children; }
		}

		/// <summary>
		/// Returns true if this object's Children have not yet been populated.
		/// </summary>
		public bool HasDummyChild
		{
			get { return Children.Count == 1 && Children[0] == DummyChild; }
		}

		/// <summary>
		/// Gets/sets whether the TreeViewItem 
		/// associated with this object is expanded.
		/// </summary>
		public bool IsExpanded
		{
			get { return isExpanded; }
			set
			{
				if (value != isExpanded)
				{
					isExpanded = value;
					OnPropertyChanged("IsExpanded");

					// Expand all the way up to the root.
					if (isExpanded && parent != null)
						parent.IsExpanded = true;

					// Lazy load the child items, if necessary.
					if (isExpanded && HasDummyChild)
					{
						Children.Remove(DummyChild);
						LoadChildren();
					}
				}
			}
		}

		/// <summary>
		/// Invoked when the child items need to be loaded on demand.
		/// Subclasses can override this to populate the Children collection.
		/// </summary>
		protected virtual void LoadChildren()
		{
			for (int i = 0; i < 100; i++)
			{
				Children.Add(new Node(this, true) { DisplayName = "subnode " + i });
			}
		}

		/// <summary>
		/// Gets/sets whether the TreeViewItem 
		/// associated with this object is selected.
		/// </summary>
		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				if (value != isSelected)
				{
					isSelected = value;
					OnPropertyChanged("IsSelected");
				}
			}
		}

		public bool IsEditable
		{
			get { return isEditable; }
			set
			{
				if (value != isEditable)
				{
					isEditable = value;
					OnPropertyChanged("IsEditable");
				}
			}
		}

		public bool IsEditing
		{
			get { return isEditing; }
			set
			{
				if (value != isEditing)
				{
					isEditing = value;
					OnPropertyChanged("IsEditing");
				}
			}
		}

		public bool IsEnabled
		{
			get { return isEnabled; }
			set
			{
				if (value != isEnabled)
				{
					isEnabled = value;
					OnPropertyChanged("IsEnabled");
				}
			}
		}

		public bool IsVisible
		{
			get { return isVisible; }
			set
			{
				if (value != isVisible)
				{
					isVisible = value;
					OnPropertyChanged("IsVisible");
				}
			}
		}

		public Node Parent
		{
			get { return parent; }
		}

		public override string ToString()
		{
			return "{Node " + DisplayName + "}";
		}

		#endregion Public properties

		#region ViewModelBase

		/// <summary>
		/// Returns the user-friendly name of this object.
		/// Child classes can set this property to a new value,
		/// or override it to determine the value on-demand.
		/// </summary>
		private string displayName;
		public virtual string DisplayName
		{
			get { return displayName; }
			set
			{
				if (value != displayName)
				{
					displayName = value;
					OnPropertyChanged("DisplayName");
				}
			}
		}

		#endregion ViewModelBase

		#region INotifyPropertyChanged members

		/// <summary>
		/// Raised when a property on this object has a new value.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises this object's PropertyChanged event.
		/// </summary>
		/// <param name="propertyName">The property that has a new value.</param>
		protected void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion INotifyPropertyChanged members
	}
}