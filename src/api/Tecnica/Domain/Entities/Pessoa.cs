using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tecnica.Domain.Entities
{
    [Table("pessoa")]
    public class Pessoa
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
    }
}