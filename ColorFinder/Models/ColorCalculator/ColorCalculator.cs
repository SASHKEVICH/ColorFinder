using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace ColorFinder.Models.ColorCalculator
{
    public class ColorCalculator
    {
        private readonly IColorCalculatorMethod _colorCalculatorMethod;
        private Bitmap _resizedImage;
        private List<Color> _dominantColors;

        public ColorCalculator(IColorCalculatorMethod calculatorMethod)
        {
            _colorCalculatorMethod = calculatorMethod;
            
            _resizedImage = new Bitmap(100, 100);
            _dominantColors = new List<Color>();
        }

        /// <summary>
        /// Находит доминантые цвета на изображении.
        /// </summary>
        /// <param name="imageFileName">Путь до изображения.</param>
        /// <returns>Список цветов (Colors).</returns>
        public async Task<List<Color>> GetDominantColors(string? imageFileName)
        { 
            // Проверка на пустой путь до изображения
            if (imageFileName is "" or null)
            {
                throw new ArgumentNullException(imageFileName);
            }
            
            // Уменьшение разрешения изображения
            _resizedImage = ImageResizer.ResizeImage(imageFileName);
            
            // Добавление цветов, взятых с каждого пикселя, в список
            var imageColors = AddColorsFromImageToList();
            
            // Присваивание этого списка цветов методу подсчета
            _colorCalculatorMethod.ImageColors = imageColors;
            
            // Получение доминатных цветов
            _dominantColors = await _colorCalculatorMethod.GetDominantColors();
            
            return _dominantColors;
        }

        /// <summary>
        /// Добавляет цвета из изображения в список.
        /// </summary>
        /// <returns>Список цветов (Color).</returns>
        private List<Color> AddColorsFromImageToList()
        {
            var imageColors = new List<Color>(_resizedImage.Width * _resizedImage.Height);

            for (var x = 0; x < _resizedImage.Width; x++)
            {
                for (var y = 0; y < _resizedImage.Height; y++)
                {
                    imageColors.Add(_resizedImage.GetPixel(x, y));
                }
            }

            return imageColors;
        }
    }
}