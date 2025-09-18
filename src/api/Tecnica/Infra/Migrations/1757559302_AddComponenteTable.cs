using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559302)]
    public class AddRecursoTable : Migration
    {
        public override void Up()
        {
            Create.Table("componente")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("codigo").AsCustom("varchar(200)").NotNullable()
                .WithColumn("nome").AsCustom("varchar(100)").NotNullable()
                .WithColumn("descricao").AsCustom("varchar(255)").Nullable()
                .WithColumn("tipo").AsCustom("varchar(1)").NotNullable()
                .WithColumn("ordem").AsCustom("varchar(20)").Nullable()
                .WithColumn("ativo").AsBoolean().NotNullable().WithDefaultValue(true);
        }

        public override void Down()
        {
            
        }
    }
}