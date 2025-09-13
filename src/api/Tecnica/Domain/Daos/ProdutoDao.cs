using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class ProdutoDao
    {
        private readonly ApplicationDbContext _context;

        public ProdutoDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Produto> ObterPorIdAsync(long id)
        {
            return await _context.Set<Produto>().FindAsync(id);
        }

        public async Task<List<Produto>> ListarTodosAsync()
        {
            return await _context.Set<Produto>().ToListAsync();
        }

        public async Task AdicionarAsync(Produto produto)
        {
            await _context.Set<Produto>().AddAsync(produto);
        }

        public async Task RemoverAsync(Produto produto)
        {
            _context.Set<Produto>().Remove(produto);
        }
    }
}
