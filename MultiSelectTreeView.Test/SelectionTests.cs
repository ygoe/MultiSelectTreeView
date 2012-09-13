namespace MultiSelectTreeView.Test.Model
{
   #region

   using System.Windows.Automation;

   using Microsoft.VisualStudio.TestTools.UnitTesting;

   using MultiSelectTreeView.Test.Model.Helper;

   #endregion

   [TestClass]
   [DeploymentItem("W7StyleSample.exe")]
   public class SelectionTests
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
      public void SelectElement1()
      {
          using (TreeApplication app = new TreeApplication("SimpleSample"))
          {
              SimpleSampleTree sst = new SimpleSampleTree(app);
              sst.Element1.Select();
              Assert.IsTrue(sst.Element1.IsSelected);
          }
      }

      [TestMethod]
      public void SelectElement11()
      {
          using (TreeApplication app = new TreeApplication("SimpleSample"))
          {
              SimpleSampleTree sst = new SimpleSampleTree(app);
              sst.Element1.Expand();
              sst.Element11.Select();
              Assert.IsTrue(sst.Element11.IsSelected);
          }
      }

      [TestMethod]
      public void SelectElement11ByClickOnIt()
      {
          using (TreeApplication app = new TreeApplication("SimpleSample"))
          {
              SimpleSampleTree sst = new SimpleSampleTree(app);
              sst.Element1.Expand();
              sst.Element11.Select();
              Assert.IsTrue(sst.Element11.IsSelected);
          }
      }


      #endregion
   }
}