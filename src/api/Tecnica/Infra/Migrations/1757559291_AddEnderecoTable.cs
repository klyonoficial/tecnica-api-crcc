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
                .WithColumn("pessoa_id").AsInt64().NotNullable()
                .WithColumn("cep").AsCustom("varchar(10)").NotNullable()
                .WithColumn("logradouro").AsCustom("varchar(300)").Nullable()
                .WithColumn("codigo_ibge").AsCustom("varchar(20)").Nullable()
                .WithColumn("bairro").AsCustom("varchar(200)").Nullable()
                .WithColumn("UF").AsCustom("varchar(2)").Nullable()
                .WithColumn("numero").AsCustom("varchar(20)").Nullable()
                .WithColumn("complemento").AsCustom("varchar(200)").Nullable()
                .WithColumn("tipo").AsCustom("varchar(5)").Nullable();
        }

        public override void Down()
        {
        }
    }
}
