using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hanoi_Towers
{
    public class GameSettings
    {
        public readonly int FirstRingWidth = 150;
        public readonly int ringHeight = 20;
        public readonly int ringWidthFall = 10;
        public readonly List<String> RingColors = new List<String> { "#FFD32F2F", "#FFC2185B", "#FF7B1FA2", "#FF512DA8", "#FF303F9F", "#FF1976D2", "#FF0288D1", "#FF0097A7", "#FF00796B", "#FF388E3C", "#FF689F38", "#FFFBC02D"};
        public string gameType;
        public int ringsCount;
    }
}
