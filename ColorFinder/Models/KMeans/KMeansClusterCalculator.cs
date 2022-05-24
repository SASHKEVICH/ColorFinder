using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using ColorFinder.Models.ColorCalculator;

namespace ColorFinder.Models.KMeans
{
    public class KMeansClusterCalculatorMethod : IColorCalculatorMethod
    {
        public KMeansClusterCalculatorMethod()
        {
            _kClusters = new List<KCluster>();
            ImageColors = new List<Color>();
        }
        
        private const int ClustersAmount = 3;
        private readonly List<KCluster> _kClusters;

        public List<Color> ImageColors { get; set; }
        
        public async Task<List<Color>> GetDominantColors()
        {
            await FindClustersCenters();

            var dominantColors = new List<Color>();
            
            _kClusters.ForEach(cluster => dominantColors.Add(cluster.ClusterCenter));

            return dominantColors;
        }
        
        /// <summary>
        /// Шаблонный метод поиск центров кластеров.
        /// </summary>
        /// <returns>Список к-средних кластеров.</returns>
        private async Task FindClustersCenters()
        {
            SetupClusters();
            await DoCentersRecalclulations();
        }
        
        /// <summary>
        /// Рандомный выбор цвета, как центра кластера на изображении. 
        /// </summary>
        private void SetupClusters()
        {
            var alreadyPickedColors = new List<Color>();
            var rnd = new Random();
            var i = 0;
            var plainColorInPicture = 0;
            
            while (i < ClustersAmount)
            {
                var randomColorNumber = rnd.Next(ImageColors.Count);
                var randomColor = ImageColors[randomColorNumber];
                
                if (alreadyPickedColors.Contains(randomColor) && plainColorInPicture < 3)
                {
                    plainColorInPicture++;
                    continue;
                }
                
                alreadyPickedColors.Add(randomColor);
                _kClusters.Add(new KCluster(ImageColors[randomColorNumber]));
                
                i++;
            }
        }
        
        /// <summary>
        /// Смещение центров кластеров к средним цветам.
        /// </summary>
        private async Task DoCentersRecalclulations()
        {
            var iterations = 0;

            await Task.Run(() =>
            {
                while (iterations < 25)
                { 
                    FillClustersByColors();

                    foreach (var cluster in _kClusters)
                    {
                        if (cluster.IsItEnoughForRecalcultaions())
                        {
                            break;
                        }
                    }

                    iterations++;
                }
            });
        }
        
        /// <summary>
        /// Заполнение кластеров ближайшими для них цветами.
        /// </summary>
        private void FillClustersByColors()
        {
            foreach (var color in ImageColors)
            {
                var shortedDistance = double.MaxValue;
                var closestCluster = new KCluster(new Color());

                foreach (var cluster in _kClusters)
                {
                    var distance = cluster.EuclidianRangeFromCenter(color);

                    if (distance >= shortedDistance) continue;
                    
                    shortedDistance = distance;
                    closestCluster = cluster;
                }
                    
                closestCluster.AddColor(color);
            }
        }
    }
}