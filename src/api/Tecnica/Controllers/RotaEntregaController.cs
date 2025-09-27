using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tecnica.Domain.Daos;
using Tecnica.Domain.Dtos;
using Tecnica.Domain.Entities;
using Tecnica.Dtos;
using Tecnica.Infra;

namespace Tecnica.Api.Controllers
{
    [ApiController]
    [Route("rota-entrega")]
    public class RotaEntregaController : ControllerBase
    {
        private readonly RotaEntregaDao _rotaEntregaDao;
        private readonly VendaDao _vendaDao;
        private readonly ClienteDao _clienteDao;
        private readonly ApplicationDbContext _context;

        public RotaEntregaController(RotaEntregaDao rotaEntregaDao, VendaDao vendaDao, ClienteDao clienteDao, ApplicationDbContext context)
        {
            _rotaEntregaDao = rotaEntregaDao;
            _vendaDao = vendaDao;
            _clienteDao = clienteDao;
            _context = context;
        }

        /// <summary>
        /// Lista rotas de entrega com filtros opcionais e paginação
        /// </summary>
        /// <param name="id_cliente">ID do cliente para filtrar</param>
        /// <param name="data_entrega">Data de entrega para filtrar</param>
        /// <param name="pageNumber">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Tamanho da página (padrão: 10)</param>
        /// <returns>Lista de rotas de entrega</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RotaEntregaResultDto>>> GetRotasEntrega(
            [FromQuery] long? idCliente,
            [FromQuery] DateTime? dataEntrega,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var rotas = await _rotaEntregaDao.ListarComFiltrosAsync(idCliente, dataEntrega, pageNumber, pageSize);
            return Ok(rotas);
        }

        /// <summary>
        /// Cria uma nova rota de entrega
        /// </summary>
        /// <param name="rotaEntregaDto">Dados da rota de entrega</param>
        /// <returns>Rota de entrega criada</returns>
        [HttpPost]
        public async Task<ActionResult<RotaEntregaDto>> CreateRotaEntrega([FromBody] RotaEntregaDto rotaEntregaDto)
        {
            var rotaExistente = await _rotaEntregaDao.ObterPorDataAsync(rotaEntregaDto.Data);
            Assegure.Que(rotaExistente == null, "Já existe uma rota de entrega para a data informada.");

            var entregas = rotaEntregaDto.Entregas?.Select(e => new Entrega(e.VendaId)).ToList();
            var rotaEntrega = new RotaEntrega(rotaEntregaDto.Data, entregas);

            await _rotaEntregaDao.AdicionarAsync(rotaEntrega);
            await _context.SaveChangesAsync();

            rotaEntregaDto.Id = rotaEntrega.Id;
            return CreatedAtAction(nameof(GetRotaEntrega), new { id = rotaEntrega.Id });
        }

        /// <summary>
        /// Adiciona uma venda à rota de entrega
        /// </summary>
        /// <param name="id">ID da rota de entrega</param>
        /// <param name="vendaId">ID da venda</param>
        /// <returns>Status da operação</returns>
        [HttpPut("{id}/venda/{vendaId}")]
        public async Task<ActionResult> AdicionarVendaARota(long id, long vendaId)
        {
            var rotaEntrega = await _rotaEntregaDao.ObterPorIdAsync(id);
            if (rotaEntrega == null)
            {
                return NotFound(new { mensagem = "Rota de entrega não encontrada" });
            }

            var venda = await _vendaDao.ObterPorIdAsync(vendaId);
            if (venda == null)
            {
                return NotFound(new { mensagem = "Venda não encontrada" });
            }

            // Verifica se a venda já está na rota
            var entregaExistente = await _rotaEntregaDao.ObterEntregaPorRotaEVendaAsync(id, vendaId);
            if (entregaExistente != null)
            {
                return BadRequest(new { mensagem = "Venda já está adicionada à rota de entrega" });
            }

            var entrega = new Entrega(vendaId);
            rotaEntrega.AddEntrega(entrega);

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Remove uma venda da rota de entrega
        /// </summary>
        /// <param name="id">ID da rota de entrega</param>
        /// <param name="vendaId">ID da venda</param>
        /// <returns>Status da operação</returns>
        [HttpDelete("{id}/venda/{vendaId}")]
        public async Task<ActionResult> RemoverVendaDaRota(long id, long vendaId)
        {
            var rotaEntrega = await _rotaEntregaDao.ObterPorIdAsync(id);
            if (rotaEntrega == null)
            {
                return NotFound(new { mensagem = "Rota de entrega não encontrada" });
            }

            var entrega = await _rotaEntregaDao.ObterEntregaPorRotaEVendaAsync(id, vendaId);
            if (entrega == null)
            {
                return NotFound(new { mensagem = "Venda não encontrada na rota de entrega" });
            }

            rotaEntrega.RemoverEntrega(entrega);

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Obtém uma rota de entrega por ID
        /// </summary>
        /// <param name="id">ID da rota de entrega</param>
        /// <returns>Dados da rota de entrega</returns>
        [HttpGet("details/{id}")]
        public async Task<ActionResult<RotaEntregaDto>> GetRotaEntrega(long id)
        {
            var rotaEntrega = await _rotaEntregaDao.ObterPorIdAsync(id);
            if (rotaEntrega == null)
            {
                return NotFound();
            }
            return Ok(RetornarEntregasDataRota(rotaEntrega));
        }

        /// <summary>
        /// Lista as Rotas entregas para um dia específico
        /// </summary>
        /// <param name="data">Data no formato yyyy-MM-dd</param>
        /// <returns>Lista de entregas do dia</returns>
        [HttpGet("data/{data}")]
        public async Task<ActionResult<List<RotaEntregaDto>>> GetRotaEntregasPorData(DateTime data)
        {
            var rotaEntregas = await _rotaEntregaDao.ObterPorDataAsync(data);
            return Ok(RotaEntregasToDto(rotaEntregas));
        }

        [HttpGet("data/{data}/entregas")]
        public async Task<ActionResult<List<RotaEntregaDto>>> GetEntregasPorData(DateTime data)
        {
            var rotaEntregas = await _rotaEntregaDao.ListarEntregasEmAberto(data);
            return Ok(RetornarEntregasDasVendas(rotaEntregas));
        }

        private static List<RotaEntregaDto> RotaEntregasToDto(List<RotaEntrega> rotaEntregas)
        {
            var entregas = new List<RotaEntregaDto>();

            foreach (var rotaEntrega in rotaEntregas)
            {
                var rotaEntregaItemsDto = new RotaEntregaDto
                {
                    Id = rotaEntrega.Id,
                    Data = rotaEntrega.Data
                };

                entregas.Add(rotaEntregaItemsDto);
            }

            return entregas;
        }

        private static RotaEntregaDto RetornarEntregasDataRota(RotaEntrega rotaEntrega)
        {
            var entregas = new List<EntregaDto>();

            foreach (var entrega in rotaEntrega.Entregas)
            {
                entregas.Add(new EntregaDto
                {
                    Id = entrega.Id,
                    VendaId = entrega.VendaId,
                    Status = entrega.Status,
                    ClienteId = entrega.Venda.PessoaId,
                    ClienteNome = entrega.Venda.Cliente.RazaoSocial,
                    ValorTotal = entrega.Venda.ValorTotal,
                    EnderecoBairro = entrega.Venda.EnderecoBairro,
                    EnderecoUF = entrega.Venda.EnderecoUF,
                    EnderecoNumero = entrega.Venda.EnderecoNumero,
                    EnderecoComplemento = entrega.Venda.EnderecoComplemento
                });

            }

            var rotaEntregaItemsDto = new RotaEntregaDto
            {
                Id = rotaEntrega.Id,
                Data = rotaEntrega.Data,
                Entregas = entregas
            };
            return rotaEntregaItemsDto;
        }

        private static List<EntregaDto> RetornarEntregasDasVendas(List<Venda> vendas)
        {
            var entregas = new List<EntregaDto>();

            foreach (var venda in vendas)
            {
                var entrega = new EntregaDto
                {
                    Id = null, // ID fictício, pois não existe uma entrega real
                    VendaId = venda.Id,
                    Status = venda.Status,
                    ClienteId = venda.PessoaId,
                    ClienteNome = venda.Cliente.RazaoSocial,
                    ValorTotal = venda.ValorTotal,
                    EnderecoBairro = venda.EnderecoBairro,
                    EnderecoUF = venda.EnderecoUF,
                    EnderecoNumero = venda.EnderecoNumero,
                    EnderecoComplemento = venda.EnderecoComplemento
                };

                entregas.Add(entrega);
            }

            return entregas;
        }

        /// <summary>
        /// Endpoint para impressão da rota de entrega
        /// </summary>
        /// <param name="id">ID da rota de entrega</param>
        /// <returns>Dados formatados para impressão</returns>
        [HttpGet("{id}/imprimir")]
        public async Task<ActionResult> ImprimirRotaEntrega(long id)
        {
            var rotaEntrega = await _rotaEntregaDao.ObterPorIdAsync(id);
            if (rotaEntrega == null)
            {
                return NotFound();
            }

            // TODO: Implementar lógica de formatação para impressão
            // Por enquanto retorna os dados básicos da rota
            var entregas = rotaEntrega.Entregas?.Select(e => new
            {
                VendaId = e.VendaId,
                Status = e.Status,
                Cliente = e.Venda?.PessoaId,
                Endereco = $"{e.Venda?.EnderecoBairro}, {e.Venda?.EnderecoNumero} - {e.Venda?.EnderecoUF}"
            }).ToList();

            var dadosImpressao = new
            {
                RotaId = rotaEntrega.Id,
                Data = rotaEntrega.Data.ToString("dd/MM/yyyy"),
                TotalEntregas = rotaEntrega.Entregas?.Count ?? 0,
                Entregas = entregas
            };

            return Ok(dadosImpressao);
        }

        /// <summary>
        /// Atualiza o status de entrega de uma venda
        /// </summary>
        /// <param name="id">ID da rota de entrega</param>
        /// <param name="vendaId">ID da venda</param>
        /// <param name="status">Status da entrega (ENTR/NENTR)</param>
        /// <returns>Status da operação</returns>
        [HttpPut("{id}/venda/{vendaId}/status")]
        public async Task<ActionResult> AtualizarStatusEntrega(long id, long vendaId, [FromBody] string status)
        {
            var rotaEntrega = await _rotaEntregaDao.ObterPorIdAsync(id);
            if (rotaEntrega == null)
            {
                return NotFound(new { mensagem = "Rota de entrega não encontrada" });
            }

            rotaEntrega.AtualizarStatusEntrega(vendaId, status);

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Retorna a quantidade de entregas agrupadas por data dentro de um período especificado.
        /// </summary>
        /// <param name="dataInicio">Data inicial da pesquisa (obrigatória)</param>
        /// <param name="dataFim">Data final da pesquisa (obrigatória)</param>
        /// <returns>Lista de entregas agrupadas por data com a quantidade</returns>
        [HttpGet("periodo")]
        public async Task<ActionResult<IEnumerable<EntregaPorDataDto>>> GetVendasPorPeriodo(
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

            // Busca as entregas agrupadas por data
            var entregas = await _vendaDao.ListarEntregasPorPeriodoAsync(dataInicio, dataFim);
            
            return Ok(entregas);
        }
    }
}