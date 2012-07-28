namespace W7StyleSample.Model
{
   #region

   using System.Collections.ObjectModel;
    using System.Collections.Generic;

   #endregion

   /// <summary>
   /// Model for testing
   /// </summary>
   public class Node
   {
      #region Constructors and Destructors

      public Node()
      {
         Children = new List<Node>();
      }

      #endregion

      #region Public Properties

      public List<Node> Children { get; set; }

      public string Name { get; set; }

      #endregion

      #region Public Methods

      public override string ToString()
      {
         return Name;
      }

      #endregion
   }
}