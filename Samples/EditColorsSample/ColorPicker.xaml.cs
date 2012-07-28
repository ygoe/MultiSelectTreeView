using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TreeViewEx.SimpleSample
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();
        }



        public bool Editing
        {
            get { return (bool)GetValue(EditingProperty); }
            set { SetValue(EditingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Editing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditingProperty =
            DependencyProperty.Register("Editing", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(false));

        public Brush SelectedColor
        {
            get { return (Brush)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Brush), typeof(ColorPicker), new UIPropertyMetadata(null));



        public List<Brush> Colors
        {
            get { return (List<Brush>)GetValue(ColorsProperty); }
            set { SetValue(ColorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Colors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorsProperty =
            DependencyProperty.Register("Colors", typeof(List<Brush>), typeof(ColorPicker), new UIPropertyMetadata(new List<Brush>()));

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement fe = (FrameworkElement)sender;
            SelectedColor = (Brush)fe.DataContext;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Editing = false;
        }
    }
}
