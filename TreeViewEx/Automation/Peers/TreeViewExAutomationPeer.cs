namespace System.Windows.Automation.Peers
{
   #region

   using System.Windows.Automation.Provider;
   using System.Windows.Controls;

   #endregion

   public class TreeViewExAutomationPeer : ItemsControlAutomationPeer, ISelectionProvider
   {
      #region Constructors and Destructors

      public TreeViewExAutomationPeer(TreeViewEx owner)
         : base(owner)
      {
      }

      #endregion

      #region Public Properties

      public bool CanSelectMultiple
      {
         get
         {
            return false;
         }
      }

      public bool IsSelectionRequired
      {
         get
         {
            return false;
         }
      }

      #endregion

      #region Public Methods

      public override object GetPattern(PatternInterface patternInterface)
      {
         if (patternInterface == PatternInterface.Selection)
         {
            return this;
         }

         // if (patternInterface == PatternInterface.Scroll)
         // {
         // ItemsControl itemsControl = (ItemsControl)Owner;
         // if (itemsControl.ScrollHost != null)
         // {
         // AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(itemsControl.ScrollHost);
         // if (automationPeer != null && automationPeer is IScrollProvider)
         // {
         // automationPeer.EventsSource = this;
         // return (IScrollProvider)automationPeer;
         // }
         // }
         // }
         return base.GetPattern(patternInterface);
      }

      #endregion

      #region Explicit Interface Methods

      IRawElementProviderSimple[] ISelectionProvider.GetSelection()
      {
         IRawElementProviderSimple[] array = null;

         // TreeViewExItem selectedContainer = ((TreeViewEx)base.Owner).SelectedContainer;
         // if (selectedContainer != null)
         // {
         // AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(selectedContainer);
         // if (automationPeer.EventsSource != null)
         // {
         // automationPeer = automationPeer.EventsSource;
         // }

         // if (automationPeer != null)
         // {
         // array = new[] { this.ProviderFromPeer(automationPeer) };
         // }
         // }

         // if (array == null)
         // {
         // array = new IRawElementProviderSimple[0];
         // }
         return array;
      }

      #endregion

      #region Methods

      /// <summary>
      /// When overridden in a derived class, creates a new instance of the <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer"/> for a data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> collection of this <see cref="T:System.Windows.Controls.ItemsControl"/>.
      /// </summary>
      /// <param name="item">
      /// The data item that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer"/>.
      /// </param>
      /// <returns>
      /// The new <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer"/> created.
      /// </returns>
      protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
      {
         return new TreeViewExItemDataAutomationPeer(item, this);
      }

      protected override AutomationControlType GetAutomationControlTypeCore()
      {
         return AutomationControlType.Tree;
      }

      protected override string GetClassNameCore()
      {
         return "TreeViewEx";
      }

      #endregion
   }
}