using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.DataModel
{
    [Table("usuario")]
    public partial class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }
        [Column("nombre")]
        [StringLength(300)]
        public string Nombre { get; set; } = null!;
        [Column("nombre_usuario")]
        [StringLength(50)]
        public string NombreUsuario { get; set; } = null!;
        [Column("contraseña")]
        [StringLength(50)]
        public string Contraseña { get; set; } = null!;
        [Column("activo")]
        public bool Activo { get; set; }
    }
}
