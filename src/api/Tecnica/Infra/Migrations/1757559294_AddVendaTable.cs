using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559294)]
    public class AddVendaTable : Migration
    {
        public override void Up()
        {
            Create.Table("venda")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("data_cadastro").AsDateTime().NotNullable()
                .WithColumn("status").AsCustom("varchar(10)").NotNullable()
                .WithColumn("cliente_id").AsInt64().NotNullable()
                .WithColumn("vendedor_id").AsInt64().NotNullable()
                .WithColumn("data_entrega").AsDateTime().Nullable()
                .WithColumn("valor_total").AsCustom("decimal(18,2)").NotNullable()
                .WithColumn("valor_entrada").AsCustom("decimal(18,2)").Nullable()
                .WithColumn("dia_acerto").AsInt32().NotNullable()
                .WithColumn("quantidade_parcelas").AsInt32().NotNullable()
                .WithColumn("observacao").AsCustom("text").Nullable()
                .WithColumn("endereco_cep").AsCustom("varchar(10)").Nullable()
                .WithColumn("endereco_codigo_ibge").AsCustom("varchar(20)").Nullable()
                .WithColumn("endereco_bairro").AsCustom("varchar(200)").Nullable()
                .WithColumn("endereco_UF").AsCustom("varchar(2)").Nullable()
                .WithColumn("endereco_numero").AsCustom("varchar(20)").Nullable()
                .WithColumn("endereco_complemento").AsCustom("varchar(200)").Nullable()
                .WithColumn("endereco_tipo").AsCustom("varchar(5)").Nullable();

            Create.ForeignKey("fk_venda_cliente")
                .FromTable("venda").ForeignColumn("cliente_id")
                .ToTable("cliente").PrimaryColumn("id");

            Create.ForeignKey("fk_venda_vendedor")
                .FromTable("venda").ForeignColumn("vendedor_id")
                .ToTable("usuario").PrimaryColumn("id");
        }

        public override void Down()
        {
            Delete.Table("venda");
        }
    }
}
