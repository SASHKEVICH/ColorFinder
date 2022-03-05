using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ColorFinder.Services;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;

namespace ColorFinder.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            _imageUpload = new ImageUploadService();
            ImageUploadCommand = new DelegateCommand(ImageUploadCommandExecute);
        }

        private readonly ImageUploadService _imageUpload;
        
        public string MainImage
        {
            get => _mainImage;
            set => SetProperty(ref _mainImage, value);
        }

        private string _mainImage;
        public DelegateCommand ImageUploadCommand { get; }
        private void ImageUploadCommandExecute()
        {
            MainImage = _imageUpload.GetImageFileName();
        }
    }
}