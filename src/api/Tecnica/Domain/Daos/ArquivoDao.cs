using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class ArquivoDao
    {
        private readonly ApplicationDbContext _context;

        public ArquivoDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Arquivo> ObterPorIdAsync(long id)
        {
            return await _context.Set<Arquivo>().FindAsync(id);
        }

        public async Task<List<Arquivo>> ListarTodosAsync()
        {
            return await _context.Set<Arquivo>().ToListAsync();
        }

        public async Task AdicionarAsync(Arquivo arquivo)
        {
            await _context.Set<Arquivo>().AddAsync(arquivo);
        }

        public async Task AtualizarAsync(Arquivo arquivo)
        {
            _context.Set<Arquivo>().Update(arquivo);
        }

        public async Task RemoverAsync(Arquivo arquivo)
        {
            _context.Set<Arquivo>().Remove(arquivo);
        }
    }
}
