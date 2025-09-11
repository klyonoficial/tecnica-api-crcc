using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559289)]
    public class AddProdutoTable : Migration
    {
        public override void Up()
        {
            Create.Table("produto")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("nome").AsCustom("varchar(200)").NotNullable()
                .WithColumn("fornecedor_id").AsInt64().NotNullable();

            Create.ForeignKey("fk_produto_fornecedor")
                .FromTable("produto").ForeignColumn("fornecedor_id")
                .ToTable("fornecedor").PrimaryColumn("id");
        }

        public override void Down()
        {
            Delete.Table("produto");
        }
    }
}
