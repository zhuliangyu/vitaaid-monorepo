using MySystem.Base.Extensions;
using MyToolkit.Base.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit;

namespace MyToolkit.Base
{
  public class CommonUtil
  {
    public static BusyIndicator oBusyIndicator { get; set; } = null;

    //public static void dgSelItem(DataGrid oDataGrid, Func<Object, bool> predicate, int iSelIdx = 0)
    //{
    //    try
    //    {
    //        if (oDataGrid == null || oDataGrid.ItemsSource == null)
    //            return;
    //        int idx = -1;

    //        foreach (var o in oDataGrid.ItemsSource)
    //        {
    //            idx++;
    //            if (predicate(o))
    //            {
    //                oDataGrid.SelectedIndex = idx;
    //                return;
    //            }
    //        }
    //        if (iSelIdx <= idx)
    //            oDataGrid.SelectedIndex = (iSelIdx <= -1) ? 0 : iSelIdx;
    //        else
    //            oDataGrid.SelectedIndex = idx;
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //    finally
    //    {
    //        if (oDataGrid.SelectedItem != null)
    //            oDataGrid.ScrollIntoView(oDataGrid.SelectedItem);
    //    }
    //}
    //public static DateTime RetrieveLinkerTimestamp(Assembly oAssembly)
    //{
    //    return File.GetLastWriteTime(oAssembly.Location);

    /*
    string filePath = oAssembly.Location;
    const int c_PeHeaderOffset = 60;
    const int c_LinkerTimestampOffset = 8;
    byte[] b = new byte[2048];
    System.IO.Stream s = null;

    try
    {
        s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        s.Read(b, 0, 2048);
    }
    finally
    {
        if (s != null)
        {
            s.Close();
        }
    }

    int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
    int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
    DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
    dt = dt.AddSeconds(secondsSince1970);
    dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
    return dt;
    */
    //}
    public static void SetBusyState(bool busy)
    {
      Cursor oldCursor = Mouse.OverrideCursor;
      Mouse.OverrideCursor = busy ? Cursors.Wait : null;

      if (oldCursor != Mouse.OverrideCursor)
      {
        new DispatcherTimer(TimeSpan.FromSeconds(0), DispatcherPriority.ApplicationIdle,
            dispatcherTimer_Tick, Application.Current.Dispatcher);
      }
    }

    public static async Task SetBusyStateAsync(bool busy)
    {
      if (oBusyIndicator != null)
        oBusyIndicator.IsBusy = busy;
      if (busy)
        await Task.Delay(500);
    }

    /// <summary>
    /// Handles the Tick event of the dispatcherTimer control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private static void dispatcherTimer_Tick(object sender, EventArgs e)
    {
      var dispatcherTimer = sender as DispatcherTimer;
      if (dispatcherTimer != null)
      {
        SetBusyState(false);
        dispatcherTimer.Stop();
      }
    }

    public static IEnumerable<T> FindLogicalChildren<T>(DependencyObject depObj) where T : DependencyObject
    {
      if (depObj != null)
      {
        foreach (object rawChild in LogicalTreeHelper.GetChildren(depObj))
        {
          if (rawChild is DependencyObject)
          {
            DependencyObject child = (DependencyObject)rawChild;
            if (child is T)
            {
              yield return (T)child;
            }

            foreach (T childOfChild in FindLogicalChildren<T>(child))
            {
              yield return childOfChild;
            }
          }
        }
      }
    }

    public static void SortDataGrid(DataGrid dataGrid, int columnIndex = 0, ListSortDirection sortDirection = ListSortDirection.Ascending)
    {
      var column = dataGrid.Columns[columnIndex];

      // Clear current sort descriptions
      dataGrid.Items.SortDescriptions.Clear();

      // Add the new sort description
      dataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, sortDirection));

      // Apply sort
      foreach (var col in dataGrid.Columns)
      {
        col.SortDirection = null;
      }
      column.SortDirection = sortDirection;

      // Refresh items to display sort
      //dataGrid.Items.Refresh();
    }

    public static void ClearSortDataGrid(DataGrid dataGrid)
    {
      // Clear current sort descriptions
      dataGrid.Items.SortDescriptions.Clear();

      // Apply sort
      foreach (var col in dataGrid.Columns)
      {
        col.SortDirection = null;
      }
    }


    public static System.Drawing.Bitmap CombineBitmap(string[] files)
    {
      //read all images into memory
      List<System.Drawing.Bitmap> images = new List<System.Drawing.Bitmap>();
      System.Drawing.Bitmap finalImage = null;

      try
      {
        int width = 0;
        int height = 0;

        foreach (string image in files)
        {
          //create a Bitmap from the file and add it to the list
          System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(image);

          //update the size of the final bitmap
          width += bitmap.Width;
          height = bitmap.Height > height ? bitmap.Height : height;

          images.Add(bitmap);
        }

        //create a bitmap to hold the combined image
        finalImage = new System.Drawing.Bitmap(width, height);

        //get a graphics object from the image so we can draw on it
        using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
        {
          //set background color
          g.Clear(System.Drawing.Color.Black);

          //go through each image and draw it on the final image
          int offset = 0;
          foreach (System.Drawing.Bitmap image in images)
          {
            g.DrawImage(image,
              new System.Drawing.Rectangle(offset, 0, image.Width, image.Height));
            offset += image.Width;
          }
        }

        return finalImage;
      }
      catch (Exception ex)
      {
        if (finalImage != null)
          finalImage.Dispose();

        throw ex;
      }
      finally
      {
        //clean up memory
        foreach (System.Drawing.Bitmap image in images)
        {
          image.Dispose();
        }
      }
    }

    public static BitmapImage LoadBitmapFromResource(string pathInApplication, Assembly assembly = null)
    {
      if (assembly == null)
      {
        assembly = Assembly.GetCallingAssembly();
      }

      if (pathInApplication[0] == '/')
      {
        pathInApplication = pathInApplication.Substring(1);
      }
      return new BitmapImage(new Uri(@"pack://application:,,,/" + assembly.GetName().Name + ";component/" + pathInApplication, UriKind.Absolute));
    }

    [DllImport("gdi32.dll")]
    public static extern int GetDeviceCaps(IntPtr hDc, int nIndex);

    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);
    public const int LOGPIXELSX = 88;
    public const int LOGPIXELSY = 90;

    /// <summary>
    /// Transforms device independent units (1/96 of an inch)
    /// to pixels
    /// </summary>
    /// <param name="unitX">a device independent unit value X</param>
    /// <param name="unitY">a device independent unit value Y</param>
    /// <param name="pixelX">returns the X value in pixels</param>
    /// <param name="pixelY">returns the Y value in pixels</param>
    public static void TransformToPixels(Visual visual,
                          double unitX,
                          double unitY,
                          out int pixelX,
                          out int pixelY)
    {
      Matrix matrix;
      var source = PresentationSource.FromVisual(visual);
      if (source != null)
      {
        matrix = source.CompositionTarget.TransformToDevice;
      }
      else
      {
        using (var src = new HwndSource(new HwndSourceParameters()))
        {
          matrix = src.CompositionTarget.TransformToDevice;
        }
      }

      pixelX = (int)(matrix.M11 * 96 * unitX);
      pixelY = (int)(matrix.M22 * 96 * unitY);
    }
    private static void UpdateBindingState(BindingExpression binding, bool bReadOnly)
    {
      if (binding == null) return;
      bool b = false;
      if ((b = binding.ParentBinding?.ValidationRules?.Any() ?? false))
        binding.ParentBinding.ValidationRules[0].ValidatesOnTargetUpdated = !bReadOnly;
      binding.UpdateTarget();
    }
    public static void ClearData(DependencyObject depObj)
    {
      IEnumerable<FrameworkElement> oChilds = FindLogicalChildren<FrameworkElement>(depObj);
      //FrameworkElement oUI = null;
      foreach (DependencyObject child in oChilds)
      {
        //oUI = child as FrameworkElement;
        //if (oUI != null && (oUI.Tag as string) == "XUI")
        //  continue;
        if (child is TextBox)
        {
          ((TextBox)child).Text = "";
        }
        else if (child is ComboBox)
        {
          ((ComboBox)child).SelectedItem = null;
        }
        else if (child is TextBlock)
        {
          ((TextBlock)child).Text = null;
        }
        else if (child is DataGrid)
        {
          ((DataGrid)child).ItemsSource = null;
        }
      }
    }
    public static bool ValidateData(DependencyObject depObj)
    {
      IEnumerable<FrameworkElement> oChilds = FindLogicalChildren<FrameworkElement>(depObj);
      FrameworkElement oUI = null;
      foreach (DependencyObject child in oChilds)
      {
        oUI = child as FrameworkElement;
        if (oUI != null && (oUI.Tag as string) == "XUI")
          continue;
        if (!((child as TextBox)?.GetBindingExpression(TextBox.TextProperty)?.ValidateWithoutUpdate() ?? true))
        {
          ((TextBox)child).Focus();
          return false;
        }
        if (!((child as ComboBox)?.GetBindingExpression(ComboBox.SelectedValueProperty)?.ValidateWithoutUpdate() ?? true))
        {
          ((ComboBox)child).Focus();
          return false;
        }
        if (!((child as ComboBox)?.GetBindingExpression(ComboBox.SelectedItemProperty)?.ValidateWithoutUpdate() ?? true))
        {
          ((ComboBox)child).Focus();
          return false;
        }
      }
      return true;
    }
    public static void SetReadonly<T>(DependencyObject depObj, bool bReadOnly) where T : DependencyObject
    {
      if (depObj != null)
      {
        IEnumerable<T> oChilds = FindLogicalChildren<T>(depObj);
        FrameworkElement oUI = null;
        foreach (DependencyObject child in oChilds)
        {
          oUI = child as FrameworkElement;
          if (oUI != null && (oUI.Tag as string) == "XUI")
            continue;
          if (child is TextBox)
          {
            ((TextBox)child).IsReadOnly = bReadOnly;
            UpdateBindingState(((TextBox)child).GetBindingExpression(TextBox.TextProperty), bReadOnly);
          }
          else if (child is ComboBox)
            ((ComboBox)child).IsHitTestVisible = !bReadOnly;
          else if (child is ToggleButton)
            ((ToggleButton)child).IsHitTestVisible = !bReadOnly;
          else if (child is Button)
            ((Button)child).IsEnabled = !bReadOnly;
          else if (child is TextBoxBase)
            ((TextBoxBase)child).IsReadOnly = bReadOnly;
          else if (child is DataGrid)
            ((DataGrid)child).IsEnabled = !bReadOnly;
          else if (child is UIElement)
            ((UIElement)child).IsEnabled = !bReadOnly;
        }
      }
    }
    public static void UpdateTarget<T>(DependencyObject depObj) where T : DependencyObject
    {
      if (depObj != null)
      {
        IEnumerable<T> oChilds = FindLogicalChildren<T>(depObj);
        FrameworkElement oUI = null;
        foreach (DependencyObject child in oChilds)
        {
          oUI = child as FrameworkElement;
          if (child is TextBox && oUI.GetBindingExpression(TextBox.TextProperty) != null)
            oUI.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
          else if (child is TextBlock && oUI.GetBindingExpression(TextBlock.TextProperty) != null)
            oUI.GetBindingExpression(TextBlock.TextProperty).UpdateTarget();
          else if (child is Label && oUI.GetBindingExpression(Label.ContentProperty) != null)
            oUI.GetBindingExpression(Label.ContentProperty).UpdateTarget();
          else if (child is ComboBox)
          {
            if (oUI.GetBindingExpression(ComboBox.SelectedItemProperty) != null)
              oUI.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateTarget();
            if (oUI.GetBindingExpression(ComboBox.SelectedValueProperty) != null)
              oUI.GetBindingExpression(ComboBox.SelectedValueProperty).UpdateTarget();
          }
        }
      }
    }

    public static void getWeeklyPeriod(DateTime dtRefDate, out DateTime rtnStartDate, out DateTime rtnEndDate)
    {
      try
      {
        rtnStartDate = new DateTime(dtRefDate.Year, dtRefDate.Month, dtRefDate.Day, 0, 0, 0).AddDays(DayOfWeek.Sunday - dtRefDate.DayOfWeek);
        rtnEndDate = new DateTime(dtRefDate.Year, dtRefDate.Month, dtRefDate.Day, 23, 59, 59).AddDays(DayOfWeek.Saturday - dtRefDate.DayOfWeek);
      }
      catch (Exception ex) { throw ex; }
    }
    public static void getSemiMonthlyPeriod(DateTime dtRefDate, out DateTime rtnStartDate, out DateTime rtnEndDate)
    {
      try
      {
        if (dtRefDate.Day <= 15)
        {
          rtnStartDate = new DateTime(dtRefDate.Year, dtRefDate.Month, 1, 0, 0, 0);
          rtnEndDate = new DateTime(dtRefDate.Year, dtRefDate.Month, 15, 23, 59, 59);
        }
        else
        {
          rtnStartDate = new DateTime(dtRefDate.Year, dtRefDate.Month, 16, 0, 0, 0);
          rtnEndDate = new DateTime(dtRefDate.Year, dtRefDate.Month, 1, 23, 59, 59).AddMonths(1).AddDays(-1);
        }
      }
      catch (Exception ex) { throw ex; }
    }
    public static void getMonthlyPeriod(DateTime dtRefDate, out DateTime rtnStartDate, out DateTime rtnEndDate)
    {
      try
      {
        rtnStartDate = new DateTime(dtRefDate.Year, dtRefDate.Month, 1, 0, 0, 0);
        rtnEndDate = new DateTime(dtRefDate.Year, dtRefDate.Month, 1, 23, 59, 59).AddMonths(1).AddDays(-1);
      }
      catch (Exception ex) { throw ex; }
    }

    public static DateTime GetFiscalYear()
    {
      try
      {
        DateTime Now = DateTime.Now;
        DateTime FiscalByThisYear = new DateTime(Now.Year, 10, 1, 0, 0, 0);
        if (Now >= new DateTime(Now.Year, 10, 1, 0, 0, 0))
          return FiscalByThisYear;
        else
          return new DateTime(Now.Year - 1, 10, 1, 0, 0, 0);
      }
      catch (Exception)
      {

        throw;
      }
    }

    //'1cm = 37.79527559055 pixels
    //const Single cmToPx = 37.79527559055F;

    //public static string SetMaxFont(string Text, Single boxWidth, Single boxHeight, int FontMax, int FontMin)
    //{
    //	for (int i = FontMax; i >= FontMin; i--)
    //		if (IsTextSmaller(Text, i, boxWidth, boxHeight))
    //			return i.ToString() + "pt";
    //	return "10pt";
    //}

    //public static bool IsTextSmaller(string Text, int fontValue, Single boxWidth, Single boxHeight)
    //{
    //	Font stringFont = new System.Drawing.Font("Arial", fontValue);
    //	SizeF stringSize = new SizeF();
    //	SizeF boxSize = new SizeF(boxWidth* cmToPx, boxHeight* cmToPx * 10); //we set box height bigger than textbox that we check
    //	Bitmap bitmap = new Bitmap(1,1);
    //	Graphics g = System.Drawing.Graphics.FromImage(bitmap);
    //	g.PageUnit = System.Drawing.GraphicsUnit.Pixel;
    //	stringSize = g.MeasureString(Text, stringFont, boxSize);

    //	bitmap = null;
    //	return (stringSize.Height < (boxHeight * cmToPx));
    //}

    //'1cm = 37.79527559055 pixels


    public static string SetMaxFont(string Text, Single boxWidth, Single boxHeight, int FontMax, int FontMin)
    {
      for (int i = FontMax; i >= FontMin; i--)
        if (IsTextSmaller(Text, i, boxWidth, boxHeight))
          return i.ToString() + "pt";
      return "10pt";
    }
    public static bool IsTextSmaller(string Text, int fontValue, Single boxWidth, Single boxHeight)
    {
      Single cmToPx = 37.79527559055F;
      Font stringFont = new System.Drawing.Font("Arial", fontValue);
      SizeF stringSize = new SizeF();
      SizeF boxSize = new SizeF(boxWidth * cmToPx, boxHeight * cmToPx * 10); //we set box height bigger than textbox that we check
      Bitmap bitmap = new Bitmap(1, 1);
      Graphics g = System.Drawing.Graphics.FromImage(bitmap);
      g.PageUnit = System.Drawing.GraphicsUnit.Pixel;
      stringSize = g.MeasureString(Text, stringFont, boxSize);

      bitmap = null;
      return (stringSize.Height < (boxHeight * cmToPx));
    }
    public static void killProcess(string ProcessName, int excludePID = 0)
    {
      var oldProcs = System.Diagnostics.Process.GetProcesses()
                          .Where(p => p.ProcessName.ToLower() == ProcessName.ToLower() && p.Id != excludePID)
                          .ToList();
      if (oldProcs.Any())
      {
        foreach (var app in oldProcs)
        {
          app.Kill();
          Thread.Sleep(500);
        }
      }
    }
    public static string RandomString(int size)
    {
      try
      {
        StringBuilder builder = new StringBuilder();
        Random random = new Random(Guid.NewGuid().GetHashCode());

        char ch;
        for (int i = 0; i < size; i++)
        {
          ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
          builder.Append(ch);
        }

        return builder.ToString();
      }
      catch (Exception ex) { throw ex; }
    }
    public static string DataRowToStr(DataRow dr)
    {
      string s = "";
      dr.ItemArray.ForEachWithIndex((x, _) => s += x.ToString() + ",");
      if (s.Last() == ',')
        return s.Substring(0, s.Length - 1);
      else
        return s;
    }
    public static DataTable ImportCSV(string srcFilePath, bool hasHeader, int skipFirstRows = 0, int skipLastRows = 0)
    {
      // Initilization  
      DataTable datatable = new DataTable();
      StreamReader sr = null;
      string line = "";
      long toIdx = long.MaxValue;
      long lineIdx = 0;
      try
      {
        if (skipLastRows != 0)
        {
          (new FileStream(srcFilePath, FileMode.Open, FileAccess.Read)).Also(x =>
          {
            toIdx = x.CountLines() - skipLastRows;
            x.Close();
          });
        }
        // Creating data table without header.  
        using (sr = new StreamReader(new FileStream(srcFilePath, FileMode.Open, FileAccess.Read)))
        {
          // Initialization.
          for (; skipFirstRows > 0; skipFirstRows--)
          {
            sr.ReadLine();
            lineIdx++;
          }

          line = sr.ReadLine();
          lineIdx++;
          //action?.Invoke(line);
          string[] headers = line?.SplitEx(',');

          DataRow dr = datatable.NewRow();

          // Preparing header.  
          for (int i = 0; i < headers.Length; i++)
          {
            // Verification.  
            if (hasHeader)
            {
              // Setting.  
              datatable.Columns.Add(headers[i]);
            }
            else
            {
              // Setting.  
              datatable.Columns.Add("COL -" + i);
              dr[i] = headers[i];
            }
          }

          // Verification.  
          if (!hasHeader)
          {
            // Adding.  
            datatable.Rows.Add(dr);
          }

          // Adding data.  
          while ((line = sr.ReadLine()) != null && lineIdx < toIdx)
          {
            lineIdx++;
            if (string.IsNullOrWhiteSpace(line))
              continue;
            //action?.Invoke(line);
            // Initialization.  
            string[] rows = line.SplitEx(',');
            dr = datatable.NewRow();

            // Verification  
            if (string.IsNullOrEmpty(line))
            {
              // Info.  
              continue;
            }

            // Adding row.  
            for (int i = 0; i < headers.Length; i++)
            {
              // Setting.  
              dr[i] = rows[i];
            }

            // Adding.  
            datatable.Rows.Add(dr);
          }
        }
      }
      catch (Exception ex)
      {
        // Info.  
        throw new Exception(((line == "") ? ""  : line + "\n") + ex.Message);
      }
      finally
      {
        // Closing.  
        sr?.Dispose();
        sr?.Close();
      }

      // Info.  
      return datatable;
    }

    public static bool IsImageFile(string fileName)
    {
      if (string.IsNullOrWhiteSpace(fileName))
        return false;
      var lower_fileName = fileName.ToLower();
      if (lower_fileName.EndsWith(".gif") ||
          lower_fileName.EndsWith(".jpg") ||
          lower_fileName.EndsWith(".jpeg") ||
          lower_fileName.EndsWith(".bmp") ||
          lower_fileName.EndsWith(".png") ||
          lower_fileName.EndsWith(".tiff"))
        return true;
      return false;
    }
  }
}
