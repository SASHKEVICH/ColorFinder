using System.Collections.Generic;
using System.Windows.Media;
using ColorFinder.Models;
using ColorFinder.Services;
using Prism.Commands;
using Prism.Mvvm;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace ColorFinder.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            _imageUpload = new ImageUploadService();
            _colorCounter = new ColorCalculator();
            ImageUploadCommand = new DelegateCommand(ImageUploadCommandExecute);
            
            var standardColor = Color.FromArgb(100, 196, 196, 196);
            
            Brush1 = new SolidColorBrush(standardColor);
            Brush2 = new SolidColorBrush(standardColor);
            Brush3 = new SolidColorBrush(standardColor);
        }

        public DelegateCommand ImageUploadCommand { get; }
        
        private readonly ImageUploadService _imageUpload;
        private readonly ColorCalculator _colorCounter;
        private string _mainImage = "";

        private Brush? _brush1;
        private Brush? _brush2;
        private Brush? _brush3;
        
        public Brush Brush1
        {
            get => _brush1;
            set => SetProperty(ref _brush1, value);
        }
        public Brush Brush2
        {
            get => _brush2;
            set => SetProperty(ref _brush2, value);
        }
        
        public Brush Brush3
        {
            get => _brush3;
            set => SetProperty(ref _brush3, value);
        }

        public string MainImage
        {
            get => _mainImage;
            set => SetProperty(ref _mainImage, value);
        }
        
        private async void ImageUploadCommandExecute()
        {
            MainImage = _imageUpload.GetImageFileName();

            var mediaColors = new List<Color>();
            var dominantColors = await _colorCounter.GetDominantColors(MainImage);

            foreach (var color in dominantColors)
            {
                mediaColors.Add(Color.FromArgb(color.A, color.R, color.G, color.B));
            }
            
            Brush1 = new SolidColorBrush(mediaColors[0]);
            Brush2 = new SolidColorBrush(mediaColors[1]);
            Brush3 = new SolidColorBrush(mediaColors[2]);
        }
    }
}