using System;
using System.Collections.Generic;
using Color = System.Drawing.Color;

namespace ColorFinder.Models.KMeans
{
    public class KCluster
    {
        /// <summary>
        /// Создание к-среднего кластера.
        /// </summary>
        /// <param name="initClusterCenter">Стартовый центр (цвет) кластера.</param>
        public KCluster(Color initClusterCenter)
        {
            ClusterCenter = initClusterCenter;
            _clusterColors = new List<Color>();
        }
        
        public Color ClusterCenter { get; private set; }
        private readonly List<Color> _clusterColors;
        private const double Accuracy = 0.001;

        /// <summary>
        /// Добавить цвет в список цветов этого кластера.
        /// </summary>
        /// <param name="color"></param>
        public void AddColor(Color color)
        {
            _clusterColors.Add(color);
        }
        
        /// <summary>
        /// Метод вычисляет, достигнута ли точность вычислений, и, соответсвенно, нужно ли прекращать пересчет центров.
        /// </summary>
        /// <returns></returns>
        public bool IsItEnoughForRecalcultaions()
        {
            var rangeFromCurrentCenterToNew = RecalculateCenter();

            if (rangeFromCurrentCenterToNew > Accuracy) return false;
            
            return true;
        }

        /// <summary>
        /// Calculates Euclidian range between cluster's center and a new color in rgb-dimension.
        /// </summary>
        /// <param name="newColor"></param>
        /// <returns></returns>
        public double EuclidianRangeFromCenter(Color newColor)
        {
            return EuclidianRangeHelper.EuclidianRange(ClusterCenter, newColor);
        }
        
        private double RecalculateCenter()
        {
            var newCenter = MeanNewCenter();

            var rangeFromCurrentCenterToNew = EuclidianRangeFromCenter(newCenter);
            ClusterCenter = newCenter;
            
            _clusterColors.Clear();

            return rangeFromCurrentCenterToNew;
        }

        private Color MeanNewCenter()
        {
            var newCenter = new Color();

            double a = 0;
            double r = 0;
            double g = 0;
            double b = 0;
            
            foreach (var color in _clusterColors)
            {
                a += color.A;
                r += color.R;
                g += color.G;
                b += color.B;

                newCenter = Color.FromArgb(
                    (int)Math.Floor(a / _clusterColors.Count),
                    (int)Math.Floor(r / _clusterColors.Count), 
                    (int)Math.Floor(g / _clusterColors.Count),
                    (int)Math.Floor(b / _clusterColors.Count)
                );
            }

            return newCenter;
        }

    }
}
    