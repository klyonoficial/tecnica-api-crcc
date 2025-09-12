using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class VendaProdutoDao
    {
        private readonly ApplicationDbContext _context;

        public VendaProdutoDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VendaProduto> ObterPorIdAsync(long id)
        {
            return await _context.Set<VendaProduto>().FindAsync(id);
        }

        public async Task<List<VendaProduto>> ListarTodosAsync()
        {
            return await _context.Set<VendaProduto>().ToListAsync();
        }

        public async Task AdicionarAsync(VendaProduto vendaProduto)
        {
            await _context.Set<VendaProduto>().AddAsync(vendaProduto);
        }

        public async Task AtualizarAsync(VendaProduto vendaProduto)
        {
            _context.Set<VendaProduto>().Update(vendaProduto);
        }

        public async Task RemoverAsync(VendaProduto vendaProduto)
        {
            _context.Set<VendaProduto>().Remove(vendaProduto);
        }
    }
}
