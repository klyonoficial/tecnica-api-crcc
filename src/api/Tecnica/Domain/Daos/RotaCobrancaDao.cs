using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;
using Tecnica.Dtos;
using static Tecnica.Domain.Entities.DomainEnums;

namespace Tecnica.Domain.Daos
{
    public class RotaCobrancaDao
    {
        private readonly ApplicationDbContext _context;

        public RotaCobrancaDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CobrancaPorDataDto>> ListarPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            // Buscamos as parcelas que têm data de vencimento no período informado
            var parcelas = await _context.Set<Parcela>()
                .Where(p => p.DataVencimento.Date >= dataInicio.Date && 
                       p.DataVencimento.Date <= dataFim.Date)
                .GroupBy(p => p.DataVencimento.Date)
                .Select(g => new CobrancaPorDataDto
                {
                    Data = g.Key,
                    Quantidade = g.Count(),
                    Total = g.Sum(p => p.Valor)
                })
                .OrderBy(dto => dto.Data)
                .ToListAsync();

            return parcelas;
        }

        public async Task<RotaCobranca?> ObterPorIdAsync(long id)
        {
            return await _context.Set<RotaCobranca>()
                .Include(r => r.Cobrancas)
                .ThenInclude(c => c.Parcela)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<RotaCobranca>?> ObterPorDataAsync(DateTime data)
        {
            return await _context.Set<RotaCobranca>()
                .Where(r => r.Data.Date == data.Date)
                .ToListAsync();
        }

        public async Task<List<RotaCobranca>> ListarPorFiltroAsync(long? clienteId, DateTime? dataCobranca, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Set<RotaCobranca>()
                .AsQueryable();

            if (dataCobranca.HasValue)
            {
                query = query.Where(r => r.Data.Date == dataCobranca.Value.Date);
            }

            if (clienteId.HasValue)
            {
                query = query.Where(r => r.Cobrancas.Any(c => c.Parcela != null && c.Parcela.Venda != null && c.Parcela.Venda.ClienteId == clienteId.Value));
            }

            // Aplicar paginação
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task AdicionarAsync(RotaCobranca rotaCobranca)
        {
            await _context.Set<RotaCobranca>().AddAsync(rotaCobranca);
        }

        public void Remover(RotaCobranca rotaCobranca)
        {
            _context.Set<RotaCobranca>().Remove(rotaCobranca);
        }

        internal async Task<List<Parcela>> ListarParcelasEmAberto(DateTime data)
        {
            return await _context.Set<Parcela>()
                .Where(p => p.DataVencimento.Date == data.Date
                && (p.Status == ParcelaStatus.ABT.ToString()
                    || p.Status == ParcelaStatus.NPAG.ToString())
                && !_context.Set<Cobranca>().Any(x => x.RotaCobranca.Data.Date == data.Date
                && x.ParcelaId == p.Id))
                .ToListAsync();
        }
    }
}
