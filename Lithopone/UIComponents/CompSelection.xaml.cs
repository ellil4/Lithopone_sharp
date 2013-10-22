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
using Lithopone.Memory;

namespace Lithopone.UIComponents
{
    /// <summary>
    /// CompSelection.xaml 的互動邏輯
    /// </summary>
    /// class of selections and highlights only
    public partial class CompSelection : UserControl
    {
        private List<CompText> mSelections;

        public delegate void MouseEnterDele();
        public delegate void MouseLeaveDele();
        public delegate void MouseClickDele();

        MouseEnterDele mEnterFunc = null;
        MouseLeaveDele mLeaveFunc = null;
        MouseClickDele mClickFunc = null;

        public MainWindow mMainWindow;
        public ItemPage mItemPage;
        public int mSizeX;

        public CompSelection(MainWindow mw, ItemPage itemPage, int sizeX)
        {
            InitializeComponent();
            mSelections = new List<CompText>();
            mMainWindow = mw;
            mItemPage = itemPage;
            mSizeX = sizeX;
        }

        private int mSelIdx = -1;

        public void Set(Dictionary<int, StSelection> selection, int selected = -1,
            MouseEnterDele enterFunc = null, MouseLeaveDele leaveFunc = null, MouseClickDele clickFunc = null)
        {
            mSelections.Clear();
            amCanvas.Children.Clear();

            for (int j = 0; j < selection.Count; j++)
            {
                CompText line = new CompText(mMainWindow, mSizeX);
                line.SetText(selection[j].Casual);
                mSelections.Add(line);
            }

            double accumVerticalPos = 0;

            for (int i = 0; i < mSelections.Count; i++)
            {
                mSelections[i].MouseEnter +=
                    new System.Windows.Input.MouseEventHandler(CompSelectionGroup_MouseEnter);
                mSelections[i].MouseLeave +=
                    new System.Windows.Input.MouseEventHandler(CompSelectionGroup_MouseLeave);
                mSelections[i].MouseDown +=
                    new System.Windows.Input.MouseButtonEventHandler(CompSelectionGroup_MouseDown);

                amCanvas.Children.Add(mSelections[i]);
                Canvas.SetTop(mSelections[i], accumVerticalPos);
                Canvas.SetLeft(mSelections[i], 0);
                accumVerticalPos += CompText.HEIGHT + 5;
            }

            mSelIdx = selected;
            if (mSelIdx > -1)
            {
                SetUISelected(mSelIdx);
            }
        }

        public void SetUISelected(int index)
        {
            for (int i = 0; i < mSelections.Count; i++)
            {
                if (index == i)
                {
                    mSelections[i].BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    mSelIdx = i;
                }
                else
                {
                    mSelections[i].BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }
        }

        void CompSelectionGroup_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (mClickFunc == null)
            {
                for (int i = 0; i < mSelections.Count; i++)
                {

                    if (sender.Equals(mSelections[i]))
                    {
                        //mSelections[i].BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        ((CompText)sender).amLabel.Background =
                            new SolidColorBrush(Color.FromRgb(134, 217, 255));
                        mSelIdx = i;
                    }
                    else
                    {
                        //mSelections[i].BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        mSelections[i].amLabel.Background =
                            new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    }
                }
            }
            else
            {
                mClickFunc();
            }

            //test
            //mItemPage.toNextPage();
        }

        void CompSelectionGroup_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (mLeaveFunc == null)
            {
                if (getObjectIdx(sender) != mSelIdx)
                {
                    //((UserControl)sender).BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    ((CompText)sender).amLabel.Background =
                        new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }
            else
            {
                mLeaveFunc();
            }
        }

        void CompSelectionGroup_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (mEnterFunc == null)
            {
                //((UserControl)sender).BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                ((CompText)sender).amLabel.Background = 
                    new SolidColorBrush(Color.FromRgb(134, 217, 255));
            }
            else
            {
                mEnterFunc();
            }
        }

        private int getObjectIdx(object obj)
        {
            int retval = -1;
            int upper = mSelections.Count;

            for (int i = 0; i < upper; i++)
            {
                if (obj.Equals(mSelections[i]))
                {
                    retval = i;
                    break;
                }
            }

            return retval;
        }

        public int GetSelectedIndex()
        {
            return mSelIdx;
        }
    }
}
