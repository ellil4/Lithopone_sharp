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

namespace Lithopone.UIComponents
{
    /// <summary>
    /// CompTextCasual.xaml 的互動邏輯
    /// </summary>
    public partial class CompText : UserControl
    {
        public static int HEIGHT = 38;

        public CompText()
        {
            InitializeComponent();
            BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        public void SetText(String text)
        {
            amLabel.Content = text;
            amLabel.Width = text.Length * 15 + 22;
            this.Width = text.Length * 15 + 22;
        }
    }
}
