using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
			ShowSecondCheck_Checked(null, null);

			DataContext = new MainViewModel();

			
		}

		private void TheTreeView_PreviewSelectionChanged(object sender, PreviewSelectionChangedEventArgs e)
		{
			if (LockSelectionCheck.IsChecked == true)
			{
				// The current selection is locked by user request (Lock CheckBox is checked).
				// Don't allow any changes to the selection at all.
				e.CancelThis = true;
			}
			else
			{
				// Selection is not locked, apply other conditions.
				// Require all selected items to be of the same type. If an item of another data
				// type is already selected, don't include this new item in the selection.
				if (e.Selecting && TheTreeView.SelectedItems.Count > 0)
				{
					e.CancelThis = e.Item.GetType() != TheTreeView.SelectedItems[0].GetType();
				}
			}

			//if (e.Selecting)
			//{
			//    System.Diagnostics.Debug.WriteLine("Preview: Selecting " + e.Item + (e.Cancel ? " - cancelled" : ""));
			//}
			//else
			//{
			//    System.Diagnostics.Debug.WriteLine("Preview: Deselecting " + e.Item + (e.Cancel ? " - cancelled" : ""));
			//}
		}

		private void ClearChildrenButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			var selection = new object[TheTreeView.SelectedItems.Count];
			TheTreeView.SelectedItems.CopyTo(selection, 0);
			foreach (TreeItemViewModel node in selection)
			{
				if (node.Children != null)
				{
					node.Children.Clear();
				}
			}
		}

		private void AddChildButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (TreeItemViewModel node in TheTreeView.SelectedItems)
			{
				if (!node.HasDummyChild)
				{
					node.Children.Add(new TreeItemViewModel(node, false) { DisplayName = "newborn child" });
					node.IsExpanded = true;
				}
			}
		}

		private void ExpandNodesButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (TreeItemViewModel node in TheTreeView.SelectedItems)
			{
				node.IsExpanded = true;
			}
		}

		private void HideNodesButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (TreeItemViewModel node in TheTreeView.SelectedItems.OfType<TreeItemViewModel>().ToArray())
			{
				node.IsVisible = false;
			}
		}

		private void ShowNodesButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (TreeItemViewModel node in TheTreeView.Items)
			{
				DoShowAll(node, (n) => true);
			}
		}

		private void DoShowAll(TreeItemViewModel node, Func<TreeItemViewModel, bool> selector)
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
			foreach (TreeItemViewModel node in TheTreeView.Items)
			{
				DoSelectAll(node, (n) => false);
			}
		}

		private void SelectSomeButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Random rnd = new Random();
			foreach (TreeItemViewModel node in TheTreeView.Items)
			{
				DoSelectAll(node, (n) => rnd.Next(0, 2) > 0);
			}
		}

		private void SelectAllButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (TreeItemViewModel node in TheTreeView.Items)
			{
				DoSelectAll(node, (n) => true);
			}
		}

		private void ToggleSelectButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (TreeItemViewModel node in TheTreeView.Items)
			{
				DoSelectAll(node, (n) => !n.IsSelected);
			}
		}

		private void DoSelectAll(TreeItemViewModel node, Func<TreeItemViewModel, bool> selector)
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
			foreach (TreeItemViewModel node in TheTreeView.SelectedItems)
			{
				node.IsExpanded = true;
			}
		}

		private void RenameMenuItem_Click(object sender, RoutedEventArgs e)
		{
			foreach (TreeItemViewModel node in TheTreeView.SelectedItems)
			{
				node.IsEditing = true;
				break;
			}
		}

		private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
		{
			foreach (TreeItemViewModel node in TheTreeView.SelectedItems.Cast<TreeItemViewModel>().ToArray())
			{
				node.Parent.Children.Remove(node);
			}
		}

		private void ShowSecondCheck_Checked(object sender, RoutedEventArgs e)
		{
			if (ShowSecondCheck.IsChecked == true)
			{
				if (LastColumn.ActualWidth == 0)
					Width += FirstColumn.ActualWidth;
				LastColumn.Width = new GridLength(1, GridUnitType.Star);
			}
			else
			{
				if (LastColumn.ActualWidth > 0)
					Width -= LastColumn.ActualWidth;
				LastColumn.Width = new GridLength(0, GridUnitType.Pixel);
			}
		}

		private void AllowMultiSelection_Checked(object sender, RoutedEventArgs e)
		{
			if (AllowMultiSelect == null || TheTreeView == null)
			{
				return;
			}
			
			if (e.RoutedEvent == ToggleButton.CheckedEvent)
			{
				TheTreeView.SelectionMode = TreeViewSelectionMode.MultiSelectEnabled;
			}
			else
			{
				TheTreeView.SelectionMode = TreeViewSelectionMode.SingleSelectOnly;
			}
		}
	}
}
