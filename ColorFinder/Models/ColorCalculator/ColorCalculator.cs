using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using ColorFinder.Models.KMeans;

namespace ColorFinder.Models.ColorCalculator
{
    public class ColorCalculator
    {
        private Bitmap _resizedImage = new (100, 100);
        private List<Color> _dominantColors = new ();
        private IColorCalculator? _colorCalculator;

        /// <summary>
        /// Находит доминантые цвета на изображении.
        /// </summary>
        /// <param name="imageFileName">Путь до изображения.</param>
        /// <returns>Список цветов (Colors).</returns>
        public async Task<List<Color>> GetDominantColors(string? imageFileName)
        { 
            if (imageFileName is "" or null)
            {
                throw new ArgumentNullException(imageFileName);
            }
            
            _resizedImage = ResizeImage(imageFileName);

            var imageColors = AddColorsFromImageToList();

            _colorCalculator = new KMeansClusterCalculator(imageColors);

            _dominantColors = await _colorCalculator.GetDominantColors();
            
            return _dominantColors;
        }

        /// <summary>
        /// Сжимает изображение до разрешения, у которого одно из измерений строго 150 пикселей, сохраняя пропорции.
        /// </summary>
        /// <param name="imageFileName">Имя файла с изображением.</param>
        /// <returns>Сжатое изображение.</returns>
        private static Bitmap ResizeImage(string imageFileName)
        {
            using var originalImage = Image.FromFile(imageFileName);
            
            Size newSize;
            const int maxResizedDimension = 150;

            if (originalImage.Height > originalImage.Width)
            {
                newSize = new Size(maxResizedDimension, 
                    (int)Math.Floor(originalImage.Height / originalImage.Width * 1.0f * maxResizedDimension));
            }
            else
            {
                newSize = new Size((int)Math.Floor(originalImage.Width / originalImage.Height * 1.0f * maxResizedDimension), 
                    maxResizedDimension);
            }

            var resizedImage = new Bitmap(originalImage, newSize);

            return resizedImage;
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