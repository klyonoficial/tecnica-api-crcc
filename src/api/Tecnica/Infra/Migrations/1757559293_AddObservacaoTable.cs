using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559293)]
    public class AddObservacaoTable : Migration
    {
        public override void Up()
        {
            Create.Table("observacao")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("entidade").AsCustom("varchar(200)").NotNullable()
                .WithColumn("entidade_id").AsInt64().NotNullable()
                .WithColumn("texto").AsCustom("text").NotNullable();
        }

        public override void Down()
        {
            Delete.Table("observacao");
        }
    }
}
