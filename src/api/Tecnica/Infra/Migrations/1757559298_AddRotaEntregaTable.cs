using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559298)]
    public class AddRotaEntregaTable : Migration
    {
        public override void Up()
        {
            Create.Table("rota_entrega")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("data").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("rota_entrega");
        }
    }
}
