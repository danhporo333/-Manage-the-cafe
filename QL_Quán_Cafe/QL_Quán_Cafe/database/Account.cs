namespace QL_Quán_Cafe.database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        [Key]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Displayname { get; set; }

        [StringLength(1000)]
        public string PassWord { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; }
    }
}
