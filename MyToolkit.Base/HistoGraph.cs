using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace MyToolkit.Base
{
    /// <summary>
    /// </summary>
    [TemplatePart(Name = "PART_HistoGraph", Type = typeof(Grid))]
    public class HistoGraph : Control
    {

        public static readonly DependencyProperty HistoProperty;
        public Histo HistoValue
        {
            get { return (Histo)GetValue(HistoProperty); }
            set { SetValue(HistoProperty, value); }
        }

        static HistoGraph()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HistoGraph), new FrameworkPropertyMetadata(typeof(HistoGraph)));
            HistoProperty = DependencyProperty.Register(
              "HistoValue", typeof(Histo), typeof(HistoGraph),
              new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnHistoValueChanged)));

        }

        private byte[] origRgb;     //original, complete rgb data from full image
        public byte[] OrigRgb
        {
            get { return origRgb; }
        }
        private BitmapSource origBitmapSource;    //original bms for full image

        public virtual BitmapSource Source
        {
            get
            {
                return origBitmapSource;
            }
            set
            {
                origBitmapSource = value;
                HistoValue = null;
                if (value != null)
                {
                    origRgb = GetRgbData(origBitmapSource);
                    HistoValue = new Histo(origRgb);
                }
            }
        }

        private static byte[] GetRgbData(BitmapSource bms)
        {
            //calc its image stride
            int stride = bms.PixelWidth * ((bms.Format.BitsPerPixel + 7) / 8);
            //calc its image data length
            int dataLength = stride * bms.PixelHeight;
            //save orignal bms data needed to clone a BitmapSource using new passed data

            byte[] rgb = new byte[dataLength];
            bms.CopyPixels(rgb, stride, 0);
            return rgb;
        }

        private int _MinVal = 0;
        public virtual int MinVal { get { return _MinVal; } set { _MinVal = value; } }
        private int _MaxVal = 254;
        public virtual int MaxVal { get { return _MaxVal; } set { _MaxVal = value; } }

        private Grid g;         //resizable grid for polygon graphs

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //attach the Thumb drag events to the StackPanel
            g = GetTemplateChild("PART_HistoGraph") as Grid;
        } // OnApplyTemplate()


        //This gets called BOTH when external class set WhiteValue and when WhiteValue is set within the class.
        static void OnHistoValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Histo histo = (Histo)args.NewValue;
            HistoGraph hg = obj as MyToolkit.Base.HistoGraph;
            if (hg != null)          //you may not have an instance yet
                hg.OnHistoValueChanged(histo);   //call INSTANCE method passing args

        } // static OnHistoValueChanged()


        Polygon CreateHistoPolygon(int[] histoArray, int pixelCount, double yMax)
        {
            PointCollection ptCollection = new PointCollection();
            ptCollection.Add(new Point(0, yMax));  //set origin
            int iMax = (histoArray.Length < MaxVal + 1) ? histoArray.Length - 1 : MaxVal;
            double dFactor = 255.0 / (iMax - MinVal);

            for (int i = MinVal; i <= iMax; i++)
            {
                double yValue = ((double)histoArray[i] / pixelCount) * 100;
                yValue = -yValue + yMax;        //flip it and move it down
                ptCollection.Add(new Point((i - MinVal) * dFactor, yValue));
            }
            ptCollection.Add(new Point(histoArray.Length - 1, yMax));  //set end point
            Polygon poly = new Polygon();
            poly.Stretch = Stretch.Fill;
            poly.Points = ptCollection;
            poly.StrokeThickness = 1;
            poly.SnapsToDevicePixels = true;
            return poly;
        }


        void OnHistoValueChanged(Histo histo)
        {
            RefreshHistogram(histo);
            /*
            g.Children.Clear();
            if (histo == null)
                return;
            histo.CalcHistoRange(); //update Histo range
            double yMax = ((double)histo.largestValue / histo.pixelCount) * 100;

            Polygon poly = CreateHistoPolygon(histo.rHisto, histo.pixelCount, yMax);
            poly.Stroke = Brushes.Red;
            g.Children.Add(poly);

            poly = CreateHistoPolygon(histo.gHisto, histo.pixelCount, yMax);
            poly.Stroke = Brushes.Green;
            g.Children.Add(poly);

            poly = CreateHistoPolygon(histo.bHisto, histo.pixelCount, yMax);
            poly.Stroke = Brushes.Blue;
            g.Children.Add(poly);
            */
            /*
            poly = CreateHistoPolygon(histo.rgbHisto, histo.pixelCount, yMax);
            poly.Stroke = Brushes.Black;
            poly.StrokeThickness = 2;
            g.Children.Add(poly);
             * */
        } // instance OnHistoValueChanged()

        public void RefreshHistogram()
        {
            if (HistoValue == null) return;
            RefreshHistogram(HistoValue);
        }
        private void RefreshHistogram(Histo histo)
        {
            g.Children.Clear();
            if (histo == null)
                return;
            histo.CalcHistoRange(); //update Histo range
            double yMax = ((double)histo.largestValue / histo.pixelCount) * 100;

            Polygon poly = CreateHistoPolygon(histo.rHisto, histo.pixelCount, yMax);
            poly.Stroke = Brushes.Red;
            g.Children.Add(poly);

            poly = CreateHistoPolygon(histo.gHisto, histo.pixelCount, yMax);
            poly.Stroke = Brushes.Green;
            g.Children.Add(poly);

            poly = CreateHistoPolygon(histo.bHisto, histo.pixelCount, yMax);
            poly.Stroke = Brushes.Blue;
            g.Children.Add(poly);
        }
    }
}
