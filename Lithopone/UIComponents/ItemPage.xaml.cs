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
using Lithopone.Kernel;
using Lithopone.Memory;
using Lithopone.FileReader;

namespace Lithopone.UIComponents
{
    /// <summary>
    /// ItemPage.xaml 的互動邏輯
    /// </summary>
    /// class of the whole test page(selection, buttons, pages, stem)
    public partial class ItemPage : UserControl
    {
        public Dictionary<int, StItem> mItems;
        private TestRunner mTest;
        //private String mPackagePath;
        //private ResExtractor mExtractor;

        public CompSelection amCompSelection;

        public ItemPage(Dictionary<int, StItem> items, TestRunner runner, int sizeX, int sizeY)
        {
            InitializeComponent();
            mItems = items;
            mTest = runner;
            amCompSelection = new CompSelection(mTest.mMainWindow, this, sizeX);
            amCanvas.Children.Add(amCompSelection);
            Canvas.SetTop(amCompSelection, 100);
            Canvas.SetLeft(amCompSelection, 0);

            //sizing & centalize
            this.Width = sizeX;
            this.Height = sizeY;
            amCanvas.Width = sizeX;
            amCanvas.Height = sizeY;

            amStem.Width = sizeX;
            amCompSelection.Width = sizeX;
            amCompSelection.Height = sizeY - 25 - 27 - 47 - 44 - 35 - 20;
            amLabelPageNum.Width = sizeX;
            Canvas.SetLeft(amLabelPageNum, 0);
            Canvas.SetTop(amLabelPageNum, sizeY - 45);

            Canvas.SetLeft(amBtnPrev, (sizeX - (86 * 2 + 10)) / 2);
            Canvas.SetTop(amBtnPrev, sizeY - 35 - 20 - 44);
            Canvas.SetLeft(amBtnNext, (sizeX - (86 * 2 + 10)) / 2 + 10 + 86);
            Canvas.SetTop(amBtnNext, sizeY - 35 - 20 - 44);
        }

        public void SetPage(int index, int uiSelected = -1)
        {
            StItem item = mItems[index];
            amCompSelection.Set(item.Selections);
            amCompSelection.SetUISelected(uiSelected);

            amStem.Text = item.Casual;
            amLabelPageNum.Text = "第" + (mTest.mCurTill + 1) + "页, 共" + mItems.Count + "页";
        }

        public void toNextPage()
        {
            if (amCompSelection.GetSelectedIndex() != -1)
            {
                mTest.Save();

                mTest.mCurTill++;

                if (mTest.mCurTill < mTest.mTestAttr.ItemCount)
                {
                    mTest.SetPage(mTest.mCurTill);
                }
                else
                {
                    mTest.TestEnd();
                }
            }
            else
            {
                MessageBox.Show("请做出选择");
            }
        }

        private void amBtnNext_Click(object sender, RoutedEventArgs e)
        {
            toNextPage();
        }

        private void amBtnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (amCompSelection.GetSelectedIndex() != -1)
            {
                mTest.Save();
            }

            if (mTest.mCurTill > 0)
                mTest.mCurTill--;

            if (mTest.mCurTill > -1)
            {
                mTest.SetPage(mTest.mCurTill);
            }
        }
    }
}
