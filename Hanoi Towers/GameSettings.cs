using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Hanoi_Towers
{
    public class GameSettings
    {
        /// <summary>
        /// Хранит параметры и определения цветов
        /// </summary>
        public class Colors
        {
            public static readonly List<String> RingColors = new List<String> { "#FFD32F2F", "#FFC2185B", "#FF7B1FA2", "#FF512DA8", "#FF303F9F", "#FF1976D2", "#FF0288D1", "#FF0097A7", "#FF00796B", "#FF388E3C", "#FF689F38", "#FFFBC02D" };
            public static readonly String ErrorColor = "#33E23636";
            public static readonly String SuccessColor = "#3361D161";
            public static readonly String Transparent = "#00000000";
        }

        public static readonly string MessageBoxDoneMessage = "Башня собрана";
        public static readonly string MessageBoxDoneCaption = "Welcome to the limit";
        /// <summary>
        /// Ширина нижнего кольца
        /// </summary>
        public static readonly int FirstRingWidth = 150;
        /// <summary>
        /// Высота колец
        /// </summary>
        public static readonly int ringHeight = 20;
        /// <summary>
        /// "Потеря ширины" кольца - половина разницы между шириной соседних колец
        /// </summary>
        public readonly int ringWidthFall = 5;
        /// <summary>
        /// Тип игры - Автоматический, Ручной
        /// </summary>
        public string gameType;
        /// <summary>
        /// Количество колец
        /// </summary>
        public int ringsCount;
        /// <summary>
        /// Возвращает сплошную кисть на базе определенного RGB или RGBA цвета в HEX формате
        /// </summary>
        /// <param name="hex">Строка вида #AARRGGBB или #RRGGBB</param>
        /// <returns></returns>
        public static SolidColorBrush GetColorFromRGBA(String hex)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFrom(hex);
        }
        /// <summary>
        /// Создает копию кольца
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static Rectangle GetCopy(Rectangle origin)
        {
            Rectangle temp = new Rectangle();
            temp.Height = origin.Height;
            temp.Width = origin.Width;
            temp.Fill = origin.Fill;
            temp.Stroke = origin.Stroke;
            temp.StrokeThickness = origin.StrokeThickness;
            return temp;
        }
    }
}
