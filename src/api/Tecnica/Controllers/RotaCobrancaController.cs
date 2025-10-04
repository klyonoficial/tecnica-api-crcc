using Microsoft.AspNetCore.Mvc;
using Tecnica.Domain.Daos;
using Tecnica.Domain.Dtos;
using Tecnica.Domain.Entities;
using Tecnica.Dtos;

namespace Tecnica.Controllers
{
    [ApiController]
    [Route("rota-cobranca")]
    public class RotaCobrancaController : ControllerBase
    {
        private readonly RotaCobrancaDao _rotaCobrancaDao;
        private readonly ApplicationDbContext _context;
        private readonly VendaDao _vendas;

        public RotaCobrancaController(RotaCobrancaDao rotaCobrancaDao, ApplicationDbContext context, VendaDao vendas)
        {
            _rotaCobrancaDao = rotaCobrancaDao;
            _context = context;
            _vendas = vendas;
        }

        // GET /rota-cobranca - Filtros: idCliente, dataCobranca - RotaCobrancaResultDto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RotaCobrancaResultDto>>> GetRotasCobranca(
            [FromQuery] long? idCliente,
            [FromQuery] DateTime? dataCobranca,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var rotas = await _rotaCobrancaDao.ListarPorFiltroAsync(idCliente, dataCobranca, pageNumber, pageSize);

            var result = rotas.Select(r => new RotaCobrancaResultDto
            {
                Id = r.Id,
                Data = r.Data
            }).ToList();

            return Ok(result);
        }

        // POST /rota-cobranca - RotaCobrancaDto
        [HttpPost]
        public async Task<ActionResult<RotaCobrancaDto>> CreateRotaCobranca([FromBody] RotaCobrancaDto rotaCobrancaDto)
        {
            // Criar lista de cobranças se fornecidas
            List<Cobranca>? cobrancas = null;
            if (rotaCobrancaDto.Cobrancas != null && rotaCobrancaDto.Cobrancas.Any())
            {
                cobrancas = rotaCobrancaDto.Cobrancas.Select(c => new Cobranca(c.ParcelaId)).ToList();
            }

            var rotaCobranca = new RotaCobranca(
                rotaCobrancaDto.Data,
                rotaCobrancaDto.CobradorId,
                cobrancas);

            await _rotaCobrancaDao.AdicionarAsync(rotaCobranca);
            await _context.SaveChangesAsync();

            rotaCobrancaDto.Id = rotaCobranca.Id;

            return Ok(rotaCobrancaDto);
            //TODO: REVISAR - System.InvalidOperationException: No route matches the supplied values.
            //return CreatedAtAction(nameof(GetRotaCobrancaPorData), new { data = rotaCobranca.Data.Date }, rotaCobrancaDto);
        }

        // PUT /rota-cobranca/{id}/aprovacao - RotaCobrancaAprovacaoDto
        [HttpPut("{id}/aprovacao")]
        public async Task<ActionResult> AprovarRotaCobranca(long id, [FromBody] RotaCobrancaAprovacaoDto aprovacaoDto)
        {
            var rotaCobranca = await _rotaCobrancaDao.ObterPorIdAsync(id);
            if (rotaCobranca == null)
            {
                return NotFound();
            }

            if (aprovacaoDto.Aprovado)
            {
                rotaCobranca.Aprovar(aprovacaoDto.AprovadorId, aprovacaoDto.Observacao);
            }
            else
            {
                rotaCobranca.Reprovar(aprovacaoDto.AprovadorId, aprovacaoDto.Observacao);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}/venda/{parcelaId}")]
        public async Task<ActionResult> RemoverVenda(long id, long parcelaId)
        {
              var rotaCobranca = await _rotaCobrancaDao.ObterPorIdAsync(id);
            if (rotaCobranca == null)
            {
                return NotFound();
            }

            rotaCobranca.RemoverParcela(parcelaId);

            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET /rota-cobranca/{data} - Lista as cobranças para o dia - RotaCobrancaItemsDto
        [HttpGet("data/{data}")]
        public async Task<ActionResult<List<RotaCobrancaDto>>> GetRotaCobrancaPorData(DateTime data)
        {
            var rotaCobranca = await _rotaCobrancaDao.ObterPorDataAsync(data);
            var rotasDto  = rotaCobranca?.Select(r => new RotaCobrancaDto
            {
                Id = r.Id,
                Data = r.Data,
                CobradorId = r.CobradorId,
                CobradorNome = "Cobrador", // TODO: Get from Usuario table
            }).ToList();

            return Ok(rotasDto);
        }

        [HttpGet("data/{data}/parcelas")]
        public async Task<ActionResult<List<CobrancaDto>>> GetParcelasPorData(DateTime data)
        {
            var parcelas = await _rotaCobrancaDao.ListarParcelasEmAberto(data);
            return Ok(MapToCobrancaDto(parcelas));
        }

        List<CobrancaDto> MapToCobrancaDto(List<Parcela> parcelas)
        {
            return parcelas.Select(c => new CobrancaDto
            {
                Id = c.Id,
                ParcelaId = c.Id,
                VendaId = c.VendaId,
                ClienteId = c.Venda!.PessoaId,
                ClienteNome = c.Venda.Cliente?.RazaoSocial ?? "Cliente não encontrado",
                Valor = c.Valor,
                DataVencimento = c.DataVencimento,
                Status = c.Status,
                EnderecoLogradouro = c.Venda?.EnderecoLogradouro ?? string.Empty,
                EnderecoNumero = c.Venda?.EnderecoNumero ?? string.Empty,
                EnderecoComplemento = c.Venda?.EnderecoComplemento ?? string.Empty,
                EnderecoBairro = c.Venda?.EnderecoBairro ?? string.Empty,
                EnderecoCidade = c.Venda?.EnderecoCodigoIbge ?? string.Empty,
                EnderecoUf = c.Venda?.EnderecoUF ?? string.Empty,
                EnderecoCep = c.Venda?.EnderecoCep ?? string.Empty
            }).ToList();
        }

        List<CobrancaDto> MapToCobrancaDto(List<Cobranca> cobrancas)
        {
            return cobrancas.Select(c => new CobrancaDto
            {
                Id = c.Id,
                ParcelaId = c.Id,
                VendaId = c.Parcela.VendaId,
                ClienteId = c.Parcela.Venda!.PessoaId,
                ClienteNome = c.Parcela.Venda.Cliente?.RazaoSocial ?? "Cliente não encontrado",
                Valor = c.Parcela.Valor,
                DataVencimento = c.Parcela.DataVencimento,
                Status = c.Parcela.Status,
                EnderecoLogradouro = c.Parcela.Venda?.EnderecoLogradouro ?? string.Empty,
                EnderecoNumero = c.Parcela.Venda?.EnderecoNumero ?? string.Empty,
                EnderecoComplemento = c.Parcela.Venda?.EnderecoComplemento ?? string.Empty,
                EnderecoBairro = c.Parcela.Venda?.EnderecoBairro ?? string.Empty,
                EnderecoCidade = c.Parcela.Venda?.EnderecoCodigoIbge ?? string.Empty,
                EnderecoUf = c.Parcela.Venda?.EnderecoUF ?? string.Empty,
                EnderecoCep = c.Parcela.Venda?.EnderecoCep ?? string.Empty
            }).ToList();
        }

        // GET /rota-cobranca/{id}/imprimir - O que?
        [HttpGet("{id}/imprimir")]
        public async Task<ActionResult> ImprimirRotaCobranca(long id)
        {
            var rotaCobranca = await _rotaCobrancaDao.ObterPorIdAsync(id);
            if (rotaCobranca == null)
            {
                return NotFound();
            }

            // TODO: Implementar lógica de impressão (PDF, relatório, etc.)
            // Por enquanto, retornamos apenas os dados da rota
            return Ok(new { message = "Funcionalidade de impressão a ser implementada", rotaCobrancaId = id });
        }

        [HttpPut("{rotaCobrancaId}/parcela/{parcelaId}")]
        public async Task<ActionResult> InformarPagamentoParcela(long rotaCobrancaId, long parcelaId, [FromBody] PagamentoParcelaDto pagamentoParcelaDto)
        {
             var rotaCobranca = await _rotaCobrancaDao.ObterPorIdAsync(rotaCobrancaId);
            if (rotaCobranca == null)
            {
                return NotFound();
            }

            rotaCobranca.PagarParcela(parcelaId, pagamentoParcelaDto);

            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Retorna a quantidade de parcelas a serem cobradas agrupadas por data dentro de um período especificado.
        /// </summary>
        /// <param name="dataInicio">Data inicial da pesquisa (obrigatória)</param>
        /// <param name="dataFim">Data final da pesquisa (obrigatória)</param>
        /// <returns>Lista de parcelas agrupadas por data com a quantidade</returns>
        [HttpGet("periodo")]
        public async Task<ActionResult<IEnumerable<CobrancaPorDataDto>>> GetParcelasPorPeriodo(
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            // Valida se ambas as datas foram fornecidas
            if (dataInicio == DateTime.MinValue || dataFim == DateTime.MinValue)
            {
                return BadRequest("As datas de início e fim são obrigatórias.");
            }

            // Verifica se a data inicial é menor ou igual à data final
            if (dataInicio > dataFim)
            {
                return BadRequest("A data inicial deve ser menor ou igual à data final.");
            }

            // Busca as parcelas agrupadas por data
            var parcelas = await _rotaCobrancaDao.ListarPorPeriodoAsync(dataInicio, dataFim);
            
            return Ok(parcelas);
        }

        /// <summary>
        /// Retorna uma rota de cobrança pelo seu identificador único (ID).
        /// </summary>
        /// <param name="id">ID da rota de cobrança a ser retornada</param>
        /// <returns>Dados detalhados da rota de cobrança</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RotaCobrancaDto>> GetRotaCobrancaPorId(long id)
        {
            var rotaCobranca = await _rotaCobrancaDao.ObterPorIdAsync(id);
            if (rotaCobranca == null)
            {
                return NotFound();
            }

            var dto = new RotaCobrancaDto
            {
                Id = rotaCobranca.Id,
                Data = rotaCobranca.Data,
                CobradorId = rotaCobranca.CobradorId,
                CobradorNome = "Cobrador", // TODO: Get from Usuario table
            };

            // Mapeamento das cobranças (parcelas) se existirem
            if (rotaCobranca.Cobrancas != null && rotaCobranca.Cobrancas.Any())
            {
                dto.Cobrancas = MapToCobrancaDto(rotaCobranca.Cobrancas.ToList());
            }

            return Ok(dto);
        }
    }
}