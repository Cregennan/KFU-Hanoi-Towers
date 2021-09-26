using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Hanoi_Towers
{
    public class GameSettings
    {
        public class Colors
        {
            public static readonly List<String> RingColors = new List<String> { "#FFD32F2F", "#FFC2185B", "#FF7B1FA2", "#FF512DA8", "#FF303F9F", "#FF1976D2", "#FF0288D1", "#FF0097A7", "#FF00796B", "#FF388E3C", "#FF689F38", "#FFFBC02D" };
            public static readonly String ErrorColor = "#33E23636";
            public static readonly String SuccessColor = "#3361D161";
            public static readonly String TransparentColor = "#00000000";
        }


        public readonly int FirstRingWidth = 150;
        public readonly int ringHeight = 20;
        public readonly int ringWidthFall = 5;

        public string gameType;
        public int ringsCount;

        public static SolidColorBrush GetColorFromRGBA(String hex)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFrom(hex);
        }

        public static Rectangle GetCopy(Rectangle origin)
        {
            Rectangle temp = new Rectangle();
            temp.Height = origin.Height;
            temp.Width = origin.Width;
            temp.Fill = origin.Fill;
            temp.Stroke = origin.Stroke;

            return temp;
        }
    }
}
