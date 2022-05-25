using System;
using ColorFinder.Models.ColorCalculator;
using ColorFinder.Services;

namespace ColorFinder.ViewModels.Commands
{
    public class FindDominantColorsCommand
    {
        private readonly IUploadService _imageUploader;
        private readonly ColorCalculator _colorCalculator;
        private readonly MainWindowViewModel _viewModel;

        public FindDominantColorsCommand(MainWindowViewModel viewModel, IColorCalculatorMethod calculatorMethod)
        {
            _imageUploader = new ImageUploadService();
            _colorCalculator = new ColorCalculator(calculatorMethod);
            _viewModel = viewModel;
        }

        public async void Execute()
        {
            // Переменная, хранящая в себе путь до изображения
            string? imageFilePath;
            try
            {
                // Получение этого пути
                imageFilePath = _imageUploader.GetImageFilePath(); 
            }
            catch (ArgumentNullException)
            {
                return;
            }
            
            // Получение списка доминантных цветов
            var dominantColors = await _colorCalculator.GetDominantColors(imageFilePath);
            
            // Заполнение объектов интерфейса доминантными цветами
            _viewModel.FillDominantColors(dominantColors);
            
            // Отображение оригинального изображения на экране
            _viewModel.MainImagePath = imageFilePath;
        }
    }
}