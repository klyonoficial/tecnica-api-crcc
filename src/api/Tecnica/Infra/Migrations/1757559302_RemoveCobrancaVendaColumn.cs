using FluentMigrator;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559302)]
    public class RemoveEntregaVendaTable_venda : Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("fk_rota_cobranca_venda_venda");

            Delete.Column("venda_id").FromTable("rota_cobranca_venda");

            Rename.Table("rota_cobranca_venda").To("rota_cobranca_parcela");

            Rename.Column("parcela_id").OnTable("rota_cobranca_parcela").To("venda_parcela_id");
        }

        public override void Down()
        {

        }
    }
}
