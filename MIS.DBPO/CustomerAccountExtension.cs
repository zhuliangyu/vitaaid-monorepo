using MIS.DBBO;
using System;
using System.Collections.Generic;
using System.Linq;
using static MIS.DBPO.DBPOServiceHelper;
using MyHibernateUtil.Extensions;
using MySystem.Base;
using MySystem.Base.Extensions;
using System.Linq.Expressions;
using MyHibernateUtil;

namespace MIS.DBPO
{
	public static class CustomerAccountExtension
	{
		//public static void LoadDiscountProgram(this CustomerAccount oAccount)
		//	=> oAccount.LoadDiscountProgram(MISDB[eST.SESSION0]);
		public static void LoadDiscountProgram(this CustomerAccount oAccount, SessionProxy oSession)
		{
			try
			{
				if (oAccount == null || oAccount.oDiscountPrograms != null) return;
				oAccount.oDiscountPrograms = new List<DiscountProgram>();
				oAccount.oCustomerDiscounts = new List<CustomerDiscount>();

				// LOAD DISCOUNT PROGRAMS WHICH TARGET ACCOUNTS CONTAIN THIS ACCOUNT 
				IList<DiscountTargetAccount> oProgramOfSelectedAccounts = (oAccount.ID == 0) ? new List<DiscountTargetAccount>() :
														oSession.QueryDataElement<DiscountTargetAccount>()
																.Where(x => x.oCustomerAccount == oAccount)
																.OrderBy(x => x.oDiscountProgram.ID)
																.ToList<DiscountTargetAccount>();
				foreach (DiscountTargetAccount oDTA in oProgramOfSelectedAccounts)
					oAccount.oDiscountPrograms.Add(oDTA.oDiscountProgram);

				// LOAD DISCOUNT PROGRAMS WHICH TargetAccountMethod == eACCOUNTRULE.ALL
				IList<DiscountProgram> oProgramOfAllAcounts = (oAccount.bEmployee) ? new List<DiscountProgram>() :
																oSession.QueryDataElement<DiscountProgram>()
																.Where(x => x.TargetAccountMethod == eACCOUNTRULE.ALL)
																.OrderBy(x => x.ID)
																.ToList<DiscountProgram>();
				foreach (DiscountProgram oDiscountProgram in oProgramOfAllAcounts)
				{
					if (string.IsNullOrWhiteSpace(oDiscountProgram.TargetAccountExclude) == false)
					{
						string[] sExcludeAccount = oDiscountProgram.TargetAccountExclude.Split(new char[] { ';', ',' });
						bool bExclude = false;
						foreach (string exclude in sExcludeAccount)
						{
							if (exclude.Equals(oAccount.CustomerCode, StringComparison.OrdinalIgnoreCase))
							{
								bExclude = true;
								break;
							}
						}
						if (bExclude)
							continue;
					}
					oAccount.oDiscountPrograms.Add(oDiscountProgram);
				}

				IList<CustomerDiscount> oCDs;
				CustomerDiscount oCD = null;
				foreach (DiscountProgram oDP in oAccount.oDiscountPrograms)
				{
					oCDs = (oAccount.ID == 0) ? new List<CustomerDiscount>() :
						oSession.QueryDataElement<CustomerDiscount>()
						.Where(x => x.oDiscountProgram == oDP && x.oCustomer == oAccount)
						.ToList<CustomerDiscount>();
					//GetXObjs<CustomerDiscount>("x.oDiscountProgram.ID = " + oDP.ID + " AND x.oCustomer.ID = " + oAccount.ID);
					if (oCDs.Count >= 2)
					{
						throw new Exception("SYSTEM ERROR ServiceHelper.cs(47): mulitple-customer discount referenced by discount program");
					}
					else if (oCDs.Count == 1)
						oCD = oCDs[0];
					else
						oCD = new CustomerDiscount
						{
							oCustomer = oAccount,
							CustomerCode = oAccount.CustomerCode,
							oDiscountProgram = oDP,
							UsedCount = 0,
							iState = eOPSTATE.NEW,
							IsActive = true
						};
					oAccount.oCustomerDiscounts.Add(oCD);
					if (oDP.TargetProductMethod == ePRODUCTRULE.BYACCOUNT)
					{
						if (oCD.oProductsByAccount == null)
						{
							oCD.oProductsByAccount = (oAccount.ID == 0) ? new List<DiscountProductByAccount>() :
								oSession.QueryDataElement<DiscountProductByAccount>()
								.Where(x => x.oDiscountProgram == oDP && x.oCustomerAccount == oAccount)
								.OrderBy(x => x.ProductName)
								.ToList<DiscountProductByAccount>();
							//GetXObjs<DiscountProductByAccount>("x.oDiscountProgram.ID=" + oDP.ID + " AND x.oCustomerAccount.ID=" + oAccount.ID, "x.ProductName");
							DiscountProgram.TargetProdToAppliedProd(oCD.oProductsByAccount, oCD.AppliedProducts);
							oCD.sBackupAppliedProducts = string.Join<string>(",", oCD.AppliedProducts);
						}
					}
					else if (oDP.TargetProductMethod == ePRODUCTRULE.SELECTED)
					{
						if (oDP.oTargetProducts != null && oDP.oTargetProducts.Count > 0 && (oDP.AppliedProducts == null || oDP.AppliedProducts.Count == 0))
							DiscountProgram.TargetProdToAppliedProd(oDP.oTargetProducts, oDP.AppliedProducts);
					}
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public static IList<VAGiftCard> AvailableCreditOrGiftCardsByRefDate(this CustomerAccount oAccount, DateTime RefDate)
		{
			return oAccount.AvailableCreditOrGiftCardsByRefDate(RefDate, MISDB[eST.SESSION0]);

		}
		public static IList<VAGiftCard> AvailableCreditOrGiftCardsByRefDate(this CustomerAccount oAccount, DateTime RefDate, SessionProxy oSession)
		{
			try
			{
				var pred = ExpressionExtension.True<VAGiftCard>();
				pred = pred.And(x => x.AccountNo == oAccount.CustomerCode);
				if (!RefDate.IsNil())
					pred = pred.And(x => x.StartDate <= RefDate && RefDate <= x.ExpiryDate);
				var oGiftCards = oSession.QueryDataElement<VAGiftCard>()
										.Where(pred)
										.ToList();

				if (oAccount.bEmployee)
				{
					var giftCardCode = VAGiftCard.GiftCardCodeForEmployeeCredit(oAccount, RefDate);
					if (!oGiftCards.Where(x => x.Code == giftCardCode).Any())
						// auto add credit($75) monthly
						oGiftCards.Add(VAGiftCard.NewGiftCardForEmployeeCredit(oAccount, giftCardCode, RefDate.Year, RefDate.Month));
				}
				if (!oGiftCards.Where(x => x.CreditType == eCREDITTYPE.CUSTOMER_CREDIT).Any())
					oGiftCards.Add(new VAGiftCard
					{
						AccountNo = oAccount.CustomerCode,
						Code = oAccount.CustomerCode + "_Credits",
						CreditType = eCREDITTYPE.CUSTOMER_CREDIT,
						Name = "Customer Credit",
						Currency = (oAccount.PricePolicy == ePRICEPOLICY.STANDARD_USD || oAccount.PricePolicy == ePRICEPOLICY.MSRP_USD) ? "USD" : "CAD",
						Amount = 0,
						Balance = 0,
						iState = eOPSTATE.NEW
					});
				return oGiftCards.OrderBy(x => x.CreditType).ToList();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public static string EmpNo(this CustomerAccount oAccount)
		{
			try
			{
				var code = oAccount.CustomerCode;
				if (string.IsNullOrWhiteSpace(code))
					return "";
				if (code.ToUpper().StartsWith("NAPI"))
					return code.Substring(4, 3);
				if (code.ToUpper().StartsWith("VA"))
					return code.Substring(2, 3);
				return "";
			}
			catch (Exception)
			{

				throw;
			}
		}
		//public static string GetNextPONo(this CustomerAccount oAccount, DateTime orderDate)
		//	=> oAccount.GetNextPONo(orderDate, MISDB[eST.SESSION0]);
		public static string GetNextPONo(this CustomerAccount oAccount, DateTime orderDate, SessionProxy oSession)
		{
			try
			{
				string PONo = "";
				if (oAccount.bEmployee)
				{//E
					int i = 0;
					do
					{
						PONo = "E" + orderDate.Year.ToString() + orderDate.ToString("MM") + oAccount.EmpNo() + ((i++ > 0) ? "-" + i.ToString() : "");
					} while (OrderHelper.bDuplicatePONo(PONo, oSession));
				}
				else
				{
					string POType = "POSeqNo";
					PONo = "D";
					int iPOSeqNo = 0;
					if (oAccount.CustomerCode == "NAPI000")
					{//M
						POType = "POSeqNoM";
						PONo = "M";
					}
					else if (oAccount.bDistributor || oAccount.CustomerCode == "EE01US")
					{//W
						POType = "POSeqNoW";
						PONo = "W";
					}

					var oSys = oSession.QueryDataElement<MISSysParameter>()
									.Where(x => x.AppName == "InvoiceSystem" && x.ConfigName == POType)
									.UniqueOrDefault();

					//IList<MISSysParameter> oSyses = oSession.GetXObjs<MISSysParameter>("x.AppName='InvoiceSystem' AND ConfigName='" + POType + "'");
					if (oSys != null)
						iPOSeqNo = (int)oSys.ConfigVal1;

					do
					{
						PONo = PONo + (orderDate.Year - 2000).ToString() + orderDate.ToString("MM") + (iPOSeqNo++).ToString("D4");
					} while (OrderHelper.bDuplicatePONo(PONo, oSession));
					if (oSys != null)
					{
						oSys.ConfigVal1 = iPOSeqNo;
						oSession.SaveObj(oSys);
					}
				}
				return PONo;

			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
