using MIS.DBBO;
using System;
using System.Linq;
using static MIS.DBPO.DBPOServiceHelper;
using MySystem.Base.Extensions;
using MyHibernateUtil;
using System.Collections.Generic;

namespace MIS.DBPO
{
	public static class CustomerAddressExtension
	{
		//Extended Area Surcharge: UPS - Canada
		//public static decimal ExtendedAreaSubcharge(this CustomerAddress self) => self.ExtendedAreaSubcharge(MISDB[eST.SESSION0]);
		public static decimal ExtendedAreaSubcharge(this CustomerAddress self, SessionProxy oSession)
		{
			try
			{
				if ((self?.Country?.ToUpper() ?? "NA") != "CANADA")
					return 0;

				string sPostCode = (self.PostalCode ?? "").Replace(" ", string.Empty).Replace("	", string.Empty).ToUpper();
				if (sPostCode.Length == 6)
				{
					var bRemoteArea = oSession.Query<CA_EAS_DOM>()
																    .Where(x => x.FromCode.CompareTo(sPostCode) <= 0 &&
																						    x.ToCode.CompareTo(sPostCode) >= 0)
																		.ToList()
																		.Let<List<CA_EAS_DOM>, bool>(x => x.Any());

					//return oEAS?.Charge ?? 0;
					return bRemoteArea ? 5 : 0;
				}
				return 0;
			}
			catch (Exception)
			{
				throw;
			}
		}

		// Determinate Tax type
		//public static (double TaxRate, string TaxTitle, string TaxChar) TaxInfo(this CustomerAddress self) 
		//	=> self.TaxInfo(MISDB[eST.SESSION0]);
		public static (double TaxRate, string TaxTitle, string TaxChar) TaxInfo(this CustomerAddress self, SessionProxy oSession)
		{
			double TaxRate = 0;
			string TaxTitle = "";
			string TaxChar = "N";
			try
			{
				if (self != null &&
						!string.IsNullOrWhiteSpace(self.Country) && !string.IsNullOrWhiteSpace(self.Province) &&
						self.Country.ToUpper() == "CANADA")
				{
					oSession.Query<CanadaTax>()
						.Where(x => x.CountryName == "Canada" && x.ProvinceID == self.Province)
						.ToList().FirstOrDefault()
						?.Also(x =>
						{
							TaxRate = (x.TaxIDfromProvionce == "G") ? x.TaxRateGST :
												(x.TaxIDfromProvionce == "H") ? x.TaxRateHST : 0;
							TaxTitle = (x.TaxIDfromProvionce == "G") ? (x.TaxRateGST * 100).ToString() + "% GST" :
												(x.TaxIDfromProvionce == "H") ? (x.TaxRateHST * 100).ToString() + "% HST" : "";
							TaxChar = x.TaxIDfromProvionce;

						});
				}
				return (TaxRate: TaxRate, TaxTitle: TaxTitle, TaxChar: TaxChar);
			}
			catch (Exception) { return (TaxRate: TaxRate, TaxTitle: TaxTitle, TaxChar: TaxChar); }

		}
	}
}
