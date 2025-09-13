using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class FornecedorDao
    {
        private readonly ApplicationDbContext _context;

        public FornecedorDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Fornecedor> ObterPorIdAsync(long id)
        {
            return await _context.Set<Fornecedor>().FindAsync(id);
        }

        public async Task<List<Fornecedor>> ListarTodosAsync()
        {
            return await _context.Set<Fornecedor>().ToListAsync();
        }

        public async Task AdicionarAsync(Fornecedor fornecedor)
        {
            await _context.Set<Fornecedor>().AddAsync(fornecedor);
        }

        public async Task RemoverAsync(Fornecedor fornecedor)
        {
            _context.Set<Fornecedor>().Remove(fornecedor);
        }
    }
}
