using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class ObservacaoDao
    {
        private readonly ApplicationDbContext _context;

        public ObservacaoDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Observacao> ObterPorIdAsync(long id)
        {
            return await _context.Set<Observacao>().FindAsync(id);
        }

        public async Task<List<Observacao>> ListarTodosAsync()
        {
            return await _context.Set<Observacao>().ToListAsync();
        }

        public async Task AdicionarAsync(Observacao observacao)
        {
            await _context.Set<Observacao>().AddAsync(observacao);
        }

        public async Task RemoverAsync(Observacao observacao)
        {
            _context.Set<Observacao>().Remove(observacao);
        }
    }
}
