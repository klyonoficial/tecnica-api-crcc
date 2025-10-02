using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559304)]
    public class AddLoginToUsuarioTable : Migration
    {
        public override void Up()
        {
            Alter.Table("usuario")
                .AddColumn("login").AsCustom("varchar(100)");
        }

        public override void Down()
        {
            Delete.Column("login").FromTable("usuario");
        }
    }
}
