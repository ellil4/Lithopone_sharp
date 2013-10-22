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
using Lithopone.UIComponents;
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

        public Dictionary<String, String> mDemogInfo;
        public Dictionary<int, StDemogLine> mDemogLines;

        public int mScreenWidth, mScreenHeight;

        TestRunner mCurRunner;
        public DemogRunner mDemogRunner;
        public StTestXmlHeader mTestHeader;

        public MainWindow(Dictionary<int, StDemogLine> DemogLine, StTestXmlHeader TestHeader)
        {
            InitializeComponent();
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            mDemogLines = DemogLine;
            mTestHeader = TestHeader;
        }

        //prameters: element: the element to centralize, sizeX & sizeY: size of the element
        public void centralize(UIElement element, int sizeX, int sizeY)
        {
            int screenX = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            int screenY = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
            int offx = (screenX - sizeX) / 2;
            int offy = (screenY - sizeY) / 2;
            Canvas.SetTop(element, (double)offy);
            Canvas.SetLeft(element, (double)offx);
        }

        public int getXScreenTaken()
        {
            return (int)(System.Windows.SystemParameters.PrimaryScreenWidth / 5 * 4);
        }

        public int getYScreenTaken()
        {
            return (int)(System.Windows.SystemParameters.PrimaryScreenHeight / 5 * 4);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mScreenWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            mScreenHeight = (int)System.Windows.SystemParameters.PrimaryScreenHeight;

            TitlePage page = new TitlePage(this, getXScreenTaken(), getYScreenTaken());
            page.amRichTxt.Document = new FlowDocument(new Paragraph(new Run(mTestHeader.Description)));
            page.amTitleBlock.Text = mTestHeader.Name;
            amCanvas.Children.Add(page);
            centralize(page, getXScreenTaken(), getYScreenTaken());


            //test below
           /* AroraCore ac = new AroraCore(new ReportForm());
            AroraNormFactory anf = new AroraNormFactory();
            Dictionary<int, StItem> items = new Dictionary<int, StItem>();
            List<StAnswer> answers = new List<StAnswer>();
            for (int i = 0; i < 68; i++)
            {
                StAnswer answer = new StAnswer(0, 3);
                answers.Add(answer);

                Dictionary<int,StSelection> selections = new Dictionary<int,StSelection>();
                for(int j = 0; j < 4; j++)
                {
                    selections.Add(j, new StSelection(0, (float)j, "aaa"));
                }
                StItem item = new StItem(i, "none", ref selections);
                items.Add(i, item);
            }
            ac.SetData(items, answers, new Dictionary<String, String>(), anf.GetNorm());
            ac.Run();*/

            FullScreen();

            //systest
            /*AroraNormFactory anf = new AroraNormFactory();
            AroraReport rep = new AroraReport(mDemogLines.Count, 68,
                anf.GetNorm());

            rep.DoReport();*/
        }

        public void FullScreen()
        {
            this.WindowState = System.Windows.WindowState.Normal;
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            //this.Topmost = true;

            this.Left = 0;
            this.Top = 0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        }

        public void OnTitleClose()
        {
            amCanvas.Children.Clear();
            //mDemogInfo = new Dictionary<string, string>();
            DemogWindow dw = new DemogWindow(this);
            mDemogRunner = new DemogRunner(dw);

            mDemogRunner.GenUI(mDemogLines);
            dw.ShowDialog();
        }

        //how to call report
        public void OnThanksClose()
        {
            AroraCore ac = new AroraCore();
            AroraNormFactory anf = new AroraNormFactory();
            ac.SetData(mCurRunner.mItemPage.mItems, mCurRunner.mAnswers, mDemogInfo, anf.GetNorm());
            ac.Sta_Save();

            AroraReport rep = new AroraReport(mDemogLines.Count, mCurRunner.mItemPage.mItems.Count,
                anf.GetNorm());

            rep.DoReport();

            this.Close();
        }

        public void OnDemogClose()
        {
            if (mDemogInfo != null)
            {
                mCurRunner = new TestRunner(Lib.TestFileName, this);
                mCurRunner.SetInstructionPage();
                mCurTestIndex++;
            }
        }

        public void OnTestClose()
        {
            ThanksPage page = new ThanksPage(this);
            amCanvas.Children.Clear();
            amCanvas.Children.Add(page);
            Canvas.SetTop(page, 0);
            Canvas.SetLeft(page, 0);
        }

        ~MainWindow()
        {
        }
    }
}
