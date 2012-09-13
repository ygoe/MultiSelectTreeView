namespace MultiSelectTreeView.Test.Model.Helper
{
    #region

    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Windows.Automation;
    using System.Runtime.InteropServices;

    #endregion

    /// <summary>
    /// Helper to handle a processes automation element
    /// </summary>
    public class TreeApplication : IDisposable
    {
        #region Constants and Fields

        private readonly string processName;

        private static Process p;

        private AutomationElement app;

        #endregion

        #region Constructors and Destructors

        public TreeApplication(string fileName)
        {
            this.processName = fileName;// +".exe";

            p = GetProcess(processName);
            if (p != null)
            {
                p.Kill();
            }

            ProcessStartInfo ps = new ProcessStartInfo { FileName = fileName };
            p = new Process();
            p.StartInfo = ps;
            p.Start();

            while (!p.Responding)
            {
                Trace.WriteLine("waiting process");
                Thread.Sleep(200);
            }

            while (app == null)
            {
                app = InitializeAutomationElement();
                Trace.WriteLine("waiting for app");
                Thread.Sleep(200);
            }
        }
        #endregion

        #region Public Properties

        public AutomationElement Ae
        {
            get
            {
                return app;
            }
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            if (p != null && p.MainWindowHandle != IntPtr.Zero)
            {
                p.Kill();
                while (GetProcess(processName) != null)
                {
                    Thread.Sleep(50);
                }
            }
        }

        #endregion

        #region Methods

        private static Process GetProcess(string processname)
        {
            Process[] processes = Process.GetProcessesByName(processname);
            return processes.FirstOrDefault();
        }

        private AutomationElement InitializeAutomationElement()
        {
            AutomationElement ae = AutomationElement.RootElement;
            PropertyCondition cond = new PropertyCondition(AutomationElement.ProcessIdProperty, p.Id);

            return ae.FindFirst(TreeScope.Children, cond);
        }

        #endregion
    }
}