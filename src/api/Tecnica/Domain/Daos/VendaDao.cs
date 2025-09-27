using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;
using Tecnica.Domain.Dtos;
using Tecnica.Dtos;
using static Tecnica.Domain.Entities.DomainEnums;

namespace Tecnica.Domain.Daos
{
    public class VendaDao
    {
        private readonly ApplicationDbContext _context;

        public VendaDao(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Venda?> ObterPorIdAsync(long id)
        {
            return await _context.Set<Venda>()
                .Include(v => v.Items)
                .Include(v => v.Parcelas)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<VendaResultDto>> ListarTodosAsync(int pageNumber, int pageSize)
        {
            return await _context.Set<Venda>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(v => new VendaResultDto
                {
                    Id = v.Id,
                    DataCadastro = v.DataCadastro,
                    Status = v.Status,
                    ClienteId = v.PessoaId,
                    VendedorId = v.VendedorId,
                    DataEntrega = v.DataEntrega,
                    ValorTotal = v.ValorTotal,
                    ValorEntrada = v.ValorEntrada,
                    DiaAcerto = v.DiaAcerto,
                    QuantidadeParcelas = v.QuantidadeParcelas,
                    Observacao = v.Observacao,
                    EnderecoCep = v.EnderecoCep,
                    EnderecoCodigoIbge = v.EnderecoCodigoIbge,
                    EnderecoBairro = v.EnderecoBairro,
                    EnderecoUF = v.EnderecoUF,
                    EnderecoNumero = v.EnderecoNumero,
                    EnderecoComplemento = v.EnderecoComplemento,
                    EnderecoTipo = v.EnderecoTipo
                })
                .ToListAsync();
        }

        public async Task AdicionarAsync(Venda venda)
        {
            await _context.Set<Venda>().AddAsync(venda);
        }

        public void Remover(Venda venda)
        {
            _context.Set<Venda>().Remove(venda);
        }

        internal List<Venda> ObterPorDataEntrega(DateTime dataEntrega)
        {
            return _context.Set<Venda>()
            .Where(v => v.DataEntrega.HasValue && v.DataEntrega.Value.Date == dataEntrega.Date).ToList();
        }

        /// <summary>
        /// Obtém vendas com parcelas em aberto que vencem na data especificada
        /// </summary>
        /// <param name="dataVencimento">Data de vencimento das parcelas a filtrar</param>
        /// <returns>Lista de vendas com parcelas em aberto na data especificada</returns>
        public async Task<List<Parcela>> ObterParcelasEmAberto(DateTime dataVencimento)
        {
            return await _context.Set<Parcela>()
                .Include(p => p.Venda)
                .Where(p => p.DataVencimento.Date == dataVencimento.Date && 
                             p.Status != ParcelaStatus.PAG.ToString())
                .ToListAsync();
        }
        
        /// <summary>
        /// Lista entregas agrupadas por data dentro de um período especificado
        /// </summary>
        /// <param name="dataInicio">Data inicial do período</param>
        /// <param name="dataFim">Data final do período</param>
        /// <returns>Lista de entregas agrupadas por data com suas quantidades</returns>
        public async Task<List<EntregaPorDataDto>> ListarEntregasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            // Buscamos as vendas que têm data de entrega no período informado
            var entregas = await _context.Set<Venda>()
                .Where(v => v.DataEntrega.HasValue && 
                      v.DataEntrega.Value.Date >= dataInicio.Date && 
                      v.DataEntrega.Value.Date <= dataFim.Date)
                .GroupBy(v => v.DataEntrega!.Value.Date)
                .Select(g => new EntregaPorDataDto
                {
                    Data = g.Key,
                    Quantidade = g.Count()
                })
                .OrderBy(dto => dto.Data)
                .ToListAsync();

            return entregas;
        }
    }
}
