using System;
using ColorFinder.Services;

namespace ColorFinder.ViewModels.Commands
{
    public class FindDominantColorsCommand
    {
        private readonly ImageUploadService _imageUploader = new ();
        private readonly MainWindowViewModel _viewModel;

        public FindDominantColorsCommand(MainWindowViewModel viewModel)
        {
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

            await _viewModel.FindDominantColors(imageFilePath);

            _viewModel.SetImageInWindow(imageFilePath);
        }
    }
}