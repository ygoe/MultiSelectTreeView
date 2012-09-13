namespace MultiSelectTreeView.Test.Model.Helper
{
    #region

    using System;
    using System.Windows.Automation;
    using System.Linq;
    #endregion

    /// <summary>
    /// Helper methods for AutomationElements
    /// </summary>
    public static class AutomationElementHelper
    {
        #region Methods

        public static AutomationPattern GetPattern<T>() where T : BasePattern
        {
            if (typeof(T) == typeof(ExpandCollapsePattern))
            {
                return ExpandCollapsePattern.Pattern;
            }

            if (typeof(T) == typeof(SelectionItemPattern))
            {
                return SelectionItemPattern.Pattern;
            }

            if (typeof(T) == typeof(ValuePattern))
            {
                return ValuePattern.Pattern;
            }

            throw new InvalidOperationException("Pattern not supported of GetPattern extension method.");
        }

        public static T GetPattern<T>(this AutomationElement treeItem) where T : BasePattern
        {
            T t = treeItem.GetCurrentPattern(GetPattern<T>()) as T;
            if (t == null)
            {
                throw new InvalidOperationException("Pattern was not found for object.");
            }

            return t;
        }

        public static AutomationElement FindFirstDescendant(this AutomationElement element, ControlType type)
        {
            PropertyCondition cond = new PropertyCondition(AutomationElement.ControlTypeProperty, type);

            AutomationElement treeItem = element.FindFirst(TreeScope.Descendants, cond);

            return treeItem;
        }

        public static AutomationElement FindDescendant(this AutomationElement element, ControlType type, int index)
        {
            PropertyCondition cond = new PropertyCondition(AutomationElement.ControlTypeProperty, type);

            AutomationElementCollection treeItems = element.FindAll(TreeScope.Descendants, cond);

            return treeItems[index];
        }

        public static AutomationElement FindDescendantByName(this AutomationElement element, ControlType type, string name)
        {
            PropertyCondition typeCond = new PropertyCondition(AutomationElement.ControlTypeProperty, type);
            PropertyCondition nameCond = new PropertyCondition(AutomationElement.NameProperty, name);
            AndCondition and = new AndCondition(new Condition[] { typeCond, nameCond });

            return element.FindFirst(TreeScope.Descendants, and);
        }

        public static AutomationElement FindDescendantByAutomationId(this AutomationElement element, ControlType type, string automationId)
        {
            PropertyCondition typeCond = new PropertyCondition(AutomationElement.ControlTypeProperty, type);
            PropertyCondition nameCond = new PropertyCondition(AutomationElement.AutomationIdProperty, automationId);
            AndCondition and = new AndCondition(new Condition[] { typeCond, nameCond });

            return element.FindFirst(TreeScope.Descendants, and);
        }
        #endregion
    }
}