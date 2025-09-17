using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tecnica.Domain.Entities;
using Tecnica.Domain.Dtos;

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
                    ClienteId = v.ClienteId,
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
    }
}
