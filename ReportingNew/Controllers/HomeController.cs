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
        public ActionResult Index(Guid? userID)
        {


           // var user = userID;
            //var returnFamilies = context.P_Mob_Get_ReportFamilies("John");
            var model = new SPMenuModel();



            userID = Guid.Parse("01181168-6215-4050-9F46-9B1DCAA1626E");
            model.familiesReport = context.P_Mob_Get_ReportFamilies(userID).ToList();
            model.CategoriesReport = context.P_Mob_Get_ReportCategories(userID, 1).ToList();
            model.namesReport = context.P_Mob_Get_ReportNames(userID, 1, 2).ToList();


            var family = 0;
            var category = 0;


            ObjectResult<P_Mob_Get_ReportFamilies_Result> families_Results = context.P_Mob_Get_ReportFamilies(userID);
            ObjectResult<P_Mob_Get_ReportCategories_Result> categories_results = context.P_Mob_Get_ReportCategories(userID,family);
            ObjectResult<P_Mob_Get_ReportNames_Result> report_results = context.P_Mob_Get_ReportNames(userID, family,category);

            /*
            
            foreach(var i in families_Results.AsEnumerable())
            {
                i.FamilyID = family;

                foreach(var c in categories_results.AsEnumerable())
                {
                    i.FamilyID = family;
                    c.CategoryID = category;

                    foreach(var n in report_results.AsEnumerable())
                    {

                        i.FamilyID = family;
                        c.CategoryID = category;
                        
                    }


                }

                   
            }

            */





            model.sitesRep = context.P_Mob_Get_SitesForAUser(userID).ToList();
            ViewBag.Brand = model.sitesRep;
            return View(model);



        }

       
        public ActionResult urlT(Guid? user_id)
        {
            SPMenuModel model = new SPMenuModel();
            user_id =Guid.Parse("01181168-6215-4050-9F46-9B1DCAA1626E");

            var keys = Request.Form.AllKeys;

            var site = Request.Form.Get(keys[0]);
            var dateFrom = Request.Form.Get(keys[1]);
            var dateTo = Request.Form.Get(keys[2]);
            var urlFromSP = "0";


            var brand = 0;
            var siteID = 0;
            P_Mob_GetReportURL_Result test = new P_Mob_GetReportURL_Result();
            ObjectResult<P_Mob_Get_SitesForAUser_Result> objectResult_sites = context.P_Mob_Get_SitesForAUser(user_id);

            foreach(var i in objectResult_sites.AsEnumerable())
            {

                if(i.SiteName==site)
                {

                     brand = Convert.ToInt32(i.Brandid);
                     siteID = Convert.ToInt32(i.SiteID);
                }
            }


            ObjectResult<P_Mob_GetReportURL_Result> objectResultURL = context.P_Mob_GetReportURL(1, brand, siteID, dateFrom, dateTo, user_id);


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

            return Redirect(response.ResponseUri.ToString());
        }


        /*
        public HttpWebRequest urlMVP()
        {

            HttpWebRequest request = null;

            request.Method = "POST";
            request.Credentials = new NetworkCredential("username", "password");

            return redirect()request;

        }


        */


    }
}