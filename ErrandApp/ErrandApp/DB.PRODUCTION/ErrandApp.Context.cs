﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ErrandApp.UI.DB.PRODUCTION
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ErrandAppEntities : DbContext
    {
        public ErrandAppEntities()
            : base("name=ErrandAppEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccountDetail> AccountDetails { get; set; }
        public virtual DbSet<CardDetail> CardDetails { get; set; }
        public virtual DbSet<Earning> Earnings { get; set; }
        public virtual DbSet<ErrandeeAvailability> ErrandeeAvailabilities { get; set; }
        public virtual DbSet<ErrandeeLocation> ErrandeeLocations { get; set; }
        public virtual DbSet<ErrandeeProfileImage> ErrandeeProfileImages { get; set; }
        public virtual DbSet<ErrandeeRating> ErrandeeRatings { get; set; }
        public virtual DbSet<ErranderLocation> ErranderLocations { get; set; }
        public virtual DbSet<ErranderRegisteration> ErranderRegisterations { get; set; }
        public virtual DbSet<ErrandType> ErrandTypes { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Registeration> Registerations { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
    }
}
