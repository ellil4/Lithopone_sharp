using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lithopone.Memory;
using Lithopone.FileReader;
using System.IO;
using System.Windows.Controls;
using Lithopone.UIComponents;
using System.Windows.Forms;

namespace Lithopone.Kernel
{
    public class TestRunner
    {
        public StTestXmlHeader mTestAttr;
        private String mSrcPath;

        public ItemPage mItemPage;

        public List<StAnswer> mAnswers;
        public MainWindow mMainWindow;

        //public static String BUFFER_FOLDER = AppDomain.CurrentDomain.BaseDirectory + "LithoponeBuf\\";
        //public static String XMLFILE_PATH = BUFFER_FOLDER + "test.xml";

        private FEITTimer mTimer;

        public int mCurTill = 0;

        public TestRunner(String srcPath, MainWindow mw)
        {
            //folderWork();

            mMainWindow = mw;
            mSrcPath = srcPath;

            mAnswers = new List<StAnswer>();

            mTimer = new FEITTimer();

            startItemFileSystem();
        }

        private void startItemFileSystem()
        {
            //test xml
            TestXmlReader xmlReader = new TestXmlReader();
            xmlReader.Begin(Lib.TestFileName);
            mTestAttr = xmlReader.GetHeader();
            Dictionary<int, StItem> items = xmlReader.GetItems();
            xmlReader.Finish();
            //itemsRunner
            mItemPage = new ItemPage(items, this,
                mMainWindow.getXScreenTaken(), mMainWindow.getYScreenTaken());
            
            /*if(mTestAttr.GraphicCasual || mTestAttr.GraphicSelection)
                mItemRunner.OpenConnection();*/
        }

        public void SetInstructionPage()
        {
            mMainWindow.amCanvas.Children.Clear();

            CompInstructionPage page = new CompInstructionPage(this,
                mMainWindow.getXScreenTaken(), mMainWindow.getYScreenTaken());

            page.amTitleLabel.Content = mTestAttr.Name;
            page.amInstructionText.Text = mTestAttr.Instruction;
            mMainWindow.amCanvas.Children.Add(page);
            mMainWindow.centralize(page,
                mMainWindow.getXScreenTaken(), mMainWindow.getYScreenTaken());
        }

        public void StartTest()
        {
            //mMainWindow.Width = 590;
            //mMainWindow.amScrollViewer.Width = 630;
            //mMainWindow.Height = 430;
            //mMainWindow.amScrollViewer.Height = 430;

            mMainWindow.amCanvas.Children.Clear();
            mMainWindow.amCanvas.Children.Add(mItemPage);
            //centralize
            mMainWindow.centralize(mItemPage,
                mMainWindow.getXScreenTaken(), mMainWindow.getYScreenTaken());

            SetPage(0);
        }

        public void SetPage(int index)
        {
            mTimer.Stop();
            mTimer.Reset();
            int uiSelected = -1;
            //load result
            if (mAnswers.Count > index)
            {
                uiSelected = mAnswers[index].Selected;
            }

            mItemPage.SetPage(index, uiSelected);

            //set timer
            mTimer.Start();
        }

        public StTestXmlHeader GetTestAttribute()
        {
            return mTestAttr;
        }

        public void Save()
        {
            if (mCurTill < mAnswers.Count)
            {
                mAnswers[mCurTill].Selected = mItemPage.amCompSelection.GetSelectedIndex();
                mAnswers[mCurTill].RT = mTimer.GetElapsedTime();
            }
            else
            {
                mAnswers.Add(new StAnswer(mTimer.GetElapsedTime(),
                    mItemPage.amCompSelection.GetSelectedIndex()));
            }
        }

        public void TestEnd()
        {
            Console.WriteLine("TestEnd()");

            /*if(mTestAttr.GraphicCasual || mTestAttr.GraphicSelection)
                mItemRunner.CloseConnection();*/

            mMainWindow.OnTestClose();
        }

    }
}
