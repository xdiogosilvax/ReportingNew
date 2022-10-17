using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportingNew.Models;

namespace ReportingNew.Models
{
    public class SPMenuModel
    {


        public IEnumerable<P_Mob_Get_SitesForAUser_Result> sitesRep { get; set; }

        public IEnumerable<P_Mob_Get_ReportControls_Result> reportControls { get; set; }

        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public string DbxUrl { get; set; }
        public Guid? SessionGuid { get; set; }
        public Guid? UserID { get; set; }

    }
}