using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559291)]
    public class AddEnderecoTable : Migration
    {
        public override void Up()
        {
            Create.Table("endereco")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("cliente_id").AsInt64().NotNullable()
                .WithColumn("cep").AsCustom("varchar(10)").NotNullable()
                .WithColumn("codigo_ibge").AsCustom("varchar(20)").Nullable()
                .WithColumn("bairro").AsCustom("varchar(200)").Nullable()
                .WithColumn("UF").AsCustom("varchar(2)").Nullable()
                .WithColumn("numero").AsCustom("varchar(20)").Nullable()
                .WithColumn("complemento").AsCustom("varchar(200)").Nullable()
                .WithColumn("tipo").AsCustom("varchar(5)").Nullable();

            Create.ForeignKey("fk_endereco_cliente")
                .FromTable("endereco").ForeignColumn("cliente_id")
                .ToTable("cliente").PrimaryColumn("id");
        }

        public override void Down()
        {
            Delete.Table("endereco");
        }
    }
}
