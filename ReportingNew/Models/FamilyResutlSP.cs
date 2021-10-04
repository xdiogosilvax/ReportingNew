using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportingNew.Models
{
    


    //Each class will call a diferent  class and save all results for a list.
    //when family SP is called its results are saved on a list, then when categories report is called, one of the parameters used
    // is the list where the result of the SP families are stores
    //for report names works the same way, using the result of the previous stored procedures that are stored in list.


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
        public int DateDisabled { get; set; }

    }
    public class Sites
    {
        public Nullable<int> ID { get; set; }
        public string Text { get; set; }
    }






}