using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.DataModel
{
    [Table("Survey")]
    public partial class Survey
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("activity_id")]
        public int ActivityId { get; set; }
        [Column("answers", TypeName = "json")]
        public string Answers { get; set; } = null!;
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
    }
}
