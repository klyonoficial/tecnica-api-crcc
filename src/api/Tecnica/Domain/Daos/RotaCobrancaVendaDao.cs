using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class RotaCobrancaVendaDao
    {
        private readonly ApplicationDbContext _context;

        public RotaCobrancaVendaDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RotaCobrancaVenda> ObterPorIdAsync(long id)
        {
            return await _context.Set<RotaCobrancaVenda>().FindAsync(id);
        }

        public async Task<List<RotaCobrancaVenda>> ListarTodosAsync()
        {
            return await _context.Set<RotaCobrancaVenda>().ToListAsync();
        }

        public async Task AdicionarAsync(RotaCobrancaVenda rotaCobrancaVenda)
        {
            await _context.Set<RotaCobrancaVenda>().AddAsync(rotaCobrancaVenda);
        }

        public async Task AtualizarAsync(RotaCobrancaVenda rotaCobrancaVenda)
        {
            _context.Set<RotaCobrancaVenda>().Update(rotaCobrancaVenda);
        }

        public async Task RemoverAsync(RotaCobrancaVenda rotaCobrancaVenda)
        {
            _context.Set<RotaCobrancaVenda>().Remove(rotaCobrancaVenda);
        }
    }
}
