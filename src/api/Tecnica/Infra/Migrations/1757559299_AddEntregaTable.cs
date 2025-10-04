using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559299)]
    public class AddEntregaTable : Migration
    {
        public override void Up()
        {
            Create.Table("entrega")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("rota_entrega_id").AsInt64().NotNullable()
                .WithColumn("venda_id").AsInt64().NotNullable()
                .WithColumn("status").AsCustom("varchar(5)").NotNullable();
        }

        public override void Down()
        {
        }
    }
}
