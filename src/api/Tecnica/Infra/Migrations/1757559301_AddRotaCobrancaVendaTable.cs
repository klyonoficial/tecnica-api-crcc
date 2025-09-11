using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559301)]
    public class AddRotaCobrancaVendaTable : Migration
    {
        public override void Up()
        {
            Create.Table("rota_cobranca_venda")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("rota_cobranca_id").AsInt64().NotNullable()
                .WithColumn("venda_id").AsInt64().NotNullable()
                .WithColumn("parcela_id").AsInt64().NotNullable();

            Create.ForeignKey("fk_rota_cobranca_venda_rota_cobranca")
                .FromTable("rota_cobranca_venda").ForeignColumn("rota_cobranca_id")
                .ToTable("rota_cobranca").PrimaryColumn("id");

            Create.ForeignKey("fk_rota_cobranca_venda_venda")
                .FromTable("rota_cobranca_venda").ForeignColumn("venda_id")
                .ToTable("venda").PrimaryColumn("id");

            Create.ForeignKey("fk_rota_cobranca_venda_parcela")
                .FromTable("rota_cobranca_venda").ForeignColumn("parcela_id")
                .ToTable("venda_parcela").PrimaryColumn("id");
        }

        public override void Down()
        {
            Delete.Table("rota_cobranca_venda");
        }
    }
}
