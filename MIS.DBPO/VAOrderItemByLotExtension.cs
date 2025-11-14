using MIS.DBBO;
using MyHibernateUtil;
using MyHibernateUtil.Extensions;
using MySystem.Base.Extensions;
using MyToolkit.Base.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIS.DBPO
{
	public static class VAOrderItemByLotExtension
	{
		//public static void XferToVitaaidWH(this VAOrderItemByLot oItemByLot, VitaAidFinishProduct oNAFinishProduct, ORMServer VADBServer, int Qty)
		//	=> oItemByLot.XferToVitaaidWH(oNAFinishProduct, VADBServer[eST.SESSION0], Qty);

		public static void XferToVitaaidWH(this VAOrderItemByLot oItemByLot, VitaAidFinishProduct oNAFinishProduct, SessionProxy oVMSession, int Qty)
		{
			try
			{
				var oVAFinishProduct = oVMSession.QueryDataElement<VitaAidFinishProduct>()
												 .Where(x => x.SupplyCode == oNAFinishProduct.SupplyCode).FirstOrDefault();

				//if (oItemByLot.oVAOrder.dAdjustmentDiscountPercentage )
				//var x = oItemByLot.oItemOwner.
				if (oVAFinishProduct == null)
				{
					oVAFinishProduct = (VitaAidFinishProduct)oNAFinishProduct.ShallowCopy();
					oVAFinishProduct.ID = 0;
					oVAFinishProduct.oParentProduct = null;
					oVAFinishProduct.oProductInfo = null;
					oVAFinishProduct.oLot = null;
					oVAFinishProduct.InitSupplyCount = Qty;
					oVAFinishProduct.SupplyCount = oVAFinishProduct.InitSupplyCount;
					oVAFinishProduct.ReservedCount = 0;
					oVAFinishProduct.StockCount = Qty;
					oVAFinishProduct.CreatedDate = DateTime.Now;
					oVAFinishProduct.CreatedID = DataElement.sDefaultUserID;
				}
				else
				{
					oVAFinishProduct.StockCount += Qty;
					oVAFinishProduct.InitSupplyCount += Qty;
					oVAFinishProduct.SupplyCount = oVAFinishProduct.InitSupplyCount;
				}

				var oSrcItem = oItemByLot.oVAOrder.oOrderItems.Where(x => x.getSrcItem().ID == oItemByLot.oItemOwner.ID).UniqueOrDefault();
				var unitCost = (decimal)Math.Round(((double)oSrcItem.UnitPrice) * (100 - oSrcItem.DiscountPercentage) * 0.01, 2, MidpointRounding.AwayFromZero);
				if (unitCost > oVAFinishProduct.UnitCost)
					oVAFinishProduct.UnitCost = unitCost;
				oVAFinishProduct.UpdatedDate = DateTime.Now;
				oVAFinishProduct.UpdatedID = DataElement.sDefaultUserID;
				oVMSession.SaveObj(oVAFinishProduct);
			}
			catch (Exception)
			{
				throw;
			}
		}
		public static decimal getCostOfGoodsSold(this VAOrderItemByLot oItemByLot, SessionProxy oROSession)//ORMServer VADBServer)
		{
			if (oItemByLot.ShipQty == null || oItemByLot.ShipQty.HasValue == false)
				return 0;
			var oVAFinishProduct = oROSession.QueryDataElement<VitaAidFinishProduct>()
											 .Where(x => x.SupplyCode == oItemByLot.SupplyCode).FirstOrDefault();
			if (oVAFinishProduct == null || oVAFinishProduct.UnitCost == 0) return 0;
			return decimal.Round((decimal)(oVAFinishProduct.UnitCost * (decimal)oItemByLot.ShipQty), 2, MidpointRounding.AwayFromZero);
		}
	}
}
