using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using ColorFinder.Models;
using ColorFinder.Models.KMeans;
using ColorFinder.ViewModels.Commands;
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
            // Определяется метод вычисления доминантных цветов
            var calculatorMethod = new KMeansClusterCalculatorMethod(); 
            
            // Экземпляр класса команды, отвечающей за делегирование логики
            var findDominantColorsCommand = new FindDominantColorsCommand(this, calculatorMethod);
            
            // Присваивание экземпляра класса команды объекту, реагирующего на нажатие кнопки интерфейса
            FindDominantColorsCommand = new DelegateCommand(findDominantColorsCommand.Execute);
            
            // Начальное заполнение серым цветом элементов интерфейса, отображающих доминантные цвета
            FillRectanglesByStandardColor();
            
            // Стартовый выбор цветовой интерпретации кодировки
            _interpretation = ColorInterpretations.First();
            
            // Начальные значения закругления окна приложения
            _outerMarginSize = 10;
            _windowRadius = 10;
        }
        
        #endregion

        #region Commands
        public DelegateCommand FindDominantColorsCommand { get; }

        #endregion

        #region PrivateFields

        private List<System.Drawing.Color>? _dominantColors;

        private readonly int _outerMarginSize;
        private readonly int _windowRadius;
        
        private string? _mainImagePath;

        private Brush? _color1;
        private Brush? _color2;
        private Brush? _color3;

        private string? _colorInterpretation1;
        private string? _colorInterpretation2;
        private string? _colorInterpretation3;

        private Brush? _titleBarBrush;
        private Brush? _titleBarTextBrush;

        private string _interpretation;

        #endregion

        #region PublicProperties

        public Brush Color1
        {
            get => _color1 ?? new SolidColorBrush();
            set => SetProperty(ref _color1, value);
        }
        
        public Brush Color2
        {
            get => _color2 ?? new SolidColorBrush();
            set => SetProperty(ref _color2, value);
        }
        
        public Brush Color3
        {
            get => _color3 ?? new SolidColorBrush();
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

        public string MainImagePath
        {
            get => _mainImagePath;
            set => SetProperty(ref _mainImagePath, value);
        }
        
        public int OuterMarginSize => _outerMarginSize;
        
        public int WindowRadius => _windowRadius;
        
        public Thickness OuterMarginSizeThickness => new (OuterMarginSize);
        
        public CornerRadius WindowCornerRadius => new(WindowRadius);
        
        public SolidColorBrush ForegroundLightBrush => new (Color.FromRgb(255, 255, 255));
        
        public Brush TitleBarBrush
        {
            get => _titleBarBrush ?? new SolidColorBrush();
            set => SetProperty(ref _titleBarBrush, value);
        }
        
        public Brush TitleBarTextBrush
        {
            get => _titleBarTextBrush ?? new SolidColorBrush(Color.FromRgb(104, 104, 104));
            set => SetProperty(ref _titleBarTextBrush, value);
        }
        
        public ColorInterpretation ColorInterpretations => new();

        public string SelectedColorInterpretation
        {
            get => _interpretation;
            set
            {
                SetProperty(ref _interpretation, value);
                SetColorsInTextBlocks();
            }
        }

        #endregion
        
        #region Private Methods
        
        private void SetColorsInTextBlocks()
        {
            var color1 = _dominantColors![0];
            var color2 = _dominantColors![1];
            var color3 = _dominantColors![2];
            
            switch (SelectedColorInterpretation)
            {
                case "Rgb":
                    ColorInterpretation1 = $"RGB: ({color1.R}, {color1.G}, {color1.B})";
                    ColorInterpretation2 = $"RGB: ({color2.R}, {color2.G}, {color2.B})";
                    ColorInterpretation3 = $"RGB: ({color3.R}, {color3.G}, {color3.B})";
                    break;
                case "Hex":
                    ColorInterpretation1 = "#" + 
                                           color1.R.ToString("X2") + 
                                           color1.G.ToString("X2") + 
                                           color1.B.ToString("X2");
                    ColorInterpretation2 = "#" + 
                                           color2.R.ToString("X2") + 
                                           color2.G.ToString("X2") + 
                                           color2.B.ToString("X2");
                    ColorInterpretation3 = "#" + 
                                           color3.R.ToString("X2") + 
                                           color3.G.ToString("X2") + 
                                           color3.B.ToString("X2");
                    break;
            }
        }

        private void FillRectanglesByStandardColor()
        {
            var standardColor = Color.FromArgb(100, 196, 196, 196);
            
            Color1 = new SolidColorBrush(standardColor);
            Color2 = new SolidColorBrush(standardColor);
            Color3 = new SolidColorBrush(standardColor);
        }
        
        private void FillRectanglesByDominantColors(List<Color> colors)
        {
            Color1 = new SolidColorBrush(colors[0]);
            Color2 = new SolidColorBrush(colors[1]);
            Color3 = new SolidColorBrush(colors[2]);
        }

        private List<Color> ConvertDrawingColorsToMedia(List<System.Drawing.Color> colors)
        {
            return colors.Select(color => Color.FromArgb(color.A, color.R, color.G, color.B)).ToList();
        }

        private void SetTitleBarBrush(List<Color> colors)
        {
            var random = new Random();
            var randomDominantColorNumber = random.Next(colors.Count);
            var titleBarColor = colors[randomDominantColorNumber];
            
            var titleBarDrawingColor = System.Drawing.Color.FromArgb(
                    titleBarColor.A, 
                    titleBarColor.R, 
                    titleBarColor.G, 
                    titleBarColor.B);
            
            SetTitleBarTextBrush(titleBarDrawingColor);
            TitleBarBrush = new SolidColorBrush(titleBarColor);
        }

        private void SetTitleBarTextBrush(System.Drawing.Color color)
        {
            var colorRangeToBlack = EuclidianRangeHelper.EuclidianRange(color, System.Drawing.Color.Black);
            var colorRangeToWhite = EuclidianRangeHelper.EuclidianRange(color, System.Drawing.Color.White);

            TitleBarTextBrush = colorRangeToBlack < colorRangeToWhite 
                ? new SolidColorBrush(Color.FromRgb(191, 191, 191)) 
                : new SolidColorBrush(Color.FromRgb(35, 34, 34));
        }

        #endregion

        #region Public Methods
        
        public void FillDominantColors(List<System.Drawing.Color> dominantColors)
        {
            _dominantColors = dominantColors;

            var mediaColors = ConvertDrawingColorsToMedia(_dominantColors);
            
            FillRectanglesByDominantColors(mediaColors);

            SetTitleBarBrush(mediaColors);
            
            SetColorsInTextBlocks();
        }
        
        #endregion
    }
}