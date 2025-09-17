using FluentMigrator;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559296)]
    public class AddFormaPagamentoTable : Migration
    {
        public override void Up()
        {
            Create.Table("forma_pagamento")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("nome").AsCustom("varchar(200)").NotNullable();
        }

        public override void Down()
        {
        }
    }
}
