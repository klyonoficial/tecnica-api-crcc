using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class ClienteDao
    {
        private readonly ApplicationDbContext _context;

        public ClienteDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente> ObterPorIdAsync(long id)
        {
            return await _context.Set<Cliente>().FindAsync(id);
        }

        public async Task<List<Cliente>> ListarTodosAsync()
        {
            return await _context.Set<Cliente>().ToListAsync();
        }

        public async Task AdicionarAsync(Cliente cliente)
        {
            await _context.Set<Cliente>().AddAsync(cliente);
        }

        public async Task AtualizarAsync(Cliente cliente)
        {
            _context.Set<Cliente>().Update(cliente);
        }

        public async Task RemoverAsync(Cliente cliente)
        {
            _context.Set<Cliente>().Remove(cliente);
        }
    }
}
