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
        }

        public override void Down()
        {
        }
    }
}