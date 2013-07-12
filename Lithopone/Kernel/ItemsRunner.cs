using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Lithopone.Memory;
using Lithopone.UIComponents;
using Lithopone.FileReader;
using System.IO;
using System.Drawing;

namespace Lithopone.Kernel
{
    public class ItemsRunner
    {
        public MainWindow mMainWindow;

        public int mVerTaken = 0;

        static int CASUAL_ITEM_GAP = 50;
        static int SEL_GAP = 10;

        public CompSelectionGroup mSelGrp;

        public Dictionary<int, StItem> mItems;
        private TestRunner mTest;
        
        private String mPackagePath;

        private ResExtractor mExtractor;

        public ItemsRunner(MainWindow _mainWindow, 
            Dictionary<int, StItem> items, TestRunner test, String packagePath)
        {
            mMainWindow = _mainWindow;
            mItems = items;
            mTest = test;
            mSelGrp = new CompSelectionGroup();
            mPackagePath = packagePath;
        }

        public void OpenConnection()
        {
            mExtractor = new ResExtractor();
            mExtractor.Begin(mPackagePath);
        }

        public void CloseConnection()
        {
            mExtractor.End();
        }

        public void SetPage(int index, int UISlected = -1)
        {
            mVerTaken = 0;
            mMainWindow.amCanvas.Children.Clear();

            if (mItems.ContainsKey(index))
            {
                if (mTest.GetTestAttribute().GraphicCasual)
                {
                    layGraphicCasual(index);
                }
                else
                {
                    layTextCasual(index);
                }

                if (mTest.GetTestAttribute().GraphicSelection)
                {
                    layGraphicSelection(index);
                }
                else
                {
                    layTextSelection(index, UISlected); 
                }

                layProgressText(index);
            }
        }

        private void layProgressText(int index)
        {
            CompText ct = new CompText();
            ct.SetText("第" + (index + 1).ToString() + "题，共" + mItems.Count + "题");
            mMainWindow.amCanvas.Children.Add(ct);
            Canvas.SetTop(ct, mMainWindow.Height - 180);
            Canvas.SetLeft(ct, mMainWindow.Width / 2 - 65);
        }

        private void layTextCasual(int index)
        {
            CompTextLarge ct = new CompTextLarge();
            ct.SetText(mItems[index].Casual);
            mMainWindow.amCanvas.Children.Add(ct);
            Canvas.SetTop(ct, 0);
            Canvas.SetLeft(ct, (mMainWindow.Width - ct.amLabel.Width) / 2);
            mVerTaken += CompText.HEIGHT + CASUAL_ITEM_GAP;
        }

        private void layTextSelection(int index, int UIselected)
        {
            Dictionary<int, StSelection> selection = mItems[index].Selections;
            List<UserControl> list = new List<UserControl>();

            for (int i = 0; i < selection.Count; i++)
            {
                CompText ct = new CompText();
                ct.SetText(selection[i].Casual);
                mMainWindow.amCanvas.Children.Add(ct);
                Canvas.SetTop(ct, mVerTaken);
                Canvas.SetLeft(ct, (mMainWindow.Width - ct.Width) / 2);
                mVerTaken += CompText.HEIGHT + SEL_GAP;
                list.Add(ct);
            }


            mMainWindow.amCanvas.Height = mVerTaken + CompText.HEIGHT + 100;
            mSelGrp.Set(list, UIselected);
        }

        private void layGraphicCasual(int index)
        {
            GRAPH_SIZE size = mTest.GetTestAttribute().CasualSize;
            CompGraph cg = new CompGraph(size, ref mExtractor);
            
            int resID = mItems[index].ResID;

            cg.SetGraph(resID);
            cg.SetText(mItems[index].Casual);

            mMainWindow.amCanvas.Children.Add(cg);
            Canvas.SetTop(cg, 0);
            Canvas.SetLeft(cg, (mMainWindow.Width - cg.Width) / 2);

            mVerTaken += (int)cg.Height + CASUAL_ITEM_GAP;
        }

        private void layGraphicSelection(int index)
        {
            Dictionary<int, StSelection> selection = mItems[index].Selections;
            List<UserControl> list = new List<UserControl>();

            GRAPH_SIZE size = mTest.GetTestAttribute().SelectionSize;

            for (int i = 0; i < selection.Count; i++)
            {

                CompGraph cg = new CompGraph(size, ref mExtractor);

                int resID = mItems[index].Selections[i].ResID;

                cg.SetGraph(resID);
                cg.SetText(mItems[index].Selections[i].Casual);

                mMainWindow.amCanvas.Children.Add(cg);                

                list.Add(cg);
            }

            placeComGraphSelections(CompGraph.GetVarSize(size) + 10, list);

            mMainWindow.amCanvas.Height = mVerTaken + CompGraph.GetVarSize(size) + 150;
            mSelGrp.Set(list);
        }

        private void placeComGraphSelections(int comWidth, List<UserControl> list)
        {
            int tryInt = 0;
            int placed = 0;

            while (tryInt * comWidth < mMainWindow.amCanvas.Width)
                tryInt++;

            tryInt--;

            int totalWidth = 0;

            if (list.Count < tryInt)
            {
                totalWidth = list.Count * comWidth;
            }
            else
            {
                totalWidth = tryInt * comWidth;
            }

            int xOff = (int)(mMainWindow.amCanvas.Width - totalWidth) / 2;
            int verTaken = mVerTaken;

            for (int i = 0; i < list.Count; i++)
            {
                if (placed == tryInt)
                {
                    verTaken += (int)list[i].Height + SEL_GAP;
                    placed = 0;
                }

                Canvas.SetLeft(list[i], xOff + placed * comWidth);
                Canvas.SetTop(list[i], verTaken);

                placed++;
            }

            mVerTaken = verTaken;
        }
    }
}
