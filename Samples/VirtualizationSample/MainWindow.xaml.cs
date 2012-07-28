namespace W7StyleSample
{
    #region

    using W7StyleSample.Model;
    using System.Diagnostics;
    using System.Windows;
    using System;
    using System.Windows.Threading;
    using System.Windows.Controls;

    #endregion

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constructors and Destructors
        Node firstNode;
        public MainWindow()
        {
            InitializeComponent();

            firstNode = new Node { Name = "element" };
            for (int i = 0; i < 2000; i++)
            {
                var node = new Node { Name = string.Format("element{0}", i) };
                node.Children.Add(new Node { Name = string.Format("element / {0}", i) });
                firstNode.Children.Add(node);                
            }
        }

        #endregion

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            DataContext = firstNode;
        }

        private void OnClear(object sender, RoutedEventArgs e)
        {
            DataContext = null;
        }
    }
}