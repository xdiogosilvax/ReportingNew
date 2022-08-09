using ReportingNew.Models;
using RestSharp;
using RestSharp.Authenticators;
using System;
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
using System.Web.Mvc;


namespace ReportingNew.Controllers
{
    public class HomeController : Controller
    {
     
        private bool t;
        WarehouseEntities context = new WarehouseEntities();

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
        public ActionResult Index(Guid? userGuid, Guid? sessionGuid, bool showrep=false)
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
            Session["sessionGuid"] = sessionGuid.Value;

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
        


        [Route("Home/getReport/{userguid}/{sessionGuid}/{repid}")]
        public ActionResult getReport(Guid? userGuid, Guid? sessionGuid, int? repid)
        {

            Session["RepID"] = repid;
            var userID = Session["userID"].ToString();
            var sessionID = Session["sessionGuid"].ToString();
            var dc = new WarehouseEntities();
            var query = (from t
                        in context.Rep_Report_Names
                         where t.ReportID == repid
                         select new { t.ReportName }).Single();

            var repName = query.ReportName;

            Session["RepName"] = repName;

            return RedirectToAction("Index", "Home", new { userguid = userID, sessionGuid= sessionID});
        }

        [Route("Home/getReport/{userguid}/{sessionGuid}")]
        public PartialViewResult LoadForm(Guid? userGuid, Guid? sessionGuid, int? repid)
        {
            userGuid = Guid.Parse(Session["userID"].ToString());
            int repID = Convert.ToInt32(Session["RepID"]);

            SPMenuModel model = new SPMenuModel();
            model.sitesRep = context.P_Mob_Get_SitesForAUser(userGuid);

            // model.reportControls = context.P_Mob_Get_ReportControls(repID);
            var paramenterRecords = context.P_Mob_Get_ReportControls(repID);
            model.reportControls = paramenterRecords;
            return PartialView("_Form", model);
        }


        
        [Route("Home/urlJT/{userguid}/{sessionGuid}/{repid}")]
        public ActionResult urlJT(string userGuid)
        {
            SPMenuModel model = new SPMenuModel();
            userGuid = Session["userID"].ToString();
            var sessionGuid= Session["sessionGuid"].ToString();
            var keys = Request.Form.AllKeys;
            var repID = Convert.ToInt32(Session["RepID"]);
            Dictionary<string, string> ParmsAndValues = new Dictionary<string, string>();
            var reprec = GetReportRec(repID);

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
                ObjectResult<P_Mob_GetReportURL_Result> objectResultURL = context.P_Mob_GetReportURL(repID, brand, siteID, dateFrom, dateTo, userID);

                foreach (var reportURL_Result in objectResultURL.AsEnumerable())
                {
                    urlFromSP = reportURL_Result.URL.ToString();
                }
                CreateToken.Program token = new CreateToken.Program();
                string AuthToken = token.GetTokenDiogo();
               // string AuthToken = " JaxRX1Hkk0X5Y7iyuGCCqwYvYTez4VgSrIqUf - 8BHS5cC6VY2Y9yUju6fzS5hc8upvqGP7epaZtxCTTXFeddoQ3pIdKia3YBB_MaKN01FL6t-uqt0";
                string ulrWithoutToken = urlFromSP.ToString();
                var authenticatedToken= ulrWithoutToken +"?token="+ AuthToken;
                return Redirect(authenticatedToken.ToString());
            }
            else
            {
                t = true;
                Session["ShowPaginatedReport"] = t;
                var family = new FamilyResultResponse();
                family.ShowReports = true;
                Session["ReportName"] = reprec.ReportName;
                //   return RedirectToAction("ReportTemplateLoader", new { reportName = reprec.ReportName });
                return RedirectToAction("Index", "Home", new { userguid = userID, sessionGuid= sessionGuid, showrep=t });
            }
        }
        //[Route("Home/ReportTemplateLoader/{reportName}")]
        public ActionResult ReportTemplateLoader() 
        {
            var height = 500;
            var reportName = Session["ReportName"].ToString();
            var rptInfo = new ReportInfo
            {
                
                Width = 100,
                Height = 800,
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










