using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace projectManagementToolWebAPI.Model
{
    [DataContract]
    public class MaterialList
    {
        [Key]
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
        public Nullable<int> ItemQuantity { get; set; }
        [DataMember, StringLength(25)]
        public string InvoiceNumber { get; set; }
        [DataMember]
        public Nullable<int> ProjectID { get; set; }
    }
}