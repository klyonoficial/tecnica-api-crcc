using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559292)]
    public class AddArquivoTable : Migration
    {
        public override void Up()
        {
            Create.Table("arquivo")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("cliente_id").AsInt64().NotNullable()
                .WithColumn("tipo").AsCustom("varchar(50)").NotNullable()
                .WithColumn("nome").AsCustom("varchar(200)").NotNullable();

            Create.ForeignKey("fk_arquivo_cliente")
                .FromTable("arquivo").ForeignColumn("cliente_id")
                .ToTable("cliente").PrimaryColumn("id");
        }

        public override void Down()
        {
            Delete.Table("arquivo");
        }
    }
}
