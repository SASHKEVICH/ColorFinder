using System;
using System.Collections.Generic;
using System.Drawing;
using ColorFinder.Models.KMeans;
using NUnit.Framework;

namespace ColorFinderTests.ModelsTests.KMeansTests
{
    public class KClusterTests
    {
        private KCluster? _testCluster;
        
        [SetUp]
        public void SetUp()
        {
            _testCluster = new KCluster(Color.Black);
        }
        
        [Test]
        public void RecalculateCenter_BlackColorIsCenter_clustersColorEmpty_BlackNewCenterReturned()
        {
            // Получение приватного метода путём рефлексии
            var method = GetterPrivateMethods.GetMethod<KCluster>(_testCluster,"RecalculateCenter"); 
            
            // Ожидаемый цвет - полностью чёрный
            var expectedValue = Color.FromArgb(255, 0, 0,0);
            
            // Получаемый цвет
            var actualValue = method.Invoke(_testCluster, null);
            
            Assert.That(actualValue, Is.Not.Null); // Проверка, что получаемый цвет существует
            Assert.AreEqual(expectedValue, actualValue); // Ожидаемый и получаемый цвет совпадают
        }
        
        [Test]
        public void RecalculateCenter_ClustersColorsFilled_NearlyPinkReturned()
        {
            // Получение приватного метода путём рефлексии
            var method = GetterPrivateMethods.GetMethod<KCluster>(_testCluster, "RecalculateCenter");

            // Ожидаемый цвет - близко к розовому
            var expectedColor = Color.FromArgb(255, 170, 85, 85);
            
            // Список цветов кластера
            var clusterColors = new List<Color>
            {
                Color.Black,
                Color.White,
                Color.Red
            };
            
            // Заполнение кластера цветами
            clusterColors.ForEach(color => _testCluster.AddColor(color));
            
            // Получаемый цвет
            var actualValue = method.Invoke(_testCluster, null);
            
            Assert.That(actualValue, Is.Not.Null); // Проверка, что получаемый цвет существует
            Assert.AreEqual(expectedColor, actualValue); // Ожидаемый и получаемый цвет совпадают
        }

        [Test]
        public void GetRangeFromCurrentCenterToNew_ClusterColorsFilled_BlackIsOldCenter_RangeToNewCenterReturned()
        {
            // Получение приватного метода путём рефлексии
            var method = GetterPrivateMethods.GetMethod<KCluster>(_testCluster,"GetRangeFromCurrentCenterToNew");
            
            // Ожидаемое значение расстояния от старого центра до нового
            var expectedValue = 284.726;
            
            // Список цветов кластера
            var clusterColors = new List<Color>
            {
                Color.Chartreuse,
                Color.White,
                Color.Red
            };
            
            // Заполнение кластера цветами
            clusterColors.ForEach(color => _testCluster.AddColor(color));
            
            // Ожидаемое расстояние
            var actualValue = (double) method.Invoke(_testCluster, null);
            
            Assert.That(actualValue, Is.Not.Null); // Проверка, что получаемое расстояние существует
            Assert.AreEqual(expectedValue, Math.Round(actualValue, 3)); // Ожидаемое и получаемое расстояния совпадают
        }
    }
}