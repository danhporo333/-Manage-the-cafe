namespace QL_Quán_Cafe.database
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bill")]
    public partial class Bill
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bill()
        {
            BillInfo = new HashSet<BillInfo>();
        }

        public int id { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateCheckIn { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateCheckOut { get; set; }

        public int? idTable { get; set; }

        public int status { get; set; }

        public int? discount { get; set; }

        public int? totalPrice { get; set; }

        public virtual TableFood TableFood { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillInfo> BillInfo { get; set; }
    }
}
