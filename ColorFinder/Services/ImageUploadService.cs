using System;
using Microsoft.Win32;

namespace ColorFinder.Services
{
    public class ImageUploadService : IUploadService
    {
        public string GetImageFileName()
        {
            var openDialog = new OpenFileDialog
            {
                Title = "Загрузить фото",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +  
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +  
                         "Portable Network Graphic (*.png)|*.png"
            };
            
            return openDialog.ShowDialog() == true ? openDialog.FileName : "";
        }

        public string GetImageFilePath()
        {
            var imageName = GetImageFileName();

            if (imageName is "" or null)
            {
                throw new ArgumentNullException();
            }
            
            return imageName;
        }
    }
}