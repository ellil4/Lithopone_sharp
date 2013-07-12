using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lithopone.UIComponents
{
    public class CompSelectionGroup
    {
        public List<UserControl> mSelections;
        public delegate void MouseEnter();
        public delegate void MouseLeave();
        public delegate void MouseClick();

        MouseEnter mEnterFunc;
        MouseLeave mLeaveFunc;
        MouseClick mClickFunc;

        private int mSelIdx = -1;

        public void Set(List<UserControl> selections, int selected = -1, 
            MouseEnter enterFunc = null, MouseLeave leaveFunc = null, MouseClick clickFunc = null)
        {
            mSelections = selections;
            mEnterFunc = enterFunc;
            mLeaveFunc = leaveFunc;
            mClickFunc = clickFunc;
            
            for (int i = 0; i < mSelections.Count; i++)
            {
                mSelections[i].MouseEnter +=
                    new System.Windows.Input.MouseEventHandler(CompSelectionGroup_MouseEnter);
                mSelections[i].MouseLeave += 
                    new System.Windows.Input.MouseEventHandler(CompSelectionGroup_MouseLeave);
                mSelections[i].MouseDown += 
                    new System.Windows.Input.MouseButtonEventHandler(CompSelectionGroup_MouseDown);
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
                        mSelections[i].BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                        mSelIdx = i;
                    }
                    else
                    {
                        mSelections[i].BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    }
                }
            }
            else
            {
                mClickFunc();
            }
        }

        void CompSelectionGroup_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (mLeaveFunc == null)
            {
                if (getObjectIdx(sender) != mSelIdx)
                {
                    ((UserControl)sender).BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
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
                ((UserControl)sender).BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
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
