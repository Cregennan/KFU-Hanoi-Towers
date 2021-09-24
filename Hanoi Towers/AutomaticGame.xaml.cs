using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для AutomaticGame.xaml
    /// </summary>
    public partial class AutomaticGame : Window
    {
        List<Rectangle> Rings = new List<Rectangle>();
        List<int> ColumnCenters = new List<int> { 148, 396, 644 };
        List<int> ColumnsContainment = new List<int> { 0, 0, 0 };
        GameSettings Settings;
        int RingMoveTime = 5000;


        public AutomaticGame(GameSettings gameSettings)
        {
            InitializeComponent();
            Settings = gameSettings;
            ColumnsContainment[0] = gameSettings.ringsCount;

            int RingWidth = gameSettings.FirstRingWidth;
            for(int i = 0; i<gameSettings.ringsCount; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = RingWidth;
                rect.Height = gameSettings.ringHeight;

                Canvas.SetBottom(rect,Rings.Count * gameSettings.ringHeight);
                Canvas.SetLeft(rect, 100 - RingWidth / 2);




                rect.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(gameSettings.RingColors[i]);
                rect.Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom(gameSettings.RingColors[i]);
                rect.StrokeThickness = 1;

                column0.Children.Add(rect);
                Rings.Add(rect);
                RingWidth -= gameSettings.ringWidthFall * 2;
                
            }
            Point that = ((Rectangle)column0.Children[0]).TransformToAncestor(gameField).Transform(new Point(1,1));
            Debug.WriteLine(that);
            MoveRing(0, 1);
        }

        public void MoveRing(int from, int to)
        {
            Canvas fromColumn = new Canvas();
            Canvas toColumn = new Canvas();

            switch (from)
            {
                case 0:
                    fromColumn = column0;
                    break;
                case 1:
                    fromColumn = column1;
                    break;
                case 2:
                    fromColumn = column2;
                    break;
                default:
                    break;
            }
            switch (to)
            {
                case 0:
                    toColumn = column0;
                    break;
                case 1:
                    toColumn = column1;
                    break;
                case 2:
                    toColumn = column2;
                    break;
                default:
                    break;
            }


            Rectangle rect = (Rectangle)fromColumn.Children[column0.Children.Count - 1];

            int CalculatedOriginLeft = (int)Canvas.GetLeft(rect) + (int)Canvas.GetLeft(fromColumn);
            int CalculatedOriginBottom = (int)Canvas.GetBottom(rect) + (int)Canvas.GetBottom(fromColumn);

            fromColumn.Children.Remove(rect);
            Canvas.SetBottom(rect, ColumnsContainment[to] * Settings.ringHeight);
            ColumnsContainment[from]--;
            ColumnsContainment[to]++;

            int CalculatedDestinationLeft = (int)Canvas.GetLeft(rect) + (int)Canvas.GetLeft(toColumn);
            int CalculatedDestinationBottom = (int)Canvas.GetBottom(rect) + (int)Canvas.GetBottom(toColumn);


            Rectangle tempRect = new Rectangle();

            tempRect.Width = rect.Width;
            tempRect.Height = rect.Height;
            tempRect.Stroke = rect.Stroke;
            tempRect.StrokeThickness = rect.StrokeThickness;
            Canvas.SetLeft(tempRect, CalculatedOriginLeft);
            Canvas.SetBottom(tempRect, CalculatedOriginBottom);
            tempRect.Fill = rect.Fill;
            gameField.Children.Add(tempRect);



            DoubleAnimation animx = new DoubleAnimation();
            animx.From = CalculatedOriginLeft;
            animx.To = CalculatedDestinationLeft;
            animx.Duration = TimeSpan.FromMilliseconds(RingMoveTime);
            animx.Completed += (sender, e) => RingMoveCompleted(sender, e, rect, toColumn, tempRect);

            DoubleAnimation animy = new DoubleAnimation();
            animy.From = CalculatedOriginBottom;
            animy.To = CalculatedDestinationBottom;
            animy.Duration = TimeSpan.FromMilliseconds(RingMoveTime);


            tempRect.BeginAnimation(Canvas.LeftProperty, animx);
            tempRect.BeginAnimation(Canvas.BottomProperty, animy);
            //System.Threading.Thread.Sleep(RingMoveTime);
            //toColumn.Children.Add(rect);
        }
        private void RingMoveCompleted(object sender, EventArgs e, Rectangle rect, Canvas canv, Rectangle that)
        {
            gameField.Children.Remove(that);
            canv.Children.Add(rect);
        }
    }
}
