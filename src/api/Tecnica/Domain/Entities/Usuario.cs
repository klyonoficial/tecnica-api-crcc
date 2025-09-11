using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        public long Id { get; private set; }

        [Column("nome")]
        [Required]
        public string Nome { get; private set; }

        [Column("email")]
        [Required]
        public string Email { get; private set; }

        [Column("senha")]
        [Required]
        public string Senha { get; private set; }

        [Column("ativo")]
        public bool Ativo { get; private set; }

        [Column("data_criacao")]
        public DateTime DataCriacao { get; private set; }

        // Construtor requerido pelo Entity Framework
        private Usuario() { }

        public Usuario(string nome, string email, string senha)
        {
            Assegure.NaoNulo(nome, "O nome deve ser informado.");
            Assegure.NaoNulo(email, "O e-mail deve ser informado.");
            Assegure.NaoNulo(senha, "A senha deve ser informada.");

            Nome = nome;
            Email = email;
            Senha = senha;
            Ativo = true;
            DataCriacao = DateTime.UtcNow;
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;
    }
}
