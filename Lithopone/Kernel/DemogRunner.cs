using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Lithopone.Memory;
using Lithopone.UIComponents;


namespace Lithopone.Kernel
{

    class DemogRunner
    {
        MainWindow mMW;

        private List<LINETYPE> mLinetypes;
        private List<String> mVarNames;
        public static String DEMOG_FILENAME = "demog.xml";

        public DemogRunner(MainWindow mw)
        {
            mMW = mw;
            mLinetypes = new List<LINETYPE>();
            mVarNames = new List<String>();
        }

        private void addTitle()
        {
            AddTextField("请在下面填写个人信息", "", 0, "TITLE");
        }

        public void GenFromFile(String filename)
        {
            Lithopone.FileReader.DemogXmlReader rd = new FileReader.DemogXmlReader();
            Dictionary<int, StDemogLine> lines = rd.GetDemogItems(filename);

            addTitle();

            for (int i = 0; i < lines.Count; i++)
            {
                if(lines.ContainsKey(i))
                {
                    switch (lines[i].Type)
                    {
                        case LINETYPE.TXTFIELD:
                            AddTextField(lines[i].PreText, lines[i].PostText, 
                                lines[i].Width, lines[i].VarName);
                            break;
                        case LINETYPE.RADIO:
                            AddRadioButtonLine(lines[i].PreText, lines[i].PostText, 
                                lines[i].SubItems, lines[i].VarName);
                            if (lines[i].ForcedChoice)
                            {
                                ((RadioButton)(getLinePane(mMW.amCanvas.Children.Count - 1).Children[1])).IsChecked = true;
                            }
                            break;
                        case LINETYPE.COMBO:
                            AddComboLine(lines[i].PreText, lines[i].PostText,
                                lines[i].SubItems, lines[i].VarName);
                            if (lines[i].ForcedChoice)
                            {
                                ((ComboBox)(getLinePane(mMW.amCanvas.Children.Count - 1).Children[1])).SelectedIndex = 0;
                            }
                            break;
                    }
                }
            }

            CompButton btnSubmmit = new CompButton("提交");

            int btnWidth = 90;
            int btnHeight = 25;
            int btnUpGap = 31;
            int btnDownGap = 10;
            
            btnSubmmit.amButton.Width = btnWidth;
            btnSubmmit.Width = btnWidth;
            btnSubmmit.Height = btnHeight;
            btnSubmmit.amButton.Height = btnHeight;
            btnSubmmit.amButton.FontSize = 13;

            btnSubmmit.SetBehavior(Submmit);
            mMW.amCanvas.Children.Add(btnSubmmit);

            Canvas.SetTop(btnSubmmit, 
                (mMW.amCanvas.Children.Count - 1) * 
                (KernelLib.LINE_HEIGHT + KernelLib.LINE_GAP) + btnUpGap);

            Canvas.SetLeft(btnSubmmit,
                (MainWindow.CANVAS_SIZE_W - CompButton.WIDTH) / 2);

            int neededHeight = mMW.amCanvas.Children.Count * 
                (KernelLib.LINE_HEIGHT + KernelLib.LINE_GAP) + btnHeight + btnUpGap + btnDownGap;

            if (neededHeight > MainWindow.CANVAS_SIZE_H)
            {
                mMW.amCanvas.Height = neededHeight;
            }
            else
            {
                mMW.Height = neededHeight + btnDownGap;
            }
        }

        public void Submmit()
        {
            mMW.OnDemogClose();
        }

        private StackPanel getLinePane(int index)
        {
            return (StackPanel)mMW.amCanvas.Children[index];
        }

        private void addLabel2StackPane(ref StackPanel pane, String text)
        {
            Label label = new Label();
            label.Content = text;
            label.Width = text.Length * KernelLib.CHARA_WIDTH + 20;
            label.Height = KernelLib.LINE_HEIGHT;
            label.FontSize = 12;

            pane.Children.Add(label);
        }

        private void addCombobox2StackPane(ref StackPanel pane, Dictionary<int, String> selections)
        {
            int upper = selections.Count;
            int longest = 0;

            ComboBox cb = new ComboBox();

            for (int i = 0; i < upper; i++)
            {
                if (selections.ContainsKey(i))
                {
                    if (selections[i].Length > longest)
                        longest = selections[i].Length;

                    cb.Items.Add(selections[i]);
                }
            }

            cb.Height = KernelLib.LINE_HEIGHT;
            cb.Width = KernelLib.CHARA_WIDTH * longest;
            cb.FontSize = 12;

            pane.Children.Add(cb);
        }

        private void addTextField2StackPane(ref StackPanel pane, int width)
        {
            TextBox tb = new TextBox();
            tb.Width = width;
            tb.Height = KernelLib.LINE_HEIGHT;
            tb.FontSize = 12;

            pane.Children.Add(tb);
        }

        private void addRadioButtonGroup2StackPane(ref StackPanel sp, Dictionary<int, String> selections)
        {
            int upper = selections.Count;
            for (int i = 0; i < upper; i++)
            {
                if (selections.ContainsKey(i))
                {
                    RadioButton rb = new RadioButton();
                    rb.Content = selections[i] + " ";
                    rb.Height = 16;
                    sp.Children.Add(rb);
                }
            }
        }

        private void appendLinePan2Canvas(ref Canvas cvs, ref StackPanel sp)
        {
            cvs.Children.Add(sp);
            Canvas.SetLeft(sp, 0);
            Canvas.SetTop(sp, (mMW.amCanvas.Children.Count - 1) *
                (KernelLib.LINE_HEIGHT + KernelLib.LINE_GAP));
        }

        public void AddComboLine(String preText, String postText, 
            Dictionary<int, String> selections, String name)
        {
            StackPanel sp = KernelLib.GenLinePane();

            addLabel2StackPane(ref sp, preText);

            addCombobox2StackPane(ref sp, selections);

            addLabel2StackPane(ref sp, postText);


            int elementCount = mMW.amCanvas.Children.Count;

            appendLinePan2Canvas(ref mMW.amCanvas, ref sp);

            LINETYPE type = LINETYPE.COMBO;
            mLinetypes.Add(type);
            mVarNames.Add(name);
        }

        public void AddTextField(String preText, String postText, int width, String name)
        {
            StackPanel sp = KernelLib.GenLinePane();

            addLabel2StackPane(ref sp, preText);

            addTextField2StackPane(ref sp, width);

            addLabel2StackPane(ref sp, postText);

            appendLinePan2Canvas(ref mMW.amCanvas, ref sp);

            LINETYPE type = LINETYPE.TXTFIELD;
            mLinetypes.Add(type);
            mVarNames.Add(name);
        }

        public void AddRadioButtonLine(String preText, String postText, 
            Dictionary<int, String> contents, String name)
        {
            StackPanel sp = KernelLib.GenLinePane();

            addLabel2StackPane(ref sp, preText);

            addRadioButtonGroup2StackPane(ref sp, contents);

            addLabel2StackPane(ref sp, postText);

            appendLinePan2Canvas(ref mMW.amCanvas, ref sp);

            LINETYPE type = LINETYPE.RADIO;
            mLinetypes.Add(type);
            mVarNames.Add(name);
        }

        private StGeneralInfo getLineInfo(int index)
        {
            StGeneralInfo retval = null;
            StackPanel sp = ((StackPanel)(mMW.amCanvas.Children[index]));
            String value = "";

            switch(mLinetypes[index])
            {
                case LINETYPE.COMBO:
                    ComboBox cb = (ComboBox)sp.Children[1];
                    value = cb.SelectedIndex.ToString();
                    break;
                case LINETYPE.TXTFIELD:
                    TextBox tb = (TextBox)sp.Children[1];
                    value = tb.Text;
                    break;
                case LINETYPE.RADIO:
                    for (int i = 1; i < sp.Children.Count - 1; i++)
                    {
                        if (((RadioButton)sp.Children[i]).IsChecked == true)
                        {
                            value = (i - 1).ToString();
                            break;
                        }
                    }
                    break;
            }

            retval = new StGeneralInfo(value, mVarNames[index]);
            return retval;
        }

        public Dictionary<String, String> GetResult()
        {
            Dictionary<String, String> retval = new Dictionary<String, String>();

            int upper = mMW.amCanvas.Children.Count;

            for (int i = 1; i < upper - 1; i++)//title and button exclueded
            {
                StGeneralInfo info = getLineInfo(i);
                if (info != null)
                {
                    retval.Add(info.Name, info.Value);
                }
            }

            return retval;
        }
    }
}
