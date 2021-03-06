//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class SIPRegistrationData
    {
        public int ID { get; set; }
        public string SubsID { get; set; }
        public string txnToken { get; set; }
        public Nullable<bool> IsTransactionInit { get; set; }
        public string TranInitResultCode { get; set; }
        public string TranInitResultStatus { get; set; }
        public Nullable<bool> IsMandateFormDowloaded { get; set; }
        public string MandateFormDownloadResultCode { get; set; }
        public string MandateFormDownloadResultStatus { get; set; }
        public Nullable<bool> IsMandateFormUploaded { get; set; }
        public string MandateFormUploadResultCode { get; set; }
        public string MandateFormUploadResultStatus { get; set; }
        public System.DateTime InsertDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public int InsertUserID { get; set; }
        public int UpdateUserID { get; set; }
        public int SIPID { get; set; }
        public string OrderID { get; set; }
        public Nullable<long> AccountDetails { get; set; }
    
        public virtual AccountDetail AccountDetail { get; set; }
        public virtual SIPData SIPData { get; set; }
        public virtual UserMaster UserMaster { get; set; }
        public virtual UserMaster UserMaster1 { get; set; }
    }
}
