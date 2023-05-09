using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace projectManagementToolWebAPI.Model
{
    [DataContract]
    public class MaterialList
    {
        private double totalPriceField = default(double);
        [Key]
        [DataMember]
        public int ID { get; set; }
        [DataMember, StringLength(150)]
        public string ItemName { get; set; }
        [DataMember, StringLength(255)]
        public string ItemDescription { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public Nullable<DateTime> PurchaseDate { get; set; }
        [DataMember]
        public Nullable<DateTime> ModifiedDate { get; set; }
        [DataMember]
        public Nullable<int> ModifiedBy { get; set; }
        [DataMember]
        public Nullable<int> ItemQuantity { get; set; }
        [DataMember, StringLength(25)]
        public string InvoiceNumber { get; set; }
        [DataMember]
        public double TotalPrice
        {
            get
            {
                totalPriceField = ((double)ItemQuantity * Price);
                return totalPriceField;
            }
            set { totalPriceField = value; }
        }


        [DataMember, ForeignKey("Projects")]
        public Nullable<int> ProjectID { get; set; }
        [DataMember]
        public Projects Projects { get; set; }
    }
}