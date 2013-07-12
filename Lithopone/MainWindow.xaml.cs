using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lithopone.FileReader;
using Lithopone.Kernel;
using Lithopone.Memory;
using Arora;

namespace Lithopone
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        static String BASEFOLER = AppDomain.CurrentDomain.BaseDirectory;
        int mCurTestIndex = 0;

        Dictionary<String, String> mDemogInfo;
        //List<StAnswer> mAnswers;
        //Dictionary<int, StItem> mItems;

        TestRunner mCurRunner;
        DemogRunner mDemogRunner;

        public static int CANVAS_SIZE_W = 778;
        public static int CANVAS_SIZE_H = 561;
        public static int WND_WIDTH = 800;
        public static int WND_HEIGHT = 600;

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mDemogInfo = new Dictionary<string, string>();
            mDemogRunner = new DemogRunner(this);
            mDemogRunner.GenFromFile(Lib.DemogFileName);

            //adjustInnerComs();

        }

        public void OnDemogClose()
        {
            mDemogInfo = mDemogRunner.GetResult();

            mCurRunner = new TestRunner(Lib.TestFileName, this);
            mCurRunner.SetInstructionPage();
            mCurTestIndex++;
        }

        public void OnTestClose()
        {
            AroraCore ac = new AroraCore(new ReportForm());
            AroraNormFactory anf = new AroraNormFactory();
            ac.SetData(mCurRunner.mItemRunner.mItems, mCurRunner.mAnswers, mDemogInfo, anf.GetNorm());
            ac.Run();
            //Close();
        }

        ~MainWindow()
        {
        }

        private void adjustInnerComs()
        {
            amScrollViewer.Width = this.Width - 16;
            amCanvas.Width = this.Width - 16;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            adjustInnerComs();
        }
    }
}
