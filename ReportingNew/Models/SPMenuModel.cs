using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReportingNew.Models;

namespace ReportingNew.Models
{
    public class SPMenuModel
    {
     

        public IEnumerable<P_Mob_Get_BrandsForUser_Result> Brands_User { get; set; }
        public IEnumerable<P_Mob_Get_BrandSites_Result> BrandsSite { get; set; }
        public IEnumerable<P_Mob_Get_ReportCategories_Result> CategoriesReport{ get; set; }
        public IEnumerable<P_Mob_Get_ReportFamilies_Result> familiesReport{ get; set; }
        public IEnumerable<P_Mob_Get_ReportNames_Result> namesReport{ get; set; }
        public IEnumerable<P_Mob_Get_SitesForABrand_Result> sitesForABrand { get; set; }
    }
}