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
            const double expectedRange = 0.0;

            var actualRange = EuclidianRangeHelper.EuclidianRange(Color.Black, Color.Black);

            Assert.AreEqual(expectedRange, actualRange);
        }

        [Test]
        public void EuclidianRange_BlueIsColor1_RedIsColor2_realRangeReturned()
        {
            const double expectedRange = 360.624458;

            var actualRange = EuclidianRangeHelper.EuclidianRange(Color.Blue, Color.Red);

            Assert.AreEqual(expectedRange, Math.Round(actualRange, 6));
        }
    }
}