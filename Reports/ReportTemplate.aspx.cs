using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Security.Principal;

namespace ReportingNew.Reports
{
    public partial class ReportTemplate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    String reportFolder = System.Configuration.ConfigurationManager.AppSettings["SSRSReportsFolder"].ToString();


                    rvSiteMapping.Height = Unit.Pixel(800);/*(Convert.ToInt32(Request["Height"]));*/
                    rvSiteMapping.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                    rvSiteMapping.ServerReport.ReportServerUrl = new Uri("http://172.20.64.50/ReportServer"); // Add the Reporting Server URL
                    rvSiteMapping.ServerReport.ReportPath = String.Format("/{0}/{1}","Finished", Request["ReportName"].ToString());
                    IReportServerCredentials irsc = new CustomReportCredentials("SSRSAdmin", "QuadraN3t!1", "quadranet");
                    rvSiteMapping.ServerReport.ReportServerCredentials =irsc;
                    //List<ReportParameter> paramList = new List<ReportParameter>();

                    //paramList.Add(new ReportParameter("datefrom", "17 Mar 2022", true));
                    //paramList.Add(new ReportParameter("dateto", "2 Jan 2023", true));
                    //paramList.Add(new ReportParameter("userguid", "01181168-6215-4050-9F46-9B1DCAA1626E", true));
                    //paramList.Add(new ReportParameter("brandid", "356", true));Q
                    //paramList.Add(new ReportParameter("siteid", "965", true));
                    //paramList.Add(new ReportParameter("surname", "Smith", true));

                    //rvSiteMapping.ServerReport.SetParameters(paramList);
                    rvSiteMapping.ServerReport.Refresh();
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
    

    public class CustomReportCredentials : IReportServerCredentials
    {

        // local variable for network credential
        private string _UserName;
        private string _PassWord;
        private string _DomainName;
        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }
        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null; // not use ImpersonationUser
            }
        }
        public ICredentials NetworkCredentials
        {
            get
            {

                // use NetworkCredentials
                return new NetworkCredential(_UserName, _PassWord, _DomainName);
            }
        }
        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {

            // not use FormsCredentials unless you have implements a custom autentication.
            authCookie = null;
            user = password = authority = null;
            return false;
        }

    }

}