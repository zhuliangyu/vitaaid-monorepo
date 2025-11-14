using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToolkit.Base
{
	public interface IReportViewerDialog
	{
		ReportViewerCtl oReportCtl { get; }
		ReportViewer oViewer { get; }
		ZoomMode ZoomMode { get; set; }
		void ClearDataResource();
		void AddDataSource(Type type, object NewValue);
		void AddDataSource(object NewValue);
		void AddDataSource(string sName, object NewValue);
		void RefreshReport();

		void SaveAs(string sPath, string sAbsoultFile, bool bOverwrite = true);
		bool? ShowDialog();
		byte[] Render(string Format = "PDF");
	}
}
