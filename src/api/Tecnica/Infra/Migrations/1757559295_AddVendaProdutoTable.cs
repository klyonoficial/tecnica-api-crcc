using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559295)]
    public class AddVendaProdutoTable : Migration
    {
        public override void Up()
        {
            Create.Table("venda_produto")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("venda_id").AsInt64().NotNullable()
                .WithColumn("produto_id").AsInt64().NotNullable()
                .WithColumn("preco").AsCustom("decimal(18,2)").NotNullable()
                .WithColumn("quantidade").AsInt32().NotNullable()
                .WithColumn("numeros_serie").AsCustom("text").Nullable();

            Create.ForeignKey("fk_venda_produto_venda")
                .FromTable("venda_produto").ForeignColumn("venda_id")
                .ToTable("venda").PrimaryColumn("id");

            Create.ForeignKey("fk_venda_produto_produto")
                .FromTable("venda_produto").ForeignColumn("produto_id")
                .ToTable("produto").PrimaryColumn("id");
        }

        public override void Down()
        {
            Delete.Table("venda_produto");
        }
    }
}
