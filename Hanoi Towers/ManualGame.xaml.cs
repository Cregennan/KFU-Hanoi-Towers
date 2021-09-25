using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hanoi_Towers
{
    /// <summary>
    /// Логика взаимодействия для ManualGame.xaml
    /// </summary>
    public partial class ManualGame : Window
    {
        List<Rectangle> Rings = new List<Rectangle>();
        List<int> ColumnCenters = new List<int> { 148, 396, 644 };
        List<int> ColumnsContainment = new List<int> { 0, 0, 0 };


        GameSettings Settings;
        //<DoubleAnimation> Animations = new List<DoubleAnimation>();
        //List<Tuple<int, int>> Movements = new List<Tuple<int, int>>();


        public ManualGame(GameSettings settings)
        {
            InitializeComponent();
            Settings = settings;
            InitField();
        }
        

        public void InitField()
        {
            ColumnsContainment[0] = Settings.ringsCount;
            ColumnsContainment[1] = 0;
            ColumnsContainment[2] = 0;

            column0.Children.Clear();
            column1.Children.Clear();
            column2.Children.Clear();

            int RingWidth = Settings.FirstRingWidth;
            for (int i = 0; i < Settings.ringsCount; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = RingWidth;
                rect.Height = Settings.ringHeight;
                rect.MouseDown += Rect_MouseDown;

                Canvas.SetBottom(rect, Rings.Count * Settings.ringHeight);
                Canvas.SetLeft(rect, 100 - RingWidth / 2);

                rect.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.RingColors[i]);
                rect.Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.RingColors[i]);
                rect.StrokeThickness = 1;

                column0.Children.Add(rect);
                Rings.Add(rect);
                RingWidth -= Settings.ringWidthFall * 2;
            }
            Rings.Clear();
        }

        private void Rect_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Rectangle rect = (Rectangle)sender;
            Canvas owner = (Canvas)rect.Parent;

            if (e.LeftButton == MouseButtonState.Pressed && owner.Children.IndexOf(rect) == owner.Children.Count - 1)
            {
                DragDrop.DoDragDrop((Rectangle)sender, new DataObject(), DragDropEffects.Move);
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
