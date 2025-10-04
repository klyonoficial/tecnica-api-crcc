using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559295)]
    public class AddItemTable : Migration
    {
        public override void Up()
        {
            Create.Table("item")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("venda_id").AsInt64().NotNullable()
                .WithColumn("produto_id").AsInt64().NotNullable()
                .WithColumn("preco").AsCustom("decimal(18,2)").NotNullable()
                .WithColumn("quantidade").AsInt32().NotNullable()
                .WithColumn("numeros_serie").AsCustom("text").Nullable();
        }

        public override void Down()
        {
        }
    }
}
