using FluentMigrator;
using System.IO;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559308)]
    public class CreateTableFabricante : Migration
    {
        public override void Up()
        {
            Create.Table("fabricante")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("nome").AsCustom("varchar(200)").NotNullable();

            Create.ForeignKey("fk_produto_fabricante")
                .FromTable("produto").ForeignColumn("fabricante_id")
                .ToTable("fabricante").PrimaryColumn("id");
        }

        public override void Down()
        {

        }

    }
}
