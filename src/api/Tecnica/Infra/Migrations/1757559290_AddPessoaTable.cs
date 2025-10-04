using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559290)]
    public class AddClienteTable : Migration
    {
        public override void Up()
        {
            Create.Table("pessoa")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("tipo").AsCustom("varchar(10)").NotNullable()
                .WithColumn("vendedor_id").AsInt64().Nullable()
                .WithColumn("cpf_cnpj").AsCustom("varchar(20)").Nullable()
                .WithColumn("razao_social").AsCustom("varchar(200)").NotNullable()
                .WithColumn("nome_fantasia").AsCustom("varchar(200)").Nullable()
                .WithColumn("contato").AsCustom("varchar(200)").Nullable()
                .WithColumn("telefone_ddd").AsCustom("varchar(4)").Nullable()
                .WithColumn("telefone_numero").AsCustom("varchar(20)").Nullable()
                .WithColumn("dia_cobranca").AsInt32().Nullable()
                .WithColumn("status").AsCustom("varchar(10)").Nullable()
                .WithColumn("data_aprovou").AsDateTime().Nullable()
                .WithColumn("data_cadastro").AsDateTime().NotNullable()
                .WithColumn("dados_serasa").AsCustom("text").Nullable();
        }

        public override void Down()
        {
        }
    }
}
