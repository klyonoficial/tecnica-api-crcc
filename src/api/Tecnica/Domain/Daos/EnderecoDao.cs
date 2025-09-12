using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class EnderecoDao
    {
        private readonly ApplicationDbContext _context;

        public EnderecoDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Endereco> ObterPorIdAsync(long id)
        {
            return await _context.Set<Endereco>().FindAsync(id);
        }

        public async Task<List<Endereco>> ListarTodosAsync()
        {
            return await _context.Set<Endereco>().ToListAsync();
        }

        public async Task AdicionarAsync(Endereco endereco)
        {
            await _context.Set<Endereco>().AddAsync(endereco);
        }

        public async Task AtualizarAsync(Endereco endereco)
        {
            _context.Set<Endereco>().Update(endereco);
        }

        public async Task RemoverAsync(Endereco endereco)
        {
            _context.Set<Endereco>().Remove(endereco);
        }
    }
}
