using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MIS.DBBO;
using MyHibernateUtil.Extensions;
using MySystem.Base.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static vitaaid.com.ServicesHelper;
using MIS.DBPO;
using NHibernate;
using WebDB.DBBO;
using vitaaid.com.Models.ShoppingCart;
using WebDB.DBPO.Helper;
using System.IO;
using static MySystem.Base.EmailAttributes;
using MySystem.Base.Helpers;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using static MySystem.Base.Helpers.MailHelper;
using MyHibernateUtil;
using MySystem.Base;

namespace vitaaid.com.Controllers
{
  public class CreditCardData
  {
    public virtual string cardNo { get; set; }
    public virtual string expiryDate { get; set; }
    public virtual string cid { get; set; }
    public virtual string holder { get; set; }
    public virtual string phone { get; set; }
    public virtual string address { get; set; }
  }
  public class WebOrder
  {
    public virtual int memberID { get; set; }
    public virtual string customerCode { get; set; }
    public virtual int billingAddrID { get; set; }
    public virtual int shippingAddrID { get; set; }
    public virtual bool dropShipping { get; set; }
    public virtual string paymentMethod { get; set; }
    public virtual string orderComment { get; set; }
    public virtual ShoppingCartItem[] cart { get; set; }
    public virtual CreditCardData creditCardData { get; set; }
  }

  [Route("api/ShoppingCart")]
  [ApiController]
  public class ShoppingCartController : ControllerBase
  {
    private readonly ILogger<ShoppingCartController> _logger;
    public ShoppingCartController(ILogger<ShoppingCartController> logger)
    {
      _logger = logger;
    }

    private VAOrder PrepareOrder(string customerCode, int billingAddrID, int shippingAddrID, ShoppingCartItem[] items, string webSite, SessionProxy oVAMISSession, SessionProxy oSession)
    {
      // 98k-1
      try
      {
        CustomerAccount oCustomer = oVAMISSession.QueryDataElement<CustomerAccount>()
                                            .Where(c => c.CustomerCode == customerCode)
                                            .UniqueOrDefault();
        if (oCustomer == null)
          return null;

        var isCA = (webSite.ToUpper() == "CA");

        oCustomer.LoadDiscountProgram(oVAMISSession);
        CustomerAddress oBillingAddress = null;
        CustomerAddress oShippingAddress = null;

        if (billingAddrID > 0)
        {
          oBillingAddress = oVAMISSession.QueryDataElement<CustomerAddress>()
                                          .Where(x => x.ID == billingAddrID && x.oCustomer.ID == oCustomer.ID)
                                          .UniqueOrDefault();
          if (oBillingAddress == null)
            return null;
        }
        if (shippingAddrID > 0)
        {
          oShippingAddress = oVAMISSession.QueryDataElement<CustomerAddress>()
                                          .Where(x => x.ID == shippingAddrID && x.oCustomer.ID == oCustomer.ID)
                                          .UniqueOrDefault();
          if (oShippingAddress == null)
            return null;
        }
        else
          oShippingAddress = oBillingAddress;

        bool useDropShipPrice = false;
        //if (oBillingAddress?.Country == "CANADA" && oShippingAddress?.Country != "CANADA")
        if (isCA && oShippingAddress != null && oShippingAddress.Country != "CANADA")
          useDropShipPrice = true;

        var oOrder = new VAOrder(oCustomer, "S-" + oCustomer.CustomerCode, DateTime.Now);
        oOrder.bOnlineOrder = true;
        oOrder.oBillingAddress = oBillingAddress;
        oOrder.oShippingAddress = oShippingAddress;
        oOrder.oGiftCards = oCustomer.AvailableCreditOrGiftCardsByRefDate(DateTime.Now, oVAMISSession);

        // 98k-2
        // update oOrder.oCartDiscounts
        oOrder.buildCartDiscounts();
        oShippingAddress?.TaxInfo(oVAMISSession).Also(x =>
        {
          oOrder.TaxRate = x.TaxRate;
          oOrder.TaxTitle = x.TaxTitle;
          oOrder.TaxChar = x.TaxChar;
        });
        oOrder.Currency = (isCA && useDropShipPrice == false) ? "CAD" : "USD";
        items.ForEach(item =>
        {
          var oOrderItem =
            new VAOrderItem
            {
              Name = item.Name,
              NameOnInvoice = item.Name,
              CodeOnInvoice = item.Code,
              Code = item.Code,
              OrderQty = item.Qty,
              ShipQty = item.Qty,
              UnitPrice = (useDropShipPrice) ? item.DropShipPrice : item.Price,
              PONo = oOrder.PONo,
              oVAOrder = (VAOrder)oOrder,
            };
          oOrder.AddOrderItem(oOrder.CreateOrderItem(oOrder.oAccount, oOrderItem));
        });

        IList<ShippingPolicyMaster> oShippingPolicies = ShippingPolicyExtension.FetchAllShippingPolicies(oVAMISSession);
        
        // 98k-3
        oOrder.RebuildOrderItem(oShippingPolicies, oVAMISSession, true, true);

        return oOrder;
      }
      catch (Exception)
      {
        throw;
      }
    }

    // GET: api/ShoppingCart/buildOrder?customerCode={customerCode}&billingAddrID={billingAddrID}shippingAddrID=${shippingAddrID}&webSite=${webSite}
    //[Authorize]
    [HttpPut("buildOrder")]
    public IActionResult BuildOrder(string customerCode, int billingAddrID, int shippingAddrID, ShoppingCartItem[] items, string webSite)
    {
      _logger.LogInformation("Build order:{0}", customerCode);
      var oVAMISSession = VAMISDBServer[eST.SESSION0];
      var oSession = DBServer[eST.SESSION0];
      
      try
      {
        if (items.IsNullOrEmpty() || string.IsNullOrWhiteSpace(customerCode))
        {
          _logger.LogInformation("Item lst is null/empty, or customer code is null");
          return BadRequest();//Ok(null);
        }
        oVAMISSession.Clear();
        oSession.Clear();

        var oOrder = PrepareOrder(customerCode, billingAddrID, shippingAddrID, items, webSite, oVAMISSession, oSession);

        var ShoppingCartOrder = (new OrderData()).buildFromVAOrder(oOrder, false);
        return Ok(ShoppingCartOrder);
      }
      catch (Exception ex)
      {
        _logger.LogError("BuildOrder fail.", ex);
        return BadRequest();
      }
      finally
      {
        oVAMISSession.Close();
      }
    }

    [HttpPut("putOrder")]
    public IActionResult PutOrder(WebOrder webOrder, string webSite)
    {
      if (webOrder.cart.IsNullOrEmpty() || string.IsNullOrWhiteSpace(webOrder.customerCode))
      {
        _logger.LogInformation("Item lst is null/empty, or customer code is null");
        return BadRequest();//Ok(null);
      }

      _logger.LogInformation("Put order: {0}", webOrder.customerCode);

      var oSession = DBServer[eST.SESSION0];
      var oVMSession = VAMISDBServer[eST.SESSION0];
      ITransaction xact = null;
      ITransaction webXact = null;
      try
      {
        oSession.Clear();
        oVMSession.Clear();

        var oOrder = PrepareOrder(webOrder.customerCode, webOrder.billingAddrID, webOrder.shippingAddrID, webOrder.cart, webSite, oVMSession, oSession);

        if (oOrder == null)
          return BadRequest();//Ok(null);

        oOrder.Comment = webOrder.orderComment;

        CustomerAccount oCustomer = oVMSession.QueryDataElement<CustomerAccount>()
                                              .Where(c => c.CustomerCode == webOrder.customerCode)
                                              .UniqueOrDefault();
        oCustomer.PostProcForCreditCard();
        xact = oVMSession.BeginTransaction();
        var newPONo = oCustomer.GetNextPONo(oOrder.OrderDate, oVMSession);
        oOrder.oGiftCards?.Where(x => x.LastUsedOn == oOrder.PONo)?.Action(x => x.LastUsedOn = newPONo);
        oOrder.SetPONo(newPONo);
        oOrder.PaymentType = webOrder.paymentMethod;
        oOrder.bDropShip = webOrder.dropShipping;
        if (webOrder.paymentMethod == "Credit Card" && !string.IsNullOrWhiteSpace(webOrder.creditCardData.cardNo))
        {
          oOrder.CardNo = CrypTool.Crypto.Encrypt(webOrder.creditCardData.cardNo);
          oOrder.CardHolderName = webOrder.creditCardData.holder;
          if (oCustomer.oCreditCard1.CardNumEqual(webOrder.creditCardData.cardNo))
          {
            if (!oCustomer.oCreditCard1.Equals(webOrder.creditCardData.cardNo, webOrder.creditCardData.expiryDate, webOrder.creditCardData.cid))
            {
              // update Expiry Date and CID
              oCustomer.oCreditCard1.ExpiryDate = webOrder.creditCardData.expiryDate;
              oCustomer.oCreditCard1.CardCID = webOrder.creditCardData.cid;
              oCustomer.iState = eOPSTATE.DIRTY;
            }
          }
          else
          {
            if (!string.IsNullOrEmpty(oCustomer.oCreditCard1.CardNumber) && !oCustomer.oCreditCard1.CardNumEqual(oCustomer.oCreditCard2.CardNumber))
              oCustomer.oCreditCard1.CopyTo(oCustomer.oCreditCard2);
            oCustomer.oCreditCard1.Also(x =>
            {
              x.CardType = CreditCard.FindType(webOrder.creditCardData.cardNo);
              x.CardNumber = webOrder.creditCardData.cardNo;
              x.CardHolder = webOrder.creditCardData.holder;
              x.ExpiryDate = webOrder.creditCardData.expiryDate;
              x.CardCID = webOrder.creditCardData.cid;
            });
            oCustomer.iState = eOPSTATE.DIRTY;
          }
          if (oCustomer.iState == eOPSTATE.DIRTY)
          {
            if (oCustomer.oCreditCard2.CardNumEqual(webOrder.creditCardData.cardNo))
              oCustomer.oCreditCard2 = null;
            oCustomer.UpdateCreditCardInfo();
            oVMSession.SaveObj(oCustomer);
          }
        }

        // not save oGiftCards and payments
        var oGiftCards = oOrder.oGiftCards;
        var oPayments = oOrder.oPayments;

        var ShoppingCartOrder = (new OrderData()).buildFromVAOrder(oOrder);
        if (!string.IsNullOrWhiteSpace(ShoppingCartOrder.CardNo))
          ShoppingCartOrder.CardNo = CrypTool.Crypto.Encrypt(webOrder.creditCardData.cardNo + "," + webOrder.creditCardData.expiryDate + "," + webOrder.creditCardData.cid);

        oOrder.oGiftCards?.Action(x => oVMSession.session.Evict(x));
        oOrder.oPayments?.Action(x => oVMSession.session.Evict(x));
        oOrder.oGiftCards?.Clear();//Action(x => oVMSession.session.Evict(x));
        oOrder.oPayments?.Clear();//Action(x => oVMSession.session.Evict(x));

        oOrder.SaveOrder(oVMSession);
        xact.Commit();

        //var ShoppingCartOrder = (new OrderData()).buildFromVAOrder(oOrder);

        ShoppingCartOrder.MemberID = webOrder.memberID;
        ShoppingCartOrder.VAOrderID = oOrder.ID;

        webXact = oSession.BeginTransaction();
        ShoppingCartOrder.SaveObj(oSession);
        ShoppingCartOrder.OrderItems.Action(x => x.SaveObj(oSession));

        webXact.Commit();

        SendPOEmail(ShoppingCartOrder, webOrder.creditCardData, oSession);
        _logger.LogInformation("Put order: {0} {1} done.", webOrder.customerCode, oOrder.PONo);

        return Ok(ShoppingCartOrder);
      }
      catch (Exception ex)
      {
        if (xact?.IsActive ?? false)
          xact?.Rollback();
        if (webXact?.IsActive ?? false)
          webXact?.Rollback();
        _logger.LogError("Put order fail.", ex);

        return BadRequest(ex.ToString());//Ok(null);
      }
      finally
      {
        oSession.Close();
        oVMSession.Close();
      }
    }

    private string getMailBody(bool IncludeRemoteAreaSurcharge)
    {
      string content = "";
      _logger.LogDebug("read file: " + Startup.ms_ContentRoot + "OrderMailBody.html");
      using (TextReader StrIn = new System.IO.StreamReader(Startup.ms_ContentRoot + "OrderMailBody.html"))
      {
        try
        {
          content = StrIn.ReadToEnd();
        }
        catch (Exception) { }
      }
      return content;
    }

    private void SendPOEmail(OrderData oOrder, CreditCardData cardInfo, SessionProxy oSession)
    {
      try
      {
        _logger.LogDebug("In SendPOEmail() ...");
        var member = oSession.QueryDataElement<Member>()
                              .Where(x => x.ID == oOrder.MemberID)
                              .UniqueOrDefault();
        if (member == null) return;
        _logger.LogDebug("getMailBody() ...");
        string mailbody = getMailBody(oOrder.IncludeRemoteAreaSurcharge);
        if (mailbody.Length == 0)
        {
          _logger.LogError("getMailBody() ... FAIL");
          return;
        }
        else
          _logger.LogDebug("getMailBody() ... SUCCESS");

        string sOrderItems = "";
        oOrder.OrderItems.ForEachWithIndex((x, idx) =>
        {
          sOrderItems += "<tr><td valign=\"top\"><font face = \"Verdana\" size = \"2\">" + (idx + 1).ToString() + "</font></td>" +
                        "<td valign = \"top\" ><font face = \"Verdana\" size = \"2\">" + x.Code + "</font></td>" +
                        "<td valign = \"top\" ><font face = \"Verdana\" size = \"2\">" + x.Name + "</font></td>" +
                        "<td valign = \"top\" ><font face = \"Verdana\" size = \"2\">" + ((x.ItemType != "CREDIT") ? x.Qty.ToString() : "") + "</font></td>" +
                        "<td valign = \"top\" ><font face = \"Verdana\" size = \"2\">" + ((x.ItemType != "CREDIT") ? x.Price.ToString("C2") : "") + "</font></td>" +
                        "<td valign = \"top\" ><font face = \"Verdana\" size = \"2\">" + ((x.Discount > 0) ? x.Discount.ToString() + "% " + (string.IsNullOrWhiteSpace(x.DiscountName) ? "" : " (" + x.DiscountName) + ")" : "") + "</font></td>" +
                        "<td valign = \"top\" ><font face = \"Verdana\" size = \"2\">" + x.Amount.ToString("C2") + "</font></td></tr>";
        });
        var dropShipMsg = (oOrder.bDropShip) ? "<tr><td valign=\"center\" align=\"right\" width=\"37%\" bgcolor=\"#9acee2\"><font face=\"Verdana\" size=\"2\">Drop Shipping</font><font color=\"#000000\" size=\"2\">:</font></td><td align=\"left\" width=\"63%\" bgcolor=\"#ffffff\"><font face=\"Verdana\" size=\"2\"><b> YES(*Invoice will be mailed separately)</b></font></td></tr>" : "";
        var remoteAreaMsg = oOrder.IncludeRemoteAreaSurcharge ? "<tr><td valign=\"center\" align=\"right\" width=\"37%\"><font face=\"Verdana\" size=\"2\">Remote Area Surcharge</font><font color=\"#000000\" size=\"2\">:</font></td><td align=\"right\" width=\"63%\" bgcolor=\"#ffffff\"><font face=\"Verdana\" size=\"2\">" + oOrder.ExtendedAreaSubcharge.ToString("C2") + "</font></td></tr>" : "";

        var varDictionary =
          new Dictionary<string, string> {
            { "{PONo}", oOrder.PONo },
            { "{OrderDate}", oOrder.OrderDate.ToString() },
            { "{TitleBill}", string.IsNullOrWhiteSpace(oOrder.TitleBill) ? oOrder.CompanyNameBill : oOrder.TitleBill },
            { "{TitleShip}", string.IsNullOrWhiteSpace(oOrder.TitleShip) ? oOrder.CompanyNameShip : oOrder.TitleShip },
            { "{AddrShip}", oOrder.AddrShip },
            { "{CityShip}", oOrder.CityShip },
            { "{ProvinceShip}", oOrder.ProvinceShip },
            { "{CountryShip}", oOrder.CountryShip },
            { "{ZipCodeShip}", oOrder.ZipCodeShip },
            { "{DropShipMsg}", dropShipMsg},
            { "{TelShip}", oOrder.TelShip },
            { "{Email}", member.Email },
            { "{Comment}", oOrder.Comment },
            { "{OrderItems}", sOrderItems },
            { "{NetSales}", oOrder.NetSales.ToString("C2") },
            { "{cartDiscountName}", (oOrder.dAdjustmentDiscountPercentage > 0) ? (string.IsNullOrWhiteSpace(oOrder.CartDiscountName)? oOrder.dAdjustmentDiscountPercentage.ToString() + "% OFF" : oOrder.CartDiscountName) : ""},
            { "{cartDiscountDetail}", oOrder.Adjustment.ToCurrencyString(true, true, "$") },
            { "{ShippingFee}", oOrder.ShippingByQuote ? "Shipping by quote" : oOrder.ShippingFeeNotIncludeRemoteAreaSurcharge.ToString("C2") },
            { "{RemoteAreaBlock}", remoteAreaMsg },
            { "{Subtotal}", oOrder.SubTotal.ToString("C2") },
            { "{TaxTitle}", (oOrder.dTaxAmount > 0) ? oOrder.TaxTitle : "" },
            { "{TaxAmount}", (oOrder.dTaxAmount > 0) ? oOrder.dTaxAmount.ToString("C2") : "" },
            { "{Currency}", oOrder.Currency },
            { "{Total}", oOrder.Total.ToString("C2") }
          };

        string result = "FAIL";

        var setting = UnitTypeHelper.buildEmailAttributes(oSession);
        setting[C_SUBJECT] = "Vita Aid: Order Confirmed [" + oOrder.PONo + "]";
        setting[C_BODY] = mailbody;
        setting.processVarables(C_BODY, varDictionary);

        setting[C_FROM] = "info@vitaaid.com";
        setting[C_RECIPIENT] = member.Email;
        setting[C_BCCRECIPIENT] = "info@vitaaid.com";
        result = MailHelper.SendEmail(setting, (Attachment)null, null, true);
        if (result == SEND_SUCCESS)
          _logger.LogInformation("SendEmail to " + member.Email + ":" + result);
        else
          _logger.LogError("SendEmail to " + member.Email + ":" + result);

        /*
        setting[C_FROM] = "orders@vitaaid.com";
        setting[C_RECIPIENT] = "info@vitaaid.com";
        result = MailHelper.SendEmail(setting, (Attachment)null, null, true);
        if (result == SEND_SUCCESS)
          _logger.LogInformation("SendEmail to info@vitaaid.com:" + result);
        else
          _logger.LogError("SendEmail to info@vitaaid.com:" + result);

        if (oOrder.PaymentType == "Credit Card")
        {
          setting[C_SUBJECT] = oOrder.PONo;
          setting[C_BODY] = cardInfo.cid + "<br>" + cardInfo.expiryDate;
          result = MailHelper.SendEmail(setting, (Attachment)null, null, true);
          if (result == SEND_SUCCESS)
            _logger.LogInformation("SendEmail(cid/expirydate) to info@vitaaid.com:" + result);
          else
            _logger.LogError("SendEmail(cid/expirydate) to info@vitaaid.com:" + result);
        }
        */
        _logger.LogDebug("Out SendPOEmail().");
      }
      catch (Exception ex)
      {
        _logger.LogError("SendPOEmail", ex);
        throw;
      }
    }
  }
}
