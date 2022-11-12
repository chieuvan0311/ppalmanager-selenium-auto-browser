namespace PAYPAL.DataConnection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Category")]
    public partial class Category
    {
        [Key]
        [StringLength(250)]
        public string Name { get; set; }

        [Column(TypeName = "xml")]
        public string AccIDList { get; set; }
    }
}
