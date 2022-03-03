using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.DataModel
{
    [Table("Property")]
    public partial class Property
    {
        public Property()
        {
            Activities = new HashSet<Activity>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        [StringLength(255)]
        public string Title { get; set; } = null!;
        [Column("address")]
        public string Address { get; set; } = null!;
        [Column("description")]
        public string Description { get; set; } = null!;
        [Column("create_at", TypeName = "timestamp without time zone")]
        public DateTime CreateAt { get; set; }
        [Column("update_at", TypeName = "timestamp without time zone")]
        public DateTime UpdateAt { get; set; }
        [Column("disable_at", TypeName = "timestamp without time zone")]
        public DateTime? DisableAt { get; set; }
        [Column("status")]
        [StringLength(35)]
        public string Status { get; set; } = null!;

        [InverseProperty(nameof(Activity.Property))]
        public virtual ICollection<Activity> Activities { get; set; }
    }
}
