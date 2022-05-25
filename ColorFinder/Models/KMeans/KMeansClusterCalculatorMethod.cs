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
            ImageColors = new List<Color>();
        }
        
        private const int ClustersAmount = 3;
        private List<KCluster>? _kClusters;

        public List<Color> ImageColors { get; set; }
        
        public async Task<List<Color>> GetDominantColors()
        {
            // Обнуление кластеров при начале расчета доминатных цветов
            _kClusters = new List<KCluster>();
            
            // Метод сначала заполняет центры кластеров случайными цветами изображения, затем вызывает перерасчет центров масс кластеров
            await FindClustersCenters();
            
            // Список доминантых цветов
            var dominantColors = new List<Color>();
            
            // Добавление в этот список центров масс кластеров, то есть доминатных цветов
            _kClusters.ForEach(cluster => dominantColors.Add(cluster.ClusterCenter));

            return dominantColors;
        }
        
        /// <summary>
        /// Шаблонный метод поиска центров кластеров.
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
            // Список уже выбранных цветов
            var alreadyPickedColors = new List<Color>();
            
            // Класс рандомайзер
            var rnd = new Random();
            
            // Итератор
            var i = 0;
            
            // Счетчик, проверяющий, состоит ли из одного цвета изображение
            var plainColorInPicture = 0;
            
            while (i < ClustersAmount)
            {
                // Выбирается случайный номер цвета из списка
                var randomColorNumber = rnd.Next(ImageColors.Count);
                
                // Выбирается случайный цвет из списка
                var randomColor = ImageColors[randomColorNumber];

                // Проверка на то, состоит ли из одного цвета изображение
                if (alreadyPickedColors.Contains(randomColor) && plainColorInPicture < 3)
                {
                    plainColorInPicture++;
                    continue;
                }

                // Добавление в список уже выбранных цветов
                alreadyPickedColors.Add(randomColor);
                _kClusters.Add(new KCluster(randomColor));
                
                i++;
            }
        }
        
        /// <summary>
        /// Смещение центров кластеров к средним цветам.
        /// </summary>
        private async Task DoCentersRecalclulations()
        {
            // Итератор
            var iterations = 0;

            await Task.Run(() =>
            {
                // Повторять 5 раз для достижения точности
                while (iterations < 5)
                { 
                    // Заполнение кластеров цветами
                    FillClustersByColors();
                    
                    // Проверка каждого кластера, нужно ли дальше совершать пересчет центра
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