using FluentMigrator;
using System.IO;

namespace Tecnica.Infra.Migrations
{
    [Migration(1757559307)]
    public class AddForeignKeys : Migration
    {
        public override void Up()
        {
            Create.ForeignKey("fk_produto_fabricante")
                .FromTable("produto").ForeignColumn("fabricante_id")
                .ToTable("fabricante").PrimaryColumn("id");
            
            Create.ForeignKey("fk_pessoa_vendedor")
                .FromTable("pessoa").ForeignColumn("vendedor_id")
                .ToTable("usuario").PrimaryColumn("id");

            Create.ForeignKey("fk_endereco_pessoa")
                .FromTable("endereco").ForeignColumn("pessoa_id")
                .ToTable("pessoa").PrimaryColumn("id");

            Create.ForeignKey("fk_arquivo_pessoa")
                .FromTable("arquivo").ForeignColumn("pessoa_id")
                .ToTable("pessoa").PrimaryColumn("id");

            Create.ForeignKey("fk_venda_pessoa")
                .FromTable("venda").ForeignColumn("pessoa_id")
                .ToTable("pessoa").PrimaryColumn("id");

            Create.ForeignKey("fk_venda_vendedor")
                .FromTable("venda").ForeignColumn("vendedor_id")
                .ToTable("usuario").PrimaryColumn("id");

            Create.ForeignKey("fk_item_venda")
                .FromTable("item").ForeignColumn("venda_id")
                .ToTable("venda").PrimaryColumn("id");

            Create.ForeignKey("fk_item_produto")
                .FromTable("item").ForeignColumn("produto_id")
                .ToTable("produto").PrimaryColumn("id");

            Create.ForeignKey("fk_parcela_venda")
                .FromTable("parcela").ForeignColumn("venda_id")
                .ToTable("venda").PrimaryColumn("id");

            Create.ForeignKey("fk_parcela_forma_pagamento")
                .FromTable("parcela").ForeignColumn("forma_pagamento_id")
                .ToTable("forma_pagamento").PrimaryColumn("id");

            Create.ForeignKey("fk_rota_entrega_entregador")
                .FromTable("rota_entrega").ForeignColumn("id_entregador")
                .ToTable("usuario").PrimaryColumn("id");

            Create.ForeignKey("fk_entrega_rota_entrega")
                .FromTable("entrega").ForeignColumn("rota_entrega_id")
                .ToTable("rota_entrega").PrimaryColumn("id");

            Create.ForeignKey("fk_entrega_venda")
                .FromTable("entrega").ForeignColumn("venda_id")
                .ToTable("venda").PrimaryColumn("id");

            Create.ForeignKey("fk_rota_cobranca_cobrador")
                .FromTable("rota_cobranca").ForeignColumn("cobrador_id")
                .ToTable("usuario").PrimaryColumn("id");

            Create.ForeignKey("fk_rota_cobranca_aprovador")
                .FromTable("rota_cobranca").ForeignColumn("aprovador_id")
                .ToTable("usuario").PrimaryColumn("id");

            Create.ForeignKey("fk_cobranca_rota_cobranca")
                .FromTable("cobranca").ForeignColumn("rota_cobranca_id")
                .ToTable("rota_cobranca").PrimaryColumn("id");

            Create.ForeignKey("fk_cobranca_parcela")
                .FromTable("cobranca").ForeignColumn("parcela_id")
                .ToTable("parcela").PrimaryColumn("id");

            Create.ForeignKey("fk_permissao_componente")
                .FromTable("permissao").ForeignColumn("componente_id")
                .ToTable("componente").PrimaryColumn("id");

            Create.ForeignKey("fk_permissao_usuario")
                .FromTable("permissao").ForeignColumn("usuario_id")
                .ToTable("usuario").PrimaryColumn("id");
        }

        public override void Down()
        {

        }

    }
}
