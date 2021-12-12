using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace Demo.ViewModel
{
    public class MainViewModel : ObservableRecipient
    {

		private TreeItemViewModel RootNode;

		public ObservableCollection<TreeItemViewModel> TheSelectedFew { get; set; } = new ObservableCollection<TreeItemViewModel>();

		public ObservableCollection<TreeItemViewModel> EntireSet => RootNode.Children;

		ICommand clearSelection;
		public ICommand ClearSelectionCommand
        {

			get 
			{
				if (clearSelection == null)
                {
					clearSelection = new RelayCommand(doClearSelection);
                }
				return clearSelection;
			}
        }

        private void doClearSelection()
        {
            TheSelectedFew.Clear();
        }

        public MainViewModel()
        {

			// Create some example nodes to play with
			RootNode = new TreeItemViewModel(null, false) { DisplayName = "rootNode" };
			var node1 = new TreeItemViewModel(RootNode, false) { DisplayName = "element1 (editable)", IsEditable = true };
			var node2 = new TreeItemViewModel(RootNode, false) { DisplayName = "element2 (selected)" };
			var node11 = new TreeItemViewModel(node1, false) { DisplayName = "element11", Remarks = "Look at me!" };
			var node12 = new TreeItemViewModel(node1, false) { DisplayName = "element12 (disabled)", IsEnabled = false };
			var node13 = new TreeItemViewModel(node1, false) { DisplayName = "element13" };
			var node131 = new TreeItemViewModel(node13, false) { DisplayName = "element131" };
			var node132 = new TreeItemViewModel(node13, false) { DisplayName = "element132" };
			var node14 = new TreeItemViewModel(node1, false) { DisplayName = "element14 with colours" };
			var colorNode1 = new ColorItemViewModel(node14, false) { Color = Colors.Aqua, IsEditable = true };
			var colorNode2 = new ColorItemViewModel(node14, false) { Color = Colors.ForestGreen };
			var colorNode3 = new ColorItemViewModel(node14, false) { Color = Colors.LightSalmon };
			var node15 = new TreeItemViewModel(node1, true) { DisplayName = "element15 (lazy loading)" };

			// Add them all to each other
			RootNode.Children.Add(node1);
			RootNode.Children.Add(node2);
			node1.Children.Add(node11);
			node1.Children.Add(node12);
			node1.Children.Add(node13);
			node13.Children.Add(node131);
			node13.Children.Add(node132);
			node1.Children.Add(node14);
			node14.Children.Add(colorNode1);
			node14.Children.Add(colorNode2);
			node14.Children.Add(colorNode3);
			node1.Children.Add(node15);

			TheSelectedFew.Add(node11);
			TheSelectedFew.Add(node13);
			

			// Use the root node as the window's DataContext to allow data binding. The TreeView
			// will use the Children property of the DataContext as list of root tree items. This
			// property happens to be the same as each item DataTemplate uses to find its subitems.

			//// Preset some node states
			//node1.IsSelected = true;
			//node13.IsSelected = true;
			//node14.IsExpanded = true;




		}
    }
}