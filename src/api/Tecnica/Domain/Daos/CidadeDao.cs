using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class CidadeDao
    {
        private readonly ApplicationDbContext _context;
        private static Dictionary<string, Cidade> _cacheCidades = new Dictionary<string, Cidade>();
        private static bool _cacheInicializado = false;
        private static readonly object _lock = new object();

        public CidadeDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cidade?> BuscarPorIdAsync(long id)
        {
            return await _context.Cidades
                .FirstOrDefaultAsync(c => c.IdCidade == id);
        }

        public async Task<Cidade?> BuscarPorCodigoIbgeAsync(string codigoIbge)
        {
            return await _context.Cidades
                .FirstOrDefaultAsync(c => c.CodigoIbge == codigoIbge);
        }

        public async Task<List<Cidade>> BuscarPorUfAsync(string uf)
        {
            return await _context.Cidades
                .Where(c => c.Uf == uf.ToUpper())
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<List<Cidade>> BuscarPorNomeAsync(string nome)
        {
            return await _context.Cidades
                .Where(c => c.Nome.Contains(nome))
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<List<Cidade>> BuscarTodasAsync()
        {
            return await _context.Cidades
                .OrderBy(c => c.Uf)
                .ThenBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<bool> ExistePorCodigoIbgeAsync(string codigoIbge)
        {
            return await _context.Cidades
                .AnyAsync(c => c.CodigoIbge == codigoIbge);
        }

        public static Task InicializarCacheAsync(ApplicationDbContext context)
        {
            if (_cacheInicializado)
                return Task.CompletedTask;

            lock (_lock)
            {
                if (_cacheInicializado)
                    return Task.CompletedTask;

                var cidades = context.Cidades.ToList();
                _cacheCidades = cidades.ToDictionary(c => c.CodigoIbge, c => c);
                _cacheInicializado = true;
            }

            return Task.CompletedTask;
        }

        public static Cidade? BuscarCidadePorCodigoIbgeCache(string codigoIbge)
        {
            if (!_cacheInicializado)
                return null;

            _cacheCidades.TryGetValue(codigoIbge, out var cidade);
            return cidade;
        }

        public static void LimparCache()
        {
            lock (_lock)
            {
                _cacheCidades.Clear();
                _cacheInicializado = false;
            }
        }

        public static int ObterQuantidadeCidadesCache()
        {
            return _cacheInicializado ? _cacheCidades.Count : 0;
        }

        /// <summary>
        /// Busca uma cidade pelo código IBGE usando o cache (método otimizado)
        /// </summary>
        /// <param name="codigoIbge">Código IBGE da cidade</param>
        /// <returns>Cidade encontrada ou null se não existir</returns>
        public static Cidade? BuscarCidadePorCodigoIbgeOtimizado(string codigoIbge)
        {
            return BuscarCidadePorCodigoIbgeCache(codigoIbge);
        }
    }
}
