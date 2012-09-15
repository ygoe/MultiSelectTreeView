using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Demo
{
	/// <summary>
	/// Provides a configurable DataTemplateSelector.
	/// </summary>
	public class TypeTemplateSelector : DataTemplateSelector
	{
		/// <summary>
		/// Gets or sets the list of template definitions.
		/// </summary>
		public List<TypeTemplateDefinition> TemplateDefinitions { get; set; }

		/// <summary>
		/// Initialises a new instance of the TypeTemplateSelector class.
		/// </summary>
		public TypeTemplateSelector()
		{
			TemplateDefinitions = new List<TypeTemplateDefinition>();
		}

		/// <summary>
		/// Selects a DataTemplate for the item, based on the item's type.
		/// </summary>
		/// <param name="item">Item to select the template for.</param>
		/// <param name="container">Unused.</param>
		/// <returns></returns>
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			foreach (var def in TemplateDefinitions)
			{
				if (def.Type.IsInstanceOfType(item)) return def.Template;
			}
			return null;
		}
	}

	/// <summary>
	/// Defines a template to be selected for a type.
	/// </summary>
	public class TypeTemplateDefinition
	{
		/// <summary>
		/// Gets or sets the item type to define the template for.
		/// </summary>
		public Type Type { get; set; }
		/// <summary>
		/// Gets or sets the DataTemplate to select for this item type.
		/// </summary>
		public DataTemplate Template { get; set; }
	}
}
