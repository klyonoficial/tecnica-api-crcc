using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tecnica.Infra;

namespace Tecnica.Domain.Entities
{
    [Table("pessoa")]
    public class Cliente : Pessoa
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("vendedor_id")]
        public long VendedorId { get; set; }

        [Column("cpf_cnpj")]
        public string CpfCnpj { get; set; }

        [Column("razao_social")]
        public string RazaoSocial { get; set; }

        [Column("nome_fantasia")]
        public string? NomeFantasia { get; set; }

        [Column("contato")]
        public string? Contato { get; set; }

        [Column("telefone_ddd")]
        public string? TelefoneDdd { get; set; }

        [Column("telefone_numero")]
        public string? TelefoneNumero { get; set; }

        [Column("dia_cobranca")]
        public int DiaCobranca { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("data_aprovou")]
        public DateTime? DataAprovou { get; set; }

        [Column("data_cadastro")]
        public DateTime DataCadastro { get; set; }

        [Column("dados_serasa")]
        public string? DadosSerasa { get; set; }

        [InverseProperty("Cliente")]
        public virtual ICollection<Endereco> Enderecos { get; set; }

        [InverseProperty("Cliente")]
        public virtual ICollection<Arquivo> Arquivos { get; set; }
        
        [InverseProperty("Cliente")]
        public virtual ICollection<Venda> Vendas { get; set; }

        public Cliente(
            long vendedorId,
            string cpfCnpj,
            string razaoSocial,
            string? nomeFantasia,
            string? contato,
            string? telefoneDdd,
            string? telefoneNumero,
            int diaCobranca,
            string? dadosSerasa,
            List<Endereco>? enderecos = null,
            List<Arquivo>? arquivos = null)
        {
            Assegure.Que(vendedorId > 0, "O vendedor deve ser informado.");
            Assegure.NaoNulo(cpfCnpj, "O CPF/CNPJ deve ser informado.");
            Assegure.NaoNulo(razaoSocial, "A razão social deve ser informada.");
            Assegure.Que(diaCobranca > 0, "O dia de cobrança deve ser informado.");

            VendedorId = vendedorId;
            CpfCnpj = cpfCnpj;
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            Contato = contato;
            TelefoneDdd = telefoneDdd;
            TelefoneNumero = telefoneNumero;
            DiaCobranca = diaCobranca;
            Status = DomainEnums.ClienteStatus.CAD.ToString();
            DadosSerasa = dadosSerasa;
            Enderecos = enderecos?.ToList() ?? new List<Endereco>();
            Arquivos = arquivos?.ToList() ?? new List<Arquivo>();
        }

        // Construtor vazio para o Entity Framework
        public Cliente()
        {
            CpfCnpj = string.Empty;
            RazaoSocial = string.Empty;
            Status = DomainEnums.ClienteStatus.CAD.ToString();
            Enderecos = new List<Endereco>();
            Arquivos = new List<Arquivo>();
        }

        // Status helpers
        public void SetStatus(DomainEnums.ClienteStatus status)
        {
            Status = status.ToString();
            if (status == DomainEnums.ClienteStatus.APROV)
            {
                DataAprovou = DateTime.Now;
            }
        }

        public void Aprovar()
        {
            SetStatus(DomainEnums.ClienteStatus.APROV);
        }

        public void Reprovar()
        {
            SetStatus(DomainEnums.ClienteStatus.REPR);
        }

        public void Inativar()
        {
            SetStatus(DomainEnums.ClienteStatus.INAT);
        }

        // Note: status should only be changed by domain operations (Aprovar/Reprovar/Inativar)

        public void Atualizar(
            long vendedorId,
            string cpfCnpj,
            string razaoSocial,
            string? nomeFantasia,
            string? contato,
            string? telefoneDdd,
            string? telefoneNumero,
            int diaCobranca,
            List<Endereco>? enderecos,
            string? dadosSerasa)
        {
            // Reuse domain validations when appropriate
            Assegure.Que(vendedorId > 0, "O vendedor deve ser informado.");
            Assegure.NaoNulo(cpfCnpj, "O CPF/CNPJ deve ser informado.");
            Assegure.NaoNulo(razaoSocial, "A razão social deve ser informada.");

            VendedorId = vendedorId;
            CpfCnpj = cpfCnpj;
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            Contato = contato;
            TelefoneDdd = telefoneDdd;
            TelefoneNumero = telefoneNumero;
            DiaCobranca = diaCobranca;

            // Replace enderecos if provided
            if (enderecos != null)
            {
                Enderecos = enderecos.ToList();
            }

            DadosSerasa = dadosSerasa;
        }
    }
}
