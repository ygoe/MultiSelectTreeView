using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TreeViewEx.Test.Model.Helper;
using System.Windows.Automation;

namespace TreeViewEx.Test.Model
{
    class SimpleSampleTree:Tree
    {
        public SimpleSampleTree(TreeApplication app):base(app.Ae)
        {
        }
    }
}
