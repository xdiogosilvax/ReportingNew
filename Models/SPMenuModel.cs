using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportingNew.Models;

namespace ReportingNew.Models
{
    public class SPMenuModel
    {


        public FamilyResultResponse familyResult { get; set; }


        public IEnumerable<P_Mob_Get_SitesForAUser_Result> sitesRep { get; set; }
        public IEnumerable<P_Mob_GetReportURL_Result> reportURL { get; set; }
        public IEnumerable<P_Mob_Get_ReportControls_Result> reportControls { get; set; }


    }
}