//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vehicle_Showroom_Management_System.Areas.Admin.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Image
    {
        public int ImageId { get; set; }
        public string ModelNumberCar { get; set; }
        public string Name { get; set; }
        public Nullable<int> Status { get; set; }
    
        public virtual Car Car { get; set; }
    }
}
