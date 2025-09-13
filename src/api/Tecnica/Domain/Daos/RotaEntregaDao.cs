using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class RotaEntregaDao
    {
        private readonly ApplicationDbContext _context;

        public RotaEntregaDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RotaEntrega> ObterPorIdAsync(long id)
        {
            return await _context.Set<RotaEntrega>().FindAsync(id);
        }

        public async Task<List<RotaEntrega>> ListarTodosAsync()
        {
            return await _context.Set<RotaEntrega>().ToListAsync();
        }

        public async Task AdicionarAsync(RotaEntrega rotaEntrega)
        {
            await _context.Set<RotaEntrega>().AddAsync(rotaEntrega);
        }

        public async Task RemoverAsync(RotaEntrega rotaEntrega)
        {
            _context.Set<RotaEntrega>().Remove(rotaEntrega);
        }
    }
}
