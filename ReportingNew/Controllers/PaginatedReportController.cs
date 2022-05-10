using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportingNew.Controllers
{
    public class PaginatedReportController : Controller
    {
      //private string ssrsUrl= ConfigurationManager.AppSettings[].ToString();

        // GET: PaginetedReport
        public ActionResult Index()
        {
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Remote;
            viewer.SizeToReportContent = true;
            viewer.AsyncRendering = true;
           //iewer.ServerReport.ReportServerUrl = new Uri(ssrsUrl);
                
         // viewer.ServerReport.ReportServerCredentials = new System.Net.cre7("qnreporting", "QuadraN3t1!");

            return View();
        }
    }
}