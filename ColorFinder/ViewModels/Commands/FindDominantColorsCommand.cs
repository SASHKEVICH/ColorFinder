using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
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
            string? imageFilePath;
            try
            {
                imageFilePath = _imageUploader.GetImageFilePath();
            }
            catch (ArgumentNullException)
            {
                return;
            }

            var dominantColors = await FindDominantColors(imageFilePath);

            _viewModel.FillDominantColors(dominantColors);
            _viewModel.SetImageInWindow(imageFilePath);
        }

        private async Task<List<Color>> FindDominantColors(string imageFilePath)
        {
            var dominantColors = await _colorCalculator.GetDominantColors(imageFilePath);

            return dominantColors;
        }
    }
}