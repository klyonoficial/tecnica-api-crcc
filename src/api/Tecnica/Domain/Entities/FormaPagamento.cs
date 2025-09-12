using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("forma_pagamento")]
    public class FormaPagamento
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        public FormaPagamento(string nome)
        {
            Assegure.NaoNulo(nome, "O nome da forma de pagamento deve ser informado.");
            Nome = nome;
        }

        // Construtor vazio para o Entity Framework
        public FormaPagamento()
        {

        }
    }
}
