using System;
namespace MyToolkit.Base
{
    public enum HistoArray { rgbArray, rArray, gArray, bArray };

    public class Histo
    {
        public int[] rgbHisto = new int[256];     //R+G+B/3
        public int[] rHisto = new int[256];     //R
        public int[] gHisto = new int[256];     //G
        public int[] bHisto = new int[256];     //B
        public int pixelCount = 0;              // total pixels in image
        public int largestValue = 0;            //largest value in any of arrays

        enum RGB { Blue, Green, Red };

        public Histo() { }
        public Histo(byte[] rgb)
        {
            int red, green, blue;
            for (int i = 0; i < rgb.Length; i += 4)
            {
                red = rgb[i + (int)RGB.Red];
                green = rgb[i + (int)RGB.Green];
                blue = rgb[i + (int)RGB.Blue];
                int rgbValue = (int)Math.Floor((double)(red + green + blue) / 3);

                rgbHisto[rgbValue]++;
                rHisto[red]++;
                gHisto[green]++;
                bHisto[blue]++;
            }
            CalcHistoRange();
        }


        public int[] GetHistoArray(HistoArray ha)
        {
            int[] retArray = new int[256];

            switch (ha)
            {
                case HistoArray.rgbArray:
                    retArray = rgbHisto;
                    break;
                case HistoArray.rArray:
                    retArray = rHisto;
                    break;
                case HistoArray.gArray:
                    retArray = gHisto;
                    break;
                case HistoArray.bArray:
                    retArray = bHisto;
                    break;
            }
            return retArray;
        }//GetHistoArray()


        public void PutHistoArray(int[] arry, HistoArray ha)
        {
            switch (ha)
            {
                case HistoArray.rgbArray:
                    rgbHisto = arry;
                    break;
                case HistoArray.rArray:
                    rHisto = arry;
                    break;
                case HistoArray.gArray:
                    gHisto = arry;
                    break;
                case HistoArray.bArray:
                    bHisto = arry;
                    break;
            }
        }//PutHistoArray()


        public void CalcHistoRange()
        {
            pixelCount = largestValue = 0;
            for (int i = 0; i < 256; i++)
            {
                pixelCount += rgbHisto[i];
                if (largestValue < rgbHisto[i]) largestValue = rgbHisto[i];
                if (largestValue < rHisto[i]) largestValue = rHisto[i];
                if (largestValue < gHisto[i]) largestValue = gHisto[i];
                if (largestValue < bHisto[i]) largestValue = bHisto[i];
            }
        }//CalcHistoRange()
    } //class Histo
}
