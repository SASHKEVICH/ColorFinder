using System;
using System.Collections.Generic;
using System.Linq;
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
            
            Color1 = new SolidColorBrush(standardColor);
            Color2 = new SolidColorBrush(standardColor);
            Color3 = new SolidColorBrush(standardColor);
            
            _interpretation = ColorInterpretations.First();
        }
        #endregion

        #region Commands
        public DelegateCommand ImageUploadCommand { get; }
        private async void ImageUploadCommandExecute()
        {
            var imageFilePath = _imageUpload.GetImageFileName();
            if (imageFilePath == "")
            {
                return;
            }
            
            var mediaColors = new List<Color>();
            var dominantColors = await _colorCounter.GetDominantColors(imageFilePath);

            foreach (var color in dominantColors)
            {
                mediaColors.Add(Color.FromArgb(color.A, color.R, color.G, color.B));
            }
            
            Color1 = new SolidColorBrush(mediaColors[0]);
            Color2 = new SolidColorBrush(mediaColors[1]);
            Color3 = new SolidColorBrush(mediaColors[2]);
            MainImage = imageFilePath;

            var random = new Random();
            var randomDominantColor = random.Next(mediaColors.Count);
            var titleColor = mediaColors[randomDominantColor];

            TitleBarBrush = new SolidColorBrush(titleColor);
            // TitleBarTextBrush = new SolidColorBrush();
        }

        #endregion

        #region PrivateFields
        
        private int _outerMarginSize = 10;
        private int _windowRadius = 10;
        
        private readonly ImageUploadService _imageUpload;
        private readonly ColorCalculator _colorCounter;
        private string _mainImage = "";

        private Brush? _color1;
        private Brush? _color2;
        private Brush? _color3;

        private Brush _titleBarBrush;
        private Brush _titleBarTextBrush = new SolidColorBrush(Color.FromRgb(104, 104, 104));

        private string _interpretation;

        #endregion

        #region PublicProperties

        public Brush Color1
        {
            get => _color1;
            set => SetProperty(ref _color1, value);
        }
        public Brush Color2
        {
            get => _color2;
            set => SetProperty(ref _color2, value);
        }
        
        public Brush Color3
        {
            get => _color3;
            set => SetProperty(ref _color3, value);
        }

        public string MainImage
        {
            get => _mainImage;
            set => SetProperty(ref _mainImage, value);
        }
        public int OuterMarginSize => _outerMarginSize;
        
        public int WindowRadius => _windowRadius;
        
        public Thickness OuterMarginSizeThickness => new (OuterMarginSize);
        
        public CornerRadius WindowCornerRadius => new(WindowRadius);
        
        public SolidColorBrush ForegroundLightBrush => new (Color.FromRgb(255, 255, 255));
        
        public Brush TitleBarBrush
        {
            get => _titleBarBrush;
            set => SetProperty(ref _titleBarBrush, value);
        }
        
        public Brush TitleBarTextBrush
        {
            get => _titleBarTextBrush;
            set => SetProperty(ref _titleBarTextBrush, value);
        }
        
        public ColorInterpretation ColorInterpretations => new();

        public string SelectedColorInterpretation
        {
            get => _interpretation;
            set
            {
                SetProperty(ref _interpretation, value);
                
            }
        }

        #endregion

    }
}