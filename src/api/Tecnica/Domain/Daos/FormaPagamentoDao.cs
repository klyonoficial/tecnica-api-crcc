using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;

namespace Tecnica.Domain.Daos
{
    public class FormaPagamentoDao
    {
        private readonly ApplicationDbContext _context;

        public FormaPagamentoDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FormaPagamento> ObterPorIdAsync(long id)
        {
            return await _context.Set<FormaPagamento>().FindAsync(id);
        }

        public async Task<List<FormaPagamento>> ListarTodosAsync()
        {
            return await _context.Set<FormaPagamento>().ToListAsync();
        }

        public async Task AdicionarAsync(FormaPagamento formaPagamento)
        {
            await _context.Set<FormaPagamento>().AddAsync(formaPagamento);
        }

        public async Task RemoverAsync(FormaPagamento formaPagamento)
        {
            _context.Set<FormaPagamento>().Remove(formaPagamento);
        }
    }
}
