using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559297)]
    public class AddParcelaTable : Migration
    {
        public override void Up()
        {
            Create.Table("parcela")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("venda_id").AsInt64().NotNullable()
                .WithColumn("forma_pagamento_id").AsInt64().Nullable()
                .WithColumn("data_vencimento").AsDateTime().NotNullable()
                .WithColumn("data_pagamento").AsDateTime().Nullable()
                .WithColumn("valor").AsCustom("decimal(18,2)").NotNullable()
                .WithColumn("valor_pago").AsCustom("decimal(18,2)").Nullable()
                .WithColumn("status").AsCustom("varchar(5)").NotNullable()
                .WithColumn("data_remanejamento").AsDateTime().Nullable()
                ;

            Create.ForeignKey("fk_parcela_venda")
                .FromTable("parcela").ForeignColumn("venda_id")
                .ToTable("venda").PrimaryColumn("id");

            Create.ForeignKey("fk_parcela_forma_pagamento")
                .FromTable("parcela").ForeignColumn("forma_pagamento_id")
                .ToTable("forma_pagamento").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}
