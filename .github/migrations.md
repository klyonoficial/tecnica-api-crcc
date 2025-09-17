Abaixo esta o formato de migrations para as tabelas usando FluentMigrator para o banco mysql.
As migrations devem ficar na pasta \src\api\Tecnica\Infra\Migrations
Aqui está um exemplo de migration:
´´´
using FluentMigrator;

namespace test
{
    [Migration(20180430121800)] // timestamp unico
    public class AddLogTable : Migration // Add+Nome da tabela
    {
        public override void Up()
        {
            Create.Table("log")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                 .WithColumn("other_table_id").AsInt64().NotNullable()
                .WithColumn("Text").AsCustom("varchar(10)")
                .WithColumn("valor").AsCustom("decimal(18,2)").NotNullable()
                ;

            Create.ForeignKey("fk_log_other_table") // A Foreing Key de id_other_table
                .FromTable("log").ForeignColumn("id_other_table")
                .ToTable("other_table").PrimaryColumn("id");
        }

        public override void Down()
        {
            Delete.Table("Log");
        }
    }
}

´´´

Regras:
- todos ids sao long