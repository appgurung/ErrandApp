//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Earning
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string ErrandeeID { get; set; }
        public System.DateTime DateEarned { get; set; }
        public bool IsLiquidated { get; set; }
    }
}
