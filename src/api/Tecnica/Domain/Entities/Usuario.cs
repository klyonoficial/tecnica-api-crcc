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

        [Column("login")]
        [Required]
        public string Login { get; private set; }

        [Column("senha")]
        [Required]
        public string Senha { get; private set; }

        [Column("ativo")]
        public bool Ativo { get; private set; }

        [Column("deve_alterar_senha")]
        public bool DeveAlterarSenha { get; private set; }

        // Construtor requerido pelo Entity Framework
        private Usuario() { }

        public Usuario(string nome, string email, string login, string senha, bool deveAlterarSenha)
        {
            Assegure.NaoNulo(nome, "O nome deve ser informado.");
            Assegure.NaoNulo(email, "O e-mail deve ser informado.");
            Assegure.NaoNulo(login, "O login deve ser informado.");
            Assegure.NaoNulo(senha, "A senha deve ser informada.");
            Assegure.NaoNulo(deveAlterarSenha, "A informação sobre a necessidade de alteração de senha deve ser fornecida.");

            Nome = nome;
            Email = email;
            Login = login;
            Senha = Util.CriptografarSenha(senha);
            Ativo = true;
            DeveAlterarSenha = deveAlterarSenha;
        }

        public void Atualizar(string nome, string email, string login, bool ativo, bool deveAlterarSenha)
        {
            Assegure.NaoNulo(nome, "O nome deve ser informado.");
            Assegure.NaoNulo(email, "O e-mail deve ser informado.");
            Assegure.NaoNulo(login, "O login deve ser informado.");
            Assegure.NaoNulo(deveAlterarSenha, "A informação sobre a necessidade de alteração de senha deve ser fornecida.");

            Nome = nome;
            Email = email;
            Login = login;
            Ativo = ativo;
            DeveAlterarSenha = deveAlterarSenha;
        }

        public void AlterarSenha(string novaSenha)
        {
            Assegure.NaoNulo(novaSenha, "A nova senha deve ser informada.");
            
            Senha = Tecnica.Infra.Util.CriptografarSenha(novaSenha);
            DeveAlterarSenha = false;
        }
        
        public void Desativar() => Ativo = false;
    }
}
