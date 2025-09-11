using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559297)]
    public class AddVendaParcelaTable : Migration
    {
        public override void Up()
        {
            Create.Table("venda_parcela")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("venda_id").AsInt64().NotNullable()
                .WithColumn("data_vencimento").AsDateTime().NotNullable()
                .WithColumn("data_pagamento").AsDateTime().Nullable()
                .WithColumn("valor").AsCustom("decimal(18,2)").NotNullable()
                .WithColumn("valor_pago").AsCustom("decimal(18,2)").Nullable()
                .WithColumn("status").AsCustom("varchar(5)").NotNullable()
                .WithColumn("data_remanejamento").AsDateTime().Nullable()
                .WithColumn("forma_pagamento_id").AsInt64().Nullable();

            Create.ForeignKey("fk_venda_parcela_venda")
                .FromTable("venda_parcela").ForeignColumn("venda_id")
                .ToTable("venda").PrimaryColumn("id");

            Create.ForeignKey("fk_venda_parcela_forma_pagamento")
                .FromTable("venda_parcela").ForeignColumn("forma_pagamento_id")
                .ToTable("forma_pagamento").PrimaryColumn("id");
        }

        public override void Down()
        {
            Delete.Table("venda_parcela");
        }
    }
}
