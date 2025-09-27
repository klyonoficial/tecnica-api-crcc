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

        [Column("pessoa_id")]
        public long PessoaId { get; set; }

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


        [Column("endereco_logradouro")]
        public string? EnderecoLogradouro { get; set; }

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

        [ForeignKey("PessoaId")]
        [InverseProperty("Vendas")]
        public virtual Cliente? Cliente { get; set; }

        public Venda(
            DateTime dataCadastro,
            long pessoaId,
            long vendedorId,
            DateTime? dataEntrega,
            decimal? valorEntrada,
            int diaAcerto,
            int quantidadeParcelas,
            string? observacao,
            string? enderecoCep,
            string? enderecoLogradouro,
            string? enderecoCodigoIbge,
            string? enderecoBairro,
            string? enderecoUF,
            string? enderecoNumero,
            string? enderecoComplemento,
            string? enderecoTipo,
            ICollection<Item>? items,
            ICollection<Parcela>? parcelas)
        {
            Assegure.NaoNulo(dataCadastro, "A data de cadastro deve ser informada.");
            Assegure.Que(pessoaId > 0, "O cliente deve ser informado.");
            Assegure.Que(vendedorId > 0, "O vendedor deve ser informado.");
            Assegure.Que(pessoaId > 0, "O cliente deve ser informado.");
            Assegure.Que(diaAcerto > 0, "O dia de acerto deve ser informado.");
            Assegure.Que(quantidadeParcelas > 0, "A quantidade de parcelas deve ser informada.");
            Assegure.NaoNulo(enderecoCep, "O CEP do endereço deve ser informado.");
            Assegure.NaoNulo(enderecoLogradouro, "O logradouro do endereço deve ser informado.");
            Assegure.NaoNulo(enderecoBairro, "O bairro do endereço deve ser informado.");
            Assegure.NaoNulo(enderecoUF, "A UF do endereço deve ser informada.");
            Assegure.NaoNulo(enderecoTipo, "O tipo do endereço deve ser informado.");
            Assegure.NaoNulo(enderecoCodigoIbge, "O código IBGE do endereço deve ser informado.");
            Assegure.NaoNulo(items, "Os itens da venda devem ser informados.");
            Assegure.NaoNulo(parcelas, "As parcelas da venda devem ser informadas.");

            DataCadastro = dataCadastro;
            Status = nameof(DomainEnums.VendaStatus.CAD);
            PessoaId = pessoaId;
            VendedorId = vendedorId;
            DataEntrega = dataEntrega;
            ValorTotal = items.Sum(vp => vp.Quantidade * vp.Preco);
            ValorEntrada = valorEntrada;
            DiaAcerto = diaAcerto;
            QuantidadeParcelas = quantidadeParcelas;
            Observacao = observacao;
            EnderecoCep = enderecoCep;
            EnderecoLogradouro = enderecoLogradouro;
            EnderecoCodigoIbge = enderecoCodigoIbge;
            EnderecoBairro = enderecoBairro;
            EnderecoUF = enderecoUF;
            EnderecoNumero = enderecoNumero;
            EnderecoComplemento = enderecoComplemento;
            EnderecoTipo = enderecoTipo;
            Items = items;
            Parcelas = parcelas;
        }

        // Construtor vazio para o Entity Framework
        public Venda()
        {
            Status = nameof(DomainEnums.VendaStatus.CAD);
        }

        #region Métodos de Negócio

        /// <summary>
        /// Aprova a venda, alterando o status para APROV
        /// </summary>
        public void Aprovar()
        {
            Status = nameof(DomainEnums.VendaStatus.APROV);
        }

        /// <summary>
        /// Reprova a venda, alterando o status para REPR
        /// </summary>
        public void Reprovar()
        {
            Status = nameof(DomainEnums.VendaStatus.REPR);
        }

        /// <summary>
        /// Envia a venda para expedição, alterando o status para EXP
        /// </summary>
        public void Expedicao()
        {
            Status = nameof(DomainEnums.VendaStatus.EXP);
        }

        /// <summary>
        /// Atualiza a data de entrega da venda
        /// </summary>
        /// <param name="novaDataEntrega">Nova data de entrega</param>
        /// <param name="observacao">Observação opcional sobre o reagendamento</param>
        public void Reagendar(DateTime? novaDataEntrega, string? observacao = null)
        {
            DataEntrega = novaDataEntrega;
            if (!string.IsNullOrEmpty(observacao))
            {
                Observacao = observacao;
            }
        }

        /// <summary>
        /// Atualiza os dados da venda com base em um DTO
        /// </summary>
        public void Atualizar(DateTime? dataEntrega, decimal? valorEntrada, int diaAcerto, int quantidadeParcelas, string? observacao, string? enderecoCep, string? enderecoCodigoIbge, string? enderecoBairro, string? enderecoUF, string? enderecoNumero, string? enderecoComplemento, string? enderecoTipo)
        {
            DataEntrega = dataEntrega;
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
        }

        internal void Entregue()
        {
            Status = nameof(DomainEnums.VendaStatus.ENTR);
        }

        internal void AdicionarParcela(Parcela parcelaRemanejada)
        {
            Parcelas.Add(parcelaRemanejada);
        }

        #endregion
    }
}
