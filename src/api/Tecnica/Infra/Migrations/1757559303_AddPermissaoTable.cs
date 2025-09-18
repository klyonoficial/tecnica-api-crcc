using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559303)]
    public class AddPermissaoTable : Migration
    {
        public override void Up()
        {
            Create.Table("permissao")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("componente_id").AsInt64().NotNullable()
                .WithColumn("usuario_id").AsInt64().NotNullable();

            Create.ForeignKey("fk_permissao_componente")
                .FromTable("permissao").ForeignColumn("componente_id")
                .ToTable("componente").PrimaryColumn("id");

            Create.ForeignKey("fk_permissao_usuario")
                .FromTable("permissao").ForeignColumn("usuario_id")
                .ToTable("usuario").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}