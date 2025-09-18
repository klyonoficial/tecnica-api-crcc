using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559298)]
    public class AddRotaEntregaTable : Migration
    {
        public override void Up()
        {
            Create.Table("rota_entrega")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("data").AsDateTime().NotNullable()
                .WithColumn("id_entregador").AsInt64().NotNullable();

            Create.ForeignKey("fk_rota_entrega_entregador")
                .FromTable("rota_entrega").ForeignColumn("id_entregador")
                .ToTable("usuario").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}
