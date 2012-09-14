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
using Demo.ViewModel;

namespace Demo
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var rootNode = new Node(null, false) { DisplayName = "rootNode" };
			var node1 = new Node(rootNode, false) { DisplayName = "element1 (editable)", IsEditable = true };
			var node2 = new Node(rootNode, false) { DisplayName = "element2" };
			var node11 = new Node(node1, false) { DisplayName = "element11" };
			var node12 = new Node(node1, false) { DisplayName = "element12 (disabled)", IsEnabled = false };
			var node13 = new Node(node1, false) { DisplayName = "element13" };
			var node14 = new Node(node1, false) { DisplayName = "element14" };
			var node15 = new Node(node1, true) { DisplayName = "element15 (lazy loading)" };
			var node131 = new Node(node13, false) { DisplayName = "element131" };
			var node132 = new Node(node13, false) { DisplayName = "element132" };

			rootNode.Children.Add(node1);
			rootNode.Children.Add(node2);
			node1.Children.Add(node11);
			node1.Children.Add(node12);
			node1.Children.Add(node13);
			node1.Children.Add(node14);
			node1.Children.Add(node15);
			node13.Children.Add(node131);
			node13.Children.Add(node132);

			TheTreeView.DataContext = rootNode;

			node13.IsExpanded = true;
			node1.IsSelected = true;
			node14.IsSelected = true;

			TheTreeView.PreviewSelectionChanged += (s, e) =>
			{
				e.CancelThis = LockSelectionCheck.IsChecked == true;
				if (e.Selecting)
				{
					//System.Diagnostics.Debug.WriteLine("Preview: Selecting " + e.Item + (e.Cancel ? " - cancelled" : ""));
				}
				else
				{
					//System.Diagnostics.Debug.WriteLine("Preview: Deselecting " + e.Item + (e.Cancel ? " - cancelled" : ""));
				}
			};
		}

		private void ClearChildrenButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			var selection = new object[TheTreeView.SelectedItems.Count];
			TheTreeView.SelectedItems.CopyTo(selection, 0);
			foreach (Node node in selection)
			{
				if (node.Children != null)
				{
					node.Children.Clear();
				}
			}
		}

		private void AddChildButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (Node node in TheTreeView.SelectedItems)
			{
				if (!node.HasDummyChild)
				{
					node.Children.Add(new Node(node, false) { DisplayName = "newborn child" });
					node.IsExpanded = true;
				}
			}
		}

		private void ExpandNodesButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (Node node in TheTreeView.SelectedItems)
			{
				node.IsExpanded = true;
			}
		}

		private void HideNodesButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (Node node in TheTreeView.SelectedItems)
			{
				node.IsVisible = false;
			}
		}

		private void ShowNodesButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (Node node in TheTreeView.Items)
			{
				DoShowAll(node, (n) => true);
			}
		}

		private void DoShowAll(Node node, Func<Node, bool> selector)
		{
			node.IsVisible = selector(node);
			if (node.Children != null)
			{
				foreach (var child in node.Children)
				{
					DoShowAll(child, selector);
				}
			}
		}

		private void SelectNoneButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (Node node in TheTreeView.Items)
			{
				DoSelectAll(node, (n) => false);
			}
		}

		private void SelectSomeButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Random rnd = new Random();
			foreach (Node node in TheTreeView.Items)
			{
				DoSelectAll(node, (n) => rnd.Next(0, 2) > 0);
			}
		}

		private void SelectAllButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (Node node in TheTreeView.Items)
			{
				DoSelectAll(node, (n) => true);
			}
		}

		private void ToggleSelectButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (Node node in TheTreeView.Items)
			{
				DoSelectAll(node, (n) => !n.IsSelected);
			}
		}

		private void DoSelectAll(Node node, Func<Node, bool> selector)
		{
			node.IsSelected = selector(node);
			if (node.Children != null)
			{
				foreach (var child in node.Children)
				{
					DoSelectAll(child, selector);
				}
			}
		}

		private void ExpandMenuItem_Click(object sender, RoutedEventArgs e)
		{
			foreach (Node node in TheTreeView.SelectedItems)
			{
				node.IsExpanded = true;
			}
		}

		private void RenameMenuItem_Click(object sender, RoutedEventArgs e)
		{
			foreach (Node node in TheTreeView.SelectedItems)
			{
				node.IsEditing = true;
				break;
			}
		}

		private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
		{
			foreach (Node node in TheTreeView.SelectedItems.Cast<Node>().ToArray())
			{
				node.Parent.Children.Remove(node);
			}
		}
	}
}
