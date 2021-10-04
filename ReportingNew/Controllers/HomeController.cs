﻿using ReportingNew.Models;
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
using ReportingNew.Models;
namespace ReportingNew.Controllers
{
    public class HomeController : Controller
    {
        WarehouseEntities context = new WarehouseEntities();





        public ActionResult Index(Guid? ID)
        {

            try
            {

        
            // var user = userID;
            //var returnFamilies = context.P_Mob_Get_ReportFamilies("John");
            var model = new FamilyResultResponse();
            model.Families=new List<Family>();
            SPMenuModel modelSPMEnu = new SPMenuModel();
            Guid userID = ID.Value;
            TempData["userID"] = ID.Value;





            foreach (var f in context.P_Mob_Get_ReportFamilies(userID).ToList())
            {
                var fam = new Family();
                fam.FamilyID = f.FamilyID;
                fam.FamilyName= f.FamilyName;
                fam.FamilyCat = new List<FamilyResultCat>();

                Console.WriteLine(f.FamilyName);
                foreach (var c in  context.P_Mob_Get_ReportCategories(userID, f.FamilyID).ToList())
                {
                    var famcat = new FamilyResultCat();
                    famcat.CategoryID = c.CategoryID;
                    famcat.Category = c.Category;
                    famcat.Allowed = c.Allowed;
                    famcat.FamilCatRep= new List<FamilyResulReportByCategory>();

                    Console.WriteLine(c.Category);
                    foreach (var n in context.P_Mob_Get_ReportNames(userID, f.FamilyID, c.CategoryID).ToList())
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
            /*
            var sites = new Sites();
            foreach (var s in context.P_Mob_Get_SitesForAUser(userID).ToList())
            {
                sites.ID = s.SiteID;
                sites.Text = s.SiteName;
            }

            model.SitesGet.Add(sites);
            */

            //modelSPMEnu.familyResult.SitesGet = context.P_Mob_Get_SitesForAUser(userID).ToList();
            //ViewBag.Brand = modelSPMEnu.sitesRep;


            modelSPMEnu.familyResult = model;


            return View(model);
            }
            catch(Exception ex)
            {

            }

        }


    
        public ActionResult getReport(int reportid)
        {
           

            TempData["RepID"] = reportid;

            return RedirectToAction("Index");
           

            
        }

        public ActionResult urlT(Guid? user_id)
        {
            SPMenuModel model = new SPMenuModel();
            user_id = Guid.Parse("01181168-6215-4050-9F46-9B1DCAA1626E");

            var keys = Request.Form.AllKeys;

            var site = Request.Form.Get(keys[0]);
            var dateFrom = Request.Form.Get(keys[1]);
            var dateTo = Request.Form.Get(keys[2]);
            var urlFromSP = "0";
            var repID = Convert.ToInt32(TempData["RepID"]);
                

            var brand = 0;
            var siteID = 0;
            P_Mob_GetReportURL_Result test = new P_Mob_GetReportURL_Result();
            ObjectResult<P_Mob_Get_SitesForAUser_Result> objectResult_sites = context.P_Mob_Get_SitesForAUser(user_id);

            foreach (var i in objectResult_sites.AsEnumerable())
            {

                if (i.SiteName == site)
                {

                    brand = Convert.ToInt32(i.Brandid);
                    siteID = Convert.ToInt32(i.SiteID);
                }
            }

            ObjectResult<P_Mob_GetReportURL_Result> objectResultURL = context.P_Mob_GetReportURL(repID, brand, siteID, dateFrom, dateTo, user_id);


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



        public PartialViewResult LoadForm(Guid? userID)
        {
            userID = Guid.Parse(TempData["userID"].ToString());
            SPMenuModel model = new SPMenuModel();
            model.sitesRep = context.P_Mob_Get_SitesForAUser(userID);
            return PartialView("_Form", model);
        }

        public ActionResult Getsite(Guid? userID)
        {
            userID = Guid.Parse(TempData["userID"].ToString());
            //userID = Guid.Parse("01181168-6215-4050-9F46-9B1DCAA1626E");
            SPMenuModel model = new SPMenuModel();
            model.sitesRep = context.P_Mob_Get_SitesForAUser(userID);

            return PartialView("_Form", model);


        }


    }
}