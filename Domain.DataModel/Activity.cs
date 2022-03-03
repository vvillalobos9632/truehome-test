using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.DataModel
{
    [Table("Activity")]
    public partial class Activity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("property_id")]
        public int PropertyId { get; set; }
        [Column("schedule", TypeName = "timestamp without time zone")]
        public DateTime Schedule { get; set; }
        [Column("title")]
        [StringLength(255)]
        public string Title { get; set; } = null!;
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_ad", TypeName = "timestamp without time zone")]
        public DateTime UpdatedAd { get; set; }
        [Column("status")]
        [StringLength(35)]
        public string? Status { get; set; }

        [ForeignKey(nameof(PropertyId))]
        [InverseProperty("Activities")]
        public virtual Property Property { get; set; } = null!;
    }
}
