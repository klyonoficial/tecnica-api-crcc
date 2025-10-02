using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559305)]
    public class InsertUsuarioDev : Migration
    {
        public override void Up()
        {
            // Senha criptografada para "dev123" usando o método CriptografarSenha
            var senhaCriptografada = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918";
            
            Insert.IntoTable("usuario")
                .Row(new
                {
                    nome = "Desenvolvedor",
                    email = "dev@sistema.com",
                    login = "dev",
                    senha = senhaCriptografada,
                    ativo = true,
                    deve_alterar_senha = false
                });
        }

        public override void Down()
        {
            Delete.FromTable("usuario")
                .Row(new { email = "dev@sistema.com" });
        }
    }
}
