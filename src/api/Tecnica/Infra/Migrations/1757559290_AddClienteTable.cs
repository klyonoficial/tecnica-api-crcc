using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559290)]
    public class AddClienteTable : Migration
    {
        public override void Up()
        {
            Create.Table("cliente")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("vendedor_id").AsInt64().NotNullable()
                .WithColumn("cpf_cnpj").AsCustom("varchar(20)").NotNullable()
                .WithColumn("razao_social").AsCustom("varchar(200)").NotNullable()
                .WithColumn("nome_fantasia").AsCustom("varchar(200)").Nullable()
                .WithColumn("contato").AsCustom("varchar(200)").Nullable()
                .WithColumn("telefone_ddd").AsCustom("varchar(4)").Nullable()
                .WithColumn("telefone_numero").AsCustom("varchar(20)").Nullable()
                .WithColumn("dia_cobranca").AsInt32().NotNullable()
                .WithColumn("status").AsCustom("varchar(10)").NotNullable()
                .WithColumn("data_aprovou").AsDateTime().Nullable()
                .WithColumn("data_cadastro").AsDateTime().NotNullable()
                .WithColumn("dados_serasa").AsCustom("text").Nullable();

            Create.ForeignKey("fk_cliente_vendedor")
                .FromTable("cliente").ForeignColumn("vendedor_id")
                .ToTable("usuario").PrimaryColumn("id");
        }

        public override void Down()
        {
        }
    }
}
