using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class VendaParcelaDao
    {
        private readonly ApplicationDbContext _context;

        public VendaParcelaDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VendaParcela> ObterPorIdAsync(long id)
        {
            return await _context.Set<VendaParcela>().FindAsync(id);
        }

        public async Task<List<VendaParcela>> ListarTodosAsync()
        {
            return await _context.Set<VendaParcela>().ToListAsync();
        }

        public async Task AdicionarAsync(VendaParcela vendaParcela)
        {
            await _context.Set<VendaParcela>().AddAsync(vendaParcela);
        }

        public async Task AtualizarAsync(VendaParcela vendaParcela)
        {
            _context.Set<VendaParcela>().Update(vendaParcela);
        }

        public async Task RemoverAsync(VendaParcela vendaParcela)
        {
            _context.Set<VendaParcela>().Remove(vendaParcela);
        }
    }
}
