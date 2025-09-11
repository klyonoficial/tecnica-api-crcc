using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559287)]
    public class AddUsuarioTable : Migration
    {
        public override void Up()
        {
            Create.Table("usuario")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("nome").AsCustom("varchar(200)").NotNullable()
                .WithColumn("email").AsCustom("varchar(200)").NotNullable()
                .WithColumn("senha").AsCustom("varchar(200)").NotNullable()
                .WithColumn("ativo").AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn("perfil").AsCustom("varchar(5)").Nullable();
        }

        public override void Down()
        {
            Delete.Table("usuario");
        }
    }
}
