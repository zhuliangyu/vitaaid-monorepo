using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MyToolkit.Base.Extensions
{
  public static class ImageExtension
  {
    public static void FromFile(this System.Windows.Controls.Image self, string FileName)
    {
      if (string.IsNullOrEmpty(FileName) || File.Exists(FileName) == false)
      {
        self.Source = null;
        return;
      }
      BitmapImage bmp = new BitmapImage();
      bmp.BeginInit();
      bmp.UriSource = new Uri(FileName);
      bmp.EndInit();
      self.Source = bmp;
    }
    public static void setUri(this System.Windows.Controls.Image self, Uri uri)
    {
      if (uri != null)
      {
        BitmapImage bmp = new BitmapImage();
        bmp.BeginInit();
        bmp.UriSource = uri;
        bmp.EndInit();
        self.Source = bmp;
      }
      else
        self.Source = null;
    }
    public static void setRawData(this System.Windows.Controls.Image self, byte[] blob)
    {
      if (blob != null && blob.Length > 0)
      {
        MemoryStream stream = new MemoryStream();
        stream.Write(blob, 0, blob.Length);
        stream.Position = 0;

        System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
        MemoryStream ms = new MemoryStream();
        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
        ms.Seek(0, SeekOrigin.Begin);

        BitmapImage bi = new BitmapImage();
        bi.BeginInit();
        bi.StreamSource = ms;
        bi.EndInit();

        self.Source = bi;
      }
      else
        self.Source = null;
    }
    public static void GenerateBarcode(this System.Windows.Controls.Image self, string sCode)
    {
      try
      {
        System.Drawing.Image bmp = Code128Rendering.MakeBarcodeImage(sCode, 1, true, true);

        MemoryStream strm = new MemoryStream();
        bmp.Save(strm, System.Drawing.Imaging.ImageFormat.Bmp);

        strm.Position = 0;
        System.Drawing.Image img = System.Drawing.Image.FromStream(strm);
        BitmapImage bi = new BitmapImage();
        bi.BeginInit();
        MemoryStream ms = new MemoryStream();
        img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

        ms.Seek(0, SeekOrigin.Begin);
        bi.StreamSource = ms;
        bi.EndInit();
        self.Source = bi;
        self.Tag = sCode;
      }
      catch (Exception)
      {
      }

    }

    public static System.Drawing.Bitmap ResizeImageByWidth(this System.Drawing.Image image, int width)
    {
      return image.ResizeImage(width, width * image.Height / image.Width);
    }
    public static System.Drawing.Bitmap ResizeImageByHeight(this System.Drawing.Image image, int height)
    {
      return image.ResizeImage(height * image.Width / image.Height, height);
    }
    /// <summary>
    /// Resize the image to the specified width and height.
    /// </summary>
    /// <param name="image">The image to resize.</param>
    /// <param name="width">The width to resize to.</param>
    /// <param name="height">The height to resize to.</param>
    /// <returns>The resized image.</returns>
    public static System.Drawing.Bitmap ResizeImage(this System.Drawing.Image image, int width, int height)
    {
      var destRect = new System.Drawing.Rectangle(0, 0, width, height);
      var destImage = new System.Drawing.Bitmap(width, height);

      destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

      using (var graphics = System.Drawing.Graphics.FromImage(destImage))
      {
        graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

        using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
        {
          wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
          graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel, wrapMode);
        }
      }

      return destImage;
    }

  }
}
