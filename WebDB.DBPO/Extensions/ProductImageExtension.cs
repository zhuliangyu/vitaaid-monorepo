using MySystem.Base.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WebDB.DBBO;

namespace WebDB.DBPO.Extensions
{
    public static class ProductImageExtension
    {

        private static void getWidthHeight(string path, out double width, out double height)
        {
            width = 0;
            height = 0;
            try
            {
                using (var imageStream = File.OpenRead(path))
                {
                    var decoder = BitmapDecoder.Create(imageStream, BitmapCreateOptions.IgnoreColorProfile,
                        BitmapCacheOption.Default);
                    height = decoder.Frames[0].PixelHeight;
                    width = decoder.Frames[0].PixelWidth;
                }
            }
            catch (Exception) { }
        }
        public static void loadImageWidthHeight(this ProductImage self, string basePath, string basePath4Large)
        {
            try
            {
                double width, height;
                getWidthHeight(basePath + self.ImageName, out width, out height);
                self.Width = width;
                self.Height = height;

                getWidthHeight(basePath4Large + self.LargeImageName, out width, out height);
                self.LargeWidth = width;
                self.LargeHeight = height;
            }
            catch (Exception){}
        }
    }
}
