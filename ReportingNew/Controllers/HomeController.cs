using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Sql;
using System.Data.SqlClient;
using ReportingNew.Models;
        
namespace ReportingNew.Controllers
{
    public class HomeController : Controller
    {
        Consolidated_ReportingEntities context = new Consolidated_ReportingEntities();
        public ActionResult Index()
        {
            /*
            var families = context.Database.SqlQuery<Rep_ReportListforUser_Result>("EXEC [Report].Rep_ReportListforUser @UserName, @FamilyId, @CategoryID ",
                            new SqlParameter("@UserName", "john"),
                            new SqlParameter("@FamilyId", 0),
                            new SqlParameter("@CategoryID", 0)
                            );

            //If you run it with @Username and @FamilyID it will retrieve Categories                
            var name = context.Database.SqlQuery<Rep_ReportListforUser_Result>("EXEC [Report].Rep_ReportListforUser @UserName, @FamilyId, @CategoryID ",
                           new SqlParameter("@UserName", "john"),
                           new SqlParameter("@FamilyId", 10),
                           new SqlParameter("@CategoryID", 0)
                           );

            //If you run it with all parameters it will return Report Names
            var category = context.Database.SqlQuery<Rep_ReportListforUser_Result>("EXEC [Report].Rep_ReportListforUser @UserName, @FamilyId, @CategoryID ",
                           new SqlParameter("@UserName", "john"),
                           new SqlParameter("@FamilyId", 10),
                           new SqlParameter("@CategoryID", 24)
                           );
            */

            var returnData = context.Rep_ReportListforUser("Shi", 0, 0);

            return View(returnData);


           
        }

    }
}