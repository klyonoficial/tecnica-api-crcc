using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class RotaCobrancaDao
    {
        private readonly ApplicationDbContext _context;

        public RotaCobrancaDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RotaCobranca> ObterPorIdAsync(long id)
        {
            return await _context.Set<RotaCobranca>().FindAsync(id);
        }

        public async Task<List<RotaCobranca>> ListarTodosAsync()
        {
            return await _context.Set<RotaCobranca>().ToListAsync();
        }

        public async Task AdicionarAsync(RotaCobranca rotaCobranca)
        {
            await _context.Set<RotaCobranca>().AddAsync(rotaCobranca);
        }

        public async Task AtualizarAsync(RotaCobranca rotaCobranca)
        {
            _context.Set<RotaCobranca>().Update(rotaCobranca);
        }

        public async Task RemoverAsync(RotaCobranca rotaCobranca)
        {
            _context.Set<RotaCobranca>().Remove(rotaCobranca);
        }
    }
}
