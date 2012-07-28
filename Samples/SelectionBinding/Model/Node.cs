namespace BindingSample.Model
{
   #region

   using System.Collections.ObjectModel;

   #endregion

   /// <summary>
   /// Model for testing
   /// </summary>
   public class Node
   {
      #region Constructors and Destructors

      public Node()
      {
         Children = new ObservableCollection<Node>();
      }

      #endregion

      #region Public Properties

      public ObservableCollection<Node> Children { get; set; }

      private string name;

      public string Name
      {
         get
         {
            return name;
         }
         set
         {
            name = value;
         }
      }

      #endregion

      #region Public Methods

      public override string ToString()
      {
         return Name;
      }

      #endregion
   }
}