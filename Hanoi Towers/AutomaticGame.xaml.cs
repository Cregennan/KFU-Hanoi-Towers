using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
        List<int> ColumnsContainment = new List<int> { 0, 0, 0 };
        GameSettings Settings;
        int RingMoveTime = 500;
        List<DoubleAnimation> Animations = new List<DoubleAnimation>();
        List<Tuple<int, int>> Movements = new List<Tuple<int, int>>();
        bool GameFinished = false;

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

                Canvas.SetBottom(rect, Rings.Count * Settings.ringHeight);
                Canvas.SetLeft(rect, 100 - RingWidth / 2);




                rect.Fill = GameSettings.GetColorFromRGBA(GameSettings.Colors.RingColors[i]);
                rect.Stroke = GameSettings.GetColorFromRGBA(GameSettings.Colors.RingColors[i]);
                rect.StrokeThickness = 1;

                column0.Children.Add(rect);
                Rings.Add(rect);
                RingWidth -= Settings.ringWidthFall * 2;
            }
            Rings.Clear();
        }

        public AutomaticGame(GameSettings gameSettings)
        {
            InitializeComponent();
            Settings = gameSettings;
            InitField();
            //MoveRing(0, 1);
        }

        public void MoveRing(int from, int to)
        {
            try
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

                if (fromColumn.Children.Count == 0)
                {
                    return;
                }   

                Rectangle rect = (Rectangle)fromColumn.Children[fromColumn.Children.Count - 1];

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


                Brush originalColor = rect.Fill;
                rect.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#00000000");

                DoubleAnimation animx = new DoubleAnimation();
                animx.From = CalculatedOriginLeft;
                animx.To = CalculatedDestinationLeft;
                animx.Duration = TimeSpan.FromMilliseconds(RingMoveTime);
                animx.Completed += (sender, e) => RingMoveCompleted(sender, e, rect, tempRect, originalColor);

                DoubleAnimation animy = new DoubleAnimation();
                animy.From = CalculatedOriginBottom;
                animy.To = CalculatedDestinationBottom;
                animy.Duration = TimeSpan.FromMilliseconds(RingMoveTime);
                

                tempRect.BeginAnimation(Canvas.LeftProperty, animx);
                tempRect.BeginAnimation(Canvas.BottomProperty, animy);
                toColumn.Children.Add(rect);
            }
            catch(Exception err)
            {
                Debug.WriteLine(err.Message);
            }
        }
        private void RingMoveCompleted(object sender, EventArgs e, Rectangle rect, Rectangle that, Brush fill)
        {
            gameField.Children.Remove(that);
            rect.Fill = fill;
        }
        private void SolutionHanoibns(int n, int from_rod, int to_rod, int aux_rod)
        {
            try
            {
                if (n > 0)
                {
                    SolutionHanoibns(n - 1, from_rod, aux_rod, to_rod);

                    Movements.Add(new Tuple<int, int>(from_rod, to_rod));

                    SolutionHanoibns(n - 1, aux_rod, to_rod, from_rod);
                }
                           
            }catch(Exception err)
            {
                Debug.WriteLine(err.Message);
            }
            
        }
        private void GameStart()
        {
            startBtn.IsEnabled = false;
            clearBtn.IsEnabled = false;
            GameFinished = false;
        }
        private void GameFinish()
        {
            startBtn.IsEnabled = true;
            clearBtn.IsEnabled = true;
            GameFinished = true;
            Movements.Clear();
            MessageBox.Show("Готово");
        }
        private async void startBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GameFinished)
            {
                InitField();
            }
            GameStart();
            SolutionHanoibns(Settings.ringsCount, 0, 1, 2);
            foreach(Tuple<int, int> move in Movements)
            {
                MoveRing(move.Item1, move.Item2);
                await Task.Delay(RingMoveTime);
            }
            GameFinish();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            RingMoveTime = (int)(speedSlider.Maximum + speedSlider.Minimum) - (int)speedSlider.Value;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            InitField();
        }
    }
}
