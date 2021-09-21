using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Sql;
using System.Data.SqlClient;
using ReportingNew.Models;
using System.Dynamic;
using System.Net;

namespace ReportingNew.Controllers
{
    public class HomeController : Controller
    {
        WarehouseEntities context = new WarehouseEntities();
        public ActionResult Index()
        {

            //var returnFamilies = context.P_Mob_Get_ReportFamilies("John");

            //return View(returnFamilies);

            var model = new SPMenuModel();
            //model.BrandsSite = context.P_Mob_Get_BrandSites_Result("john",2,1).ToList();
            //model.Brands_User = context.P_Mob_Get_BrandsForUser.ToList();
            model.familiesReport= context.P_Mob_Get_ReportFamilies("John");
            model.CategoriesReport = context.P_Mob_Get_ReportCategories("John", 1);
            model.namesReport = context.P_Mob_Get_ReportNames("John", 1,2);

            model.Brands_User = context.P_Mob_Get_BrandsForUser("John");
            model.BrandsSite = context.P_Mob_Get_BrandSites("John","The White Hart".ToString(),1);


            return View(model);

        }



        //To be changed, being used for MvP

        public ActionResult urlMVP() 
        {
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