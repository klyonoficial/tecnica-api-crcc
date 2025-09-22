using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tecnica.Domain.Daos;
using Tecnica.Domain.Dtos;
using Tecnica.Domain.Entities;
using Tecnica.Infra;

namespace Tecnica.Api.Controllers
{
    [ApiController]
    [Route("venda")]
    public class VendaController : ControllerBase
    {
        private readonly VendaDao _vendaDao;
        private readonly ApplicationDbContext _context;

        public VendaController(VendaDao vendaDao, ApplicationDbContext context)
        {
            _vendaDao = vendaDao;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendaResultDto>>> GetVendas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var vendas = await _vendaDao.ListarTodosAsync(pageNumber, pageSize);
            return Ok(vendas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VendaResultDto>> GetVenda(long id)
        {
            var venda = await _vendaDao.ObterPorIdAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            var vendaDto = new VendaResultDto
            {
                Id = venda.Id,
                DataCadastro = venda.DataCadastro,
                Status = venda.Status,
                ClienteId = venda.ClienteId,
                VendedorId = venda.VendedorId,
                DataEntrega = venda.DataEntrega,
                ValorTotal = venda.ValorTotal,
                ValorEntrada = venda.ValorEntrada,
                DiaAcerto = venda.DiaAcerto,
                QuantidadeParcelas = venda.QuantidadeParcelas,
                Observacao = venda.Observacao,
                EnderecoCep = venda.EnderecoCep,
                EnderecoCodigoIbge = venda.EnderecoCodigoIbge,
                EnderecoBairro = venda.EnderecoBairro,
                EnderecoUF = venda.EnderecoUF,
                EnderecoNumero = venda.EnderecoNumero,
                EnderecoComplemento = venda.EnderecoComplemento,
                EnderecoTipo = venda.EnderecoTipo
            };

            return Ok(vendaDto);
        }

        [HttpPost]
        public async Task<ActionResult<VendaDto>> CreateVenda([FromBody] VendaDto vendaDto)
        {
            // Criar os itens da venda
            var items = vendaDto.Produtos.Select(p => new Item(
                p.ProdutoId,
                p.Preco,
                p.Quantidade,
                p.NumerosSerie
            )).ToList();

            // Criar as parcelas da venda
            var parcelas = vendaDto.Parcelas.Select(p => new Parcela(
                p.DataVencimento,
                p.Valor
            )).ToList();

            // Criar a venda
            var venda = new Venda(
                DateTime.Now,
                vendaDto.ClienteId,
                vendaDto.VendedorId,
                vendaDto.DataEntrega,
                vendaDto.ValorEntrada,
                vendaDto.DiaAcerto,
                vendaDto.QuantidadeParcelas,
                vendaDto.Observacao,
                vendaDto.EnderecoCep,
                vendaDto.EnderecoLogradouro,
                vendaDto.EnderecoCodigoIbge,
                vendaDto.EnderecoBairro,
                vendaDto.EnderecoUF,
                vendaDto.EnderecoNumero,
                vendaDto.EnderecoComplemento,
                vendaDto.EnderecoTipo,
                items,
                parcelas
            );

            await _vendaDao.AdicionarAsync(venda);
            await _context.SaveChangesAsync();

            vendaDto.Id = venda.Id;
            return CreatedAtAction(nameof(GetVendas), new { id = venda.Id }, vendaDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VendaDto>> UpdateVenda(long id, [FromBody] VendaDto vendaDto)
        {
            var vendaExistente = await _vendaDao.ObterPorIdAsync(id);
            if (vendaExistente == null)
            {
                return NotFound();
            }

            vendaExistente.Atualizar(
                vendaDto.DataEntrega,
                vendaDto.ValorEntrada,
                vendaDto.DiaAcerto,
                vendaDto.QuantidadeParcelas,
                vendaDto.Observacao,
                vendaDto.EnderecoCep,
                vendaDto.EnderecoCodigoIbge,
                vendaDto.EnderecoBairro,
                vendaDto.EnderecoUF,
                vendaDto.EnderecoNumero,
                vendaDto.EnderecoComplemento,
                vendaDto.EnderecoTipo
            );

            await _context.SaveChangesAsync();

            return Ok(vendaDto);
        }

        [HttpPut("{id}/aprovar")]
        public async Task<ActionResult> AprovarVendaAlternativa(long id)
        {
            var venda = await _vendaDao.ObterPorIdAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            venda.Aprovar();
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}/reagendar")]
        public async Task<ActionResult> ReagendarVenda(long id, [FromBody] VendaReagendarDto vendaReagendarDto)
        {
            var venda = await _vendaDao.ObterPorIdAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            venda.Reagendar(vendaReagendarDto.NovaDataEntrega, vendaReagendarDto.Observacao);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}/expedicao")]
        public async Task<ActionResult> ExpedicaoVenda(long id)
        {
            var venda = await _vendaDao.ObterPorIdAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            venda.Expedicao();
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}/reprovar")]
        public async Task<ActionResult> ReprovarVenda(long id)
        {
            var venda = await _vendaDao.ObterPorIdAsync(id);
            if (venda == null)
            {
                return NotFound();
            }

            venda.Reprovar();
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}