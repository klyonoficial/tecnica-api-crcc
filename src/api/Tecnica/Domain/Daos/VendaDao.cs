using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class VendaDao
    {
        private readonly ApplicationDbContext _context;

        public VendaDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Venda> ObterPorIdAsync(long id)
        {
            return await _context.Set<Venda>().FindAsync(id);
        }

        public async Task<List<Venda>> ListarTodosAsync()
        {
            return await _context.Set<Venda>().ToListAsync();
        }

        public async Task AdicionarAsync(Venda venda)
        {
            await _context.Set<Venda>().AddAsync(venda);
        }

        public async Task RemoverAsync(Venda venda)
        {
            _context.Set<Venda>().Remove(venda);
        }
    }
}
