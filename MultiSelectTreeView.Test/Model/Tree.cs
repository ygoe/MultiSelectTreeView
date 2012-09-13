using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiSelectTreeView.Test.Model.Helper;
using System.Windows.Automation;

namespace MultiSelectTreeView.Test.Model
{
    class Tree
    {
        AutomationElement treeAutomationElement;
        Element element1;
        Element element11;
        Element element12;
        Element element13;
        Element element14;
        Element element15;

        public Tree(AutomationElement treeAutomationElement)
        {
            this.treeAutomationElement = treeAutomationElement;
        }

        public Element Element1 { get { return element1 != null ? element1 : element1 = new Element(treeAutomationElement.FindFirstDescendant(ControlType.TreeItem)); } }
        public Element Element11 { get { return element11 != null ? element11 : element11 = new Element(Element1.Ae.FindDescendantByName(ControlType.TreeItem, "element11")); } }
        public Element Element12 { get { return element12 != null ? element12 : element12 = new Element(Element1.Ae.FindDescendantByName(ControlType.TreeItem, "element12")); } }
        public Element Element13 { get { return element13 != null ? element13 : element13 = new Element(Element1.Ae.FindDescendantByName(ControlType.TreeItem, "element13")); } }
        public Element Element14 { get { return element14 != null ? element14 : element14 = new Element(Element1.Ae.FindDescendantByName(ControlType.TreeItem, "element14")); } }
        public Element Element15 { get { return element15 != null ? element15 : element15 = new Element(Element1.Ae.FindDescendantByName(ControlType.TreeItem, "element15")); } }
    }
}
