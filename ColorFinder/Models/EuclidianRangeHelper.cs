using System;
using System.Drawing;

namespace ColorFinder.Models
{
    public static class EuclidianRangeHelper
    {
        /// <summary>
        /// Calculates Euclidian range between two colors in rgb-dimension.
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns></returns>
        public static double EuclidianRange(Color color1, Color color2)
        {
            var redSquare = Math.Pow(color1.R - color2.R, 2);
            var greenSquare = Math.Pow(color1.G - color2.G, 2);
            var blueSquare = Math.Pow(color1.B - color2.B, 2);

            return Math.Sqrt(redSquare + greenSquare + blueSquare);
        }
    }
}