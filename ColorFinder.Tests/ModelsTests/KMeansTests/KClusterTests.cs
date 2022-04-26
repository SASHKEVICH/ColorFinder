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
            var method = GetterPrivateMethods.GetMethod<KCluster>(_testCluster,"RecalculateCenter");
            
            var expectedValue = Color.FromArgb(255, 0, 0,0);

            var actualValue = method.Invoke(_testCluster, null);
            
            Assert.That(actualValue, Is.Not.Null);
            Assert.AreEqual(expectedValue, actualValue);
        }
        
        [Test]
        public void RecalculateCenter_ClustersColorsFilled_NearlyPinkReturned()
        {
            var method = GetterPrivateMethods.GetMethod<KCluster>(_testCluster,"RecalculateCenter");

            var expectedColor = Color.FromArgb(255, 170, 85, 85);
            
            var clusterColors = new List<Color>
            {
                Color.Black,
                Color.White,
                Color.Red
            };
            
            clusterColors.ForEach(color => _testCluster.AddColor(color));

            var actualValue = method.Invoke(_testCluster, null);
            
            Assert.That(actualValue, Is.Not.Null);
            Assert.AreEqual(expectedColor, actualValue);
        }

        [Test]
        public void GetRangeFromCurrentCenterToNew_ClusterColorsFilled_BlackIsOldCenter_RangeToNewCenterReturned()
        {
            var method = GetterPrivateMethods.GetMethod<KCluster>(_testCluster,"GetRangeFromCurrentCenterToNew");

            var expectedValue = 284.726;
            
            var clusterColors = new List<Color>
            {
                Color.Chartreuse,
                Color.White,
                Color.Red
            };
            
            clusterColors.ForEach(color => _testCluster.AddColor(color));

            var actualValue = (double) method.Invoke(_testCluster, null);

            Assert.That(actualValue, Is.Not.Null);
            Assert.AreEqual(expectedValue, Math.Round(actualValue, 3));
        }
    }
}