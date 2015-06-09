using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

[assembly: AssemblyProduct("MultiSelectTreeView WPF control")]
[assembly: AssemblyTitle("WPF control library with a TreeView supporting multiple selection.")]

[assembly: AssemblyCompany("")]
[assembly: AssemblyCopyright("© 2012–2015 Yves Goergen, Goroll")]

// IMPORTANT: When changing the version number, also update the NuGet package and the version history below.
[assembly: AssemblyVersion("1.0.7.0")]

[assembly: ComVisible(false)]
[assembly: ThemeInfo(
	// Where theme specific resource dictionaries are located
	// (used if a resource is not found in the page, or application resource dictionaries)
	ResourceDictionaryLocation.SourceAssembly,
	// Where the generic resource dictionary is located
	// (used if a resource is not found in the page, app, or any theme specific resource dictionaries)
	ResourceDictionaryLocation.SourceAssembly
)]

// Change history:
//
// 1.0.7 - 2015-06-09
// * Fix: ClearSelection() throws exception if a PreviewSelectionChanged handler changes the selection
//
// 1.0.6 - 2015-04-09
// * Fix: Treeview disposed on tabhide
// * Non-Ctrl click in the background clears selection
//
// 1.0.5 - 2015-03-02
// * Fix: MultiSelectTreeView does not use template from ItemTemplateSelector
//
// 1.0.4 - 2014-11-17
// * Changed Windows 8 Aero2 theme to match native style more closely (hover colours different from selected)
//
// 1.0.3 - 2014-07-28
// * Added FallbackValues to several XAML bindings to avoid "Cannot find source for binding" errors and warnings
//
// 1.0.2 - 2014-06-26
// * Only expand/collapse item on double-click with the left mouse button, not others
//
// 1.0.1 - 2014-06-07
// * Deselect hidden items and all children of hidden and collapsed items
//
// 1.0 - past (see commit history)
