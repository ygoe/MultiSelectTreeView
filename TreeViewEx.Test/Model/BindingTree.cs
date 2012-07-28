using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeViewEx.Test.Model.Helper;
using System.Windows.Automation;

namespace TreeViewEx.Test.Model
{
    class BindingTrees
    {
        public BindingTrees(TreeApplication app)
        {
            LeftTree = new Tree(app.Ae.FindDescendantByAutomationId(ControlType.Tree, "leftTree"));
            RightTree = new Tree(app.Ae.FindDescendantByAutomationId(ControlType.Tree, "rightTree"));
        }

        public Tree LeftTree { get; set; }
        public Tree RightTree { get; set; }
    }
}
