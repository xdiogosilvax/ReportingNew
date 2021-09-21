using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Sql;
using System.Data.SqlClient;
using ReportingNew.Models;
using System.Dynamic;
        
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

            return View(model);

        }


       



    }
}