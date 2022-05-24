using System;
using System.Drawing;
using ColorFinder.Models;
using NUnit.Framework;

namespace ColorFinderTests.ModelsTests.EuclidianRangeTests
{
    public class EuclidianRangeHelperTests
    {
        [Test]
        public void EuclidianRange_SameColors_ZeroReturned()
        {
            // Ожидаемое расстояние
            const double expectedRange = 0.0;
            
            // Получаемое расстояние
            var actualRange = EuclidianRangeHelper.EuclidianRange(Color.Black, Color.Black);

            Assert.That(actualRange, Is.Not.Null); // Проверка на то,что полученное расстояние не null
            Assert.AreEqual(expectedRange, actualRange); // Проверка на то, что они совпадают
        }

        [Test]
        public void EuclidianRange_BlueIsColor1_RedIsColor2_realRangeReturned()
        {
            // Ожидаемое расстояние
            const double expectedRange = 360.624458;
            
            // Получаемое расстояние
            var actualRange = EuclidianRangeHelper.EuclidianRange(Color.Blue, Color.Red);
            
            Assert.That(actualRange, Is.Not.Null); // Проверка на то,что полученное расстояние не null
            Assert.AreEqual(expectedRange, Math.Round(actualRange, 6)); // Проверка на то, что они совпадают
        }
    }
}