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
                .WithColumn("fabricante_id").AsInt64().NotNullable();

            Create.ForeignKey("fk_produto_fabricante")
                .FromTable("produto").ForeignColumn("fabricante_id")
                .ToTable("fabricante").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}
