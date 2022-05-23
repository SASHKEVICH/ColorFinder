using System;
using Microsoft.Win32;

namespace ColorFinder.Services
{
    public class ImageUploadService : IUploadService
    {
        public string GetImageFilePath()
        {
            var openDialog = new OpenFileDialog
            {
                Title = "Загрузить фото",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +  
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +  
                         "Portable Network Graphic (*.png)|*.png"
            };
            
            var imagePath = openDialog.ShowDialog() == true ? openDialog.FileName : "";
            
            if (imagePath is "" or null)
            {
                throw new ArgumentNullException();
            }
            
            return imagePath;
        }
    }
}