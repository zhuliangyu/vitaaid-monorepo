using System;
using System.IO;
using System.Net;
using System.Threading;

namespace MyToolkit.Base
{
  public class CurrencyExchange
  {
    public static float GetCurrencyRate(string sFrom, string sTo, DateTime dtHistory)
    {
      try
      {
        Stream objStream;
        StreamReader objSR;
        System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

        string str = "http://currencies.apps.grandtrunk.net/getrate/" + dtHistory.ToString("yyyy-MM-dd") + "/" + sFrom + "/" + sTo;
        HttpWebResponse getresponse = null;
        Exception prevEx = null;
        for (int i  = 0; i < 10; i++)
        {
          try
          {
            Console.WriteLine("{0}", str);
            HttpWebRequest wrquest = (HttpWebRequest)WebRequest.Create(str);
            getresponse = (HttpWebResponse)wrquest.GetResponse();
            break;
          }
          catch (Exception ex)
          {
            Console.WriteLine("{0}, try {1}", i + 1, str);
            Thread.Sleep(1000);
            prevEx = ex;
          }
        }
        if (getresponse == null)
          throw prevEx;

        objStream = getresponse.GetResponseStream();
        objSR = new StreamReader(objStream, encode, true);
        string strResponse = objSR.ReadToEnd();

        if (getresponse.StatusCode == HttpStatusCode.OK)
        {
          return float.Parse(strResponse);
        }
        return -1;
      }
      catch (Exception ex) { throw ex; }
    }
  }
}
