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
using Lithopone.Memory;
using Lithopone.FileReader;

namespace Lithopone
{
    /// <summary>
    /// MainMenu.xaml 的互動邏輯
    /// </summary>
    public partial class MainMenu : Window
    {
        ConfigCollection mConfigs;

        public MainMenu()
        {
            InitializeComponent();
            mConfigs = new ConfigCollection();
        }

        private void amRectIntro_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void amRectTest_MouseUp(object sender, MouseButtonEventArgs e)
        {
            new MainWindow(mConfigs.mDemogLines, mConfigs.mTestInfo).Show();
        }

        private void amRectResult_MouseUp(object sender, MouseButtonEventArgs e)
        {
            new DocManageWnd(mConfigs).ShowDialog();
        }

        private void amRectIntro_MouseEnter(object sender, MouseEventArgs e)
        {
            amRectIntro.Stroke = new SolidColorBrush(Color.FromRgb(150, 150, 200)); 
        }

        private void amRectIntro_MouseLeave(object sender, MouseEventArgs e)
        {
            amRectIntro.Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255));   
        }

        private void amRectTest_MouseEnter(object sender, MouseEventArgs e)
        {
            amRectTest.Stroke = new SolidColorBrush(Color.FromRgb(150, 150, 200)); 
        }

        private void amRectTest_MouseLeave(object sender, MouseEventArgs e)
        {
            amRectTest.Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255)); 
        }

        private void amRectResult_MouseEnter(object sender, MouseEventArgs e)
        {
            amRectResult.Stroke = new SolidColorBrush(Color.FromRgb(150, 150, 200)); 
        }

        private void amRectResult_MouseLeave(object sender, MouseEventArgs e)
        {
            amRectResult.Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255)); 
        }
    }
}
