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

            OnSetColorInterpretationEvent += SetColorsInTextBlocks;
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
            
            _mediaColors = new List<Color>();
            _dominantColors = await _colorCounter.GetDominantColors(imageFilePath);

            foreach (var color in _dominantColors)
            {
                _mediaColors.Add(Color.FromArgb(color.A, color.R, color.G, color.B));
            }
            
            Color1 = new SolidColorBrush(_mediaColors[0]);
            Color2 = new SolidColorBrush(_mediaColors[1]);
            Color3 = new SolidColorBrush(_mediaColors[2]);
            MainImage = imageFilePath;

            var random = new Random();
            var randomDominantColor = random.Next(_mediaColors.Count);
            var titleColor = _mediaColors[randomDominantColor];

            TitleBarBrush = new SolidColorBrush(titleColor);
            
            OnSetColorInterpretationEvent.Invoke();
        }

        #endregion

        #region PrivateFields

        private List<System.Drawing.Color> _dominantColors;
        private List<Color> _mediaColors;
        
        private int _outerMarginSize = 10;
        private int _windowRadius = 10;
        
        private readonly ImageUploadService _imageUpload;
        private readonly ColorCalculator _colorCounter;
        private string _mainImage = "";

        private Brush? _color1;
        private Brush? _color2;
        private Brush? _color3;

        private string _colorInterpretation1 = "";
        private string _colorInterpretation2 = "";
        private string _colorInterpretation3 = "";

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

        public string ColorInterpretation1
        {
            get => _colorInterpretation1;
            set => SetProperty(ref _colorInterpretation1, value);
        }
        
        public string ColorInterpretation2
        {
            get => _colorInterpretation2;
            set => SetProperty(ref _colorInterpretation2, value);
        }
        
        public string ColorInterpretation3
        {
            get => _colorInterpretation3;
            set => SetProperty(ref _colorInterpretation3, value);
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
                OnSetColorInterpretationEvent.Invoke();
            }
        }

        #endregion

        #region Events

        private delegate void SetColorInterpretation();
        private event SetColorInterpretation OnSetColorInterpretationEvent;

        #endregion

        #region Private Methods

        private void SetColorsInTextBlocks()
        {
            var color1 = _dominantColors[0];
            var color2 = _dominantColors[1];
            var color3 = _dominantColors[2];
            
            switch (SelectedColorInterpretation)
            {
                case "Rgb":
                    ColorInterpretation1 = $"RGB: ({color1.R}, {color1.G}, {color1.B})";
                    ColorInterpretation2 = $"RGB: ({color2.R}, {color2.G}, {color2.B})";
                    ColorInterpretation3 = $"RGB: ({color3.R}, {color3.G}, {color3.B})";
                    break;
                case "Hex":
                    ColorInterpretation1 = "#" + color1.R.ToString("X2") + 
                                           color1.G.ToString("X2") + 
                                           color1.B.ToString("X2");
                    ColorInterpretation2 = "#" + color2.R.ToString("X2") + 
                                           color2.G.ToString("X2") + 
                                           color2.B.ToString("X2");
                    ColorInterpretation3 = "#" + color3.R.ToString("X2") + 
                                           color3.G.ToString("X2") + 
                                           color3.B.ToString("X2");
                    break;
            }
        }

        #endregion
    }
}