using SourceChord.FluentWPF;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Hanoi_Towers
{
    /// <summary>
    /// Логика взаимодействия для ManualGame.xaml
    /// </summary>
    public partial class ManualGame
    {
        /// <summary>
        /// Глобальные игровые настройки
        /// </summary>
        GameSettings Settings;

        int Steps;

        public ManualGame(GameSettings settings)
        {
            InitializeComponent();
            Settings = settings;
            InitField();
        }
        
        /// <summary>
        /// Инициализация, очистка игрового поля - добавление колец, очистка старых
        /// </summary>
        public void InitField()
        {
            Steps = 0;

            column0.Children.Clear();
            column1.Children.Clear();
            column2.Children.Clear();

            int RingWidth = GameSettings.FirstRingWidth;
            for (int i = 0; i < Settings.ringsCount; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = RingWidth;
                rect.Height = GameSettings.ringHeight;
                rect.MouseMove += Rect_MouseMove;

                Canvas.SetBottom(rect, column0.Children.Count * GameSettings.ringHeight);
                Canvas.SetLeft(rect, 100 - RingWidth / 2);

                rect.Fill = GameSettings.GetColorFromRGBA(GameSettings.Colors.RingColors[i]);
                rect.Stroke = GameSettings.GetColorFromRGBA(GameSettings.Colors.RingColors[i]);
                rect.StrokeThickness = 1;

                column0.Children.Add(rect);
                RingWidth -= Settings.ringWidthFall * 2;
            }
        }
        /// <summary>
        /// Событие при нажатии на кольцо - подготовка к перетаскиванию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rect_MouseMove(object sender, MouseEventArgs e)
        {

            Rectangle rect = (Rectangle)sender;
            Canvas owner = (Canvas)rect.Parent;

            if (e.LeftButton == MouseButtonState.Pressed && owner.Children.IndexOf(rect) == owner.Children.Count - 1)
            {
                DragDrop.DoDragDrop((Rectangle)sender, new DataObject(DataFormats.Serializable, (Rectangle)sender), DragDropEffects.Move);
            }
        }

        private void MoveRing(Rectangle ring, Canvas dest)
        {
            Canvas origin = (Canvas)ring.Parent;
            
            if (dest == origin)
            {
                return;
            }
            if (dest.Children.Count != 0 && ((Rectangle)dest.Children[dest.Children.Count - 1]).ActualWidth < ring.ActualWidth)
            {
                return;
            }

            origin.Children.Remove(ring);
            int CalculatedLocalBottom = dest.Children.Count * GameSettings.ringHeight;
            int CalculatedDestBottom = (int)(dest.Children.Count * GameSettings.ringHeight + Canvas.GetBottom(dest));
            int CalculatedDestLeft = (int)(Canvas.GetLeft(dest) + Canvas.GetLeft(ring));
            int CalculatedOriginBottom = (int)(Canvas.GetBottom(origin) + Canvas.GetBottom(ring));
            int CalculatedOriginLeft = (int)(Canvas.GetLeft(origin) + Canvas.GetLeft(ring));

            AnimateRingMovement(ring, dest, new Point(CalculatedOriginLeft, CalculatedOriginBottom), new Point(CalculatedDestLeft, CalculatedLocalBottom));
            Canvas.SetBottom(ring, CalculatedLocalBottom);
        }
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            InitField();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ring">Настоящее кольцо, которое должно было удалено из исходного столбца</param>
        /// <param name="dest">Столбец назначение</param>
        /// <param name="origin">Point, где X,Y - исходные отступы колец, относительно игрового поля  </param>
        /// <param name="destination">Point, где X,Y - будущие отступы колец, относительно игрового поля  </param>
        private void AnimateRingMovement(Rectangle ring, Canvas dest, Point origin, Point destination)
        {
            Rectangle temp = GameSettings.GetCopy(ring);

            DoubleAnimation animx = new DoubleAnimation();
            animx.From = origin.X;
            animx.To = destination.X;
            animx.Duration = TimeSpan.FromMilliseconds(50);
            animx.Completed += (sender, e) => RingMoveCompleted1(sender, e, ring, dest, temp);

            DoubleAnimation animy = new DoubleAnimation();
            animy.From = origin.Y;
            animy.To = destination.Y;
            animy.Duration = TimeSpan.FromMilliseconds(50);

            gameField.Children.Add(temp);

            temp.BeginAnimation(Canvas.LeftProperty, animx);
            temp.BeginAnimation(Canvas.BottomProperty, animy);
        }

        /// <summary>
        /// Событие при завершении анимации перемещения кольца между столбцами. Добавляет кольцо на столбец - назначение, удаляет временное кольцо анимации.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="rect">Настоящее кольцо, которое должно быть добавлено на столбец назначение</param>
        /// <param name="dest">Столбец назначения</param>
        /// <param name="temp">Временное кольцо анимации</param>
        private void RingMoveCompleted1(object sender, EventArgs e, Rectangle rect, Canvas dest, Rectangle temp)
        {
            Steps++;
            dest.Children.Add(rect);
            gameField.Children.Remove(temp);
            if (column1.Children.Count < Settings.ringsCount && column2.Children.Count < Settings.ringsCount)
            {
                return;
            }
            Canvas col = column1;
            if (column1.Children.Count == Settings.ringsCount) col = column1;
            if (column2.Children.Count == Settings.ringsCount) col = column2;

            for (int i = 0; i < col.Children.Count - 1; i++)
            {
                if (((Rectangle)col.Children[i]).ActualWidth < ((Rectangle)col.Children[i + 1]).ActualWidth)
                {
                    return;
                }
            }

            String message = GameSettings.MessageBoxDoneMessage + Environment.NewLine;
            message += String.Format("Поздравляем! Вы успешно решили головоломку за {0} шагов. {1}", Steps, Environment.NewLine);
            message += String.Format("Идеальное решение - {0} шагов {1}", Math.Pow(2, Settings.ringsCount) - 1, Environment.NewLine);
            if (Steps == Math.Pow(2, Settings.ringsCount) - 1)
            {
                message += String.Format("Ваше решение совпадает с идеальным{0}", Environment.NewLine);
            }
            message += String.Format("Хотите начать заново?{0}", Environment.NewLine);

            MessageBoxResult r = AcrylicMessageBox.Show(this, message, GameSettings.MessageBoxDoneCaption, MessageBoxButton.YesNo);

            switch (r)
            {
                case MessageBoxResult.Yes:
                    InitField();
                    break;
                case MessageBoxResult.No:
                    //ddd
                    break;
                default:

                    break;
            }
        }

        /// <summary>
        /// Событие завершения перетаскивания - обработка перетаскивания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Column_Drop(object sender, DragEventArgs e)
        {
            Rectangle rect = (Rectangle)e.Data.GetData(DataFormats.Serializable);
            ((Canvas)sender).Background = GameSettings.GetColorFromRGBA(GameSettings.Colors.Transparent);
            MoveRing(rect, (Canvas)sender);

        }
        /// <summary>
        /// Событие при входе перетаскивания в пределы Канвас - столбца. Показывает индикатор возможности переноса кольца на столбик - красный цвет фона (невозможно), зеленый (возможно)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Column_DragEnter(object sender, DragEventArgs e)
        {
            Rectangle rect = (Rectangle)e.Data.GetData(DataFormats.Serializable);
            Canvas owner = (Canvas)sender;
            if ((Canvas)rect.Parent == owner)
            {
                return;
            }
            if (owner.Children.Count != 0 && ((Rectangle)owner.Children[owner.Children.Count - 1]).ActualWidth < rect.ActualWidth)
            {
                owner.Background = GameSettings.GetColorFromRGBA(GameSettings.Colors.ErrorColor);
                return;
            }
            owner.Background = GameSettings.GetColorFromRGBA(GameSettings.Colors.SuccessColor);
        }

        /// <summary>
        /// Событие при покидании мыши Канвас-столбца. Очищает цвет индикатора перетаскивания.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Column_DragLeave(object sender, DragEventArgs e)
        {
            ((Canvas)sender).Background = GameSettings.GetColorFromRGBA(GameSettings.Colors.Transparent);
        }


        /// <summary>
        /// Зарезерверованный обработчик события. В будущем - добавление возможности неявно перетаскивать кольца, просто перетаскиванием мышки от столбика
        /// </summary>
        /// <param name="sender">Один из трёх Canvas, которые хранят в себе кольца</param>
        /// <param name="e"></param>
        private void Column_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas owner = (Canvas)sender;
            if (owner.Children.Count == 0)
            {
                return;
            }
            Rect_MouseMove(owner.Children[owner.Children.Count - 1], e);
        }
    }
}
