using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<VendaResultDto>>> GetVendas()
        {
            var vendas = await _vendaDao.ListarTodosAsync();
            var vendasDto = vendas.Select(v => new VendaResultDto
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
                EnderecoTipo = v.EnderecoTipo,
                Produtos = v.Items?.Select(i => new VendaProdutoDto
                {
                    Id = i.Id,
                    VendaId = i.VendaId,
                    ProdutoId = i.ProdutoId,
                    Preco = i.Preco,
                    Quantidade = i.Quantidade,
                    NumerosSerie = i.NumerosSerie
                }).ToList() ?? new List<VendaProdutoDto>(),
                Parcelas = v.Parcelas?.Select(p => new VendaParcelaDto
                {
                    Id = p.Id,
                    VendaId = p.VendaId,
                    DataVencimento = p.DataVencimento,
                    DataPagamento = p.DataPagamento,
                    Valor = p.Valor,
                    ValorPago = p.ValorPago,
                    Status = p.Status,
                    DataRemanejamento = p.DataRemanejamento,
                    FormaPagamentoId = p.FormaPagamentoId
                }).ToList() ?? new List<VendaParcelaDto>()
            }).ToList();

            return Ok(vendasDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VendaResultDto>> GetVenda(long id)
        {
            var venda = await _vendaDao.ObterPorIdAsync(id);
            if (venda == null)
            {
                return NotFound(new { message = "Venda não encontrada." });
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
                EnderecoTipo = venda.EnderecoTipo,
                Produtos = venda.Items?.Select(i => new VendaProdutoDto
                {
                    Id = i.Id,
                    VendaId = i.VendaId,
                    ProdutoId = i.ProdutoId,
                    Preco = i.Preco,
                    Quantidade = i.Quantidade,
                    NumerosSerie = i.NumerosSerie
                }).ToList() ?? new List<VendaProdutoDto>(),
                Parcelas = venda.Parcelas?.Select(p => new VendaParcelaDto
                {
                    Id = p.Id,
                    VendaId = p.VendaId,
                    DataVencimento = p.DataVencimento,
                    DataPagamento = p.DataPagamento,
                    Valor = p.Valor,
                    ValorPago = p.ValorPago,
                    Status = p.Status,
                    DataRemanejamento = p.DataRemanejamento,
                    FormaPagamentoId = p.FormaPagamentoId
                }).ToList() ?? new List<VendaParcelaDto>()
            };

            return Ok(vendaDto);
        }

        [HttpPost]
        public async Task<ActionResult<VendaDto>> CreateVenda([FromBody] VendaDto vendaDto)
        {
            // Criar os itens da venda
            var items = vendaDto.Produtos.Select(p => new Item(
                0, // VendaId será definido depois
                p.ProdutoId,
                p.Preco,
                p.Quantidade,
                p.NumerosSerie
            )).ToList();

            // Criar as parcelas da venda
            var parcelas = vendaDto.Parcelas.Select(p => new Parcela(
                0, // VendaId será definido depois
                p.DataVencimento,
                p.Valor,
                p.Status
            )).ToList();

            // Criar a venda
            var venda = new Venda(
                DateTime.Now,
                vendaDto.Status,
                vendaDto.ClienteId,
                vendaDto.VendedorId,
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
                return NotFound(new { message = "Venda não encontrada." });
            }

            // Atualizar propriedades básicas
            vendaExistente.Status = vendaDto.Status;
            vendaExistente.DataEntrega = vendaDto.DataEntrega;
            vendaExistente.ValorEntrada = vendaDto.ValorEntrada;
            vendaExistente.DiaAcerto = vendaDto.DiaAcerto;
            vendaExistente.QuantidadeParcelas = vendaDto.QuantidadeParcelas;
            vendaExistente.Observacao = vendaDto.Observacao;
            vendaExistente.EnderecoCep = vendaDto.EnderecoCep;
            vendaExistente.EnderecoCodigoIbge = vendaDto.EnderecoCodigoIbge;
            vendaExistente.EnderecoBairro = vendaDto.EnderecoBairro;
            vendaExistente.EnderecoUF = vendaDto.EnderecoUF;
            vendaExistente.EnderecoNumero = vendaDto.EnderecoNumero;
            vendaExistente.EnderecoComplemento = vendaDto.EnderecoComplemento;
            vendaExistente.EnderecoTipo = vendaDto.EnderecoTipo;

            await _context.SaveChangesAsync();

            return Ok(vendaDto);
        }

        [HttpPut("{id}/aprovacao")]
        public async Task<ActionResult> AprovarVenda(long id)
        {
            var venda = await _vendaDao.ObterPorIdAsync(id);
            if (venda == null)
            {
                return NotFound(new { message = "Venda não encontrada." });
            }

            venda.Status = nameof(DomainEnums.VendaStatus.APROV);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Venda aprovada com sucesso." });
        }

        [HttpPut("{id}/aprovar")]
        public async Task<ActionResult> AprovarVendaAlternativa(long id)
        {
            var venda = await _vendaDao.ObterPorIdAsync(id);
            if (venda == null)
            {
                return NotFound(new { message = "Venda não encontrada." });
            }

            venda.Status = nameof(DomainEnums.VendaStatus.APROV);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Venda aprovada com sucesso." });
        }

        [HttpPut("{id}/reagendar")]
        public async Task<ActionResult> ReagendarVenda(long id, [FromBody] VendaReagendarDto vendaReagendarDto)
        {
            var venda = await _vendaDao.ObterPorIdAsync(id);
            if (venda == null)
            {
                return NotFound(new { message = "Venda não encontrada." });
            }

            venda.DataEntrega = vendaReagendarDto.NovaDataEntrega;
            if (!string.IsNullOrEmpty(vendaReagendarDto.Observacao))
            {
                venda.Observacao = vendaReagendarDto.Observacao;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Venda reagendada com sucesso." });
        }

        [HttpPut("{id}/expedicao")]
        public async Task<ActionResult> ExpedicaoVenda(long id)
        {
            var venda = await _vendaDao.ObterPorIdAsync(id);
            if (venda == null)
            {
                return NotFound(new { message = "Venda não encontrada." });
            }

            venda.Status = nameof(DomainEnums.VendaStatus.EXP);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Venda enviada para expedição com sucesso." });
        }

        [HttpPut("{id}/reprovar")]
        public async Task<ActionResult> ReprovarVenda(long id)
        {
            var venda = await _vendaDao.ObterPorIdAsync(id);
            if (venda == null)
            {
                return NotFound(new { message = "Venda não encontrada." });
            }

            venda.Status = nameof(DomainEnums.VendaStatus.REPR);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Venda reprovada com sucesso." });
        }
    }
}