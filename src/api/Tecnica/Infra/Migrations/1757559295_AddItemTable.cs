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

            Create.ForeignKey("fk_item_venda")
                .FromTable("item").ForeignColumn("venda_id")
                .ToTable("venda").PrimaryColumn("id");

            Create.ForeignKey("fk_item_produto")
                .FromTable("item").ForeignColumn("produto_id")
                .ToTable("produto").PrimaryColumn("id");
        }

        public override void Down()
        {
            Delete.Table("venda_produto");
        }
    }
}
