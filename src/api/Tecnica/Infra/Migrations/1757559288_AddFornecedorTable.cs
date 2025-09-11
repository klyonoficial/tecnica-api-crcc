using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559288)]
    public class AddFornecedorTable : Migration
    {
        public override void Up()
        {
            Create.Table("fornecedor")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("nome").AsCustom("varchar(200)").NotNullable();
        }

        public override void Down()
        {
            Delete.Table("fornecedor");
        }
    }
}
