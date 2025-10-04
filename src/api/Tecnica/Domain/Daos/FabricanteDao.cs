using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class FabricanteDao
    {
        private readonly ApplicationDbContext _context;

        public FabricanteDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Fabricante> ObterPorIdAsync(long id)
        {
            return await _context.Set<Fabricante>().FindAsync(id);
        }

        public async Task<List<Fabricante>> ListarTodosAsync()
        {
            return await _context.Set<Fabricante>().ToListAsync();
        }

        public async Task AdicionarAsync(Fabricante fabricante)
        {
            await _context.Set<Fabricante>().AddAsync(fabricante);
        }

        public async Task RemoverAsync(Fabricante fabricante)
        {
            _context.Set<Fabricante>().Remove(fabricante);
        }
    }
}
