using ReportingNew.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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



            
            /*
              //var user = Guid.Parse("01181168-6215-4050-9F46-9B1DCAA1626E");
              model.familiesReport = context.P_Mob_Get_ReportFamilies(user).ToList();
              model.CategoriesReport = context.P_Mob_Get_ReportCategories(user, 4).ToList();
              model.namesReport = context.P_Mob_Get_ReportNames(user, 2, 3).ToList();
            */
            
            
            
            
            foreach (var cat in model.familiesReport = context.P_Mob_Get_ReportFamilies(user).ToList()) 
             {
                Console.WriteLine(cat.FamilyName);
                foreach (var name in model.CategoriesReport = context.P_Mob_Get_ReportCategories(user, cat.FamilyID).ToList())
                {
                    model.namesReport = context.P_Mob_Get_ReportNames(user, cat.FamilyID, name.CategoryID).ToList();

                    foreach (var test in model.namesReport = context.P_Mob_Get_ReportNames(user, cat.FamilyID, name.CategoryID).ToList())
                    {
                        Console.WriteLine(test.ReportName);
                        
                    }
                    
                 }
          
            }
          
            
            model.sitesRep = context.P_Mob_Get_SitesForAUser(user).ToList();


            return View(model);

        }



        //Test changed, being used for MvP

        [HttpPost]
        public ActionResult urlMVP() 
        {

            SPMenuModel model = new SPMenuModel();
            var user = Guid.Parse("01181168-6215-4050-9F46-9B1DCAA1626E");

            var keys = Request.Form.AllKeys;
           
            var site = Request.Form.Get(keys[0]);
            var dateFrom = Request.Form.Get(keys[1]);
            var dateTo = Request.Form.Get(keys[2]);
            var url = ("https://qsl-rep-srv01/ReportS/mobilereport/YesterdaySales");

            //Console.WriteLine(site, dateTo, dateFrom);
            P_Mob_GetReportURL_Result test = new P_Mob_GetReportURL_Result();


            var shit = context.P_Mob_GetReportURL(1, 1, 1, dateFrom, dateTo, user).ToString();



            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            request.UseDefaultCredentials = false;
            request.PreAuthenticate = true;

            var cred = new NetworkCredential("Quadranet\\Qnreporting","QuadraN3t!1");
            var cache = new CredentialCache();
            cache.Add(new Uri(url), "Basic", cred);

            request.Credentials = cache;
            var response = (HttpWebResponse)request.GetResponse();

            return Redirect(response.ResponseUri.ToString());



          //  return Redirect(url);



            // P_Mob_GetReportURL_Result test = new P_Mob_GetReportURL_Result();
             //return Redirect(context.P_Mob_GetReportURL(1, 1, 1, dateFrom, dateTo, user));

        }
        
        public ActionResult test() 
        {
            string redirectUrl = "https://qsl-rep-srv01/ReportS/mobilereport/YesterdaySales";
            //Response.AppendHeader("Username", "Quadranet\\Qnreporting");
            //Response.AppendHeader("Password", "QuadraN3t!1");
            //Response.Status = "301 Moved permanently";
            //Response.AppendHeader("Location", appURL);
            //Response.End();

            var request = (HttpWebRequest)WebRequest.Create(redirectUrl);

            request.Method = "GET";
            request.UseDefaultCredentials = false;
            request.PreAuthenticate = true;

            var cred = new NetworkCredential("Quadranet\\Qnreporting", "QuadraN3t!1");
            var cache = new CredentialCache();
            cache.Add(new Uri(redirectUrl), "Basic", cred);

            request.Credentials = cache;
            var response = (HttpWebResponse)request.GetResponse();

            return Redirect(response.ResponseUri.ToString());


        }
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