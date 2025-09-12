using Microsoft.Extensions.DependencyInjection;

namespace Tecnica.Domain.Daos
{
    public static class DaoRegister
    {
        public static IServiceCollection AddDaos(this IServiceCollection services)
        {
            // Registro de DAOs:
            services.AddScoped<UsuarioDao>();
            services.AddScoped<ArquivoDao>();
            services.AddScoped<ClienteDao>();
            services.AddScoped<EnderecoDao>();
            services.AddScoped<FormaPagamentoDao>();
            services.AddScoped<FornecedorDao>();
            services.AddScoped<ObservacaoDao>();
            services.AddScoped<ProdutoDao>();
            services.AddScoped<RotaCobrancaDao>();
            services.AddScoped<RotaCobrancaVendaDao>();
            services.AddScoped<RotaEntregaDao>();
            services.AddScoped<RotaEntregaVendaDao>();
            services.AddScoped<VendaDao>();
            services.AddScoped<VendaParcelaDao>();
            services.AddScoped<VendaProdutoDao>();

            return services;
        }
    }
}