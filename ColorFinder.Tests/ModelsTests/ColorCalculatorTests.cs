﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColorFinder.Models.ColorCalculator;
using NUnit.Framework;

namespace ColorFinderTests.ModelsTests
{
    public class ColorCalculatorTests
    {
        private ColorCalculator? _calculator;
        
        [SetUp]
        public void Setup()
        {
            _calculator = new ColorCalculator();
        }

        [Test]
        public async Task GetDominantColors_onePlainColorPicture_3sameColorsReturned()
        {
            const string pictureFilePath = "../../../../ColorFinder.Tests/Assets/plain_black.jpg"; // Путь до изображения
            
            var actualDominantColors = await _calculator!.GetDominantColors(pictureFilePath); // Получаемые доминантные цвета
            
            Assert.That(actualDominantColors, Is.Not.Null); // Проверка на то, что массив цветов существует
            Assert.That(AllTheSameElements(actualDominantColors)); // Проверка на то, что все полученные цвета одинаковы друг к другу
        }
        
        [Test]
        public async Task GetDominantColors_multiColoredPicture_3diffrerentColorsReturned()
        {
            const string pictureFilePath = "../../../../ColorFinder.Tests/Assets/multi_colored.jpg"; // Путь до изображения
            
            var actualDominantColors = await _calculator!.GetDominantColors(pictureFilePath); // Получаемые доминантные цвета
            
            Assert.That(actualDominantColors, Is.Not.Null); // Проверка на то, что массив цветов существует
            Assert.That(!ListHasDuplicates(actualDominantColors)); // Проверка на то, что все полученные цвета разные
        }
        
        [Test]
        public async Task GetDominantColors_fullyTransparentPicture_3transparentColorsReturned()
        {
            const string pictureFilePath = "../../../../ColorFinder.Tests/Assets/fully_transparent.png"; // Путь до изображения
            
            var actualDominantColors = await _calculator!.GetDominantColors(pictureFilePath); // Получаемые доминантные цвета

            Assert.That(actualDominantColors, Is.Not.Null); // Проверка на то, что массив цветов существует 
            Assert.That(AllTheSameElements(actualDominantColors)); // Проверка на то, что все полученные цвета одинаковы друг к другу
        }
        
        [Test]
        public void GetDominantColors_imageFileNameIsEmptyOrNull_ExceptionReturned()
        {
            // Выбрасывается ArgumentNullExceprtion, если путь до изображения пустой или null
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _calculator!.GetDominantColors(""));
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _calculator!.GetDominantColors(null));
        }
        
        private static bool AllTheSameElements<T>(List<T> list)
        {
            return list.TrueForAll(element => element.Equals(list[0]));
        }
        
        private static bool ListHasDuplicates<T>(List<T> list)
        {
            return list.Count != list.Distinct().Count();
        }
    }
}