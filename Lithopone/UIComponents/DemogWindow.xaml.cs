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
using System.Windows.Shapes;

namespace Lithopone.UIComponents
{
    /// <summary>
    /// DemogWindow.xaml 的互動邏輯
    /// </summary>
    public partial class DemogWindow : Window
    {
        public MainWindow mMW;
        public DemogWindow(MainWindow mw)
        {
            InitializeComponent();
            mMW = mw;
            this.WindowStyle = System.Windows.WindowStyle.None;
        }

        public void OnDemogClose()
        {
            mMW.mDemogInfo = mMW.mDemogRunner.GetResult();
            if (mMW.mDemogInfo != null)
            {
                mMW.OnDemogClose();
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mMW.mDemogInfo == null)
            {
                System.Environment.Exit(0);
            }
        }
    }
}
