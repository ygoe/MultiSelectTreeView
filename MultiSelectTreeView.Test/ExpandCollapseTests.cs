namespace MultiSelectTreeView.Test.Model
{
   #region
   using System.Windows.Automation;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   using MultiSelectTreeView.Test.Model.Helper;

   #endregion

   [TestClass]
   [DeploymentItem("W7StyleSample.exe")]
   public class ExpandCollapseTests
   {
      #region Constants and Fields

      private const string FileName = "W7StyleSample.exe";

      private const string ProcessName = "W7StyleSample";

      #endregion

      #region Public Properties

      public TestContext TestContext { get; set; }

      #endregion

      #region Public Methods

      [TestMethod]
      public void Collapse()
      {
          using (TreeApplication app = new TreeApplication("SimpleSample"))
          {
              SimpleSampleTree sst = new SimpleSampleTree(app);
              sst.Element1.Expand();
              sst.Element1.Collapse();

              Assert.IsFalse(sst.Element1.IsExpanded);
          }
      }

      [TestMethod]
      public void Expand()
      {
          using (TreeApplication app = new TreeApplication("SimpleSample"))
          {
              SimpleSampleTree sst = new SimpleSampleTree(app);
              sst.Element1.Expand();

              Assert.IsTrue(sst.Element1.IsExpanded);
          }
      }

      #endregion
   }
}