﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReportingNew
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class WarehouseEntities : DbContext
    {
        public WarehouseEntities()
            : base("name=WarehouseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        [EdmFunction("WarehouseEntities", "P_Mob_Get_BrandSites")]
        public virtual IQueryable<P_Mob_Get_BrandSites_Result> P_Mob_Get_BrandSites(string username, string brandGroup, Nullable<int> siteId)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var brandGroupParameter = brandGroup != null ?
                new ObjectParameter("BrandGroup", brandGroup) :
                new ObjectParameter("BrandGroup", typeof(string));
    
            var siteIdParameter = siteId.HasValue ?
                new ObjectParameter("SiteId", siteId) :
                new ObjectParameter("SiteId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<P_Mob_Get_BrandSites_Result>("[WarehouseEntities].[P_Mob_Get_BrandSites](@username, @BrandGroup, @SiteId)", usernameParameter, brandGroupParameter, siteIdParameter);
        }
    
        public virtual ObjectResult<P_Mob_Get_BrandsForUser_Result> P_Mob_Get_BrandsForUser(string username)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<P_Mob_Get_BrandsForUser_Result>("P_Mob_Get_BrandsForUser", usernameParameter);
        }
    
        public virtual ObjectResult<P_Mob_Get_ReportCategories_Result> P_Mob_Get_ReportCategories(string username, Nullable<int> familyid)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var familyidParameter = familyid.HasValue ?
                new ObjectParameter("Familyid", familyid) :
                new ObjectParameter("Familyid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<P_Mob_Get_ReportCategories_Result>("P_Mob_Get_ReportCategories", usernameParameter, familyidParameter);
        }
    
        public virtual ObjectResult<P_Mob_Get_ReportFamilies_Result> P_Mob_Get_ReportFamilies(string username)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<P_Mob_Get_ReportFamilies_Result>("P_Mob_Get_ReportFamilies", usernameParameter);
        }
    
        public virtual ObjectResult<P_Mob_Get_ReportNames_Result> P_Mob_Get_ReportNames(string username, Nullable<int> familyid, Nullable<int> categoryID)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var familyidParameter = familyid.HasValue ?
                new ObjectParameter("Familyid", familyid) :
                new ObjectParameter("Familyid", typeof(int));
    
            var categoryIDParameter = categoryID.HasValue ?
                new ObjectParameter("CategoryID", categoryID) :
                new ObjectParameter("CategoryID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<P_Mob_Get_ReportNames_Result>("P_Mob_Get_ReportNames", usernameParameter, familyidParameter, categoryIDParameter);
        }
    
        public virtual ObjectResult<P_Mob_Get_SitesForABrand_Result> P_Mob_Get_SitesForABrand(string username, Nullable<int> brandid)
        {
            var usernameParameter = username != null ?
                new ObjectParameter("username", username) :
                new ObjectParameter("username", typeof(string));
    
            var brandidParameter = brandid.HasValue ?
                new ObjectParameter("brandid", brandid) :
                new ObjectParameter("brandid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<P_Mob_Get_SitesForABrand_Result>("P_Mob_Get_SitesForABrand", usernameParameter, brandidParameter);
        }
    }
}
