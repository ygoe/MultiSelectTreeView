using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiSelectTreeView.Test.Model.Helper;
using System.Windows.Automation;

namespace MultiSelectTreeView.Test.Model
{
    class SimpleSampleTree:Tree
    {
        public SimpleSampleTree(TreeApplication app):base(app.Ae)
        {
        }
    }
}
