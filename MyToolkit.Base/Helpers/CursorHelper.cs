using System;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;

namespace MyToolkit.Base
{
    class __foo_CursorHelper
    {
        public __foo_CursorHelper()
        {
            try
            {
                var x = new MemoryStream(Properties.Resources.loading);
                if (CursorHelper.LoadingCursor == null)
                    CursorHelper.LoadingCursor = new Cursor(new MemoryStream(Properties.Resources.loading));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
    public class CursorHelper
    {
        private struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        public static Cursor LoadingCursor = null;
        static __foo_CursorHelper init_foo = new __foo_CursorHelper();

        [DllImport("user32.dll")]
        private static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static Boolean DestroyIcon(IntPtr handle);

        
        private static Cursor InternalCreateCursor(System.Drawing.Bitmap bmp,
            int xHotSpot, int yHotSpot)
        {
            try
            {
                IconInfo tmp = new IconInfo();
                GetIconInfo(bmp.GetHicon(), ref tmp);
                tmp.xHotspot = xHotSpot;
                tmp.yHotspot = yHotSpot;
                tmp.fIcon = false;

                IntPtr ptr = CreateIconIndirect(ref tmp);
                SafeIconHandle handle = new SafeIconHandle(ptr);//SafeFileHandle(ptr, true);
                return CursorInteropHelper.Create(handle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Cursor CreateCursor(String sImgPath, int iPixelWidth, int iPixelHeight, int xHotSpot = 0, int yHotSpot = 0)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap(iPixelWidth, iPixelHeight, 96, 96, PixelFormats.Pbgra32);
            BitmapImage oBI = new BitmapImage(new Uri(sImgPath));
            DrawingVisual dv = new DrawingVisual();
            DrawingContext dc;
            dc = dv.RenderOpen();
            dc.DrawImage(oBI, new System.Windows.Rect(0, 0, iPixelWidth, iPixelHeight));
            dc.Close();
            rtb.Render(dv);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            MemoryStream ms = new MemoryStream();
            encoder.Save(ms);

            System.Drawing.Bitmap oBMP = new System.Drawing.Bitmap(ms);

            ms.Close();
            ms.Dispose();

            Cursor cur = InternalCreateCursor(oBMP, xHotSpot, yHotSpot);
            return cur;
        }

        public static Cursor CreateCursor(UIElement element, int xHotSpot, int yHotSpot)
        {
            element.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
            element.Arrange(new Rect(0, 0, element.DesiredSize.Width,
                element.DesiredSize.Height));

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)element.DesiredSize.Width,
                (int)element.DesiredSize.Height, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(element);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            MemoryStream ms = new MemoryStream();
            encoder.Save(ms);

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);

            ms.Close();
            ms.Dispose();

            Cursor cur = InternalCreateCursor(bmp, xHotSpot, yHotSpot);

            bmp.Dispose();

            return cur;
        }
        /*
        public static Cursor CreateCursor(this Icon icon, bool includeCrossHair, System.Drawing.Color crossHairColor)
        {
            if (icon == null)
                return Cursors.Arrow;

            // create an empty image
            int width = icon.Width;
            int height = icon.Height;

            using (var cursor = new Bitmap(width * 2, height * 2))
            {
                // create a graphics context, so that we can draw our own cursor
                using (var gr = System.Drawing.Graphics.FromImage(cursor))
                {
                    // a cursor is usually 32x32 pixel so we need our icon in the lower right part of it
                    gr.DrawIcon(icon, new Rectangle(width, height, width, height));

                    if (includeCrossHair)
                    {
                        using (var pen = new System.Drawing.Pen(crossHairColor))
                        {
                            // draw the cross-hair
                            gr.DrawLine(pen, width - 3, height, width + 3, height);
                            gr.DrawLine(pen, width, height - 3, width, height + 3);
                        }
                    }
                }

                try
                {
                    using (var stream = new MemoryStream())
                    {
                        // Save to .ico format
                        var ptr = cursor.GetHicon();
                        var tempIcon = Icon.FromHandle(ptr);
                        tempIcon.Save(stream);

                        int x = cursor.Width / 2;
                        int y = cursor.Height / 2;

                        #region Convert saved stream into .cur format

                        // set as .cur file format
                        stream.Seek(2, SeekOrigin.Begin);
                        stream.WriteByte(2);

                        // write the hotspot information
                        stream.Seek(10, SeekOrigin.Begin);
                        stream.WriteByte((byte)(width));
                        stream.Seek(12, SeekOrigin.Begin);
                        stream.WriteByte((byte)(height));

                        // reset to initial position
                        stream.Seek(0, SeekOrigin.Begin);

                        #endregion


                        DestroyIcon(tempIcon.Handle);  // destroy GDI resource

                        return new Cursor(stream);
                    }
                }
                catch (Exception)
                {
                    return Cursors.Arrow;
                }
            }
        }
        */
    }
}
