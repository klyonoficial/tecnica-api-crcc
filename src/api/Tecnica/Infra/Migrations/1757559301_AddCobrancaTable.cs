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

            Create.ForeignKey("fk_cobranca_rota_cobranca")
                .FromTable("cobranca").ForeignColumn("rota_cobranca_id")
                .ToTable("rota_cobranca").PrimaryColumn("id");

            Create.ForeignKey("fk_cobranca_parcela")
                .FromTable("cobranca").ForeignColumn("parcela_id")
                .ToTable("parcela").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}
