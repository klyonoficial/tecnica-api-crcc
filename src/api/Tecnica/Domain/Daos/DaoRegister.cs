using Microsoft.Extensions.DependencyInjection;

namespace Tecnica.Domain.Daos
{
    public static class DaoRegister
    {
        public static IServiceCollection AddDaos(this IServiceCollection services)
        {
            // Exemplo de registro de DAOs:
            services.AddScoped<UsuarioDao>();
            // services.AddScoped<IProdutoDao, ProdutoDao>();

            // Adicione aqui os registros dos seus DAOs

            return services;
        }
    }
}