using MIS.DBBO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToolkit.Base.Extensions;
using NHibernate;
using MyHibernateUtil;
using static MIS.DBPO.DBPOServiceHelper;
using MyHibernateUtil.Extensions;

namespace MIS.DBPO
{
  public static class OrderItemExtension
  {
    public static VAOrderItem getSrcItem(this OrderItem oOrderItem)
    {
      return (oOrderItem is DiscountDecorator) ? (oOrderItem as DiscountDecorator).DecoratedVAItem : (oOrderItem as VAOrderItem);
    }
    public static POSInvoiceDetail ToInvoiceDetail(this OrderItem oItem, SessionProxy oSession, POSInvoice oInvoice)
    {
      POSInvoiceDetail oDetail = new POSInvoiceDetail
      {
        AccountNO = oInvoice.AccountNO,
        InvoiceNO = oInvoice.InvoiceNo,
        ProductCode = oItem.ItemCode,
        ProductName = oItem.ItemName
      };
      if (oItem is CreditOrderItem)
      {
        oDetail.CountOrder = -1;
        oDetail.CountShipped = -1;
        oDetail.CountUnit = "Bottle";
        oDetail.Discount = 0;
        oDetail.GST = 0;
        oDetail.HST = 0;
        oDetail.PST = 0;
        oDetail.LotNumber = "";
        //MESProductCode = "",
        oDetail.Tax = "";
        oDetail.UnitPrice = (oItem as CreditOrderItem).Credit;
        oDetail.NetSales = decimal.Round((-1) * (oItem as CreditOrderItem).Credit, 2, MidpointRounding.AwayFromZero);
        oDetail.SalesActivity = oDetail.NetSales;
      }
      else if (oItem is MCOrderItem)
      {
        oDetail.CountOrder = oItem.OrderQty;
        oDetail.CountShipped = oItem.OrderQty;
        oDetail.CountUnit = oItem.Unit;
        oDetail.Discount = 0;
        oDetail.GST = oInvoice.GST;
        oDetail.HST = oInvoice.HST;
        oDetail.PST = oInvoice.PST;
        oDetail.LotNumber = "";
        //MESProductCode = "",
        oDetail.Tax = oInvoice.Tax;
        oDetail.UnitPrice = oItem.UnitPrice;
        oDetail.NetSales = decimal.Round((decimal)oItem.OrderQty * oItem.UnitPrice, 2, MidpointRounding.AwayFromZero);
        oDetail.SalesActivity = decimal.Round(oDetail.NetSales * (decimal)((100 - oItem.DiscountPercentage) * 0.01), 2, MidpointRounding.AwayFromZero);
      }
      else
      {
        var oDiscountItem = oItem as DiscountDecorator;
        oDetail.CountOrder = oItem.OrderQty;
        oDetail.CountShipped = oItem.ShipQty;
        oDetail.CountUnit = oItem.Unit;
        oDetail.GST = oInvoice.GST;
        oDetail.HST = oInvoice.HST;
        oDetail.PST = oInvoice.PST;
        oDetail.LotNumber = oItem.sAboutLotNoInfo;
        //MESProductCode = oItem.MESProductCode,
        oDetail.Tax = oInvoice.Tax;
        oDetail.UnitPrice = oItem.UnitPrice;
        oDetail.Discount = (oDiscountItem != null /*&& oDiscountItem.bSpecialDiscount*/) ? oItem.DiscountPercentage : (oItem is VAGiftItem) ? oItem.DiscountPercentage : 0;
       
        oDetail.NetSales = decimal.Round((decimal)oItem.ShipQty.Value * (1 - (decimal)(oDetail.Discount.Value * 0.01)) * oItem.UnitPrice, 2, MidpointRounding.AwayFromZero);
        //var unitCost = (decimal)Math.Round(((double)oDiscountItem.UnitPrice) * (100 - oDiscountItem.DiscountPercentage) * 0.01, 2, MidpointRounding.AwayFromZero);
        oDetail.SalesActivity = oDetail.NetSales;//decimal.Round((decimal)oItem.ShipQty.Value * unitCost, 2, MidpointRounding.AwayFromZero);
        oDetail.CostOfGoodsSold = oItem.CostOfGoodsSold(oSession);
      }

      oDetail.ItemType = (oItem is CreditOrderItem) ? "CREDIT" :
                         (oItem is VAGiftItem) ? "GIFT" :
                         (oItem is OptionDiscountDecorator) ? "OPTION" :
                         (oItem is DiscountDecorator) ? "DISCOUNT" : "NORMAL";
      oDetail.DiscountName = (oItem is DiscountDecorator) ? (oItem as DiscountDecorator).oDiscount?.oDiscountProgram.Name ?? "" :
                     (oItem is VAGiftItem) ? (oItem as VAGiftItem).oDiscount?.oDiscountProgram.Name ?? "" : "";
      return oDetail;
    }
    public static decimal CostOfGoodsSold(this OrderItem oOrderItem, SessionProxy oROSession)//ORMServer VADBServer)
    {
      if ((oOrderItem?.oItemsByLot?.Count() ?? 0) == 0)
      {
        if ((oOrderItem is VAOrderItem || oOrderItem is DiscountDecorator) && oOrderItem.ShipQty < 0) // returned products
        {
          var lotNoOfReturned = (oOrderItem is VAOrderItem) ? oOrderItem.RetailLotNo :
                                                              (oOrderItem as DiscountDecorator).DecoratedVAItem.RetailLotNo;
          if (string.IsNullOrWhiteSpace(lotNoOfReturned))
            return 0;

          var unitCost = oROSession.QueryDataElement<VitaAidFinishProduct>()
                           .Where(x => x.RetailLotNo.ToUpper() == lotNoOfReturned.ToUpper())
                           .ToList()
                           .FirstOrDefault()?.UnitCost ?? (decimal)0.0;
          return Math.Round(unitCost * (decimal)oOrderItem.ShipQty, 2); ;
        }
        return 0;
      }

      return oOrderItem.oItemsByLot.Sum(x => x.getCostOfGoodsSold(oROSession));
    }
  }
}
