using Microsoft.Reporting.WebForms;
using ReportingNew.Models;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ReportingNew.Controllers
{
    public class HomeController : Controller
    {
        private string url;
        private bool t;
        WarehouseEntities context = new WarehouseEntities();
        Dictionary<string, string> dic = new Dictionary<string, string>();
        Reports.ReportTemplate reportViewer = new Reports.ReportTemplate();
        public string dt;
        public string df;
        public string ug;
        public string si;
        public string bi;


        private bool CheckUserGuid(Guid userGuid) 
        {
            string nuserGuid = userGuid.ToString();
            var client = new RestClient("https://dbxdev.quadranet.co.uk/api/Management/ValidateSessionGUID/"+ nuserGuid);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            if (response.Content == "true")
                return true;
           
            return false;
           
        }

        //[Route("{Home}/{Index}/{userguid}/{sessionGuid}")]    
        public ActionResult Index(Guid? userGuid, Guid? sessionGuid, int? reportId, string dbxUrl="" ,bool showrep=false)
        {
            t = showrep;
            if (t != true)
            {
                Session["ShowPaginatedReport"] = t;
            }
            var model = new FamilyResultResponse();
            model.Families = new List<Family>();
            SPMenuModel modelSPMEnu = new SPMenuModel();
            Session["userID"] = userGuid.Value; 

            if (sessionGuid != null)
                Session["sessionGuid"] = sessionGuid.Value;

            if (dbxUrl != null && dbxUrl != "") 
                Session["dbxUrl"] = dbxUrl;

            if (reportId > 0 && reportId != null)
                Session["RepID"] = reportId.Value;

            foreach (var f in context.P_Mob_Get_ReportFamilies(userGuid).ToList())
            {
                var fam = new Family();
                fam.FamilyID = f.FamilyID;
                fam.FamilyName = f.FamilyName;
                fam.FamilyCat = new List<FamilyResultCat>();

                Console.WriteLine(f.FamilyName);
                foreach (var c in context.P_Mob_Get_ReportCategories(userGuid, f.FamilyID).ToList())
                {
                    var famcat = new FamilyResultCat();
                    famcat.CategoryID = c.CategoryID;
                    famcat.Category = c.Category;
                    famcat.Allowed = c.Allowed;
                    famcat.FamilCatRep = new List<FamilyResulReportByCategory>();

                    Console.WriteLine(c.Category);
                    foreach (var n in context.P_Mob_Get_ReportNames(userGuid, f.FamilyID, c.CategoryID).ToList())
                    {
                        var famcatrep = new FamilyResulReportByCategory();
                        famcatrep.ReportID = n.ReportID;
                        famcatrep.ReportName = n.ReportName;
                        famcatrep.categoryid = n.categoryid;
                        Console.WriteLine(n.ReportName);
                        famcat.FamilCatRep.Add(famcatrep);
                    }
                    fam.FamilyCat.Add(famcat);
                }
                model.Families.Add(fam);

            }
            modelSPMEnu.familyResult = model;
            return View(model);
        }



        [Route("Home/getReport/{userguid}/{sessionGuid}/{repid}/{dbxUrl}")]
        public ActionResult getReport(Guid? userGuid, Guid? sessionGuid, int? repid, string dbxUrl)
        {

            string userID;
            string sessionID;
            Session["RepID"] = repid;
            var reportId = Session["RepID"];
            if (userGuid == null)
                userID = Session["userID"].ToString();
            else
            { 
                userID = userGuid.ToString();
                Session["userID"]= userID;

            }
           // var url= HttpUtility.UrlDecode(dbxUrl);
            Session["dbxUrl"]=url;
            if (Session == null)
                sessionID = Session["sessionGuid"].ToString();
            else
                sessionID = sessionGuid.ToString();

            var dc = new WarehouseEntities();
            var query = (from t
                        in context.Rep_Report_Names
                         where t.ReportID == repid
                         select new { t.ReportName }).Single();

            var repName = query.ReportName;

            Session["RepName"] = repName;

            return RedirectToAction("Index", "Home", new { userguid = userID, sessionGuid= sessionID, reportId = reportId, dbxUrl=url});
        }


    //   [Route("Home/LoadForm2/{userguid}/{sessionGuid}/{repid}/{dbxUrl}")]
        public ActionResult LoadForm2(SPMenuModel  model)
        {
            var userGuid = Guid.Parse(Session["userID"].ToString());
            int repID = Convert.ToInt32(Session["RepID"]);
   
            model.sitesRep = context.P_Mob_Get_SitesForAUser(userGuid);

            var paramenterRecords = context.P_Mob_Get_ReportControls(repID);
            model.reportControls = paramenterRecords;
            return PartialView("_Form", model);
        }


            
        [Route("Home/urlJT/{userGuid}/{sessionGuid}/{repid}/{dbxUrl}")]
        public ActionResult urlJT(string userGuid, string sessionGuid, int repid, string dbxUrl)
        {
            SPMenuModel model = new SPMenuModel();
            var keys = Request.Form.AllKeys;
            Dictionary<string, string> ParmsAndValues = new Dictionary<string, string>();
            var reprec = GetReportRec(repid);

            var dateFrom = "0";
            var dateTo = "0";
            var userID = Guid.Parse(userGuid);
            var site = Request.Form.Get(keys[0]);
            ParmsAndValues.Add(keys[0], site);
            if (Convert.ToInt32(keys.Length) <= 1)
            {
                dateFrom = null;
                dateTo = null;
            }
            else
            {
                var dates = Request.Form.Get(keys[1]).ToString();
                string[] split = dates.Split(',');
                dateFrom = split[0];
                dateTo = split[1];
                ParmsAndValues.Add("dateFrom", dateFrom);
                ParmsAndValues.Add("dateTo", dateTo);
            }
            
            var urlFromSP = "0";
            var brand = 0;
            var siteID = 0;

            P_Mob_GetReportURL_Result test = new P_Mob_GetReportURL_Result();
            ObjectResult<P_Mob_Get_SitesForAUser_Result> objectResult_sites = context.P_Mob_Get_SitesForAUser(userID);

            foreach (var i in objectResult_sites.AsEnumerable())
            {
                if (i.SiteName == site)
                {
                    brand = Convert.ToInt32(i.Brandid);
                    siteID = Convert.ToInt32(i.SiteID);
                }
            }
            if (reprec.Format == "1")
            {
                ObjectResult<P_Mob_GetReportURL_Result> objectResultURL = context.P_Mob_GetReportURL(repid, brand, siteID, dateFrom, dateTo, userID);

                foreach (var reportURL_Result in objectResultURL.AsEnumerable())
                {
                    urlFromSP = reportURL_Result.URL.ToString();
                }
                var dbxUrlCoded =HttpUtility.UrlEncode(dbxUrl);
                
                var authenticatedUrl = urlFromSP + "?sessionGuid=" + sessionGuid+ "?dbx="+ dbxUrlCoded;
                Session["RepID"] = null;
                List<string> keyss = new List<string>();

                var staleItem = Url.Action("urlJT", "Home", new
                {
                    userGuid=userGuid,
                    sessionGuid=sessionGuid,
                    repid=repid,
                    dbxUrl=dbxUrl
                });

                //  Remove the item from cache  
                Response.RemoveOutputCacheItem(staleItem);
                Session["RepID"] = null;
                return Redirect(authenticatedUrl.ToString());
            }
            else if(reprec.Format=="2")
            {
                t = true;
                Session["ShowPaginatedReport"] = t;
                var family = new FamilyResultResponse();
                family.ShowReports = true;
                df = dateFrom;
                dt = dateTo;
                ug = userGuid;
                bi = brand.ToString();
                si = siteID.ToString();
                Session["reportname"] = reprec.ReportName;
                dic.Add("datefrom", dateFrom);
                dic.Add("dateto", dateTo);
                dic.Add("userguid", userGuid);
                dic.Add("brandid", brand.ToString());
                dic.Add("siteid", siteID.ToString());
                Session["paramdic"] = dic;

                return RedirectToAction("Index", "Home", new { userguid = userID, sessionGuid= sessionGuid, showrep=t });
                
            }
            return RedirectToAction("Index", "Home", new { userguid = userID, sessionGuid = sessionGuid, showrep = t });

        }
        //[Route("Home/ReportTemplateLoader/{reportName}")]
        public ActionResult ReportTemplateLoader() 
        {
            
            
            var height = 500;
            var reportName = Session["ReportName"].ToString();
            var rptInfo = new ReportInfo
            {
                
                Width = 100,
                Height = 600,
                ReportName = reportName,
                ReportDescription = reportName,
                ReportURL = String.Format("../../../Reports/ReportTemplate.aspx?ReportName={0}&Height={1}", reportName, height)
                
            };
            return PartialView("ReportTemplate", rptInfo);
        }
        

        public Rep_Report_Names GetReportRec(int reportID)
        {
            var dc = new WarehouseEntities();
            var reprec = dc.Rep_Report_Names.Where(w => w.ReportID == reportID).FirstOrDefault();
            if (reprec == null)
                return null;
            return reprec;
        }


    }
}










