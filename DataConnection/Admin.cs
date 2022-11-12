namespace PAYPAL.DataConnection
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Admin")]
    public partial class Admin
    {
        [Key]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Value { get; set; }
    }
}
