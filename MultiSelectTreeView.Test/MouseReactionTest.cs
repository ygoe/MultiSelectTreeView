using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiSelectTreeView.Test.Model.Helper;
using System.Windows.Automation;
using System.Threading;

namespace MultiSelectTreeView.Test.Model
{
    [TestClass]
    public class MouseReactionTest
    {
        [TestMethod]
        public void KeyboardLeftRightUpDownArrow()
        {
            using (TreeApplication app = new TreeApplication("SimpleSample"))
            {
                SimpleSampleTree sst = new SimpleSampleTree(app);

                Mouse.Click(sst.Element1);
                Mouse.ExpandCollapseClick(sst.Element1);
                Assert.IsTrue(sst.Element1.IsExpanded, "Element1 not expanded");

                Mouse.Click(sst.Element11);
                Assert.IsTrue(sst.Element11.IsFocused, "Element11 has not focus after down");
                Assert.IsTrue(sst.Element11.IsSelected, "Element11 not selected after down");
            }
        }

        [TestMethod]
        public void KeyboardUpDownWithShiftArrow()
        {
            using (TreeApplication app = new TreeApplication("SimpleSample"))
            {
                SimpleSampleTree sst = new SimpleSampleTree(app);
                Mouse.ExpandCollapseClick(sst.Element1);

                Mouse.Click(sst.Element11);
                Assert.IsTrue(sst.Element11.IsFocused, "Element11 has not focus after down");
                Assert.IsTrue(sst.Element11.IsSelected, "Element11 not selected after down");

                Mouse.ShiftClick(sst.Element12);
                Assert.IsTrue(sst.Element11.IsSelected, "Element11 not selected after second down");
                Assert.IsTrue(sst.Element12.IsSelected, "Element12 not selected after second down");
                Assert.IsTrue(sst.Element12.IsFocused, "Element12 has not focus after second down");

                Mouse.Click(sst.Element13);
                Assert.IsFalse(sst.Element11.IsSelected, "Element11 selected after second down");
                Assert.IsFalse(sst.Element12.IsSelected, "Element12 selected after second down");
                Assert.IsTrue(sst.Element13.IsSelected, "Element13 not selected after down");
                Assert.IsTrue(sst.Element13.IsFocused, "Element13 has not focus after down");

                Mouse.ShiftClick(sst.Element11);
                Assert.IsTrue(sst.Element11.IsSelected, "Element11 selected after up");
                Assert.IsTrue(sst.Element12.IsSelected, "Element12 selected after up");
                Assert.IsTrue(sst.Element13.IsSelected, "Element13 not selected after up");
                Assert.IsTrue(sst.Element11.IsFocused, "Element13 has not focus after up");


                Mouse.ShiftClick(sst.Element14);
                Assert.IsTrue(sst.Element13.IsSelected, "Element13 not selected after fourth down");
                Assert.IsTrue(sst.Element14.IsSelected, "Element14 not selected after fourth down");
                Assert.IsTrue(sst.Element14.IsFocused, "Element14 has not focus after fourth down");
            }
        }

        [TestMethod]
        public void DoubleClick()
        {
            using (TreeApplication app = new TreeApplication("SimpleSample"))
            {
                SimpleSampleTree sst = new SimpleSampleTree(app);
                
                Mouse.DoubleClick(sst.Element1);
                Assert.IsTrue(sst.Element1.IsExpanded, "Element1 is not expanded after double click");

                Mouse.DoubleClick(sst.Element13);
                Assert.IsTrue(sst.Element13.IsExpanded, "Element13 is not expanded after double click");
            }
        }

        [TestMethod]
        public void KeyboardUpDownWithCtrlArrow()
        {
            using (TreeApplication app = new TreeApplication("SimpleSample"))
            {
                SimpleSampleTree sst = new SimpleSampleTree(app);
                Mouse.ExpandCollapseClick(sst.Element1);
                Mouse.Click(sst.Element11);
                Assert.IsTrue(sst.Element11.IsSelected, "Element11 not selected after down");
                Assert.IsTrue(sst.Element11.IsFocused, "Element11 not has focus after down");

                Mouse.CtrlClick(sst.Element14);
                Assert.IsTrue(sst.Element11.IsSelected, "Element11 not selected after ctrlspace");
                Assert.IsTrue(sst.Element14.IsFocused, "Element14 not has focus after ctrlspace");
                Assert.IsTrue(sst.Element14.IsSelected, "Element14 not selected after ctrlspace");
                Assert.IsFalse(sst.Element14.IsSelectedPreview, "Element14 selectedPreview after ctrlspace");

                Mouse.CtrlClick(sst.Element15);
                Assert.IsTrue(sst.Element11.IsSelected, "Element11 not selected after ctrlspace");
                Assert.IsTrue(sst.Element14.IsSelected, "Element14 not selected after ctrlspace");
                Assert.IsFalse(sst.Element14.IsSelectedPreview, "Element14 selectedPreview after ctrlspace");
                Assert.IsTrue(sst.Element15.IsSelected, "Element15 not selected after space");
                Assert.IsTrue(sst.Element15.IsFocused, "Element15 not has focus after space");
            }
        }

        #region Public Properties

        public TestContext TestContext { get; set; }

        #endregion
    }
}
