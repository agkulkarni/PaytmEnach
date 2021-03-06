﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PaytmEnach
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PaytmEnachEntities : DbContext
    {
        public PaytmEnachEntities()
            : base("name=PaytmEnachEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AccountDetail> AccountDetails { get; set; }
        public virtual DbSet<FrequencyUnitMaster> FrequencyUnitMasters { get; set; }
        public virtual DbSet<SIPData> SIPDatas { get; set; }
        public virtual DbSet<SIPRegistrationData> SIPRegistrationDatas { get; set; }
        public virtual DbSet<UserMaster> UserMasters { get; set; }
    
        public virtual ObjectResult<Nullable<decimal>> CreateUser(string userName, Nullable<System.DateTime> dOB, string email, string mobile, string aadharNumber, string pANNumber, string passportNumber, Nullable<int> userID, Nullable<int> insupdUsrID, Nullable<bool> active, string mode)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var dOBParameter = dOB.HasValue ?
                new ObjectParameter("DOB", dOB) :
                new ObjectParameter("DOB", typeof(System.DateTime));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var mobileParameter = mobile != null ?
                new ObjectParameter("Mobile", mobile) :
                new ObjectParameter("Mobile", typeof(string));
    
            var aadharNumberParameter = aadharNumber != null ?
                new ObjectParameter("AadharNumber", aadharNumber) :
                new ObjectParameter("AadharNumber", typeof(string));
    
            var pANNumberParameter = pANNumber != null ?
                new ObjectParameter("PANNumber", pANNumber) :
                new ObjectParameter("PANNumber", typeof(string));
    
            var passportNumberParameter = passportNumber != null ?
                new ObjectParameter("PassportNumber", passportNumber) :
                new ObjectParameter("PassportNumber", typeof(string));
    
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(int));
    
            var insupdUsrIDParameter = insupdUsrID.HasValue ?
                new ObjectParameter("InsupdUsrID", insupdUsrID) :
                new ObjectParameter("InsupdUsrID", typeof(int));
    
            var activeParameter = active.HasValue ?
                new ObjectParameter("Active", active) :
                new ObjectParameter("Active", typeof(bool));
    
            var modeParameter = mode != null ?
                new ObjectParameter("Mode", mode) :
                new ObjectParameter("Mode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("CreateUser", userNameParameter, dOBParameter, emailParameter, mobileParameter, aadharNumberParameter, pANNumberParameter, passportNumberParameter, userIDParameter, insupdUsrIDParameter, activeParameter, modeParameter);
        }
    }
}
