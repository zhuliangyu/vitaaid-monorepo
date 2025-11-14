using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace ECSServerObj
{
  public class RESTfullObject
  {
    public static OPICfg ms_opiCfg { get; set; }
    public static string account { get; set; } = "";
    public static string pwd { get; set; } = "";
    public static bool icp_mode { get; set; } = false;
    public static bool bPROD { get => ms_opiCfg.bPROD; set { ms_opiCfg.bPROD = value; }}
    private static string HEAD_Authorization
    {
      get => (icp_mode) ? Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("admin:32168421")) 
                        : Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(account + ":" + pwd));
    }

    public static void setConfig(bool bPROD = false, string FileName = @"ecsopicfg.ini")
    {
      ms_opiCfg = OPICfg.load(bPROD, FileName);
    }

    #region alex start
    public static bool bTimeout = false;
    private static System.Timers.Timer timer = null;
    private static string sLastAction = "";
    private static DateTime TimeoutPoint = DateTime.Now;
    public delegate void SessionCallBack(string sLastAction);
    public static SessionCallBack TimeoutCallback = null;
    public static SessionCallBack CheckSessionCallback = null;
    public static SessionCallBack CloseSessionCallback = null;
    public static SessionCallBack CountDownCallback = null;
    public static IDisposable CountDownObserver = null;

    public static void CheckSessionTimeout()
    {
      try
      {
        if (ms_opiCfg.SessionTimeout == 0)
        {
          bTimeout = false;
          return;
        }
        //if (bTimeout)
        //{
        //    HandleTimeout();
        //    return;
        //}
        if (timer == null)
        {
          timer = new System.Timers.Timer(ms_opiCfg.SessionTimeout * 1000 * 60);
          timer.Elapsed += (sender, e) => HandleTimeout();
#if DEBUG
          Console.WriteLine("[" + DateTime.Now.ToString() + "]CheckSessionTimeout");
#endif
          //                   timer.Start();
          //                   sLastAction = DateTime.Now.ToString();
          //                   CheckSessionCallback?.Invoke(sLastAction);
          //                   return;
        }
        timer.Stop();
        timer.Start();
        sLastAction = DateTime.Now.ToString();
        CheckSessionCallback?.Invoke(sLastAction);

        if (CountDownCallback != null)
        {
          TimeoutPoint = DateTime.Now.AddMinutes(ms_opiCfg.SessionTimeout);
          if (CountDownObserver != null)
            CountDownObserver.Dispose();
          CountDownObserver = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                                        //.ObserveOnDispatcher()
                                        .Subscribe((_) => CountDownCallback.Invoke((TimeoutPoint > DateTime.Now) ? (TimeoutPoint - DateTime.Now).ToString(@"mm\:ss") : ""));

        }

#if DEBUG
        Console.WriteLine("[" + DateTime.Now.ToString() + "]CheckSessionTimeout");
#endif
      }
      catch (Exception)
      {
      }
    }
    private static void HandleTimeout()
    {
      timer.Stop();
      bTimeout = true;
      TimeoutCallback?.Invoke(sLastAction);
      CloseSessionCallback?.Invoke(sLastAction);
      if (CountDownObserver != null)
        CountDownObserver.Dispose();
      CountDownObserver = null;
    }
    public static void ResetSessionTimeout()
    {
      try
      {
        if (timer != null)
          timer.Stop();
      }
      catch (Exception) { }
      bTimeout = false;
      timer = null;
    }
    #endregion alex end
    public static RestRequest NewRestRequest(Method method)
    {
      var request = new RestRequest(method);
      request.AddHeader("Authorization", HEAD_Authorization);
      return request;
    }

    public static ROBase<WS_Employee> login(string account, string passwd)
    {
      try
      {
        var request = new RestRequest(Method.GET);
        request.RequestFormat = DataFormat.Json;
        request.Resource = "login/{account}?pwd={pwd}&company={companyId}";
        request.AddUrlSegment("account", account);
        request.AddUrlSegment("pwd", passwd);
        request.AddUrlSegment("companyId", ms_opiCfg.CompanyID);
        request.AddHeader("Authorization", HEAD_Authorization);

        var client = new RestClient();

        client.BaseUrl = new Uri(ms_opiCfg.APServer);
        IRestResponse response = client.Execute(request);

        if (string.IsNullOrWhiteSpace(response.Content) || response.StatusCode != System.Net.HttpStatusCode.OK)
        {
          throw new Exception("Can not connect to AP Server. " + response.StatusDescription);
        }
        return JsonConvert.DeserializeObject<ROBase<WS_Employee>>(response.Content);
      }
      catch (Exception ex) { throw ex; }
    }

    public static ROBase<WS_Employee> updateProfile(int empId, string newShortName, string newPWD)
    {
      try
      {
        var request = new RestRequest(Method.PUT);
        request.RequestFormat = DataFormat.Json;
        request.Resource = "updateprofile?empid={empId}&newshortname={newShortName}&newpwd={newPWD}&company={companyId}";
        request.AddUrlSegment("empId", empId);
        request.AddUrlSegment("newShortName", newShortName);
        request.AddUrlSegment("newPWD", newPWD);
        request.AddUrlSegment("companyId", ms_opiCfg.CompanyID);
        request.AddHeader("Authorization", HEAD_Authorization);

        var client = new RestClient();
        client.BaseUrl = new Uri(ms_opiCfg.APServer);
        IRestResponse response = client.Execute(request);

        if (string.IsNullOrWhiteSpace(response.Content) || response.StatusCode != System.Net.HttpStatusCode.OK)
        {
          throw new Exception("Can not connect to AP Server. " + response.StatusDescription);
        }
        return JsonConvert.DeserializeObject<ROBase<WS_Employee>>(response.Content);
      }
      catch (Exception ex) { throw ex; }
    }

    public static ROBase<WS_Employee> authApp(string account, string passwd, string appCode, string versionInfo = "")
    {
      try
      {
        CheckSessionTimeout();
        var request = new RestRequest(Method.GET);
        request.RequestFormat = DataFormat.Json;
        request.Resource = "authapp/{account}?pwd={pwd}&app={appCode}&company={companyId}&version={versionInfo}";
        request.AddUrlSegment("account", account);
        request.AddUrlSegment("pwd", passwd);
        request.AddUrlSegment("appCode", appCode);
        request.AddUrlSegment("companyId", ms_opiCfg.CompanyID);
        request.AddUrlSegment("versionInfo", versionInfo);
        request.AddHeader("Authorization", HEAD_Authorization);

        var client = new RestClient();
        client.BaseUrl = new Uri(ms_opiCfg.APServer);
        IRestResponse response = client.Execute(request);

        if (string.IsNullOrWhiteSpace(response.Content) || response.StatusCode != System.Net.HttpStatusCode.OK)
        {
          throw new Exception("Can not connect to AP Server. " + response.StatusDescription);
        }
        return JsonConvert.DeserializeObject<ROBase<WS_Employee>>(response.Content);
      }
      catch (Exception ex) { throw ex; }
    }
    public static ROBase<AuthMap> authMap(int empId, string funCode, int level = 0)
    {
      try
      {
        CheckSessionTimeout();

        var request = new RestRequest(Method.GET);
        request.RequestFormat = DataFormat.Json;
        request.Resource = "authmap?empId={empId}&funCode={funCode}&companyId={companyId}&level={level}";
        request.AddUrlSegment("empId", empId);
        request.AddUrlSegment("funCode", funCode);
        request.AddUrlSegment("companyId", ms_opiCfg.CompanyID);
        request.AddUrlSegment("level", level);

        var client = new RestClient();
        client.BaseUrl = new Uri(ms_opiCfg.APServer);
        IRestResponse response = client.Execute(request);

        if (string.IsNullOrWhiteSpace(response.Content) || response.StatusCode != System.Net.HttpStatusCode.OK)
        {
          throw new Exception("Can not connect to AP Server. " + response.StatusDescription);
        }
        return JsonConvert.DeserializeObject<ROBase<AuthMap>>(response.Content);
      }
      catch (Exception ex) { throw ex; }
    }

    public static ROBase<List<WS_Employee>> listEmployeesByGroup(string GroupName)
    {
      try
      {
        CheckSessionTimeout();
        var request = new RestRequest(Method.GET);
        request.RequestFormat = DataFormat.Json;
        request.Resource = "listemployeesbygroup/{GroupName}?company={companyId}";
        request.AddUrlSegment("GroupName", GroupName);
        request.AddUrlSegment("companyId", ms_opiCfg.CompanyID);
        request.AddHeader("Authorization", HEAD_Authorization);

        var client = new RestClient();
        client.BaseUrl = new Uri(ms_opiCfg.APServer);
        IRestResponse response = client.Execute(request);

        if (string.IsNullOrWhiteSpace(response.Content) || response.StatusCode != System.Net.HttpStatusCode.OK)
        {
          throw new Exception("Can not connect to AP Server. " + response.StatusDescription);
        }
        return JsonConvert.DeserializeObject<ROBase<List<WS_Employee>>>(response.Content);
      }
      catch (Exception ex) { throw ex; }
    }
    public static ROBase<WS_Employee> getEmployeeByShortName(string shortName)
    {
      try
      {
        CheckSessionTimeout();
        var request = new RestRequest(Method.GET);
        request.RequestFormat = DataFormat.Json;
        request.Resource = "getemployeebyshortname?shortName={shortName}&company={companyId}";
        request.AddUrlSegment("shortName", shortName);
        request.AddUrlSegment("companyId", ms_opiCfg.CompanyID);
        request.AddHeader("Authorization", HEAD_Authorization);

        var client = new RestClient();
        client.BaseUrl = new Uri(ms_opiCfg.APServer);
        IRestResponse response = client.Execute(request);

        if (string.IsNullOrWhiteSpace(response.Content) || response.StatusCode != System.Net.HttpStatusCode.OK)
        {
          throw new Exception("Can not connect to AP Server. " + response.StatusDescription);
        }
        return JsonConvert.DeserializeObject<ROBase<WS_Employee>>(response.Content);
      }
      catch (Exception ex) { throw ex; }
    }

    public static ROBase<List<WS_Employee>> listEmployees(IEnumerable<string> accounts) => listEmployees(accounts.ToList());
    public static ROBase<List<WS_Employee>> listEmployees(IList<string> accounts)
    {
      try
      {
        CheckSessionTimeout();
        var request = new RestRequest(Method.GET);
        request.RequestFormat = DataFormat.Json;
        request.Resource = "listemployees?accounts={accounts}&company={companyId}";
        request.AddUrlSegment("accounts", string.Join(",", accounts));
        request.AddUrlSegment("companyId", ms_opiCfg.CompanyID);
        request.AddHeader("Authorization", HEAD_Authorization);

        var client = new RestClient();
        client.BaseUrl = new Uri(ms_opiCfg.APServer);
        IRestResponse response = client.Execute(request);

        if (string.IsNullOrWhiteSpace(response.Content) || response.StatusCode != System.Net.HttpStatusCode.OK)
        {
          throw new Exception("Can not connect to AP Server. " + response.StatusDescription);
        }
        return JsonConvert.DeserializeObject<ROBase<List<WS_Employee>>>(response.Content);
      }
      catch (Exception ex) { throw ex; }
    }
    public static ROBase<string> getCompanyName(int companyId)
    {
      try
      {
        CheckSessionTimeout();
        var request = new RestRequest(Method.GET);
        request.RequestFormat = DataFormat.Json;
        request.Resource = "getcompanyname/{companyId}";
        request.AddUrlSegment("companyId", companyId);
        request.AddHeader("Authorization", HEAD_Authorization);

        var client = new RestClient();
        client.BaseUrl = new Uri(ms_opiCfg.APServer);
        IRestResponse response = client.Execute(request);

        if (string.IsNullOrWhiteSpace(response.Content) || response.StatusCode != System.Net.HttpStatusCode.OK)
        {
          throw new Exception("Can not connect to AP Server. " + response.StatusDescription);
        }
        return JsonConvert.DeserializeObject<ROBase<string>>(response.Content);
      }
      catch (Exception ex) { throw ex; }
    }
    public static ROBase<WS_Company> getCompany(int companyId)
    {
      try
      {
        CheckSessionTimeout();
        var request = new RestRequest(Method.GET);
        request.RequestFormat = DataFormat.Json;
        request.Resource = "getcompany/{companyId}";
        request.AddUrlSegment("companyId", companyId);
        request.AddHeader("Authorization", HEAD_Authorization);

        var client = new RestClient();
        client.BaseUrl = new Uri(ms_opiCfg.APServer);
        IRestResponse response = client.Execute(request);

        if (string.IsNullOrWhiteSpace(response.Content) || response.StatusCode != System.Net.HttpStatusCode.OK)
        {
          throw new Exception("Can not connect to AP Server. " + response.StatusDescription);
        }
        return JsonConvert.DeserializeObject<ROBase<WS_Company>>(response.Content);
      }
      catch (Exception ex) { throw ex; }
    }
  }
}
