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
        public ActionResult Index()
        {


            var user = Guid.Parse("01181168-6215-4050-9F46-9B1DCAA1626E");
            //var returnFamilies = context.P_Mob_Get_ReportFamilies("John");
            var model = new SPMenuModel();






            //var user = Guid.Parse("01181168-6215-4050-9F46-9B1DCAA1626E");
            model.familiesReport = context.P_Mob_Get_ReportFamilies(user).ToList();
            model.CategoriesReport = context.P_Mob_Get_ReportCategories(user, 1).ToList();
            model.namesReport = context.P_Mob_Get_ReportNames(user, 1, 2).ToList();





            model.sitesRep = context.P_Mob_Get_SitesForAUser(user).ToList();
            ViewBag.Brand = model.sitesRep;
            return View(model);



        }



        //Test changed, being used for MvP

       
        public ActionResult urlT()
        {

            SPMenuModel model = new SPMenuModel();
            var user = Guid.Parse("01181168-6215-4050-9F46-9B1DCAA1626E");

            var keys = Request.Form.AllKeys;

            var site =Request.Form.Get(keys[0]);
            var brand =Request.Form.Get(keys[1]);
            var dateFrom = Request.Form.Get(keys[2]);
            var dateTo = Request.Form.Get(keys[3]);
            var urlFromSP = "0";

            P_Mob_GetReportURL_Result test = new P_Mob_GetReportURL_Result();
            //ObjectResult<P_> objectResult_BrandID = context.P_Mob_Get_SitesForAUser(user);





            ObjectResult<P_Mob_GetReportURL_Result> objectResultURL = context.P_Mob_GetReportURL(1, 1, 1, dateFrom, dateTo, user);


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