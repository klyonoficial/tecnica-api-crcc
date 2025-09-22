using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559300)]
    public class AddRotaCobrancaTable : Migration
    {
        public override void Up()
        {
            Create.Table("rota_cobranca")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("data").AsDateTime().NotNullable()
                .WithColumn("cobrador_id").AsInt64().NotNullable()
                .WithColumn("aprovador_id").AsInt64().Nullable()
                .WithColumn("observacao").AsCustom("varchar(800)").Nullable()
                .WithColumn("status").AsCustom("varchar(10)").NotNullable();

            Create.ForeignKey("fk_rota_cobranca_cobrador")
                .FromTable("rota_cobranca").ForeignColumn("cobrador_id")
                .ToTable("usuario").PrimaryColumn("id");

            Create.ForeignKey("fk_rota_cobranca_aprovador")
                .FromTable("rota_cobranca").ForeignColumn("aprovador_id")
                .ToTable("usuario").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}
