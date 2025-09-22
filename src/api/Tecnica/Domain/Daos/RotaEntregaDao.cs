using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;
using Tecnica.Domain.Dtos;

namespace Tecnica.Domain.Daos
{
    public class RotaEntregaDao
    {
        private readonly ApplicationDbContext _context;

        public RotaEntregaDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RotaEntrega?> ObterPorIdAsync(long id)
        {
            return await _context.Set<RotaEntrega>()
                .Include(r => r.Entregas)
                .ThenInclude(e => e.Venda)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<RotaEntrega>> ObterPorDataAsync(DateTime data)
        {
            return await _context.Set<RotaEntrega>()
                .Include(r => r.Entregas)
                .Where(r => r.Data.Date == data.Date)
                .ToListAsync();
        }

        public async Task<List<RotaEntregaResultDto>> ListarComFiltrosAsync(long? clienteId, DateTime? dataEntrega, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Set<RotaEntrega>()
                .Include(r => r.Entregas)
                .ThenInclude(e => e.Venda)
                .AsQueryable();

            if (dataEntrega.HasValue)
            {
                query = query.Where(r => r.Data.Date == dataEntrega.Value.Date);
            }

            if (clienteId.HasValue)
            {
                query = query.Where(r => r.Entregas.Any(e => e.Venda != null && e.Venda.ClienteId == clienteId.Value));
            }

            return await query
                .OrderBy(r => r.Data)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RotaEntregaResultDto
                {
                    Id = r.Id,
                    Data = r.Data,
                    TotalEntregas = r.Entregas.Count
                })
                .ToListAsync();
        }

        public async Task<List<RotaEntrega>> ListarTodosAsync()
        {
            return await _context.Set<RotaEntrega>().ToListAsync();
        }

        public async Task<Entrega?> ObterEntregaPorIdAsync(long entregaId)
        {
            return await _context.Set<Entrega>()
                .Include(e => e.Venda)
                .FirstOrDefaultAsync(e => e.Id == entregaId);
        }

        public async Task<Entrega?> ObterEntregaPorRotaEVendaAsync(long rotaId, long vendaId)
        {
            return await _context.Set<Entrega>()
                .FirstOrDefaultAsync(e => e.RotaEntregaId == rotaId && e.VendaId == vendaId);
        }

        public async Task AdicionarAsync(RotaEntrega rotaEntrega)
        {
            await _context.Set<RotaEntrega>().AddAsync(rotaEntrega);
        }


        public void RemoverAsync(RotaEntrega rotaEntrega)
        {
            _context.Set<RotaEntrega>().Remove(rotaEntrega);
        }

        internal async Task<List<Venda>> ListarEntregasEmAberto(DateTime data)
        {
            return await _context.Set<Venda>()
                .Where(r => r.DataEntrega == data.Date
                    && (r.Status == nameof(DomainEnums.VendaStatus.APROV)
                    || r.Status == nameof(DomainEnums.VendaStatus.EXP))
                    && !_context.Set<Entrega>().Any(e => e.RotaEntrega.Data.Date == data.Date
                    && e.VendaId == r.Id))
                .ToListAsync();
        }
    }
}
