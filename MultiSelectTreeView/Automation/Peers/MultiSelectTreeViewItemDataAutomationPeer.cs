using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	public class MultiSelectTreeViewItemDataAutomationPeer :
		ItemAutomationPeer,
		ISelectionItemProvider,
		IScrollItemProvider,
		IExpandCollapseProvider,
		IValueProvider
	{
		#region Constructor

		public MultiSelectTreeViewItemDataAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer)
			: base(item, itemsControlAutomationPeer)
		{
		}

		#endregion Constructor

		#region Explicit interface properties

		ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				return ItemPeer.ExpandCollapseState;
			}
		}

		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				return ((ISelectionItemProvider) ItemPeer).IsSelected;
			}
		}

		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				// TreeViewItemAutomationPeer treeViewItemAutomationPeer = GetWrapperPeer() as TreeViewItemAutomationPeer;
				// if (treeViewItemAutomationPeer != null)
				// {
				// ISelectionItemProvider selectionItemProvider = treeViewItemAutomationPeer;
				// return selectionItemProvider.SelectionContainer;
				// }

				// this.ThrowElementNotAvailableException();
				return null;
			}
		}

		#endregion Explicit interface properties

		#region Properties

		private MultiSelectTreeViewItemAutomationPeer ItemPeer
		{
			get
			{
				AutomationPeer automationPeer = null;
				UIElement wrapper = GetWrapper();
				if (wrapper != null)
				{
					automationPeer = UIElementAutomationPeer.CreatePeerForElement(wrapper);
					if (automationPeer == null)
					{
						if (wrapper is FrameworkElement)
						{
							automationPeer = new FrameworkElementAutomationPeer((FrameworkElement) wrapper);
						}
						else
						{
							automationPeer = new UIElementAutomationPeer(wrapper);
						}
					}
				}

				var treeViewItemAutomationPeer = automationPeer as MultiSelectTreeViewItemAutomationPeer;

				if (treeViewItemAutomationPeer == null)
				{
					throw new InvalidOperationException("Could not find parent automation peer.");
				}

				return treeViewItemAutomationPeer;
			}
		}

		#endregion Properties

		#region Public methods

		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.ExpandCollapse)
			{
				return this;
			}

			if (patternInterface == PatternInterface.SelectionItem)
			{
				return this;
			}

			if (patternInterface == PatternInterface.ScrollItem)
			{
				return this;
			}

			if (patternInterface == PatternInterface.Value)
			{
				return this;
			}

			if (patternInterface == PatternInterface.ItemContainer
				|| patternInterface == PatternInterface.SynchronizedInput)
			{
				return ItemPeer;
			}

			return base.GetPattern(patternInterface);
		}

		#endregion Public methods

		#region Explicit interface methods

		void IExpandCollapseProvider.Collapse()
		{
			ItemPeer.Collapse();
		}

		void IExpandCollapseProvider.Expand()
		{
			ItemPeer.Expand();
		}

		void IScrollItemProvider.ScrollIntoView()
		{
			((IScrollItemProvider) ItemPeer).ScrollIntoView();
		}

		void ISelectionItemProvider.AddToSelection()
		{
			((ISelectionItemProvider) ItemPeer).AddToSelection();
		}

		void ISelectionItemProvider.RemoveFromSelection()
		{
			((ISelectionItemProvider) ItemPeer).RemoveFromSelection();
		}

		void ISelectionItemProvider.Select()
		{
			((ISelectionItemProvider) ItemPeer).Select();
		}

		#endregion Explicit interface methods

		#region Methods

		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.TreeItem;
		}

		protected override string GetClassNameCore()
		{
			return "TreeViewItem";
		}

		private UIElement GetWrapper()
		{
			UIElement result = null;
			ItemsControlAutomationPeer itemsControlAutomationPeer = ItemsControlAutomationPeer;
			if (itemsControlAutomationPeer != null)
			{
				ItemsControl itemsControl = (ItemsControl) itemsControlAutomationPeer.Owner;
				if (itemsControl != null)
				{
					result = itemsControl.ItemContainerGenerator.ContainerFromItem(Item) as UIElement;
				}
			}

			return result;
		}

		#endregion Methods

		#region IValueProvider members

		bool IValueProvider.IsReadOnly
		{
			get { return ((IValueProvider) ItemPeer).IsReadOnly; }
		}

		void IValueProvider.SetValue(string value)
		{
			((IValueProvider) ItemPeer).SetValue(value);
		}

		string IValueProvider.Value
		{
			get { return ((IValueProvider) ItemPeer).Value; }
		}

		#endregion IValueProvider members
	}
}