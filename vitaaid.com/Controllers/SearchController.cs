using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyHibernateUtil;
using MyHibernateUtil.Extensions;
using static vitaaid.com.ServicesHelper;
using WebDB.DBBO;
using MySystem.Base.Extensions;

namespace vitaaid.com.Controllers
{
  public class SearchResultData
  {
    public virtual string Content { get; set; }
    public virtual string Identity { get; set; }
    public virtual string Type { get; set; }
  }
  public class SearchResults
  {
    public virtual IList<SearchResultData> Products { get; set; } = new List<SearchResultData>();
    public virtual IList<SearchResultData> Blogs { get; set; } = new List<SearchResultData>();
    public virtual IList<SearchResultData> Webinars { get; set; } = new List<SearchResultData>();
    public virtual IList<SearchResultData> Protocols { get; set; } = new List<SearchResultData>();
  }


  [Route("api/search")]
  [ApiController]
  public class SearchController : ControllerBase
  {
    private IWebHostEnvironment _hostEnvironment;
    private readonly ILogger<SearchController> _logger;
    public SearchController(IWebHostEnvironment environment, ILogger<SearchController> logger)
    {
      _hostEnvironment = environment;
      _logger = logger;
    }

    // GET: api/search?keyword={keyword}&country={country}&memberType={memberType}
    [HttpGet()]
    public IActionResult Get(string keyword, string country, int memberType)
    {
      if (string.IsNullOrWhiteSpace(keyword))
        return Ok(new List<SearchResultData>());

      keyword = keyword.Trim();
      _logger.LogInformation("search by keyword: {0}", keyword);
      var oSession = DBServer[eST.READONLY];
      SearchResults results = new SearchResults();
      try
      {
        
        oSession.Clear();
        eWEBSITE site = eWEBSITE.CA;
        try
        {
          site = (eWEBSITE)Enum.Parse(typeof(eWEBSITE), country.ToUpper());
        }
        catch (Exception)
        {
          return BadRequest();
        }

        // Search product
        var oProducts = oSession.QueryDataElement<Product>()
                .Where(x => (x.VisibleSite == eWEBSITE.ALL || site == eWEBSITE.ALL || x.VisibleSite == site) &&
                             x.Published == true &&
                            (x.ProductName.Contains(keyword) || x.Function.Contains(keyword) || x.Tags.Contains(keyword)))
                .ToList();
        //.Action(x => results.Products.Add(new SearchResultData { Content = x.ProductName, Identity = x.ProductCode, Type = "Product" }));

        oSession.QueryDataElement<Ingredient>()
                .Where(x => x.GroupNo != 5 && x.Name.Contains(keyword))
                .Select(x => x.ProductCode)
                .ToList()
                .Except(oProducts.Select(x => x.ProductCode))
                .Also(pCodesByIngr =>
                {
                  oSession.QueryDataElement<Product>()
                          .Where(x => pCodesByIngr.Contains(x.ProductCode) && x.Published == true &&
                                      (x.VisibleSite == eWEBSITE.ALL || site == eWEBSITE.ALL || x.VisibleSite == site))
                          .ToList()
                          .Action(x => oProducts.Add(x));
                });
        oProducts.OrderBy(x=> x.ProductName).Action(x => results.Products.Add(new SearchResultData { Content = x.ProductName, Identity = x.ProductCode, Type = "Product" }));


        if (memberType != 2)
        {
          // Search blog
          oSession.QueryDataElement<TherapeuticFocus>()
                  .Where(x => x.Published == true &&
                              (x.Topic.Contains(keyword) || x.BlogContent.Contains(keyword) || x.Tags.Contains(keyword)))
                  .OrderBy(x => x.Topic)
                  .ToList()
                  .Action(x => results.Blogs.Add(new SearchResultData { Content = x.Topic, Identity = x.ID.ToString(), Type = "Blog" }));

          // Search webinar
          oSession.QueryDataElement<Webinar>()
                  .Where(x => x.Published == true &&
                              (x.Topic.Contains(keyword) || x.WebinarContent.Contains(keyword) || x.Tags.Contains(keyword)))
                  .OrderBy(x => x.Topic)
                  .ToList()
                  .Action(x => results.Webinars.Add(new SearchResultData { Content = x.Topic, Identity = x.ID.ToString(), Type = "Webinar" }));

          // Search webinar
          oSession.QueryDataElement<TherapeuticProtocol>()
                  .Where(x => x.Published == true &&
                              (x.Topic.Contains(keyword) || x.BlogContent.Contains(keyword) || x.Tags.Contains(keyword)))
                  .OrderBy(x => x.Topic)
                  .ToList()
                  .Action(x => results.Protocols.Add(new SearchResultData { Content = x.Topic, Identity = x.ID.ToString(), Type = "Protocol" }));
        }
        return Ok(results);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error occurred");
        return BadRequest();
      }
      finally
      {
        oSession.Close();
      }
    }

  }
}
