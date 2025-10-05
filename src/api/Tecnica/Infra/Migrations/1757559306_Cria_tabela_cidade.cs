using FluentMigrator;
using System.IO;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559306)]
    public class Cria_tabela_cidade : Migration
    {
        public override void Up()
        {
            Create.Table("cidade")
                .WithColumn("id_cidade").AsInt64().PrimaryKey().Identity()
                .WithColumn("codigo_ibge").AsString(30).NotNullable()
                .WithColumn("nome").AsString(400).NotNullable()
                .WithColumn("uf").AsString(2).NotNullable()
                .WithColumn("status").AsString(4).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("cidade");
        }

    }
}
