namespace EditSample.Model
{
    #region

    using System.Collections.ObjectModel;
    using System.Windows.Media;
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
            Children = new ObservableCollection<Node>();
            Editable = true;
        }

        #endregion

        #region Public Properties

        public ObservableCollection<Node> Children { get; set; }

        public string Name { get; set; }

        public Brush Color { get; set; }

        public List<Brush> Colors { get; set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion

        public bool Editable { get; set; }
    }
}