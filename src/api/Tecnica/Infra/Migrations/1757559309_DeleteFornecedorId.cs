using FluentMigrator;
using System.IO;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559309)]
    public class DeleteFornecedorId : Migration
    {
        public override void Up()
        {
            Delete.Column("fornecedor_id").FromTable("produto");

            Delete.Table("fornecedor");
        }

        public override void Down()
        {

        }

    }
}
