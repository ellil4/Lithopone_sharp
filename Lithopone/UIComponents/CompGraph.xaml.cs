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
using System.Drawing;
using System.IO;
using Lithopone.FileReader;
using Lithopone.Memory;

namespace Lithopone.UIComponents
{
    /// <summary>
    /// CompGraph.xaml 的互動邏輯
    /// </summary>
    public partial class CompGraph : UserControl
    {
        public static int TEXTFIELD_HEIGHT = 38;
        
        public static int SIZE_SMALL = 150;
        public static int SIZE_MEDIUM = 300;
        public static int SIZE_LARGE = 600;
        public static int EDGE = 4;

        ResExtractor mExtractor;

        public static int GetVarSize(GRAPH_SIZE size)
        {
            int VAR_SIZE = 0;

            switch (size)
            {
                case GRAPH_SIZE.SMALL:
                    VAR_SIZE = SIZE_SMALL;
                    break;
                case GRAPH_SIZE.MEDIUM:
                    VAR_SIZE = SIZE_MEDIUM;
                    break;
                case GRAPH_SIZE.LARGE:
                    VAR_SIZE = SIZE_LARGE;
                    break;
            }

            return VAR_SIZE;
        }

        public CompGraph(GRAPH_SIZE size, ref ResExtractor extractor)
        {
            InitializeComponent();

            mExtractor = extractor;

            int VAR_SIZE = GetVarSize(size);

            amImage.Width = VAR_SIZE;
            amImage.Height = VAR_SIZE;
            amLabel.Width = VAR_SIZE;
            amLabel.Height = TEXTFIELD_HEIGHT;

            this.Height = VAR_SIZE + TEXTFIELD_HEIGHT + EDGE;
            this.Width = VAR_SIZE + EDGE;

            Canvas.SetLeft(amImage, 0);
            Canvas.SetTop(amImage, 0);
            Canvas.SetLeft(amLabel, 0);
            Canvas.SetTop(amLabel, VAR_SIZE);

            BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
        }

        public void SetText(String text)
        {
            amLabel.Content = text;
        }

        public void SetGraph(int index)
        {
            //BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open));
            //FileInfo fi = new FileInfo(path);
            byte[] data = mExtractor.ExtractRes2Mem(index);//br.ReadBytes((int)fi.Length);
            //br.Close();

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(data);
            bi.EndInit();

            amImage.Source = bi;
            
        }
    }
}
