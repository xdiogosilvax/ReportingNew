using ReportingNew.Models;
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
        WarehouseEntities context = new WarehouseEntities();


        [Route("{Home}/{Index}/{id?}")]
        public ActionResult Index(Guid? ID)
        {

            var model = new FamilyResultResponse();
            model.Families = new List<Family>();
            SPMenuModel modelSPMEnu = new SPMenuModel();
            Session["userID"] = ID.Value;


            foreach (var f in context.P_Mob_Get_ReportFamilies(ID).ToList())
            {
                var fam = new Family();
                fam.FamilyID = f.FamilyID;
                fam.FamilyName = f.FamilyName;
                fam.FamilyCat = new List<FamilyResultCat>();

                Console.WriteLine(f.FamilyName);
                foreach (var c in context.P_Mob_Get_ReportCategories(ID, f.FamilyID).ToList())
                {
                    var famcat = new FamilyResultCat();
                    famcat.CategoryID = c.CategoryID;
                    famcat.Category = c.Category;
                    famcat.Allowed = c.Allowed;
                    famcat.FamilCatRep = new List<FamilyResulReportByCategory>();

                    Console.WriteLine(c.Category);
                    foreach (var n in context.P_Mob_Get_ReportNames(ID, f.FamilyID, c.CategoryID).ToList())
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



        [Route("Home/getReport/{repid}")]
        public ActionResult getReport(int? repid)
        {


            Session["RepID"] = repid;

            var userID = Session["userID"].ToString();



            var context = new WarehouseEntities();

            var query = (from t
                        in context.Rep_Report_Names
                        where t.ReportID == repid
                        select new { t.ReportName}).Single();

            var repName = query.ReportName;

            Session["RepName"] = repName;
         
            return RedirectToAction("Index", "Home", new { id = userID });
        }


        public PartialViewResult LoadForm(Guid? userID)
        {
            userID = Guid.Parse(Session["userID"].ToString());
            var repID = Convert.ToInt32(Session["RepID"]);

            SPMenuModel model = new SPMenuModel();
            model.sitesRep = context.P_Mob_Get_SitesForAUser(userID);


            model.reportControls = context.P_Mob_Get_ReportControls(repID);
            return PartialView("_Form", model);
        }




        [Route("Home/urlJT/id/repid")]
        public ActionResult urlJT(string user_id)
            {
            SPMenuModel model = new SPMenuModel();
            user_id = Session["userID"].ToString();
            var keys = Request.Form.AllKeys;

            var dateFrom = "0";
            var dateTo = "0";
            var userID = Guid.Parse(user_id);
            var site = Request.Form.Get(keys[0]);
            if (Convert.ToInt32( keys.Length) <= 1)
            {
             dateFrom=null;
             dateTo= null;
            }
          else
            {
                var dates = Request.Form.Get(keys[1]).ToString();

                string[] split = dates.Split(',');

                dateFrom = split[0];
                dateTo = split[1];
                

            }

            var urlFromSP = "0";
            var repID = Convert.ToInt32(Session["RepID"]);
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

            ObjectResult<P_Mob_GetReportURL_Result> objectResultURL = context.P_Mob_GetReportURL(repID, brand, siteID, dateFrom, dateTo, userID);


            foreach (var reportURL_Result in objectResultURL.AsEnumerable())
            {
                urlFromSP = reportURL_Result.URL.ToString();
            }

            var request = (HttpWebRequest)WebRequest.Create(urlFromSP);

            request.Method = "GET";
            request.UseDefaultCredentials = false;
            request.PreAuthenticate = true;

            var cred = new NetworkCredential("Quadranet\\Qnreporting", "QuadraN3t!1");
            var cache = new CredentialCache();
            cache.Add(new Uri(urlFromSP), "Basic", cred);


            ServicePointManager.ServerCertificateValidationCallback = new
         RemoteCertificateValidationCallback
         (
            delegate { return true; }
         );




            request.Credentials = cache;
            var response = (HttpWebResponse)request.GetResponse();

            return Redirect(urlFromSP);
        }
    }
}