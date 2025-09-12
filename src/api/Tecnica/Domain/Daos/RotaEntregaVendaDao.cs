using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class RotaEntregaVendaDao
    {
        private readonly ApplicationDbContext _context;

        public RotaEntregaVendaDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RotaEntregaVenda> ObterPorIdAsync(long id)
        {
            return await _context.Set<RotaEntregaVenda>().FindAsync(id);
        }

        public async Task<List<RotaEntregaVenda>> ListarTodosAsync()
        {
            return await _context.Set<RotaEntregaVenda>().ToListAsync();
        }

        public async Task AdicionarAsync(RotaEntregaVenda rotaEntregaVenda)
        {
            await _context.Set<RotaEntregaVenda>().AddAsync(rotaEntregaVenda);
        }

        public async Task AtualizarAsync(RotaEntregaVenda rotaEntregaVenda)
        {
            _context.Set<RotaEntregaVenda>().Update(rotaEntregaVenda);
        }

        public async Task RemoverAsync(RotaEntregaVenda rotaEntregaVenda)
        {
            _context.Set<RotaEntregaVenda>().Remove(rotaEntregaVenda);
        }
    }
}
