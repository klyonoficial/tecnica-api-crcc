using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Arquivo> Arquivos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<FormaPagamento> FormasPagamento { get; set; }
    public DbSet<Fornecedor> Fornecedores { get; set; }
    public DbSet<Observacao> Observacoes { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<RotaCobranca> RotasCobranca { get; set; }
    public DbSet<RotaCobrancaVenda> RotasCobrancaVenda { get; set; }
    public DbSet<RotaEntrega> RotasEntrega { get; set; }
    public DbSet<RotaEntregaVenda> RotasEntregaVenda { get; set; }
    public DbSet<Venda> Vendas { get; set; }
    public DbSet<VendaParcela> VendasParcela { get; set; }
    public DbSet<VendaProduto> VendasProduto { get; set; }
    }
}