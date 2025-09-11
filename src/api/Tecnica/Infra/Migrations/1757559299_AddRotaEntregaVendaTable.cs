using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559299)]
    public class AddRotaEntregaVendaTable : Migration
    {
        public override void Up()
        {
            Create.Table("rota_entrega_venda")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("rota_entrega_id").AsInt64().NotNullable()
                .WithColumn("venda_id").AsInt64().NotNullable()
                .WithColumn("status").AsCustom("varchar(5)").NotNullable();

            Create.ForeignKey("fk_rota_entrega_venda_rota")
                .FromTable("rota_entrega_venda").ForeignColumn("rota_entrega_id")
                .ToTable("rota_entrega").PrimaryColumn("id");

            Create.ForeignKey("fk_rota_entrega_venda_venda")
                .FromTable("rota_entrega_venda").ForeignColumn("venda_id")
                .ToTable("venda").PrimaryColumn("id");
        }

        public override void Down()
        {
            Delete.Table("rota_entrega_venda");
        }
    }
}
