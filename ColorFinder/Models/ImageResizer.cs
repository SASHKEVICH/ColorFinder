using System;
using System.Drawing;

namespace ColorFinder.Models;

public static class ImageResizer
{
    /// <summary>
    /// Сжимает изображение до разрешения, у которого одно из измерений строго 150 пикселей, сохраняя пропорции.
    /// </summary>
    /// <param name="imageFileName">Имя файла с изображением.</param>
    /// <returns>Сжатое изображение.</returns>
    public static Bitmap ResizeImage(string imageFileName)
    {
        using var originalImage = Image.FromFile(imageFileName);
            
        Size newSize;
        const int maxResizedDimension = 150;

        if (originalImage.Height > originalImage.Width)
        {
            newSize = new Size(maxResizedDimension, 
                (int)Math.Floor(originalImage.Height / originalImage.Width * 1.0f * maxResizedDimension));
        }
        else
        {
            newSize = new Size((int)Math.Floor(originalImage.Width / originalImage.Height * 1.0f * maxResizedDimension), 
                maxResizedDimension);
        }

        var resizedImage = new Bitmap(originalImage, newSize);

        return resizedImage;
    }
}