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
using Microsoft.Extensions.Logging;
using NHibernate;
using MyHibernateUtil;
using vitaaid.com.Models.Account;

namespace vitaaid.com.Controllers
{
    [Route("api/AddressBook")]
  [ApiController]
  public class AddressBookController : ControllerBase
  {
    private readonly ILogger<AddressBookController> _logger;
    public AddressBookController(ILogger<AddressBookController> logger)
    {
      _logger = logger;
    }

    private IList<AddressData> ToAddressBookData(IList<CustomerAddress> addresses)
    {
      var addressBook = addresses.OrderByDescending(x => x.ID)
                                 .Select(x => new AddressData(x))
                                 .ToList()
                                 .Also(x =>
                                 {
                                   if (x.Count() == 1)
                                   {
                                     x[0].DefaultBillingAddress = true;
                                     x[0].DefaultShippingAddress = true;
                                   }
                                   else
                                   {
                                     if (x.Where(x => x.DefaultBillingAddress).IsNullOrEmpty())
                                       x[0].DefaultBillingAddress = true;
                                     if (x.Where(x => x.DefaultShippingAddress).IsNullOrEmpty())
                                       x[0].DefaultShippingAddress = true;
                                   }
                                 });
      return addressBook;
    }
    //[Authorize]
    // GET: api/AddressBook/{customerCode}
    [HttpGet("{customerCode}")]
    public IActionResult Get(string customerCode)
    {
      if (string.IsNullOrWhiteSpace(customerCode))
        return BadRequest();
      _logger.LogDebug("{0} get address book", customerCode);
      var oVMSession = VAMISDBServer[eST.READONLY];
      try
      {
        oVMSession.Clear();
        var oCustomer = oVMSession.QueryDataElement<CustomerAccount>()
                                      .Where(x => x.CustomerCode == customerCode)
                                      .UniqueOrDefault();
        if (oCustomer == null)
          return BadRequest();

        var addresses = oVMSession.QueryDataElement<CustomerAddress>()
          .Where(x => x.oCustomer == oCustomer)
          .ToList();

        return Ok(ToAddressBookData(addresses));
      }
      catch (Exception ex)
      {
        _logger.LogError("Get fail.", ex);
        return BadRequest();
      }
      finally
      {
        oVMSession.Close();
      }
    }

    //[Authorize]    
    [HttpPut("defaultAddressChange/{customerCode}")]
    public IActionResult DefaultAddressChange(string customerCode, [FromQuery] int addressID, [FromQuery] string type)
    {
      if (string.IsNullOrWhiteSpace(customerCode))
        return BadRequest();
      _logger.LogInformation("{0} change default {1} address to id={2}", customerCode, type, addressID);

      var oVMSession = VAMISDBServer[eST.SESSION0];
      try
      {
        oVMSession.Clear();
        var oCustomer = oVMSession.QueryDataElement<CustomerAccount>()
                                      .Where(x => x.CustomerCode == customerCode)
                                      .UniqueOrDefault();
        if (oCustomer == null)
          return BadRequest();

        var addresses = oVMSession.QueryDataElement<CustomerAddress>()
                                .Where(x => x.oCustomer == oCustomer)
                                .ToList();

        addresses.Where(x => x.ID == addressID)
                  .UniqueOrDefault()
                  ?.Also(x =>
                  {
                    var xact = oVMSession.BeginTransaction();
                    try
                    {
                      if (type == "billing" || type == "both")
                        addresses.Action(a => a.DefaultBillingAddress = (a.ID == addressID));
                      if (type == "shipping" || type == "both")
                        addresses.Action(a => a.DefaultShippingAddress = (a.ID == addressID));

                      addresses.Action(a => oVMSession.SaveObj(a));
                      xact.Commit();
                    }
                    catch (Exception)
                    {
                      xact.Rollback();
                    }
                  });
        return Ok(ToAddressBookData(addresses));
      }
      catch (Exception ex)
      {
        _logger.LogError("DefaultAddressChange fail.", ex);
        return BadRequest();
      }
      finally
      {
        oVMSession.Close();
      }
    }

    [HttpPut("updateAddress/{customerCode}")]
    public IActionResult UpdateAddress(string customerCode, [FromBody] AddressData address)
    {
        if (string.IsNullOrWhiteSpace(customerCode))
          return BadRequest();

      _logger.LogInformation("{0} update address, id={1} ", customerCode, address.ID);
      var oVMSession = VAMISDBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oVMSession.Clear();
        var oCustomer = oVMSession.QueryDataElement<CustomerAccount>()
                                      .Where(x => x.CustomerCode == customerCode)
                                      .UniqueOrDefault();
        if (oCustomer == null)
          return BadRequest();

        xact = oVMSession.BeginTransaction();

        if (address.ID == 0)
        {
          new CustomerAddress
          {
            oCustomer = oCustomer,
            DefaultBillingAddress = address.DefaultBillingAddress,
            DefaultShippingAddress = address.DefaultShippingAddress,
            AddressPerson = address.AddressPerson,
            AddressName = address.AddressName,
            Address = address.Address,
            City = address.City,
            Province = address.Province,
            PostalCode = address.PostalCode,
            Country = address.Country,
            Tel = address.Tel,
            Fax = address.Fax
          }.Also(x => x.SaveObj(oVMSession));
        }
        else
        {
          var vaAddress = oVMSession.QueryDataElement<CustomerAddress>()
                                  .Where(x => x.ID == address.ID)
                                  .UniqueOrDefault();
          if (vaAddress == null)
            return BadRequest();
          else
          {
            vaAddress.DefaultBillingAddress = address.DefaultBillingAddress;
            vaAddress.DefaultShippingAddress = address.DefaultShippingAddress;
            vaAddress.AddressPerson = address.AddressPerson;
            vaAddress.AddressName = address.AddressName;
            vaAddress.Address = address.Address;
            vaAddress.City = address.City;
            vaAddress.Province = address.Province;
            vaAddress.PostalCode = address.PostalCode;
            vaAddress.Country = address.Country;
            vaAddress.Tel = address.Tel;
            vaAddress.Fax = address.Fax;
            vaAddress.SaveObj(oVMSession);
          }
        }
        xact.Commit();
        return Ok(ToAddressBookData(oVMSession.QueryDataElement<CustomerAddress>().Where(x => x.oCustomer == oCustomer).ToList()));
      }
      catch (Exception ex)
      {
        xact?.Rollback();
        _logger.LogError("UpdateAddress fail.", ex);
        return BadRequest();
      }
      finally
      {
        oVMSession.Close();
      }
    }

    [HttpPut("removeAddress/{customerCode}")]
    public IActionResult RemoveAddress(string customerCode, [FromQuery] int addressID)
    {
      if (string.IsNullOrWhiteSpace(customerCode))
        return BadRequest();

      _logger.LogInformation("{0} delete address, id={1} ", customerCode, addressID);

      var oVMSession = VAMISDBServer[eST.SESSION0];
      ITransaction xact = null;
      try
      {
        oVMSession.Clear();
        var oCustomer = oVMSession.QueryDataElement<CustomerAccount>()
                                      .Where(x => x.CustomerCode == customerCode)
                                      .UniqueOrDefault();
        if (oCustomer == null)
          return BadRequest();

        var addresses = oVMSession.QueryDataElement<CustomerAddress>()
                                .Where(x => x.oCustomer == oCustomer)
                                .ToList();
        var others = addresses.Where(x => x.ID != addressID).ToList();
        if (others.IsNullOrEmpty())
          return BadRequest();

        var deletingAddr = addresses.Where(x => x.ID == addressID).UniqueOrDefault();
        xact = oVMSession.BeginTransaction();
        deletingAddr.Also(x =>
        {
            var deletingIdx = addresses.IndexOf(x);
            CustomerAddress newDefaultBillingAddres = null;
            CustomerAddress newDefaultShippingAddres = null;
            if (x.DefaultBillingAddress)
            {
              if (addresses.Count() > deletingIdx + 1)
                newDefaultBillingAddres = addresses[deletingIdx + 1];
              else
                newDefaultBillingAddres = addresses[deletingIdx - 1];
            }
            if (x.DefaultShippingAddress)
            {
              if (addresses.Count() > deletingIdx + 1)
                newDefaultShippingAddres = addresses[deletingIdx + 1];
              else
                newDefaultShippingAddres = addresses[deletingIdx - 1];
            }

          oVMSession.DeleteObj(x, false);
            newDefaultBillingAddres?.Also(a =>
            {
              a.DefaultBillingAddress = true;
              a.SaveObj(oVMSession);
            });
            newDefaultShippingAddres?.Also(a =>
            {
              a.DefaultShippingAddress = true;
              a.SaveObj(oVMSession);
            });
        });
        xact.Commit();

        return Ok(ToAddressBookData(others));
      }
      catch (Exception ex)
      {
        xact?.Rollback();
        _logger.LogError("RemoveAddress fail.", ex);
        return BadRequest();
      }
      finally
      {
        oVMSession.Close();
      }
    }
  }
}
