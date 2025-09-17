using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559299)]
    public class AddEntregaTable : Migration
    {
        public override void Up()
        {
            Create.Table("entrega")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("rota_entrega_id").AsInt64().NotNullable()
                .WithColumn("venda_id").AsInt64().NotNullable()
                .WithColumn("status").AsCustom("varchar(5)").NotNullable();

            Create.ForeignKey("fk_entrega_rota_entrega")
                .FromTable("entrega").ForeignColumn("rota_entrega_id")
                .ToTable("rota_entrega").PrimaryColumn("id");

            Create.ForeignKey("fk_entrega_venda")
                .FromTable("entrega").ForeignColumn("venda_id")
                .ToTable("venda").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}
