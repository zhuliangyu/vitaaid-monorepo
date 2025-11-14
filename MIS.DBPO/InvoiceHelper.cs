using MIS.DBBO;
using MyHibernateUtil;
using MyHibernateUtil.Extensions;
using MySystem.Base.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using static MIS.DBPO.DBPOServiceHelper;

namespace MIS.DBPO
{
	public static class InvoiceHelper
	{
		//public static POSInvoice LoadInvoice(string sInvoiceNo) => LoadInvoice(sInvoiceNo, MISDB[eST.SESSION0]);
		/*
		public static POSInvoice LoadInvoice(SessionProxy oSession, string sInvoiceNo)
		{
			try
			{
				return oSession.Query<POSInvoice>()
					.Where(x => x.InvoiceNo == sInvoiceNo)
					.ToList().FirstOrDefault();
				//if (oInvoice.oItemDetails == null)
				//	oInvoice.oItemDetails = new ObservableCollection<POSInvoiceDetail>();
				//oInvoice.oItemDetails.Clear();
				//IList<POSInvoiceDetail> oInvoiceDetails = MISDBBO.oSession.GetXObjs<POSInvoiceDetail>("x.InvoiceNO='" + sInvoiceNo + "'", "ID");
				//foreach (POSInvoiceDetail oDetail in oInvoiceDetails)
				//	oInvoice.oItemDetails.Add(oDetail);
			}
			catch (Exception)
			{
				return null;
			}
		}
		*/
		//public static void DeleteInvoice(string sInvoiceNo) => DeleteInvoice(sInvoiceNo, MISDB[eST.SESSION0]);
		/*
		public static void DeleteInvoice(string sInvoiceNo, SessionProxy oSession)
		{
			try
			{
				LoadInvoice(oSession, sInvoiceNo)?.Let(oOldInvoice => {
					foreach (POSInvoiceDetail oOldDetail in oOldInvoice?.oInvoiceDetails)
						oSession.DeleteObj(oOldDetail, true);
					oSession.DeleteObj(oOldInvoice, true);
				});
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
		}
		*/
		public static void DeleteInvoice(SessionProxy oSession, VAOrder order)
		{
			try
			{
				oSession.QueryDataElement<POSInvoice>().Where(x => x.oOrder.ID == order.ID)
					.UniqueOrDefault()
				 ?.Also(oOldInvoice => {
					 foreach (POSInvoiceDetail oOldDetail in oOldInvoice?.oInvoiceDetails)
						 oSession.DeleteObj(oOldDetail, true);
					 oSession.DeleteObj(oOldInvoice, true);
				 });
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
		}
		public static (decimal TotalNetSales, decimal TotalDiscount, decimal TotalAdjustment, decimal TotalShipping, decimal TotalSubTotal, 
									 decimal TotalTax, decimal TotalTotal, decimal TotalCredit, decimal TotalBalanceDue) StatisticalData(IList<POSInvoice> oInvoices)
    {
			try
			{
				decimal TotalNetSales = 0, TotalDiscount = 0, TotalAdjustment = 0, TotalShipping = 0, TotalSubTotal = 0, 
								TotalTax = 0, TotalTotal = 0, TotalCredit = 0, TotalBalanceDue = 0;

				oInvoices.Action(oInvoice =>
				{
					TotalNetSales += oInvoice.NetSales;
					TotalDiscount += (oInvoice.Adjustment.HasValue) ? oInvoice.Adjustment.Value : 0;
					TotalAdjustment += oInvoice.ExtraAdjustmentAmount;
					TotalShipping += (oInvoice.ShipHandling.HasValue) ? oInvoice.ShipHandling.Value : 0;
					TotalSubTotal += oInvoice.SubTotal;
					TotalTax += oInvoice.dTaxAmount;
					TotalTotal += oInvoice.Total;
					TotalCredit += oInvoice.Credits;
					TotalBalanceDue += oInvoice.Total - oInvoice.Credits;
				});
				return (Decimal.Round(TotalNetSales, 2, MidpointRounding.AwayFromZero), Decimal.Round(TotalDiscount, 2, MidpointRounding.AwayFromZero),
								Decimal.Round(TotalAdjustment, 2, MidpointRounding.AwayFromZero), Decimal.Round(TotalShipping, 2, MidpointRounding.AwayFromZero),
								Decimal.Round(TotalSubTotal, 2, MidpointRounding.AwayFromZero), Decimal.Round(TotalTax, 2, MidpointRounding.AwayFromZero),
								Decimal.Round(TotalTotal, 2, MidpointRounding.AwayFromZero), Decimal.Round(TotalCredit, 2, MidpointRounding.AwayFromZero),
								Decimal.Round(TotalBalanceDue, 2, MidpointRounding.AwayFromZero));
			}
			catch (Exception)
			{

				throw;
			}
    }
		//public static bool IsShippingByQuote(string ShipToCountry, string BillToCountry)
		//{
		//	try
		//	{
		//		if (string.IsNullOrWhiteSpace(ShipToCountry) || string.IsNullOrWhiteSpace(BillToCountry))
		//			return true;
		//		if (ShipToCountry.Equals(BillToCountry, StringComparison.OrdinalIgnoreCase) == false)
		//			return true;
		//		var country = ShipToCountry.ToUpper();
		//		if (country != "CANADA" && country != "USA" && country != "CA" && country != "UNITED STATES")
		//			return true;
		//		return false;
		//	}
		//	catch (Exception)
		//	{
		//		return true;
		//	}
		//}

	}
}
