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
        private StTestXmlHeader mTestAttr;
        private String mSrcPath;

        public ItemsRunner mItemRunner;

        public List<StAnswer> mAnswers;
        public MainWindow mMainWindow;

        public CompButton mOkBtn, mPassBtn, mPrevBtn;

        //public static String BUFFER_FOLDER = AppDomain.CurrentDomain.BaseDirectory + "LithoponeBuf\\";
        //public static String XMLFILE_PATH = BUFFER_FOLDER + "test.xml";

        private FEITTimer mTimer;

        private int mCurTill = 0;

        public static int WINDOW_HEIGHT = 600;
        public static int WINDOW_WIDTH = 800;

        public TestRunner(String srcPath, MainWindow mw)
        {
            //folderWork();

            mMainWindow = mw;
            mSrcPath = srcPath;

            mAnswers = new List<StAnswer>();

            mOkBtn = new CompButton("下一题");
            mOkBtn.SetBehavior(onOK);

            mPrevBtn = new CompButton("上一题");
            mPrevBtn.SetBehavior(onPrev);

            /*mPassBtn = new CompButton("放弃");
            mPassBtn.SetBehavior(onPass);*/

            mTimer = new FEITTimer();

            mMainWindow.Height = WINDOW_HEIGHT;
            mMainWindow.Width = WINDOW_WIDTH;

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
            mItemRunner = new ItemsRunner(mMainWindow, items, this, mSrcPath);
            
            if(mTestAttr.GraphicCasual || mTestAttr.GraphicSelection)
                mItemRunner.OpenConnection();
        }

        public void SetInstructionPage()
        {
            mMainWindow.amCanvas.Children.Clear();

            mMainWindow.amCanvas.Width = MainWindow.CANVAS_SIZE_W;
            mMainWindow.amCanvas.Height = MainWindow.CANVAS_SIZE_H;

            CompInstructionPage page = new CompInstructionPage();
            page.amTitleLabel.Content = mTestAttr.Name;
            page.amInstructionText.Text = mTestAttr.Description;
            mMainWindow.amCanvas.Children.Add(page);

            CompButton btn = new CompButton("开始");
            mMainWindow.amCanvas.Children.Add(btn);
            Canvas.SetLeft(btn, (mMainWindow.amCanvas.Width - CompButton.WIDTH) / 2);
            Canvas.SetBottom(btn, CompButton.HEIGHT + 20);
            btn.SetBehavior(startTest);
        }

        private void startTest()
        {
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

            mItemRunner.SetPage(index, uiSelected);
            mMainWindow.amCanvas.Children.Add(mOkBtn);
            mMainWindow.amCanvas.Children.Add(mPrevBtn);
            //mMainWindow.amCanvas.Children.Add(mPassBtn);

            Canvas.SetLeft(mPrevBtn, (mMainWindow.Width/ 2) - CompButton.WIDTH - 5);
            Canvas.SetTop(mPrevBtn, mMainWindow.amCanvas.Height - CompButton.HEIGHT - 10);
            Canvas.SetLeft(mOkBtn, (mMainWindow.Width / 2) + 5);
            Canvas.SetTop(mOkBtn, mMainWindow.amCanvas.Height - CompButton.HEIGHT - 10);
            //Canvas.SetLeft(mPassBtn, ((mMainWindow.Width - CompButton.WIDTH * 2 - 10)) / 2 + CompButton.WIDTH + 10);
            //Canvas.SetTop(mPassBtn, mMainWindow.amCanvas.Height - CompButton.HEIGHT - 10)

            //set timer
            mTimer.Start();
        }

        public StTestXmlHeader GetTestAttribute()
        {
            return mTestAttr;
        }

        private void save()
        {
            if (mCurTill < mAnswers.Count)
            {
                mAnswers[mCurTill].Selected = mItemRunner.mSelGrp.GetSelectedIndex();
                mAnswers[mCurTill].RT = mTimer.GetElapsedTime();
            }
            else
            {
                mAnswers.Add(new StAnswer(mTimer.GetElapsedTime(),
                    mItemRunner.mSelGrp.GetSelectedIndex()));
            }
        }

        private void onOK()
        {
            if (mItemRunner.mSelGrp.GetSelectedIndex() != -1)
            {
                save();

                mCurTill++;

                if (mCurTill < mTestAttr.ItemCount)
                {
                    SetPage(mCurTill);
                }
                else
                {
                    TestEnd();
                }
            }
            else
            {
                MessageBox.Show("请做出选择");
            }
        }

        private void onPrev()
        {
            if (mItemRunner.mSelGrp.GetSelectedIndex() != -1)
            {
                save();

                if(mCurTill > 0)
                    mCurTill--;

                if (mCurTill > -1)
                {
                    SetPage(mCurTill);
                }
            }
            else
            {
                MessageBox.Show("请做出选择");
            }
        }

        private void onPass()
        {
            save();

            mCurTill++;

            if (mCurTill < mTestAttr.ItemCount)
            {
                SetPage(mCurTill);
            }
            else
            {
                TestEnd();
            }
        }

        public void TestEnd()
        {
            Console.WriteLine("TestEnd()");

            if(mTestAttr.GraphicCasual || mTestAttr.GraphicSelection)
                mItemRunner.CloseConnection();

            mMainWindow.OnTestClose();
        }

    }
}
