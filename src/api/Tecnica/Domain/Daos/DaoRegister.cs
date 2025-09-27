using Microsoft.Extensions.DependencyInjection;

namespace Tecnica.Domain.Daos
{
    public static class DaoRegister
    {
        public static IServiceCollection AddDaos(this IServiceCollection services)
        {
            // Registro de DAOs:
            services.AddScoped<UsuarioDao>();
            services.AddScoped<ClienteDao>();
            services.AddScoped<FormaPagamentoDao>();
            services.AddScoped<FabricanteDao>();
            services.AddScoped<ObservacaoDao>();
            services.AddScoped<ProdutoDao>();
            services.AddScoped<RotaCobrancaDao>();
            services.AddScoped<RotaEntregaDao>();
            services.AddScoped<VendaDao>();

            return services;
        }
    }
}