namespace System.Windows.Automation.Peers
{
   #region

   using System.Collections.Generic;
   using System.Reflection;
   using System.Windows.Automation.Provider;
   using System.Windows.Controls;
   using System.Windows.Controls.Primitives;
   using System.Windows.Media;

   #endregion

   /// <summary>
   /// Macht <see cref="T:System.Windows.Controls.TreeViewExItem"/>-Typen für UI-Automatisierung verfügbar.
   /// </summary>
   public class TreeViewExItemAutomationPeer : ItemsControlAutomationPeer,
                                               IExpandCollapseProvider,
                                               ISelectionItemProvider,
                                               IScrollItemProvider,
                                               IValueProvider,
                                               IInvokeProvider
   {
      #region Constructors and Destructors

      /// <summary>
      ///    Initializes a new instance of the <see cref = "TreeViewExItemAutomationPeer" /> class. 
      ///    Initialisiert eine neue Instanz der <see cref = "T:System.Windows.Automation.Peers.TreeViewExItemAutomationPeer" />-Klasse.
      /// </summary>
      /// <param name = "owner">
      ///    Das <see cref = "T:System.Windows.Controls.TreeViewExItem" />, das diesem <see cref = "T:System.Windows.Automation.Peers.TreeViewExItemAutomationPeer" /> zugeordnet ist.
      /// </param>
      public TreeViewExItemAutomationPeer(TreeViewExItem owner)
         : base(owner)
      {
      }

      #endregion

      protected override Rect GetBoundingRectangleCore()
      {
         var treeViewExItem = (TreeViewExItem)Owner;
         var contentPresenter = GetContentPresenter(treeViewExItem);
         if (contentPresenter != null)
         {
            Vector offset = VisualTreeHelper.GetOffset(contentPresenter);
            Point p = new Point(offset.X, offset.Y);
            p = contentPresenter.PointToScreen(p);
            return new Rect(p.X, p.Y, contentPresenter.ActualWidth, contentPresenter.ActualHeight);
         }

         return base.GetBoundingRectangleCore();
      }

      protected override Point GetClickablePointCore()
      {
         var treeViewExItem = (TreeViewExItem)Owner;
         var contentPresenter = GetContentPresenter(treeViewExItem);
         if (contentPresenter != null)
         {
            Vector offset = VisualTreeHelper.GetOffset(contentPresenter);
            Point p = new Point(offset.X, offset.Y);
            p = contentPresenter.PointToScreen(p);
            return p;
         }

         return base.GetClickablePointCore();
      }

      private static ContentPresenter GetContentPresenter(TreeViewExItem treeViewExItem)
      {
         var contentPresenter = treeViewExItem.Template.FindName("PART_Header", treeViewExItem) as ContentPresenter;
         return contentPresenter;
      }

      /// <summary>
      /// Overridden because original wpf tree does show the expander button and the contents of the
      /// header as children, too. That was requested by the users.
      /// </summary>
      /// <returns>Returns a list of children.</returns>
      protected override List<AutomationPeer> GetChildrenCore()
      {
         TreeViewExItem owner = (TreeViewExItem)Owner;

         List<AutomationPeer> children = new List<AutomationPeer>();
         var button = owner.Template.FindName("Expander", owner) as ToggleButton;
         AddAutomationPeer(children, button);

         var contentPresenter = GetContentPresenter(owner);

         if (contentPresenter != null)
         {
            int childrenCount = VisualTreeHelper.GetChildrenCount(contentPresenter);
            for (int i = 0; i < childrenCount; i++)
            {
               var child = VisualTreeHelper.GetChild(contentPresenter, i) as UIElement;
               AddAutomationPeer(children, child);
            }
         }

         ItemCollection items = owner.Items;
         for (int i = 0; i < items.Count; i++)
         {
            TreeViewExItem treeViewItem = owner.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewExItem;
            AddAutomationPeer(children, treeViewItem);
         }

         if (children.Count > 0)
         {
            return children;
         }

         return null;
      }

      private static void AddAutomationPeer(List<AutomationPeer> children, UIElement child)
      {
         if (child != null)
         {
            AutomationPeer peer = FromElement(child);
            if (peer == null)
            {
               peer = CreatePeerForElement(child);
            }

            children.Add(peer);
         }
      }

      #region Public Properties

      public ExpandCollapseState ExpandCollapseState
      {
         get
         {
            TreeViewExItem treeViewItem = (TreeViewExItem)Owner;
            if (!treeViewItem.HasItems)
            {
               return ExpandCollapseState.LeafNode;
            }

            if (!treeViewItem.IsExpanded)
            {
               return ExpandCollapseState.Collapsed;
            }

            return ExpandCollapseState.Expanded;
         }
      }

      #endregion

      #region Explicit Interface Properties

      bool ISelectionItemProvider.IsSelected
      {
         get
         {
            return ((TreeViewExItem)Owner).IsSelected;
         }
      }

      IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
      {
         get
         {
            ItemsControl parentItemsControl = ((TreeViewExItem)Owner).ParentTreeView;
            if (parentItemsControl != null)
            {
               AutomationPeer automationPeer = FromElement(parentItemsControl);
               if (automationPeer != null)
               {
                  return ProviderFromPeer(automationPeer);
               }
            }

            return null;
         }
      }

      #endregion

      #region Public Methods

      public void Collapse()
      {
         if (!IsEnabled())
         {
            throw new ElementNotEnabledException();
         }

         TreeViewExItem treeViewItem = (TreeViewExItem)Owner;
         if (!treeViewItem.HasItems)
         {
            throw new InvalidOperationException("Cannot collapse because item has no children.");
         }

         treeViewItem.IsExpanded = false;
      }

      public void Expand()
      {
         if (!IsEnabled())
         {
            throw new ElementNotEnabledException();
         }

         TreeViewExItem treeViewItem = (TreeViewExItem)Owner;
         if (!treeViewItem.HasItems)
         {
            throw new InvalidOperationException("Cannot expand because item has no children.");
         }

         treeViewItem.IsExpanded = true;
      }

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

         return base.GetPattern(patternInterface);
      }

      #endregion

      #region Explicit Interface Methods

      void IScrollItemProvider.ScrollIntoView()
      {
         ((TreeViewExItem)Owner).BringIntoView();
      }

      void ISelectionItemProvider.AddToSelection()
      {
         throw new NotImplementedException();
      }

      void ISelectionItemProvider.RemoveFromSelection()
      {
         throw new NotImplementedException();
      }

      void ISelectionItemProvider.Select()
      {
         ((TreeViewExItem)Owner).ParentTreeView.Selection.SelectCore((TreeViewExItem)Owner);
      }

      #endregion

      #region Methods

      protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
      {
         return new TreeViewExItemDataAutomationPeer(item, this);
      }

      protected override AutomationControlType GetAutomationControlTypeCore()
      {
         return AutomationControlType.TreeItem;
      }

      protected override string GetClassNameCore()
      {
         return "TreeViewExItem";
      }

      #endregion

      #region IValueProvider Members

      public bool IsReadOnly
      {
         get { return false; }
      }

      string requestedValue;

      public void SetValue(string value)
      {
         try
         {
            if (String.IsNullOrWhiteSpace(value)) return;

            string[] ids = value.Split(new[] { ';' });

            object obj;
            if (ids.Length > 0 && ids[0] == "Context")
            {
               TreeViewExItem treeViewItem = (TreeViewExItem)Owner;
               obj = treeViewItem.DataContext;
            }
            else
            {
               obj = Owner;
            }

            if (ids.Length < 2)
            {
               requestedValue = obj.ToString();
            }
            else
            {
               Type type = obj.GetType();
               PropertyInfo pi = type.GetProperty(ids[1]);
               requestedValue = pi.GetValue(obj, null).ToString();
            }
         }
         catch (Exception ex)
         {
            requestedValue = ex.ToString();
         }
      }

      public string Value
      {
         get
         {
            if (requestedValue == null)
            {
               TreeViewExItem treeViewItem = (TreeViewExItem)Owner;
               return treeViewItem.DataContext.ToString();
            }

            return requestedValue;
         }
      }

      #endregion

      #region IInvokeProvider Members

      public void Invoke()
      {
         TreeViewExItem treeViewItem = (TreeViewExItem)Owner;
         treeViewItem.InvokeMouseDown();
      }

      #endregion
   }
}