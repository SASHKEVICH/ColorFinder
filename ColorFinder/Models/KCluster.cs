using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace  ColorFinder.Models
{
    public class KCluster
    {
        /// <summary>
        /// Создание к-среднего кластера.
        /// </summary>
        /// <param name="initClusterCenter">Стартовый центр кластера.</param>
        public KCluster(Color initClusterCenter)
        {
            ClusterCenter = initClusterCenter;
            _clusterColors = new List<Color>();
        }
        
        public Color ClusterCenter { get; set; }
        private List<Color> _clusterColors;
        private double _accuracy = 0.01;
        
        public void AddColor(Color color)
        {
            _clusterColors.Add(color);
        }

        public bool IsItEnoughForRecalcultaions()
        {
            var range = RecalculateCenter();

            if (range > _accuracy) return false;
            
            return true;
        }

        /// <summary>
        /// Calculates Euclidian range between two colors in rgb-dimension.
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns></returns>
        public double EuclidianRange(Color color1, Color color2)
        {
            var redSquare = Math.Pow(color1.R - color2.R, 2);
            var greenSquare = Math.Pow(color1.G - color2.G, 2);
            var blueSquare = Math.Pow(color1.B - color2.B, 2);

            return Math.Sqrt(redSquare + greenSquare + blueSquare);
        }
        
        /// <summary>
        /// Calculates Euclidian range between cluster's center and a new color in rgb-dimension.
        /// </summary>
        /// <param name="newColor"></param>
        /// <returns></returns>
        public double EuclidianRangeFromCenter(Color newColor)
        {
            return EuclidianRange(ClusterCenter, newColor);
        }
        
        private double RecalculateCenter()
        {
            var newCenter = new Color();

            if (_clusterColors.Count > 0)
            {
                double r = 0;
                double g = 0;
                double b = 0;
                
                foreach (var color in _clusterColors)
                {
                    r += color.R;
                    g += color.G;
                    b += color.B;

                    newCenter = Color.FromArgb(
                        (int)Math.Floor(r / _clusterColors.Count), 
                        (int)Math.Floor(g / _clusterColors.Count),
                        (int)Math.Floor(b / _clusterColors.Count)
                    );
                }
            }
            else
            {
                newCenter = Color.FromArgb(0, 0, 0);
            }

            var rangeFromCurrentCenterToNew = EuclidianRangeFromCenter(newCenter);
            ClusterCenter = newCenter;
            
            _clusterColors.Clear();

            return rangeFromCurrentCenterToNew;
        }

    }
}
    