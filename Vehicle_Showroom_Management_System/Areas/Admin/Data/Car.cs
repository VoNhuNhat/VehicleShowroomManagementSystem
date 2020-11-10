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
    
    public partial class Car
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Car()
        {
            this.Images = new HashSet<Image>();
        }
    
        public string ModelNumberCar { get; set; }
        public Nullable<int> Id { get; set; }
        public string CarName { get; set; }
        public Nullable<double> PriceInput { get; set; }
        public Nullable<double> PriceOutput { get; set; }
        public Nullable<int> SeatQuantity { get; set; }
        public string Color { get; set; }
        public string Gearbox { get; set; }
        public string Engine { get; set; }
        public Nullable<double> FuelConsumption { get; set; }
        public Nullable<double> KilometerGone { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Checking { get; set; }
        public Nullable<System.DateTime> PurchaseOrderDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> Sold { get; set; }
    
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Image> Images { get; set; }
    }
}
