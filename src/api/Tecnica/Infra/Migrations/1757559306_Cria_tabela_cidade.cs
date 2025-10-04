using FluentMigrator;
using System.IO;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559306)]
    public class Cria_tabela_cidade : Migration
    {
        public override void Up()
        {
            /*
            Create.Table("cidade").
                .WithColumn("id_cidade").AsInt64().PrimaryKey().Identity()
                .WithColumn("codigo_ibge").AsString(30).NotNullable()
                .WithColumn("nome").AsString(400).NotNullable()
                .WithColumn("uf").AsString(2).NotNullable()
                .WithColumn("status").AsString(4).NotNullable();

            //Id,Codigo,Nome,Uf
            //var lines = File.ReadLines("./municipios.csv");
            
            var csvPath = Path.Combine(AppContext.BaseDirectory, "municipios.csv");

            if(!File.Exists(csvPath))
            {
                throw new FileNotFoundException($"O arquivo '{csvPath}' não foi encontrado.");
            }

            var lines = File.ReadLines(csvPath);

            foreach (var line in lines)
            {
                var cols = line.Split(",");
                if (cols[0] == "Id")
                {
                    continue;
                }

                Insert.IntoTable("cidade").Row(new
                {
                    codigo_ibge = cols[1],
                    nome = cols[2],
                    uf = cols[3],
                    status = "ATIV"

                });
            }
            */
        }

        public override void Down()
        {

        }

    }
}
