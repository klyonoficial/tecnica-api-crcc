using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("venda")]
    public class Venda
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("data_cadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("cliente_id")]
        public long ClienteId { get; set; }

        [Column("vendedor_id")]
        public long VendedorId { get; set; }

        [Column("data_entrega")]
        public DateTime? DataEntrega { get; set; }

        [Column("valor_total")]
        public decimal ValorTotal { get; set; }

        [Column("valor_entrada")]
        public decimal? ValorEntrada { get; set; }

        [Column("dia_acerto")]
        public int DiaAcerto { get; set; }

        [Column("quantidade_parcelas")]
        public int QuantidadeParcelas { get; set; }

        [Column("observacao")]
        public string? Observacao { get; set; }

        [Column("endereco_cep")]
        public string? EnderecoCep { get; set; }

        [Column("endereco_codigo_ibge")]
        public string? EnderecoCodigoIbge { get; set; }

        [Column("endereco_bairro")]
        public string? EnderecoBairro { get; set; }

        [Column("endereco_UF")]
        public string? EnderecoUF { get; set; }

        [Column("endereco_numero")]
        public string? EnderecoNumero { get; set; }

        [Column("endereco_complemento")]
        public string? EnderecoComplemento { get; set; }

        [Column("endereco_tipo")]
        public string? EnderecoTipo { get; set; }

        [InverseProperty("Venda")]
        public virtual ICollection<Item>? Items { get; set; }

        [InverseProperty("Venda")]
        public virtual ICollection<Entrega>? Entregas { get; set; }

        [InverseProperty("Venda")]
        public virtual ICollection<Parcela>? Parcelas { get; set; }

        public Venda(
            DateTime dataCadastro,
            string status,
            long clienteId,
            long vendedorId,
            DateTime? dataEntrega,
            decimal? valorEntrada,
            int diaAcerto,
            int quantidadeParcelas,
            string? observacao,
            string? enderecoCep,
            string? enderecoCodigoIbge,
            string? enderecoBairro,
            string? enderecoUF,
            string? enderecoNumero,
            string? enderecoComplemento,
            string? enderecoTipo,
            ICollection<Item>? vendaProdutos = null)
        {
            Assegure.NaoNulo(dataCadastro, "A data de cadastro deve ser informada.");
            Assegure.NaoNulo(status, "O status deve ser informado.");
            Assegure.Que(clienteId > 0, "O cliente deve ser informado.");
            Assegure.Que(vendedorId > 0, "O vendedor deve ser informado.");
            Assegure.Que(diaAcerto > 0, "O dia de acerto deve ser informado.");
            Assegure.Que(quantidadeParcelas > 0, "A quantidade de parcelas deve ser informada.");
            Assegure.NaoNulo(enderecoCep, "O CEP do endereço deve ser informado.");
            Assegure.NaoNulo(enderecoBairro, "O bairro do endereço deve ser informado.");
            Assegure.NaoNulo(enderecoUF, "A UF do endereço deve ser informada.");
            Assegure.NaoNulo(enderecoTipo, "O tipo do endereço deve ser informado.");
            Assegure.NaoNulo(enderecoCodigoIbge, "O código IBGE do endereço deve ser informado.");

            DataCadastro = dataCadastro;
            Status = status;
            ClienteId = clienteId;
            VendedorId = vendedorId;
            DataEntrega = dataEntrega;
            ValorTotal = vendaProdutos.Sum(vp => vp.Quantidade * vp.Preco);
            ValorEntrada = valorEntrada;
            DiaAcerto = diaAcerto;
            QuantidadeParcelas = quantidadeParcelas;
            Observacao = observacao;
            EnderecoCep = enderecoCep;
            EnderecoCodigoIbge = enderecoCodigoIbge;
            EnderecoBairro = enderecoBairro;
            EnderecoUF = enderecoUF;
            EnderecoNumero = enderecoNumero;
            EnderecoComplemento = enderecoComplemento;
            EnderecoTipo = enderecoTipo;
            Items = vendaProdutos;
            Parcelas = vendaParcelas;
        }

        // Construtor vazio para o Entity Framework
        public Venda()
        {

        }
    }
}
