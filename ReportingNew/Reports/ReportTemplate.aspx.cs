using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Security.Principal;
using ReportingNew.Controllers;

namespace ReportingNew.Reports
{
    public partial class ReportTemplate : System.Web.UI.Page
    {
        public Dictionary<string, string> GetParameterList() 
        {
             return (Dictionary<string, string>)Session["paramdic"];

        }

        protected void Page_Load(object sender, EventArgs e)
        {   
            if (!IsPostBack)
            {
                try
                {
                    String reportFolder = System.Configuration.ConfigurationManager.AppSettings["SSRSReportsFolder"].ToString();

                    var test = new HomeController();
                    var log = new WebLog()
                    {
                        Level = "Info",
                        TimeStamp = DateTime.Now,
                        UserMessage="Got to the report loader"
                    };

                    test.AddToLog(log);
                    rvSiteMapping.Height = Unit.Pixel(600);/*(Convert.ToInt32(Request["Height"]));*/
                    rvSiteMapping.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                    
                    //rvSiteMapping.ServerReport.ReportServerUrl = new Uri("http://qsl-rep-srv01/ReportServer"); // Add the Reporting Server URL
                    rvSiteMapping.ServerReport.ReportServerUrl = new Uri("https://testreporting.quadranet.co.uk/ReportServer"); // Add the Reporting Server URL
                    rvSiteMapping.ServerReport.ReportPath = String.Format("/{0}/{1}","MDX Reports", Request["ReportName"].ToString());
                    IReportServerCredentials irsc = new CustomReportCredentials("SSRSAdmin", "QuadraN3t!1", "quadranet");
                    rvSiteMapping.ServerReport.ReportServerCredentials =irsc;
                    Dictionary<string, string> parameters = GetParameterList();
                    List<ReportParameter> paramList = new List<ReportParameter>();
                    foreach (var rec in parameters) 
                    {
                        paramList.Add(new ReportParameter(rec.Key, rec.Value, true));
                    }
                 

                    rvSiteMapping.ServerReport.SetParameters(paramList);
                    rvSiteMapping.ServerReport.Refresh();
                    
                }
                catch (Exception ex)
                {   
                    Console.WriteLine(ex.ToString());
                }
            }
            Session["reportname"] = null;
            Session["userID"] = null;
            Session["dbxurl"] = null;
            Session["RepID"] = null;
            Session["sessionGuid"] = null;
            Session["paramdic"] = null;
            Session["ShowPaginatedReport"] = null;
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
            //authCookie = null;
            //user = password = authority = null;
            //return false;

            user = _UserName;
            password = _PassWord;
            authority = null;
            authCookie = null;
            return true;
        }

    }

}