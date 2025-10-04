using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559301)]
    public class AddCobrancaTable : Migration
    {
        public override void Up()
        {
            Create.Table("cobranca")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("rota_cobranca_id").AsInt64().NotNullable()
                .WithColumn("parcela_id").AsInt64().NotNullable();
        }

        public override void Down()
        {
        }
    }
}
