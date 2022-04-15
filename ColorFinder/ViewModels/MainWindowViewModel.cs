using System;
using System.Collections.Generic;
using System.Windows;
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
        #region Constructor
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
        #endregion

        #region Commands
        public DelegateCommand ImageUploadCommand { get; }

        private async void ImageUploadCommandExecute()
        {
            MainImage = _imageUpload.GetImageFileName();
            if (MainImage == "")
            {
                return;
            }
            
            var mediaColors = new List<Color>();
            var dominantColors = await _colorCounter.GetDominantColors(MainImage);

            foreach (var color in dominantColors)
            {
                mediaColors.Add(Color.FromArgb(color.A, color.R, color.G, color.B));
            }
            
            Brush1 = new SolidColorBrush(mediaColors[0]);
            Brush2 = new SolidColorBrush(mediaColors[1]);
            Brush3 = new SolidColorBrush(mediaColors[2]);

            var random = new Random();
            var randomDominantColor = random.Next(mediaColors.Count + 1);

            TitleBarBrush = new SolidColorBrush(mediaColors[randomDominantColor]);
        }

        #endregion

        #region PrivateFields

        private Window _window; 
        private int _outerMarginSize = 5;
        private int _windowRadius = 5;
        
        private readonly ImageUploadService _imageUpload;
        private readonly ColorCalculator _colorCounter;
        private string _mainImage = "";

        private Brush? _brush1;
        private Brush? _brush2;
        private Brush? _brush3;

        private Brush _titleBarBrush;

        #endregion

        #region PublicProperties

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
        
        public int OuterMarginSize
        {
            get => _outerMarginSize;
            set => _outerMarginSize = value;
        }

        public int WindowRadius
        {
            get => _windowRadius;
            set => _windowRadius = value;
        }
        public Thickness OuterMarginSizeThickness => new (OuterMarginSize);
        public CornerRadius WindowCornerRadius => new(WindowRadius);
        public SolidColorBrush ForegroundLightBrush => new (Color.FromRgb(255, 255, 255));
        public Brush TitleBarBrush
        {
            get => _titleBarBrush;
            set => SetProperty(ref _titleBarBrush, value);
        }
        

        #endregion

    }
}