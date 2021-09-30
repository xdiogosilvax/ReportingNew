using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportingNew.Models
{
    public class FamilyResultResponse
    {
        public List<Family> Families { get; set; }
        public List<Sites> SitesGet { get; set; }
    }

    public class Family
    {

        public Nullable<int> FamilyID { get; set; }
        public string FamilyName { get; set; }

        public List<FamilyResultCat> FamilyCat { get; set; }


    }
    public class FamilyResultCat
    {
        public Nullable<int> familyid { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string Category { get; set; }
        public int Allowed { get; set; }

        public List<FamilyResulReportByCategory> FamilCatRep { get; set; }

    }

    public class FamilyResulReportByCategory
    {

        public Nullable<int> ReportID { get; set; }
        public Nullable<int> categoryid { get; set; }
        public string ReportName { get; set; }


    }
    public class Sites
    {
        public Nullable<int> ID { get; set; }
        public string Text { get; set; }
    }



}