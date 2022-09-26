//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Klada_v3.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class CalcOddsTable
    {
        public int EventID { get; set; }
        public string Home { get; set; }
        public string Away { get; set; }
        public string EventTime { get; set; }
        public Nullable<System.DateTime> EventDateTime { get; set; }
        public Nullable<decimal> Odd1 { get; set; }
        public string Klada1 { get; set; }
        public Nullable<decimal> OddX { get; set; }
        public string KladaX { get; set; }
        public Nullable<decimal> Odd2 { get; set; }
        public string Klada2 { get; set; }
        public Nullable<decimal> Odd1X { get; set; }
        public string Klada1X { get; set; }
        public Nullable<decimal> OddX2 { get; set; }
        public string KladaX2 { get; set; }
        public Nullable<decimal> Odd12 { get; set; }
        public string Klada12 { get; set; }
        public Nullable<decimal> CalcOdd { get; set; }
        public string SportType { get; set; }
        public Nullable<int> SportTypeID { get; set; }
        public System.Guid HomeSystemID { get; set; }
        public System.Guid AwaySystemID { get; set; }
    }
}
