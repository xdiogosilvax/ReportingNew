using ReportingNew.Models;
using System;
using System.Linq;
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
            
            model.familiesReport = context.P_Mob_Get_ReportFamilies(user).ToList();
            model.CategoriesReport = context.P_Mob_Get_ReportCategories(user, 1).ToList();
            model.namesReport= context.P_Mob_Get_ReportNames(user, 1, 1).ToList();
            

            /*
            foreach (var cat in model.familiesReport = context.P_Mob_Get_ReportFamilies(user).ToList()) 
            {
                foreach (var name in model.CategoriesReport = context.P_Mob_Get_ReportCategories(user, cat.FamilyID).ToList())
                {
                    model.=context.P_Mob_Get_ReportNames(user, cat.FamilyID,name.CategoryID).ToList();
                }
            }
          */
            
            model.sitesRep = context.P_Mob_Get_SitesForAUser(user).ToList();


            return View(model);

        }



        //TTest changed, being used for MvP

        [HttpPost]
        public ActionResult urlMVP() 
        {



            var keys = Request.Form.AllKeys;

            var site = Request.Form.Get(keys[0]);
            var dateFrom = Request.Form.Get(keys[1]);
            var dateTo = Request.Form.Get(keys[2]);

            Console.WriteLine(site, dateTo, dateFrom);


            string url =("https://testreporting.quadranet.co.uk/reports/mobilereport/YesterdaySales?rs:Embed=True&Title=False");
            
           
           return Redirect(url);
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