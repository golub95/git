//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OddsMagic.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MatchSystemIDs
    {
        public int id { get; set; }
        public System.Guid EventSystemID { get; set; }
        public string EventName { get; set; }
        public System.DateTime EventDateTime { get; set; }
        public int EventSportTypeID { get; set; }
        public string KladaName { get; set; }
        public decimal Similarity { get; set; }
        public bool Matched { get; set; }
    }
}
