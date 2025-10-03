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

        public async Task<Cliente?> ObterPorIdAsync(long id)
        {
            return await _context.Set<Cliente>()
                .Include(c => c.Enderecos)
                .Include(c => c.Arquivos)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Cliente>> ListarTodosAsync()
        {
            return await _context.Set<Cliente>().ToListAsync();
        }

        public async Task<List<Cliente>> ListarPorFiltroAsync(string? status, string? filtro)
        {
            var query = _context.Set<Cliente>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(c => c.Status == status);

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var f = filtro.ToLower();
                query = query.Where(c => (c.RazaoSocial ?? "").ToLower().Contains(f)
                    || (c.NomeFantasia ?? "").ToLower().Contains(f)
                    || (c.TelefoneNumero ?? "").ToLower().Contains(f)
                    || (c.Contato ?? "").ToLower().Contains(f)
                    || (c.CpfCnpj ?? "").ToLower().Contains(f));
            }

            return await query.OrderBy(c => c.RazaoSocial).ToListAsync();
        }

        public async Task AdicionarAsync(Cliente cliente)
        {
            await _context.Set<Cliente>().AddAsync(cliente);
        }

        public void RemoverAsync(Cliente cliente)
        {
            _context.Set<Cliente>().Remove(cliente);
        }
    }
}
