namespace EditSample
{
    #region

    using EditSample.Model;
    using System.Windows.Media;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constructors and Destructors

        public MainWindow()
        {
            List<Brush> blueColors= new List<Brush> { Brushes.AliceBlue, Brushes.BlueViolet, Brushes.Blue, Brushes.CornflowerBlue };
            List<Brush> greenColors = new List<Brush> { Brushes.GreenYellow, Brushes.Green, Brushes.LawnGreen, Brushes.LightGreen };

            var firstNode = new Node { Name = "element" };
            var first1 = new Node { Name = "Blue Colors", Editable = false, Color = Brushes.LightBlue };
            var first11 = new Node { Name = "Press F2 to select blue color", Color = Brushes.AliceBlue, Colors = blueColors };
            var first12 = new Node { Name = "Press F2 to select blue color", Color = Brushes.BlueViolet, Colors = blueColors };
            var first13 = new Node { Name = "Press F2 to select blue color", Color = Brushes.Blue, Colors = blueColors };
            var first14 = new Node { Name = "Press F2 to select blue color", Color = Brushes.CadetBlue, Colors = blueColors };
            var first15 = new Node { Name = "Press F2 to select blue color", Color = Brushes.CornflowerBlue, Colors = blueColors };

            var first2 = new Node { Name = "Green Colors", Editable = false, Color = Brushes.LightGreen };
            var first21 = new Node { Name = "Press F2 to select green color", Color = Brushes.GreenYellow, Colors = greenColors };
            var first22 = new Node { Name = "Press F2 to select green color", Color = Brushes.Green, Colors = greenColors };

            firstNode.Children.Add(first1);
            firstNode.Children.Add(first2);
            first1.Children.Add(first11);
            first1.Children.Add(first12);
            first1.Children.Add(first13);
            first1.Children.Add(first14);
            first1.Children.Add(first15);
            first2.Children.Add(first21);
            first2.Children.Add(first22);

            DataContext = firstNode;

            InitializeComponent();
        }

        #endregion
    }
}